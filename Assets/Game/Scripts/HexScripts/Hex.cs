using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hex : MonoBehaviour
{
   #region Inspector
    GameObject Player;
    private Rigidbody playerRb;
    PlayerMovement playerMov;

    private GlowHighlight highlight;
    private HexCoordinates hexCoordinates;
    [SerializeField] protected HexType hexType;

        #endregion
    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    
    // wie weit kann die Unit laufen
    public int GetCost()
        => hexType switch
        {
            HexType.Water => 20,
            HexType.Default => 10,
            HexType.SlowDown => 15,
            _ => throw new Exception($"Hex of type {hexType} not supported")
        };
    
    public bool IsObstacle()
    {
        return this.hexType == HexType.Obstacle;
    }

    private void Awake()
    {
        
        hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighlight>();

    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerRb = Player.GetComponent<Rigidbody>();
        playerMov = Player.GetComponent<PlayerMovement>();
    }

    #region  HighlightHexs
    public void EnableHighlight()
    {
        highlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        highlight.ToggleGlow(false);
    }

    internal void ResetHighlight()
    {
        highlight.ResetGlowHighlight();
    }

    internal void HighlightPath()
    {
        highlight.HighlightValidPath();
    }
    
    
    
    #endregion
    
    #region OnTriggerHexTypes
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Player)
        {
            if ((hexType == HexType.SlowDown))
            {
                SlowDownStarter();
            }
            
            if ((hexType == HexType.Trampolin))
            {
                TrampolinStarter();
            }
            
            if ((hexType == HexType.ChangeDirection))
            {
                ChangeDirectionStarter();
            }
            
            if ((hexType == HexType.BoostForward))
            {
                BoostForwardStarter();
            }
        }
        
        
    }
    #endregion


    

    #region ChangeDirection

    //[SerializeField] private float ChangeDirectionBoostForce = 200f;
    //private float ChangeDirectionBoostDuration = 0.8f;
    private bool isChangingDirection = false;
    private Coroutine changeDirectionCoroutine;
    private bool allowChangeDirection = true;

    public void ChangeDirectionStarter()
    {
        Debug.Log("C");

        if (allowChangeDirection == false) return;

        allowChangeDirection = false;


        if (changeDirectionCoroutine != null)
            StopCoroutine(changeDirectionCoroutine);


        isChangingDirection = true;
        changeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine());
    }

    private IEnumerator ChangeDirectionCoroutine()
    {
        

        playerRb.velocity = playerRb.velocity * -1;
        yield return new WaitForSeconds(0.5f);

        isChangingDirection = false;
        yield return null;
    }

    
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Player)
        {
            allowChangeDirection = true;
        }
    }
    #endregion

    #region SlowDown
    [SerializeField] private AnimationCurve slowDownCurve;
    //[SerializeField] private float SlowDownForce = 400f;
    private float SlowDownDuration = 0.4f;
    
    private bool IsSlowingDown = false; //used to lock other boosts
    private Coroutine slowDownCoroutine;

    public void SlowDownStarter()
    {
        Debug.Log("S");
        if (slowDownCoroutine != null)
            StopCoroutine(slowDownCoroutine);

        slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
    }

    private IEnumerator SlowDownCoroutine()
    {
        float t = 0;
        // Vector3 halfVelocity = velocity * 0.5f;

        playerRb.velocity = playerRb.velocity / 2;

        while (t < SlowDownDuration)
        {
            t += Time.deltaTime;
            float curveValue = slowDownCurve.Evaluate(t);

            playerRb.velocity *= 0.99f;
            yield return null;
        }

        IsSlowingDown = false;
    }

    #endregion

    #region BoostForward

    [SerializeField] private float BoostForce = 200f;
    private float BoostDuration = 0.8f;
    public bool IsHexBoosting = false; //used to lock other boosts
    private Coroutine hexBoostForwardCoroutine;
    [SerializeField] private AnimationCurve boostCurve;

    public void BoostForwardStarter()
    {
        Debug.Log("B");

        if (hexBoostForwardCoroutine != null)
            StopCoroutine(hexBoostForwardCoroutine);

        hexBoostForwardCoroutine = StartCoroutine(HexBoostForwardCoroutine());
    }

    private IEnumerator HexBoostForwardCoroutine()
    {
        float t = 0;
        while (t < BoostDuration)
        {

            t += Time.deltaTime;
            float curveValue = boostCurve.Evaluate(t);

            playerMov.currentHexFowardForce += BoostForce * curveValue * Time.deltaTime;

            playerMov.OnBoostForwardHex = true;
            yield return null;
        }

        playerRb.velocity = playerRb.velocity / 2;

        playerMov.OnBoostForwardHex = false;
        playerMov.currentHexFowardForce = 0;
        IsHexBoosting = false;
    }

    #endregion

    #region Trampolin

    float reboundDuration = 0.2f;
    [SerializeField] float TramoplinForce = 15f;
    //[SerializeField] float velocityInfluence = 0.5f;
    private Coroutine trampolinCoroutine;

    Vector3 direction;
    Vector3 ReboundMovement;
    
    public void TrampolinStarter()
    {
        playerMov.rebounded = true;

        direction = Vector3.up;

        ReboundMovement = direction * (TramoplinForce * 10) * Time.deltaTime; //new Vector3(0, direction.y * yReboundVelocity, 0) * force;

        playerRb.velocity = new Vector3(playerRb.velocity.x * 0.1f, playerRb.velocity.y, playerRb.velocity.z * 0.1f);
        Debug.Log("T");

        if (trampolinCoroutine != null)
            StopCoroutine(trampolinCoroutine);

        trampolinCoroutine = StartCoroutine(TrampolinCoroutine());
    }

    IEnumerator TrampolinCoroutine()
    {
        float timer = 0;

        while (playerMov.rebounded == true)
        {
            timer += Time.deltaTime;

            if (timer < reboundDuration)
            {
                playerRb.AddForce(ReboundMovement, ForceMode.Impulse);

            }
            else
            {
                playerMov.rebounded = false;
                timer = 0;
            }
        }

        yield return null;
    }

    #endregion 
}



public enum HexType
{
    None,
    Default,
    SlowDown,
    Trampolin,
    ChangeDirection,
    BoostForward,
    Water,
    Building,
    Obstacle
}


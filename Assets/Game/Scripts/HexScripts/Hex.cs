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
    GameManager gameMng;
    AudioManager audManager;
    AudioClipsHexes audioClipHexes;

    [SerializeField] AudioSource myAudioSource;

    private GlowHighlight highlight;
    private HexCoordinates hexCoordinates;
    [SerializeField] protected HexType hexType;

    AudioClip clip;



    #endregion
    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    System.Action OnEffectHex;
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
        gameMng = GameManager.Instance;
        Player = GameObject.FindGameObjectWithTag("Player");
        playerRb = Player.GetComponent<Rigidbody>();
        playerMov = Player.GetComponent<PlayerMovement>();
        audManager = AudioManager.Instance;
        audioClipHexes = AudioManager.Instance.gameObject.GetComponent<AudioClipsHexes>();
        //myAudioSource = this.GetComponent<AudioSource>();

        OnEffectHex += PlaySound;

        if (hexType != HexType.Default)
        {
            foreach(StringAudiofileClass file in audioClipHexes.AllHexClips)
            {
                if(file.name == hexType.ToString())
                {
                    
                    clip = file.clip;
                    myAudioSource.clip = clip;
                    Debug.Log("ClipFound");
                }
            }
        }
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

            if ((hexType == HexType.BoostInDirection))
            {
                BoostInDirectionStarter();
            }

            StartCoroutine(EnableHighlightDelayed());
            StartCoroutine(DisableHighlightDelayed());
        }
        
        
    }
    #endregion
    
    IEnumerator EnableHighlightDelayed()
    {
    
        yield return new WaitForSeconds(GameManager.GlowEnableDelay);
        EnableHighlight();
    }

   
    IEnumerator DisableHighlightDelayed()
    {
        yield return new WaitForSeconds(GameManager.GlowDisableDelay);
        DisableHighlight();
    }

    #region ChangeDirection

    //[SerializeField] private float ChangeDirectionBoostForce = 200f;
    //private float ChangeDirectionBoostDuration = 0.8f;
    [Header ("ChangeDirection")]
    private bool isChangingDirection = false;
    private Coroutine changeDirectionCoroutine;
    private bool allowStartChangeDirection = true;

    public void ChangeDirectionStarter()
    {
        if (gameMng.AllowHexEffects == false) return;

        gameMng.ChangeDirectionCounter++;
        if (gameMng.AllowChangeDirection == false) return;


        if (allowStartChangeDirection == false) return;

        allowStartChangeDirection = false;


        if (changeDirectionCoroutine != null)
            StopCoroutine(changeDirectionCoroutine);


        isChangingDirection = true;
        changeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine());
    }

    private IEnumerator ChangeDirectionCoroutine()
    {
        OnEffectHex?.Invoke();

        playerRb.velocity = playerRb.velocity * -1;
        yield return new WaitForSeconds(0.5f);

        isChangingDirection = false;
        yield return null;
    }

    
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Player)
        {
            allowStartChangeDirection = true;
        }

       
    }
    #endregion

    #region SlowDown
    [Header("SlowDown")]
    [SerializeField] private AnimationCurve slowDownCurve;
    //[SerializeField] private float SlowDownForce = 400f;
    private float SlowDownDuration = 0.4f;
    
    private bool IsSlowingDown = false; //used to lock other boosts
    private Coroutine slowDownCoroutine;

    public void SlowDownStarter()
    {
        if (gameMng.AllowHexEffects == false) return;
        
        if (slowDownCoroutine != null)
            StopCoroutine(slowDownCoroutine);

        slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
    }

    private IEnumerator SlowDownCoroutine()
    {
        OnEffectHex?.Invoke();

        float t = 0;
        // Vector3 halfVelocity = velocity * 0.5f;

        playerRb.velocity = playerRb.velocity / 2;

        while (t < SlowDownDuration)
        {
            if (gameMng.AllowHexEffects == false) break;
            t += Time.deltaTime;
            float curveValue = slowDownCurve.Evaluate(t);

            playerRb.velocity *= 0.99f;
            yield return null;
        }

        IsSlowingDown = false;
    }

    #endregion

    #region BoostForward
    [Header("BoostForward")]
    [Range(10f, 80f)] [SerializeField] private float borce = 50f;
    private float BoostForwardDuration = 0.4f;
    public bool IsHexForwardBoosting = false; //used to lock other boosts
    private Coroutine hexBoostForwardCoroutine;
   // [SerializeField] private AnimationCurve boostCurve;

    public void BoostForwardStarter()
    {
        if (gameMng.AllowHexEffects == false) return;

        gameMng.BoostForwardCounter++;
        if (gameMng.AllowBoostForward == false) return;

        if (hexBoostForwardCoroutine != null)
            StopCoroutine(hexBoostForwardCoroutine);

        hexBoostForwardCoroutine = StartCoroutine(HexBoostForwardCoroutine());
    }

    private IEnumerator HexBoostForwardCoroutine()
    {
        OnEffectHex?.Invoke();

        float t = 0;
        playerMov.OnBoostForwardHex = true;

        while (t < BoostForwardDuration)
        {
            if (gameMng.AllowHexEffects == false) break;
            t += Time.deltaTime;
            //float curveValue = boostCurve.Evaluate(t);

           // playerMov.currentHexFowardForce += BoostForce * curveValue * Time.deltaTime; -> Boost DuratioN:0.8

            playerMov.CurrentHexFowardForce = borce;


             yield return null;
        }

        playerRb.velocity = playerRb.velocity / 2;

        playerMov.OnBoostForwardHex = false;
        playerMov.CurrentHexFowardForce = 0;
        IsHexForwardBoosting = false;
    }

    #endregion

    #region Trampolin
    [Header("Trampolin")]
    float reboundDuration = 0.2f;
    [SerializeField] float TramoplinForce = 15f;
    //[SerializeField] float velocityInfluence = 0.5f;
    private Coroutine trampolinCoroutine;

    Vector3 direction;
    Vector3 ReboundMovement;
    
    public void TrampolinStarter()
    {
        if (gameMng.AllowHexEffects == false) return;

        OnEffectHex?.Invoke();
        

        playerMov.rebounded = true;

        direction = Vector3.up;

        ReboundMovement = direction * (TramoplinForce * 10) * Time.deltaTime; //new Vector3(0, direction.y * yReboundVelocity, 0) * force;

        playerRb.velocity = new Vector3(playerRb.velocity.x * 0.1f, playerRb.velocity.y, playerRb.velocity.z * 0.1f);
        

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
                if (gameMng.AllowHexEffects == false) break;
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

    #region BoostinDirection
    [Header("BoostInDirection")]
    [SerializeField] float XDirection =1;
    [SerializeField] float ZDirection = 1;
    float YDirection = 0;
    Vector3 BoostInDirectionDirection;
    Coroutine hexBoostInDirectionCoroutine;
    [Range (5, 20)] [SerializeField] float force = 20;

    float BoostInDirectionDuration = 0.3f;
    bool IsBoostingInDirection = false;

    void BoostInDirectionStarter()
    {
        //Allow Hex Effects
        Debug.Log("InDirectionHex");

        BoostInDirectionDirection = new Vector3(XDirection, YDirection, ZDirection).normalized;
        playerMov.CurrentHexInDirectionForce = 100;
        if (hexBoostInDirectionCoroutine != null)
            StopCoroutine(hexBoostInDirectionCoroutine);

        hexBoostInDirectionCoroutine = StartCoroutine(HexBoostInDirectionCoroutine());
    }

    private IEnumerator HexBoostInDirectionCoroutine()
    {
        OnEffectHex?.Invoke();

        float t = 0;
        playerMov.OnBoostInDirectionHex = true;
        playerRb.velocity = Vector3.zero; //playerRb.velocity * 0.2f;


        while (t < BoostInDirectionDuration)
        {
            if (gameMng.AllowHexEffects == false) break;
            t += Time.deltaTime;

            playerMov.HexInDirectionDirection = BoostInDirectionDirection;
            playerMov.CurrentHexInDirectionForce = playerMov.CurrentHexInDirectionForce * 0.99f * Time.deltaTime * force;


            yield return null;
        }

        playerRb.velocity = playerRb.velocity / 2;

        playerMov.OnBoostInDirectionHex = false;
        playerMov.CurrentHexInDirectionForce = 100;
        IsBoostingInDirection = false;
        playerMov.HexInDirectionDirection = Vector3.zero;


        yield return null;
    }

    #endregion


    private void OnDrawGizmosSelected()
    {
        if (hexType != HexType.BoostInDirection) return;
        BoostInDirectionDirection = new Vector3(XDirection, YDirection, ZDirection);

        float arrowLength = 5f;

        Vector3 forwardVector = BoostInDirectionDirection.normalized;
        Vector3 arrowLeft = Vector3.down * arrowLength * 0.2f;
        Vector3 arrowRight = -arrowLeft;

        Vector3 arrowTip = forwardVector * arrowLength;
        arrowLeft += forwardVector * arrowLength * .7f;
        arrowRight += forwardVector * arrowLength * .7f;

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(arrowTip + new Vector3(0, 2, 0), this.transform.forward + new Vector3(0,2,0));
        Gizmos.DrawLine(arrowTip + new Vector3(0, 2, 0), arrowLeft + new Vector3(0, 2, 0));
        Gizmos.DrawLine(arrowTip + new Vector3(0, 2, 0), arrowRight + new Vector3(0, 2, 0));
    }

    void PlaySound()
    {
        
        Debug.Log("Action");
        if (myAudioSource.isPlaying == false && audManager.allowAudio == true)
        {
            myAudioSource.Play();
            Debug.Log("Playing");
        }
        
    }

}



public enum HexType
{
    None,
    Default,
    SlowDown,
    Trampolin,
    ChangeDirection,
    BoostForward,
    BoostInDirection,
    Water,
    Building,
    Obstacle
}


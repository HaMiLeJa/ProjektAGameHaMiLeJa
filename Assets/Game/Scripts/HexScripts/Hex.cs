using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hex : MonoBehaviour
{
   #region Inspector
    GameObject Player;
    private Rigidbody playerRb;
    HexMovements hexMov;
    GameManager gameMng;
   // AudioManager audManager;
    //AudioClipsHexes audioClipHexes;
    

    //[SerializeField] AudioSource myAudioSource;

    private GlowHighlight highlight;
    //private HexCoordinates hexCoordinates;
    public HexType hexType;
    public CollectableType collectableType = CollectableType.Type1; //To Be Used :)

    AudioClip clip;

    
    #endregion
    //public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    System.Action OnEffectHex;
    // wie weit kann die Unit laufen
    /*
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
    }*/

    private void Awake()
    {
        
       // hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighlight>();

    }

    private void Start()
    {
        if (hexType == HexType.DefaultCollectable)
        {

            //colRef.HexScript = this;
            // colRef.HexScript = this.gameObject.GetComponent<Hex>();
            CollectableManager.AllCollectables.Add(this.gameObject, colRef); //Nur adden, werte angepasst wird sp�ter
        }
        gameMng = ReferenceLibary.GameMng;
        Player = ReferenceLibary.Player;
        playerRb = ReferenceLibary.RigidbodyPl;
        hexMov = ReferenceLibary.HexMov;
       // audManager = ReferenceLibary.AudMng;
       // audioClipHexes = ReferenceLibary.AudMng.HexAudMng.gameObject.GetComponent<AudioClipsHexes>();
        //myAudioSource = this.GetComponent<AudioSource>();
        
        OnEffectHex += PlaySound;
        /*
        if (hexType != HexType.Default) // Delete
        {
            foreach(StringAudiofileClass file in audioClipHexes.AllHexClips)
            {
                if(file.type == hexType)
                {
                    
                    clip = file.clip;
                    myAudioSource.clip = clip;
                }
            }
        }*/
       
    }

    #region  HighlightHexs
 

    internal void ResetHighlight()
    {
        highlight.ResetGlowHighlight(false);
    }

    internal void HighlightPath()
    {
        highlight.HighlightValidPath(false );
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

            StartCoroutine(EnableHighlightDelayed(false));
            StartCoroutine(DisableHighlightDelayed(false));
        }

    }

    public void highlightProps()
    {
        StartCoroutine(EnableHighlightDelayed(true));
        StartCoroutine(DisableHighlightDelayed(true));
       // Collider[] colliders = gameObject.transform.GetChild(1).GetComponentsInChildren<Collider>();
    }

      
        

                                                      
    #endregion
    
    IEnumerator EnableHighlightDelayed(bool isProp)
    {
    
        yield return new WaitForSeconds(GameManager.GlowEnableDelay);
        EnableHighlight(isProp);
    }

   
    IEnumerator DisableHighlightDelayed(bool isProp)
    {
        yield return new WaitForSeconds(GameManager.GlowDisableDelay);
        DisableHighlight(isProp);
    }
    public void EnableHighlight(bool isProp)
    {
        highlight.ToggleGlow(true, isProp);
    }

    public void DisableHighlight(bool isProp)
    {
        highlight.ToggleGlow(false, isProp);
    }
    #region HexEffects

    [SerializeField ]ScriptableHexEffects hexEffectsSettings;

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

        //  if (allowStartChangeDirection == false) return;
        //allowStartChangeDirection = false;

        playerRb.velocity = playerRb.velocity * -1;
        //  playerMov.OnChangeDirectionHex = true;

        StartCoroutine(MultiplicatorModificationOverTime());
        OnEffectHex?.Invoke();

        //  if (changeDirectionCoroutine != null)
        //     StopCoroutine(changeDirectionCoroutine);
        // isChangingDirection = true;
        //  changeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine());
    }

    #region NO Use: old Coroutine
    /*
    private IEnumerator ChangeDirectionCoroutine()
    {
        OnEffectHex?.Invoke();

        playerRb.velocity = playerRb.velocity * -1;
        yield return new WaitForSeconds(0.5f);
        playerMov.OnChangeDirectionHex = true;
        //isChangingDirection = false;
        yield return null;
        playerMov.OnChangeDirectionHex = false;
    }
    */
    #endregion

    /*
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == Player)
        {
            allowStartChangeDirection = true;
        }

       
    }
    */
    #endregion

    #region SlowDown
   // [Header("SlowDown")]
    //[SerializeField] private AnimationCurve slowDownCurve;
    //[SerializeField] private float SlowDownForce = 400f;
   // private float SlowDownDuration = 0.4f;
    
   // private bool IsSlowingDown = false; //used to lock other boosts
    //private Coroutine slowDownCoroutine;
    //[SerializeField] float SlowDownValue = 0.99f;

    public void SlowDownStarter()
    {
        if (gameMng.AllowHexEffects == false) return;

        hexMov.SlowDownTimer = 0;

        hexMov.OnSlowDownHex = true;

        StartCoroutine(MultiplicatorModificationOverTime());

        OnEffectHex?.Invoke();

        /*
        if (slowDownCoroutine != null)
            StopCoroutine(slowDownCoroutine);
        slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
        */
    }

    #region NO use Old Coroutine
    /*
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

            playerRb.velocity *= 0.99f; // *Time.deltaTime
            yield return null;
        }

        IsSlowingDown = false;
    }
    */
    #endregion
    #endregion

    #region BoostForward
    [Header("BoostForward")]
    [Range (30f, 70f)] [SerializeField] private float forwardForce = 50f; //Range
    private float BoostForwardDuration = 0.4f;
    public bool IsHexForwardBoosting = false; //used to lock other boosts
    private Coroutine hexBoostForwardCoroutine;
   // [SerializeField] private AnimationCurve boostCurve;

    public void BoostForwardStarter()
    {
        if (gameMng.AllowHexEffects == false) return;

        gameMng.BoostForwardCounter++;
        if (gameMng.AllowBoostForward == false) return;


        hexMov.BoostForwardTimer = 0;
        hexMov.CurrentHexFowardForce = forwardForce;

        hexMov.OnBoostForwardHex = true;

        StartCoroutine(MultiplicatorModificationOverTime());
        OnEffectHex?.Invoke();

        // if (hexBoostForwardCoroutine != null)
        //   StopCoroutine(hexBoostForwardCoroutine);
        // hexBoostForwardCoroutine = StartCoroutine(HexBoostForwardCoroutine());
    }

    #region NoUse old Coroutine;
    /*
    private IEnumerator HexBoostForwardCoroutine()
    {
        Debug.Log("BoostForward");
        OnEffectHex?.Invoke();

        float t = 0;
        playerMov.OnBoostForwardHex = true;

        playerMov.ForwardDirection = this.playerRb.velocity;

        while (t < BoostForwardDuration)
        {
            if (gameMng.AllowHexEffects == false) break;
            t += Time.deltaTime;
            //float curveValue = boostCurve.Evaluate(t);

           // playerMov.currentHexFowardForce += BoostForce * curveValue * Time.deltaTime; -> Boost DuratioN:0.8

            playerMov.CurrentHexFowardForce = forwardForce;


             yield return null;
        }

        playerRb.velocity = playerRb.velocity / 2;

        playerMov.OnBoostForwardHex = false;
        playerMov.CurrentHexFowardForce = 0;
        IsHexForwardBoosting = false;
    }
    */
    #endregion
    #endregion

    #region Trampolin
    [Header("Trampolin")]
   // float reboundDuration = 0.2f;
    [SerializeField] float tramoplinForce = 15f;
    //[SerializeField] float velocityInfluence = 0.5f;
    //private Coroutine trampolinCoroutine;

    public void TrampolinStarter()
    {
        if (gameMng.AllowHexEffects == false) return;

        StartCoroutine(MultiplicatorModificationOverTime());
        OnEffectHex?.Invoke();
        
        hexMov.rebounded = true;

       

        playerRb.velocity = new Vector3(playerRb.velocity.x * 0.1f, playerRb.velocity.y, playerRb.velocity.z * 0.1f);

        hexMov.OnTrampolinHex = true;
        hexMov.CurrentTrampolinForce = tramoplinForce;

        //if (trampolinCoroutine != null)
        //  StopCoroutine(trampolinCoroutine);
        //trampolinCoroutine = StartCoroutine(TrampolinCoroutine());
    }

    #region Useless: TrampolinCoroutine
    /*
    IEnumerator TrampolinCoroutine()
    {
        float timer = 0;
        playerMov.OnTrampolinHex = true;

        while (playerMov.rebounded == true)
        {
            timer += Time.fixedDeltaTime;

            if (timer < reboundDuration)
            {
                if (gameMng.AllowHexEffects == false) break;

                //Vector3 direction = Vector3.up;
                //Vector3 ReboundMovement = direction.normalized * (tramoplinForce * 10) * Time.deltaTime;

                playerMov.CurrentTrampolinForce = tramoplinForce;
                //playerRb.AddForce(ReboundMovement * Time.deltaTime * 100, ForceMode.Impulse);

                //playerRb.AddForce(ReboundMovement.normalized, ForceMode.Impulse);

                //playerRb.AddForce(direction.normalized * tramoplinForce * 100 * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                playerMov.rebounded = false;
                timer = 0;
            }

            yield return null;
        }

        yield return null;
        playerMov.OnTrampolinHex = false;
    }
    */
    #endregion

    #endregion

    #region BoostinDirection
    [Header("BoostInDirection")]
    public float XDirection =1;
    public float ZDirection = 1;
    float YDirection = 0;
    Vector3 BoostInDirectionDirection;
    // Coroutine hexBoostInDirectionCoroutine;
    [Range(10, 30)] [SerializeField] float boostInDForce = 20;

    // float BoostInDirectionDuration = 0.3f;
    //  bool IsBoostingInDirection = false;

    void BoostInDirectionStarter()
    {
        if (gameMng.AllowHexEffects == false) return;

        hexMov.BoostInDirectionTimer = 0;
        playerRb.velocity = Vector3.zero;

       

        hexMov.CurrentHexInDirectionForce = boostInDForce;
        BoostInDirectionDirection = new Vector3(XDirection, YDirection, ZDirection);
        hexMov.HexInDirectionDirection = BoostInDirectionDirection.normalized;
       
        
        
        hexMov.OnBoostInDirectionHex = true;
        StartCoroutine(MultiplicatorModificationOverTime());
        OnEffectHex?.Invoke();
        
        //   if (hexBoostInDirectionCoroutine != null)
        //      StopCoroutine(hexBoostInDirectionCoroutine);
        //  hexBoostInDirectionCoroutine = StartCoroutine(HexBoostInDirectionCoroutine());
    }

    #region NoUse: coroutine


    /*
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
            playerMov.CurrentHexInDirectionForce = playerMov.CurrentHexInDirectionForce* Time.deltaTime * 0.99f  * boostInDForce;


            yield return null;
        }

        //playerRb.velocity = playerRb.velocity / 2;

        playerMov.OnBoostInDirectionHex = false;
        playerMov.CurrentHexInDirectionForce = 100;
        IsBoostingInDirection = false;
        playerMov.HexInDirectionDirection = Vector3.zero;


        yield return null;
    }
    */
    #endregion

    #endregion

    IEnumerator MultiplicatorModificationOverTime()
    {
        ScoreManager.OnTemporaryMultiplicatorUpdate(hexEffectsSettings.value);
        



        yield return new WaitForSeconds(hexEffectsSettings.ModificationDuration);
        ScoreManager.OnTemporaryMultiplicatorUpdate(-hexEffectsSettings.value);
        

      yield return null;
    }
    #endregion

    #region Collectables

    [Header("Collectables")]
    //[SerializeField] SpawnHexCollectableInEditor spawnHexEditor;
    public GameObject myProps;
    [Space]
    public GameObject MyCollectable; //HIdeInInsp
    [SerializeField] GameObject collectablePrefab;
    [HideInInspector] public CollectableReferences colRef;


    public void SpawnCollectable()
    {
        MyCollectable.SetActive(true);
        
        /*
        Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);

        MyCollectable = Instantiate(collectablePrefab, position, Quaternion.identity);
        MyCollectable.transform.parent = myProps.transform;
        MyCollectable.GetComponent<Collectable>().ParentHex = this.gameObject;

        //Add to List
        //colRef.activeCollectable = true;
        */

        CollectableManager.AllCollectables[this.gameObject].ActiveCollectable = true;

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
        ReferenceLibary.AudMng.HexAudMng.PlayHex(hexType);
        
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
    DefaultCollectable,
    Water,
    Building,
    Obstacle
}

public enum CollectableType
{
    Type1,
    Type2,
}


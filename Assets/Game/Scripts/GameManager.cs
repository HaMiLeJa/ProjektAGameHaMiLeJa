using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{

    #region Keys on Controller
[Header("Keys on Controller")]

    

   // [HideInInspector] public string Dash = "X";
    //[HideInInspector] public string SuperDash = "LeftTrigger";

   // [HideInInspector] public string ShadowDash = "RightBumper";


    [HideInInspector] public string Jump = "B";
    // [HideInInspector] public string DownDash = "A";

    #endregion

    public static bool DisableSpawnHexObjectsInEditMode = false;

    #region BoostCosts
    [Header("Boost Costs")]
    public float SuperDashCosts = 1;
    public float DashCosts = 1;
    public float ShadowDashCosts = 1;
    public float DownDashCosts = 1;
    #endregion
    
    #region Inspector
    [Space]
    [SerializeField] private float Skyboxspeed;
    public bool AllowMovement = true;
    public bool AllowHexEffects = true;

    public static float GlowEnableDelay = 0.1f;
    public static  float GlowDisableDelay = 2.5f;
    public static GameObject CameraHelper;
    [HideInInspector] public static bool ZeroOutAllowed = true;
    public static bool CameraTeleportActive = false;
    public static bool StopGiveVelocityBack = true;
    public static CinemachineVirtualCamera vcam;
   // GameObject player;
   
    Rigidbody playerRb;

   public bool GameOver = false;
    #endregion



   

    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        if(GameManager.Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            GameManager.Instance = this;
        }


        DisableSpawnHexObjectsInEditMode = true;
    }
    #endregion
    
    

    private void Start()
    {
        //player = ReferenceLibary.Player;
        playerRb = ReferenceLibary.RigidbodyPl;
        CameraHelper = GameObject.FindGameObjectWithTag("CameraHelper");
        vcam = GetComponent<CinemachineVirtualCamera>();

        if (PlayerPrefs.HasKey("Highscore") == false)
            PlayerPrefs.SetFloat("Highscore", 0);

        Debug.Log("Highscore: " + PlayerPrefs.GetFloat("Highscore"));
    }



    float startSkyBoxrotation = 5;
    void Update()
    {
        if (startSkyBoxrotation > 0)
            startSkyBoxrotation--;

        if (startSkyBoxrotation == 0)
            RenderSettings.skybox.SetFloat("_Rotation", Time.time*Skyboxspeed);
        
     
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        ControlEffectHexAmount();

        
        if(Input.GetKeyDown(KeyCode.P))
        {
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
        }

       

       // if(GameOver==false)
       //     CheckForEndOfGame();
    }

    Coroutine GameOverCoroutine;
   [SerializeField] Dissolve playerDissolve;
    float timer;
    public void CheckForEndOfGame()
    {
        // if (EnergyManager.CurrentEnergy > 0) return;
       

        if (Mathf.Approximately(playerRb.velocity.x, 0) && Mathf.Approximately(playerRb.velocity.y, 0) && Mathf.Approximately(playerRb.velocity.z, 0))
        {
            Debug.Log("GameOver");
            GameOver = true;

            if (ReferenceLibary.ScoreMng.CheckForNewHighscore() == true)
            {
                ReferenceLibary.ScoreMng.SetNewHighscore();

                if (GameOverCoroutine == null)
                    GameOverCoroutine = StartCoroutine(ReferenceLibary.UIMng.GameOverNewHighscoreCoroutine());
                    Debug.Log("new highscore");
                StartCoroutine(playerDissolve.Coroutine_DisolveShield(1.1f));
            }
            else
            {
                if (GameOverCoroutine == null)
                    GameOverCoroutine = StartCoroutine(ReferenceLibary.UIMng.GameOverCoroutine());

                Debug.Log("no new highscore");
                StartCoroutine(playerDissolve.Coroutine_DisolveShield(1.1f));
            }

           
        }
        else
        {
           // Debug.Log("CheckVelocity");
            return;
        }

        


       

        //Problemcode
        /*
        if(ReferenceLibary.ScoreMng.CheckForNewHighscore() == true)
        {
            ReferenceLibary.ScoreMng.SetNewHighscore();

            if (GameOverCoroutine == null)
                //GameOverCoroutine = StartCoroutine(ReferenceLibary.UIMng.GameOverNewHighscoreCoroutine());
            Debug.Log("new highscore");
            //StartCoroutine(playerDissolve.Coroutine_DisolveShield(1));
        }
        else
        {
            if (GameOverCoroutine == null)
                GameOverCoroutine = StartCoroutine(ReferenceLibary.UIMng.GameOverCoroutine());

            Debug.Log("no new highscore");
            //StartCoroutine(playerDissolve.Coroutine_DisolveShield(1));
        }
        */

    }

    /*
    [SerializeField] GameObject MeshOutside;
    private Material DissolveMaterial;

   [SerializeField] float dissolveSpeed = 0.5f;
    IEnumerator PlayerDissolve()
    {
        float dissolveValue = 0;

        DissolveMaterial = MeshOutside.GetComponent<MeshRenderer>().sharedMaterial;
        Debug.Log("1");

        while (dissolveValue < 1)
        {
            Debug.Log("2");
            dissolveValue = Mathf.Lerp(dissolveValue, 1, dissolveSpeed);

            DissolveMaterial.SetFloat("_Dissolve", dissolveValue);

        }

        Debug.Log("3");
        yield return null;
    }
  */

        #region Control Hex Effect Amount
        [HideInInspector] public int ChangeDirectionCounter;
    [HideInInspector] public bool AllowChangeDirection;

    [HideInInspector] public int BoostForwardCounter;
    [HideInInspector] public bool AllowBoostForward;

    float tBoostHex;
    float tChangeDirectionHex;
    void ControlEffectHexAmount()
    {

        // Boost Forward
        if(BoostForwardCounter <= 5)
        {
            AllowBoostForward = true;
        }
        else
        {
            AllowBoostForward = false;
        }


        if(BoostForwardCounter >0)
        {
            tBoostHex += Time.deltaTime;
            if (tBoostHex > 2)
            {
                BoostForwardCounter--;
                tBoostHex = 0;
            }
        }
        else
        {
            tBoostHex = 0;
        }


        // ChangeDirection
        if (ChangeDirectionCounter <= 3)
        {
            AllowChangeDirection = true;
        }
        else
        {
            AllowChangeDirection = false;
        }


        if (ChangeDirectionCounter > 0)
        {
            tChangeDirectionHex += Time.deltaTime;
            if (tChangeDirectionHex > 2)
            {
                ChangeDirectionCounter--;
                tChangeDirectionHex = 0;
            }
        }
        else
        {
            tChangeDirectionHex = 0;
        }

    }
    #endregion
}

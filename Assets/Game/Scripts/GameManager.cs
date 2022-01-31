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
    public float DashCosts = 0.5f;
    public float SuperDashCosts = 2;
    public float ShadowDashCosts = 2;
    public float DownDashCosts = 2;
    #endregion
    
    #region Inspector
    [Space]
    [SerializeField] private float Skyboxspeed;
    public bool AllowMovement = true;
    public bool AllowHexEffects = true;
    [Space] 
    [Space] 
    [Header("Teleport")]
    public float SpeedAfterTeleport = 70;
    public float ReduceSpeedInfluenceBeforeTeleport = 3;
    public float IncreaseSpeedInfluenceBeforeTeleport = 1;
    [Header("Teleport increase Cam speed")]
    [Range(0f,200f)]
    public float lastDistanceTreshhold = 60f;
    public float followRoughness = 0.01f;
    [Range(0f,10f)]
    public float lastDistanceSpeedIncreasePercentPerFrame = 1f;
    [Space]
    [Space] 
    
    public static float GlowEnableDelay = 0.1f;
    public static  float GlowDisableDelay = 2.5f;
    public static GameObject CameraHelper;
    public static bool CameraTeleportActive = false;
    public static bool StopGiveVelocityBack = true;
    public static CinemachineVirtualCamera vcam;
   
   
    Rigidbody playerRb;
    #endregion



    private void Awake()
    {
       
        DisableSpawnHexObjectsInEditMode = true;
        
        CameraHelper = GameObject.FindGameObjectWithTag("CameraHelper");
    }
    
    
    private void Start()
    {
        //player = ReferenceLibary.Player;
        playerRb = ReferenceLibary.RigidbodyPl;
        
        vcam = GetComponent<CinemachineVirtualCamera>();

        // if (PlayerPrefs.HasKey("Highscore") == false)
        //   PlayerPrefs.SetFloat("Highscore", 0);

        AllowMovement = true;
        AllowHexEffects = true;
    }



    float startSkyBoxrotation = 5;
    void Update()
    {
        //nicht mehr nötig
        // if (startSkyBoxrotation > 0)
        //     startSkyBoxrotation--;
        //
        // if (startSkyBoxrotation == 0)
        //     RenderSettings.skybox.SetFloat("_Rotation", Time.time*Skyboxspeed);
        //
     
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        ControlEffectHexAmount();

        
        /*
        if(Input.GetKeyDown(KeyCode.P))
        {
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
        }
        */
       

       // if(GameOver==false)
       //     CheckForEndOfGame();
    }


   

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

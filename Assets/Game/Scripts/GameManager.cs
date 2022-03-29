using UnityEngine;
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
    public float DashCosts = 0.5f, SuperDashCosts = 2, ShadowDashCosts = 2, DownDashCosts = 2;
    #endregion
    #region Inspector
    [Space]
    public bool AllowMovement = true, AllowHexEffects = true;
    [Space] [Space][Header("Teleport")]
    public float SpeedAfterTeleport = 70, ReduceSpeedInfluenceBeforeTeleport = 3, IncreaseSpeedInfluenceBeforeTeleport = 1;
    [Header("Teleport increase Cam speed")]
    [Range(0f,200f)] public float lastDistanceTreshhold = 60f;
    public float followRoughness = 0.01f;
    [Range(0f,10f)]
    public float lastDistanceSpeedIncreasePercentPerFrame = 1f;
    [Space]
    public static GameObject CameraHelper;
    public static bool CameraTeleportActive = false, StopGiveVelocityBack = true,
        bridgePause = false, LerpCameraBack = false, StartMovingGhostLayer = false;
    [Space] [Header("No Input Managemant")]
    [Tooltip("How long no input is allowed before Energy decrease is activated")]
    [SerializeField] float noInputDuration = 15;
    public float NoInputInfluence = 0.06f;
    [HideInInspector] public float CurrentNoInputInfluence = 0;
    [Tooltip("Debug: is there currently a decrease?")] public bool NoInputDecrease = false;
    bool RecentInput = false;
    float recentInputResetTimer, noInputTimer;
    Rigidbody playerRb;
    #endregion
    private void Awake()
    {
        Resources.UnloadUnusedAssets();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif
        DisableSpawnHexObjectsInEditMode = true;
        CameraHelper = GameObject.FindGameObjectWithTag("CameraHelper");
    }
    private void Start()
    {
        //player = ReferenceLibary.Player;
        playerRb = ReferenceLibary.RigidbodyPl;
        // if (PlayerPrefs.HasKey("Highscore") == false)
        //   PlayerPrefs.SetFloat("Highscore", 0);
        AllowMovement = true;
        AllowHexEffects = true;
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)) Application.Quit();
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
    private void FixedUpdate()
    {
        if(RecentInput)
        {
            recentInputResetTimer += Time.fixedDeltaTime;
            if (recentInputResetTimer > 3) RecentInput = false;
        }
        else
        {
            if(noInputTimer < noInputDuration)
            {
                CurrentNoInputInfluence = 0;
                noInputTimer += Time.fixedDeltaTime;
            }
            else
            {
                CurrentNoInputInfluence = NoInputInfluence;
                NoInputDecrease = true;
            }
        }
    }
    public void InputMade()
    {
        RecentInput = true;
        NoInputDecrease = false;
        CurrentNoInputInfluence = 0;
        recentInputResetTimer = 0;
        noInputTimer = 0;
    }
    #region Control Hex Effect Amount
    [HideInInspector] public int ChangeDirectionCounter;
    [HideInInspector] public bool AllowChangeDirection;
    [HideInInspector] public int BoostForwardCounter;
    [HideInInspector] public bool AllowBoostForward;
    private float tBoostHex, tChangeDirectionHex;
    void ControlEffectHexAmount()
    {
        if(BoostForwardCounter <= 5) AllowBoostForward = true; // Boost Forward
        else AllowBoostForward = false;
        if(BoostForwardCounter >0)
        {
            tBoostHex += Time.deltaTime;
            if (tBoostHex > 2)
            {
                BoostForwardCounter--;
                tBoostHex = 0;
            }
        }
        else tBoostHex = 0;
        
        if (ChangeDirectionCounter <= 3) AllowChangeDirection = true;  // ChangeDirection
        else AllowChangeDirection = false;
        if (ChangeDirectionCounter > 0)
        {
            tChangeDirectionHex += Time.deltaTime;
            if (tChangeDirectionHex > 2)
            {
                ChangeDirectionCounter--;
                tChangeDirectionHex = 0;
            }
        }
        else tChangeDirectionHex = 0;
    }
    #endregion
}
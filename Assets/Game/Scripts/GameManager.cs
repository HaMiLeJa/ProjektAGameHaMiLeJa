using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Keys on Controller
    [Tooltip ("Controller Input: Use X, Y, A or B.")]
    public string Jump = "B";
    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string Dash = "X";
    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string ShadowDash = "Y";
    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string SuperDash = "A";

    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string DownDash = "RightBumper";

    #endregion

    #region Events

    public delegate void DestroyableDestroyed(float value);
    public DestroyableDestroyed onDestroyableDestroyed;

    public delegate void EnergyChange(float value); //Managing the Energy Value (gain and loss)
    public EnergyChange onEnergyChange;

    public delegate void EnergyChangeUI(float value); //Managing the Energy Value (gain and loss)
    public EnergyChange onUIEnergyChange;

    #endregion


    #region Inspector
    [Space]
    [SerializeField] private float Skyboxspeed;
    public bool AllowMovement = true;
    public bool AllowHexEffects = true;

    GameObject player;
    Rigidbody playerRb;
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
    }
    #endregion

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time*Skyboxspeed);
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        ControlEffectHexAmount();

        
        if(Input.GetKeyDown(KeyCode.P))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }


        CheckForEndOfGame();
    }

    void CheckForEndOfGame()
    {
        if(ResourceManager.Instance.CurrentEnergy == 0)
        {
            if(playerRb.velocity == Vector3.zero)
            {
                UIManager.Instance.ShowEndMessage();
            }
        }
    }

    #region Control Hex Effect Amount
    [HideInInspector] public int ChangeDirectionCounter;
    [HideInInspector] public bool AllowChangeDirection;

    [HideInInspector] public int BoostForwardCounter;
    [HideInInspector] public bool AllowBoostForward;

    float tBoostHex;
    void ControlEffectHexAmount()
    {

        // Boost Forward
        if(BoostForwardCounter <= 3)
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
            if (tBoostHex > 3)
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
            tBoostHex += Time.deltaTime;
            if (tBoostHex > 3)
            {
                ChangeDirectionCounter--;
                tBoostHex = 0;
            }
        }
        else
        {
            tBoostHex = 0;
        }

    }
    #endregion
}

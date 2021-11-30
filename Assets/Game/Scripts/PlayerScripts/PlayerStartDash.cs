using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartDash : MonoBehaviour //Super boost as the initial thing to give the player super speed
{
    #region Inspector

    Rigidbody rb;
    PlayerMovement playerMov;
    GameManager gameMng;
    PlayerBoost dash;
    ShadowDash shadowDash;

    float boostDuration = 0.1f;
    bool boostButtonPressedInLastFrame = false;
    bool boostButtonPressedInLastFrame2 = false;
    bool allowSetDirection = false;
    float timerBoost;
    [Space]
    [SerializeField] float boostForce = 60;
    public bool Boosting; //used to lock bools

    [SerializeField] float setDirectionTimer;
    Vector3 boostDirection;
    bool directionSet = false;

    //[HideInInspector] public bool dealDamage = false;
    //[Tooltip("For how long Player can damage de Destroyables")] [SerializeField] float dealDamageDuration;

    [SerializeField] GameObject circle;

    Coroutine coroutine;
    bool coroutineStarted = false;

    AudioManager audManager;
    [SerializeField] AudioSource audioSource;


    float dashTimer;
    bool addBoostForce;

    #endregion

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        playerMov = this.GetComponent<PlayerMovement>();

        dash = this.GetComponent<PlayerBoost>();
        shadowDash = this.GetComponent<ShadowDash>();

        gameMng = GameManager.Instance;

        circle.SetActive(false);

        audManager = AudioManager.Instance;
    }

    void FixedUpdate()
    {
        if (gameMng.AllowMovement == false) return;
        SuperBoost();


        if (addBoostForce == true)
        {
            if (dashTimer < boostDuration)
            {
                dashTimer += Time.fixedDeltaTime;
                rb.AddForce(boostDirection.normalized * boostForce * Time.deltaTime * 10, ForceMode.Impulse);
            }
            else
            {
                dashTimer = 0;
                Boosting = false;
                addBoostForce = false;
            }
        }

    }

  
    void SuperBoost()
    {
        if (dash.IsBoosting == true || shadowDash.isShadowDashing == true) return;

        if (Input.GetAxisRaw("LeftTrigger") != 0 || Input.GetButton("Y"))
        {
            if (boostButtonPressedInLastFrame == false)
            {
                allowSetDirection = true;
            }
            boostButtonPressedInLastFrame = true;


            if (allowSetDirection == true)
            {
                SetDirecton();
            }


        }
        else
        {
            boostButtonPressedInLastFrame = false;
            allowSetDirection = false;

            setDirectionTimer = 0;
            timerBoost = 0;

            directionSet = false;
            Boosting = false;

            circle.SetActive(false);

        }

        // If ButtonPressed and Direction Set
        if (Input.GetAxisRaw("LeftTrigger") != 0 && directionSet == true || Input.GetButton("Y") && directionSet == true) 
        {
            if (boostButtonPressedInLastFrame2 == false)
            {
                StartDashStarter();
            }
            boostButtonPressedInLastFrame2 = true;
        }
        else
        {
            boostButtonPressedInLastFrame2 = false;
        }
    }

    void SetDirecton()
    {
        setDirectionTimer += Time.fixedDeltaTime;

        if (setDirectionTimer < 1f)
        {
            Boosting = true;
            circle.SetActive(true);
            rb.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            boostDirection = playerMov.MovementDirection;
            directionSet = true;
            circle.SetActive(false);
        }
    }

    
    void StartDashStarter()
    {
        addBoostForce = true;

       if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(StartDashCoroutine());
    }

  
    IEnumerator StartDashCoroutine()
    {
        if (audioSource.isPlaying == false && audManager.allowAudio == true)
            audioSource.Play();

        GameManager.Instance.onUIEnergyChange?.Invoke(-gameMng.StartDashCosts);
        
        new WaitForSeconds(boostDuration);

        GameManager.Instance.onEnergyChange?.Invoke(-gameMng.StartDashCosts);

        yield return null;
    }

}

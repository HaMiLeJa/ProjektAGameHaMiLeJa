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
    [SerializeField] float boostForce = 15;
    public bool Boosting; //used to lock bools

    [SerializeField] float setDirectionTimer;
    Vector3 boostDirection;
    bool directionSet = false;

    //[HideInInspector] public bool dealDamage = false;
    //[Tooltip("For how long Player can damage de Destroyables")] [SerializeField] float dealDamageDuration;

    [SerializeField] GameObject circle;

    Coroutine coroutine;
    bool coroutineStarted = false;

    #endregion

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        playerMov = this.GetComponent<PlayerMovement>();

        dash = this.GetComponent<PlayerBoost>();
        shadowDash = this.GetComponent<ShadowDash>();

        gameMng = GameManager.Instance;

        circle.SetActive(false);
    }

    void FixedUpdate()
    {
        if (gameMng.AllowMovement == false) return;
        SuperBoost();
    }

    //bool eventActivated = false;

    void SuperBoost() //Wait(SetDirection) and Boost
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

            /*
            if (directionSet == true && started == false)
            {
                StartDashStarter();
            }*/

        }
        else //if (coroutineStarted == false)
        {
            boostButtonPressedInLastFrame = false;
            allowSetDirection = false;

            setDirectionTimer = 0;
            timerBoost = 0;

            directionSet = false;
            Boosting = false;

            circle.SetActive(false);

            //coroutine = null;
            // eventActivated = false;
        }

        // && started == false
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
            //started = false
        }
    }

    void SetDirecton()
    {
        setDirectionTimer += Time.deltaTime;

        if (setDirectionTimer < 1f)
        {
            Boosting = true;
            circle.SetActive(true);
            rb.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            boostDirection = playerMov.MovementDirection;
           // allowSetDirection = false; //Addet
            directionSet = true;
            circle.SetActive(false);
        }
    }

    bool started = false;

    void StartDashStarter()
    {
        started = true;
        //if (Boosting == true) return;

       // Boosting = true;


        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(StartDashCoroutine());

        if (directionSet == true) //&& timerBoost < boostDuration
        {
            // timerBoost += Time.deltaTime;
            //rb.AddForce(boostDirection.normalized * boostForce, ForceMode.Impulse); //*3 zum zeigen

            /*
            if (coroutineStarted == false)
            {
                coroutineStarted = true;

                if (coroutine == null)
                    coroutine = StartCoroutine(StartDashCoroutine());

            }
            */
            


        }
        else
        {
            //Boosting = false;
        }
    }


    IEnumerator StartDashCoroutine()
    {
        Debug.Log("Coro");
        GameManager.Instance.onUIEnergyChange?.Invoke(-gameMng.StartDashCosts);
        float timer = 0;

        while (timer < boostDuration)
        {
            timer += Time.deltaTime;

            rb.AddForce(boostDirection.normalized * boostForce, ForceMode.Impulse);
            yield return null;

        }

        Boosting = false;
        coroutineStarted = false;

        GameManager.Instance.onEnergyChange?.Invoke(-gameMng.StartDashCosts);

        started = false;
        yield return null;
    }

}

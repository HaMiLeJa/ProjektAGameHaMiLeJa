using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartDash : MonoBehaviour //Super boost as the initial thing to give the player super speed
{
    Rigidbody rb;
    PlayerMovement playerMov;
    GameManager gameMng;
    PlayerBoost dash;
    ShadowDash shadowDash;

    float boostDuration = 0.1f;
    bool boostButtonPressedInLastFrame = false;
    bool allowBoost = false;
    float timerBoost;
    [SerializeField] float boostForce = 15;
    public bool Boosting; //used to lock bools

    [SerializeField] float setDirectionTimer;
    Vector3 boostDirection;
    bool directionSet = false;

    //[HideInInspector] public bool dealDamage = false;
    //[Tooltip("For how long Player can damage de Destroyables")] [SerializeField] float dealDamageDuration;

    [SerializeField] GameObject circle;

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

   

    void SuperBoost() //Wait(SetDirection) and Boost
    {
        if (dash.IsBoosting == true || shadowDash.isShadowDashing == true) return;


        if (Input.GetButton(gameMng.SuperDash)) // Boost ist abbrechbar, indem der A Knopf losgelassen wird
        {
            if (boostButtonPressedInLastFrame == false)
            {
                allowBoost = true;
            }
            boostButtonPressedInLastFrame = true;


            if (allowBoost == true)
            {
                // X Sekunden, um Richtung zu bestimmen 
                setDirectionTimer += Time.deltaTime;

                if (setDirectionTimer < 1f)
                {
                    Boosting = true;
                    circle.SetActive(true);
                    rb.velocity = new Vector3(0, 0, 0);
                }
                else
                {
                    //Circle.SetActive(false);
                    boostDirection = playerMov.MovementDirection;
                    directionSet = true;
                    circle.SetActive(false);
                }


                // Boost in Richtung
                if (directionSet == true && timerBoost < boostDuration)
                {
                  //  if(dealDamage == false)
                       // StartCoroutine(AllowToDestroyDestroyables());

                    timerBoost += Time.deltaTime;
                    
                    rb.AddForce(boostDirection.normalized * boostForce, ForceMode.Impulse); //*3 zum zeigen
                }
                else
                {
                    Boosting = false;
                }
            }
        }
        else
        {
            boostButtonPressedInLastFrame = false;
            allowBoost = false;

            setDirectionTimer = 0;
            timerBoost = 0;

            directionSet = false;
            Boosting = false;

            circle.SetActive(false);
        }
    }

    /*
    private IEnumerator AllowToDestroyDestroyables()
    {
        dealDamage = true;
        yield return new WaitForSeconds(dealDamageDuration);

        dealDamage = false;

        yield return null;
    }
    */
}

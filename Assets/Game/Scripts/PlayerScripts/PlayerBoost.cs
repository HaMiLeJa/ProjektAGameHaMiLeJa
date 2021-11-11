using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    Rigidbody rb;
    PlayerMovement playerMov;
    ShadowDash shadowDash;
    PlayerStartDash superDash;

    GameManager gameMng;

    [SerializeField] float boostDuration = 0.1f;
    bool boostButtonPressedInLastFrame = false;
    [SerializeField] bool allowBoost = false;
    float timerBoost;
    [SerializeField] float boostForce = 1;
    public bool Boosting;

    float timerSlowDown;
    [SerializeField] float slowDownDuration = 0.1f;
    bool slowedDown = false;
    [SerializeField] float slowDownValue = 0.999f;

    [HideInInspector] public bool dealDamage = false;
    [Tooltip("For how long Player can damage de Destroyables")] [SerializeField] float dealDamageDuration = 1.5f;


    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        playerMov = this.GetComponent<PlayerMovement>();
        shadowDash = this.GetComponent<ShadowDash>();
        superDash = this.GetComponent<PlayerStartDash>();

        gameMng = FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
        Boost();
    }

    void Boost() // Dash: wird langsamer und dann wuuuuush
    {
        if (shadowDash.isShadowDashing == true || superDash.Boosting == true) return;
        if (rb.velocity.x == 0 || rb.velocity.z == 0) return; //kein kleiner Boost am Anfang erlaubt!

        if(Input.GetButton(gameMng.Dash))
        {
            if (boostButtonPressedInLastFrame == false && allowBoost == false)
            {
                boostButtonPressedInLastFrame = true;
                Debug.Log("allowed");
                allowBoost = true;
            }
            
        }
        else
        {
            
        }
        
        if(allowBoost == true)
        {
            Boosting = true; //used to lock the other boosts

            timerSlowDown += Time.deltaTime;

            if (timerSlowDown < slowDownDuration)
            {
                rb.velocity *= 0.9f;
            }
            else
            {
                slowedDown = true;
            }

            if (slowedDown == true & timerBoost < boostDuration)
            {
                if (dealDamage == false)
                    StartCoroutine(AllowToDestroyDestroyables());

                timerBoost += Time.deltaTime;

                rb.AddForce(playerMov.MovementDirection.normalized * boostForce, ForceMode.Impulse);
                //ANMERKUNG: falls Boosten energie verbrauchen soll hier abziehen

                
            }

            if (timerBoost > boostDuration && slowedDown == true)
                allowBoost = false;
        }
        else
        {
            
            Boosting = false;
            boostButtonPressedInLastFrame = false;
            allowBoost = false;
            timerSlowDown = 0;
            timerBoost = 0;
            slowedDown = false;
            Boosting = false;
            
        }

    }

    private IEnumerator AllowToDestroyDestroyables()
    {
        dealDamage = true;
        yield return new WaitForSeconds(dealDamageDuration);

        dealDamage = false;

        yield return null;
    }

    void BoostNotWorking()
    {
        if (Input.GetButton("X"))
        {
            if (boostButtonPressedInLastFrame == false)
            {
                Boosting = true;
                allowBoost = true;
            }

            boostButtonPressedInLastFrame = true;
        }
        else
        {
            boostButtonPressedInLastFrame = false;
        }



        if (allowBoost == true)
        {
            timerSlowDown += Time.deltaTime;

            if (Boosting == true)
            {
                /*
                if (timerSlowDown < slowDownDuration)
                {
                    rb.velocity *= 0.9f;
                }
                else
                {
                    slowedDown = true;
                }
                */

                if (timerBoost < boostDuration) //slowedDown == true && 
                {
                    timerBoost += Time.deltaTime;

                    rb.AddForce(playerMov.MovementDirection.normalized * boostForce, ForceMode.Impulse);
                    //ANMERKUNG: falls Boosten energie verbrauchen soll hier abziehen
                }
                else
                {
                    Boosting = false;
                    allowBoost = false;
                }
            }
            else
            {
                timerSlowDown = 0;
                timerBoost = 0;
                slowedDown = false;
            }

        }


        if (Input.GetButton("X") == false && boostButtonPressedInLastFrame == true && Boosting == false)
        {
            // boostButtonPressedInLastFrame = false;
        }
    }

    void BoostSave() // Dash: wird langsamer und dann wuuuuush
    {
        if (Input.GetButton(gameMng.Dash))
        {
            if (boostButtonPressedInLastFrame == false)
            {
                allowBoost = true;
            }
            boostButtonPressedInLastFrame = true;


            if (allowBoost == true)
            {
                timerSlowDown += Time.deltaTime;

                if (timerSlowDown < slowDownDuration)
                {
                    rb.velocity *= 0.9f;
                }
                else
                {
                    slowedDown = true;
                }

                if (slowedDown == true & timerBoost < boostDuration)
                {
                    if (dealDamage == false)
                        StartCoroutine(AllowToDestroyDestroyables());

                    timerBoost += Time.deltaTime;

                    rb.AddForce(playerMov.MovementDirection.normalized * boostForce, ForceMode.Impulse);
                    //ANMERKUNG: falls Boosten energie verbrauchen soll hier abziehen

                    Boosting = true;
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
            timerSlowDown = 0;
            timerBoost = 0;
            slowedDown = false;
            Boosting = false;
        }


    }
}

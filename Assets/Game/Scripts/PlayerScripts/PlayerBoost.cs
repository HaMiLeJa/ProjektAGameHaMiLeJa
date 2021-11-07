using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    Rigidbody rb;
    EnergyManager energyMng;
    PlayerMovement playerMov;

    [SerializeField] float boostDuration = 0.1f;
    bool boostButtonPressedInLastFrame = false;
    bool allowBoost = true;
    float timerBoost;
    [SerializeField] float boostForce = 1;
    public bool boosting;

    float timerSlowDown;
    [SerializeField] float slowDownDuration;
    bool slowedDown = false;
    [SerializeField] float slowDownValue;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        energyMng = EnergyManager.Instance;
        playerMov = this.GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        Boost();
    }

    void Boost() // Dash: wird langsamer und dann wuuuuush
    {
        if(Input.GetButton("X"))
        {
            if (boostButtonPressedInLastFrame == false)
            {
                allowBoost = true;
            }
            boostButtonPressedInLastFrame = true;


            if (allowBoost == true)
            {
                timerSlowDown += Time.deltaTime;

                if(timerSlowDown < slowDownDuration)
                {
                    rb.velocity *= 0.9f;
                }
                else
                {
                    slowedDown = true;
                }

                if (slowedDown == true & timerBoost < boostDuration)
                {
                    timerBoost += Time.deltaTime;

                    rb.AddForce(playerMov.MovementDirection.normalized * boostForce * energyMng.EnergyBoostValue, ForceMode.Impulse);
                    //ANMERKUNG: falls Boosten energie verbrauchen soll hier abziehen

                    boosting = true;
                }
                else
                {
                    boosting = false;
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
            boosting = false;
        }


    }


    void BoostNotWorking()
    {
        if (Input.GetButton("X"))
        {
            if (boostButtonPressedInLastFrame == false)
            {
                boosting = true;
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

            if (boosting == true)
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

                    rb.AddForce(playerMov.MovementDirection.normalized * boostForce * energyMng.EnergyBoostValue, ForceMode.Impulse);
                    //ANMERKUNG: falls Boosten energie verbrauchen soll hier abziehen
                }
                else
                {
                    boosting = false;
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


        if (Input.GetButton("X") == false && boostButtonPressedInLastFrame == true && boosting == false)
        {
            // boostButtonPressedInLastFrame = false;
        }
    }
    
}

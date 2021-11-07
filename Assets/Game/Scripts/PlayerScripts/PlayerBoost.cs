using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    Rigidbody rb;
    EnergyManager energyMng;
    PlayerMovement playerMov;

    // void basic boos
    [SerializeField] float boostDuration = 0.1f;
    bool boostButtonPressedInLastFrame = false;
    bool allowBoost = true;
    float timerBoost;
    [SerializeField] float boostForce = 1;
    public bool boosting;

    // void Basic Jump
    [SerializeField] float forceJump = 50;
    [SerializeField] float jumpDuration = 0.1f;
    float timerJump;
    bool jumpButtonPressedInLastFrame = false;
    bool allowJump = false;

    Vector3 boostDirection;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        energyMng = EnergyManager.Instance;
        playerMov = this.GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        //BasicBoost();
        Boost();
    }

    void Boost() // Dash: wird langsamer und dann wuuuuush
    {

    }

    void BasicBoost()
    {
        if (Input.GetButton("X"))
        {

            if (boostButtonPressedInLastFrame == false)
            {
                allowBoost = true;
            }

            boostButtonPressedInLastFrame = true;

            if (allowBoost == true & timerBoost < boostDuration)
            {
                timerBoost += Time.deltaTime;

                rb.AddForce((playerMov.MovementDirection.normalized * boostForce * energyMng.EnergyBoostValue) + (Vector3.up * forceJump), ForceMode.Impulse);
                //ANMERKUNG: falls Boosten energie verbrauchen soll hier abziehen

                boosting = true;
            }
            else
            {
                boosting = false;
            }
        }
        else
        {
            timerBoost = 0;
            boostButtonPressedInLastFrame = false;
            allowBoost = false;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperBoost : MonoBehaviour
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

    [SerializeField] float setDirectionTimer;
    Vector3 boostDirection;
    bool directionSet = false;


    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        energyMng = EnergyManager.Instance;
        playerMov = this.GetComponent<PlayerMovement>();
    }

    
    void FixedUpdate()
    {
        SuperBoost();
    }

    void SuperBoost() //Wait(SetDirection) and Boost
    {
        //währenddesen normale Movement sperren (?) und verringerung der Velocity verhindern
        // oder evt Player anhalten lassen

        if (Input.GetButton("A")) // Boost ist abbrechbar, indem der A Knopf losgelassen wird
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
                    rb.velocity = new Vector3(0, 0, 0);
                }
                else
                {
                    boostDirection = playerMov.movementDirection;
                    directionSet = true;
                }


                // Boost in Richtung
                if (directionSet == true && timerBoost < boostDuration)
                {
                    
                       
                    timerBoost += Time.deltaTime;
                    boosting = true;
                    rb.AddForce(boostDirection.normalized * boostForce * energyMng.EnergyBoostValue * 3, ForceMode.Impulse); //*3 zum zeigen
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

            setDirectionTimer = 0;
            timerBoost = 0;

            directionSet = false;
            boosting = false;
        }
    }
}

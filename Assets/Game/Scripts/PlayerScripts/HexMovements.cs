using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMovements : MonoBehaviour
{

    #region Inspector
    [HideInInspector] public Rigidbody rb;
    GameManager gameMng;


    [HideInInspector] public bool OnBoostForwardHex = false;
    [HideInInspector] public float CurrentHexFowardForce;
    [HideInInspector] public Vector3 ForwardDirection;

    [HideInInspector] public bool OnChangeDirectionHex = false;

    [HideInInspector] public bool OnBoostInDirectionHex = false;
    [HideInInspector] public float CurrentHexInDirectionForce;
    [HideInInspector] public Vector3 HexInDirectionDirection;
    //public float currentHexChangeDirectionForce;

    [HideInInspector] public bool OnTrampolinHex = false;
    [HideInInspector] public float CurrentTrampolinForce;

    [HideInInspector] public bool OnSlowDownHex = false;

    [HideInInspector] public float TrampolinTimer;
    [HideInInspector] public float BoostInDirectionTimer;
    [HideInInspector] public float BoostForwardTimer;
    [HideInInspector] public float SlowDownTimer;

    public bool rebounded = false;

    #endregion
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        gameMng = GameManager.Instance;
    }


    void FixedUpdate()
    {
        HexEffects();
    }


    void HexEffects()
    {
        if (gameMng.AllowHexEffects == false) return;

        // BOOST FORWARD
        if (OnBoostForwardHex == true && BoostForwardTimer < 0.4f)
        {
            BoostForwardTimer += Time.fixedDeltaTime;
            ForwardDirection = rb.velocity.normalized;
            rb.AddForce(ForwardDirection * CurrentHexFowardForce * 500 * Time.fixedDeltaTime);
        }
        else if (OnBoostForwardHex == true && BoostForwardTimer > 0.4f)
        {
            rb.velocity = rb.velocity / 2;
            OnBoostForwardHex = false;
        }
        else
        {
            BoostForwardTimer = 0;
        }



        //  BOOST IN DIRECTION
        if (OnBoostInDirectionHex == true && BoostInDirectionTimer < 0.3f)
        {
            //  Debug.Log("HexInDirection");
            rb.AddForce(HexInDirectionDirection * CurrentHexInDirectionForce * 500 * Time.fixedDeltaTime);
            BoostInDirectionTimer += Time.fixedDeltaTime;

            //CurrentHexInDirectionForce = CurrentHexInDirectionForce * 0.99f;
        }
        else
        {
            BoostInDirectionTimer = 0;
            OnBoostInDirectionHex = false;
        }



        // SLOW DOWN
        if (OnSlowDownHex == true && SlowDownTimer < 0.4f)
        {
            SlowDownTimer += Time.fixedDeltaTime;
            rb.velocity *= 0.9f;
        }
        else
        {
            SlowDownTimer = 0;
            OnSlowDownHex = false;
        }


        /*  // Change Direction
         if(OnChangeDirectionHex == true)
         {
             rb.AddForce(rb.velocity.normalized * 20 * Time.fixedDeltaTime); //*currentHexChangeDirectionForce 
         }
         */


        // TRAMPOLIN
        if (OnTrampolinHex == true && TrampolinTimer < 0.2)
        {
            TrampolinTimer += Time.fixedDeltaTime;
            rb.AddForce(Vector3.up * CurrentTrampolinForce * 10 * Time.fixedDeltaTime, ForceMode.Impulse); //CurrentTrampolinForce
        }
        else
        {
            OnTrampolinHex = false;
            TrampolinTimer = 0;
            rebounded = false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperBoost : MonoBehaviour
{
    Rigidbody rb;
    EnergyManager energyMng;
    PlayerMovement playerMov;
    GameManager gameMng;

    [SerializeField] float boostDuration = 0.1f;
    bool boostButtonPressedInLastFrame = false;
    bool allowBoost = false;
    float timerBoost;
    [SerializeField] float boostForce = 1;
    public bool Boosting;

    [SerializeField] float setDirectionTimer;
    Vector3 boostDirection;
    bool directionSet = false;

    [HideInInspector] public bool dealDamage = false;
    [Tooltip("For how long Player can damage de Destroyables")] [SerializeField] float dealDamageDuration;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        energyMng = FindObjectOfType<EnergyManager>();
        playerMov = this.GetComponent<PlayerMovement>();

        gameMng = FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
        SuperBoost();
    }

   

    void SuperBoost() //Wait(SetDirection) and Boost
    {

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
                    rb.velocity = new Vector3(0, 0, 0);
                }
                else
                {
                    boostDirection = playerMov.MovementDirection;
                    directionSet = true;
                }


                // Boost in Richtung
                if (directionSet == true && timerBoost < boostDuration)
                {
                    if(dealDamage == false)
                        StartCoroutine(AllowToDestroyDestroyables());

                    timerBoost += Time.deltaTime;
                    Boosting = true;
                    rb.AddForce(boostDirection.normalized * boostForce * energyMng.EnergyBoostValue * 3, ForceMode.Impulse); //*3 zum zeigen
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
        }
    }

    private IEnumerator AllowToDestroyDestroyables()
    {
        dealDamage = true;
        Debug.Log(dealDamage);
        yield return new WaitForSeconds(dealDamageDuration);

        dealDamage = false;
        Debug.Log(dealDamage);

        yield return null;
    }
}

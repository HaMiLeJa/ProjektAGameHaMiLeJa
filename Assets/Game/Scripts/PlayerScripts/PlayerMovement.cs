using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    EnergyManager energyMng;

    //Movement
    Vector3 strafeMovement;
    Vector3 forwardMovement;
    public float StandardMovementSpeed = 3;
    Vector3 movement; //Umbennen
    [HideInInspector] public Vector3 MovementDirection;

    // void ControlVelocity
    public float SlowDownMultiplicator = 0.99f;

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

    // void GroundCheck und Gravity
    [SerializeField] bool OnGround = false;
    float distanceToGround;

    [SerializeField] [Tooltip("Turn off if you dont want to loose energy")] bool reduceEnergy = true;
    [Tooltip("Just for Debug use")] public Vector3 Velocity; //Debug

    private void Awake()
    {
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        energyMng = EnergyManager.Instance;
    }


    void FixedUpdate()
    {
        Velocity = rb.velocity; //Debug

        GroundCheck();

        Movement();

        if (reduceEnergy == true)
            ControlVelocity();

        //BasicJump();
        //BasicBoost();

    }

    void Movement()
    {
        //Bewegung
        strafeMovement = transform.right * Input.GetAxis("Horizontal");
        forwardMovement = transform.forward * Input.GetAxis("Vertical");

        MovementDirection =
            forwardMovement +
            strafeMovement; //Richtung, die gerade durch Controller angegeben wird inkl "Eigenen Geschwindigkeit" abhängig von der Stärke der Neigung der Joysticks
        movement = MovementDirection * Time.deltaTime * StandardMovementSpeed * energyMng.EnergyMovementValue;
        // if (ShadowDash.currentShadowDashForce != 0)
        // {
        //     movement *= ShadowDash.currentShadowDashForce;
        //     ShadowDash.mr.enabled = true;
        // }

        rb.velocity = (rb.velocity + movement);
    }




    void ControlVelocity()
    {
        //Stopp
        if (Input.GetButton("Y"))
        {
            rb.velocity = new Vector3(0.001f, 0.001f, 0.001f);
        }




        if (strafeMovement == Vector3.zero && forwardMovement == Vector3.zero || Input.GetButton("Y")) //Wenn kein Input    
        {
            // Abnahme Velocity und Energie, wenn kein Input erfolgt automatisch über das Physicsystem
            // Abnahme Energy
            energyMng.ReduceEnergy();
        }

    }

    void BasicJump()
    {
        if (Input.GetButton("B"))
        {

            if (jumpButtonPressedInLastFrame == false) //OnGround == true &&
            {
                allowJump = true;
            }

            jumpButtonPressedInLastFrame = true;


            if (allowJump == true && timerJump < jumpDuration)
            {
                timerJump += Time.deltaTime;
                rb.AddForce(Vector3.up * forceJump);
            }

        }
        else
        {
            timerJump = 0;
            jumpButtonPressedInLastFrame = false;
            allowJump = false;
        }
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

                rb.AddForce(MovementDirection.normalized * boostForce * energyMng.EnergyBoostValue, ForceMode.Impulse);
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

    

   

    void GroundCheck()
    {
        //GroundControl

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10, LayerMask.GetMask("Hex")))
        {
            distanceToGround = hit.distance;
            //Debug.Log(distanceToGround);

            if (distanceToGround <= 1.6f) //Wert müsste evt über den Spielverlauf hin angepasst werden
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Collision with wall");

            energyMng.Energy += collision.gameObject.GetComponent<EnergyGenerator>().GeneratedEnergy;

            collision.gameObject.GetComponent<EnergyGenerator>().GeneratedEnergy = 0;

        }
    }
    
}

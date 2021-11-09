using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    EnergyManager energyMng;
    GameManager gameMng;

    //Movement
    Vector3 strafeMovement;
    Vector3 forwardMovement;
    public float StandardMovementSpeed = 3;
    Vector3 movement; //Umbennen
    [HideInInspector] public Vector3 MovementDirection;

    // void ControlVelocity
    public float SlowDownMultiplicator = 0.99f;

    /*
    // void basic boost
    [SerializeField] float boostDuration = 0.1f;
    bool boostButtonPressedInLastFrame = false;
    bool allowBoost = false;
    float timerBoost;
    [SerializeField] float boostForce = 1;
    public bool boosting;
    */

    // void Basic Jump
    bool jumping = false;
    [SerializeField] float forceJump = 50;
    [SerializeField] float jumpDuration = 0.1f;
    float timerJump;
    bool jumpButtonPressedInLastFrame = false;
    bool allowJump = false;

    // void GroundCheck und Gravity
    public bool OnGround = false;
    float distanceToGround;
    [SerializeField] float fallDownSpeed = 1;

    [SerializeField] [Tooltip("Turn off if you dont want to loose energy")] bool reduceEnergy = true;
    [Tooltip("Just for Debug use")] public Vector3 Velocity; //Debug

    ShadowDash shadowDash;
    
    // Trampolin
    public bool rebounded = false;

    private void Awake()
    {
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        energyMng = FindObjectOfType<EnergyManager>();
        gameMng = FindObjectOfType<GameManager>();
        shadowDash = this.GetComponent<ShadowDash>();
    }


    void FixedUpdate()
    {
        Velocity = rb.velocity; //Debug

        GroundCheck();

        //Movement();

        MovementAlternative();

        if (reduceEnergy == true)
            ControlVelocity();


        //Stopp
        if (Input.GetButton(gameMng.Stop))
        {
            rb.velocity = rb.velocity * 0.9f;
        }

        BasicJump();

        HightControl();
    }

    void Movement()
    {
        //Bewegung
        strafeMovement = transform.right * Input.GetAxis("Horizontal");
        forwardMovement = transform.forward * Input.GetAxis("Vertical");

        MovementDirection = forwardMovement + strafeMovement; //Richtung, die gerade durch Controller angegeben wird inkl "Eigenen Geschwindigkeit" abhängig von der Stärke der Neigung der Joysticks
        movement = MovementDirection * Time.deltaTime * StandardMovementSpeed * energyMng.EnergyMovementValue;

        /*
        if (shadowDash.currentShadowDashForce != 0)
        {
            movement *= shadowDash.currentShadowDashForce;
            shadowDash.mr.enabled = true;
        }
        */

        if (shadowDash.currentShadowDashForce != 0f)
        {

           rb.velocity = (rb.velocity + movement * shadowDash.currentShadowDashForce);
        }
        else
            rb.velocity = (rb.velocity + movement);
    }

    void MovementAlternative()
    {
        //Bewegung

        
        
            strafeMovement = transform.right * Input.GetAxis("Horizontal");
            forwardMovement = transform.forward * Input.GetAxis("Vertical");

            MovementDirection = forwardMovement + strafeMovement; //Richtung, die gerade durch Controller angegeben wird inkl "Eigenen Geschwindigkeit" abhängig von der Stärke der Neigung der Joysticks
            movement = MovementDirection * Time.deltaTime * StandardMovementSpeed * energyMng.EnergyMovementValue;
        


        if (shadowDash.currentShadowDashForce != 0f)
        {
            rb.AddForce(movement.normalized * shadowDash.currentShadowDashForce * 5);
            //rb.velocity = rb.velocity * shadowDash.currentShadowDashForce *0.5f;
        }
        else
        {
            rb.AddForce(movement);
        }
    }


    void ControlVelocity()
    {
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
            if (OnGround == true && jumpButtonPressedInLastFrame == false) //OnGround == true &&
            {
                allowJump = true;
            }

            jumpButtonPressedInLastFrame = true;


            if (allowJump == true && timerJump < jumpDuration)
            {
                jumping = true;
                timerJump += Time.deltaTime;
                rb.AddForce(this.transform.up * forceJump, ForceMode.Impulse);
            }

        }
        else
        {
            timerJump = 0;
            jumpButtonPressedInLastFrame = false;
            allowJump = false;
            jumping = false;
        }
    }

    void HightControl()
    {
        if(this.rb.position.y >= 60)
        {
           rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -100f, -0.01f ), rb.velocity.y);

            //rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, -1 , 0.1f), rb.velocity.y);
        }

    }


    #region BasicBoost code (Not Used here)
    /*
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
    */

    #endregion


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

        // Idee: die Addforce auch in akutelle bewegungsrichtung?
        

        
        if(OnGround == false && jumping == false) //&&rebounding == false
        {
            //Vector3 direction = new Vector3(rb.velocity.x, -1, rb.velocity.z);
            //direction = direction.normalized;

            rb.AddForce(rb.velocity.normalized + Vector3.down * fallDownSpeed);
        }
        if(OnGround == false && rebounded == false)
        {
            rb.AddForce(rb.velocity.normalized + Vector3.down * fallDownSpeed);
        }
        


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnergyGenerator")
        {
            energyMng.Energy += collision.gameObject.GetComponent<EnergyGenerator>().GeneratedEnergy;

            collision.gameObject.GetComponent<EnergyGenerator>().GeneratedEnergy = 0;

        }
    }
    
}

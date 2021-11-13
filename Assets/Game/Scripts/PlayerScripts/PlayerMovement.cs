using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    GameManager gameMng;

    public bool OnBoostForwardHex;
    public float currentHexForce;

    [Tooltip("Speed with which the player can influence the movement")]
    public float StandardMovementSpeed = 5;

    [HideInInspector]public Vector3 MovementDirection;

    // void ControlVelocity
    public float SlowDownMultiplicator = 0.99f;

    
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

    [Tooltip("Just for Debug use")] public Vector3 Velocity; //Debug

    ShadowDash shadowDash;
    
    // Trampolin
    public bool rebounded = false;

    [HideInInspector] public bool InNoInputZone = false;

    private void Awake()
    {
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        gameMng = FindObjectOfType<GameManager>();
        shadowDash = this.GetComponent<ShadowDash>();
    }


    void FixedUpdate()
    {
        Velocity = rb.velocity; //Debug

        GroundCheck();



        //CorrectMovement();

        TestCorrectMovement();

        BasicJump();

        HightControl();
    }

   

    

    void CorrectMovement()
    {
        //Bewegung
        Vector3 strafeMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");

        MovementDirection = forwardMovement + strafeMovement; //Richtung, die gerade durch Controller angegeben wird inkl "Eigenen Geschwindigkeit" abhängig von der Stärke der Neigung der Joysticks
        Vector3 movement = MovementDirection * Time.deltaTime * StandardMovementSpeed;


        if (shadowDash.currentShadowDashForce != 0f)
        {
            rb.AddForce(movement.normalized * shadowDash.currentShadowDashForce * 5);

        }
        else if (Velocity.x != 0 && Velocity.z != 0)
        {
            float velocityPower = Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.z);

            rb.velocity = (rb.velocity + movement * velocityPower/4);  // sollte sich bei hoher Geschwindigkeit verstärken ; Wert von ca 5



            //rb.AddForce(movement, ForceMode.Force);
        }
       

    }


    void TestCorrectMovement()
    {
        //Bewegung
        Vector3 strafeMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");

        MovementDirection = forwardMovement + strafeMovement; //Richtung, die gerade durch Controller angegeben wird inkl "Eigenen Geschwindigkeit" abhängig von der Stärke der Neigung der Joysticks
        Vector3 movement = MovementDirection * Time.deltaTime * StandardMovementSpeed;


        if (shadowDash.currentShadowDashForce != 0f)
        {
            rb.AddForce(movement.normalized * shadowDash.currentShadowDashForce * 5);

        }
        else if (OnBoostForwardHex == true)
        {
            rb.AddForce(movement.normalized * currentHexForce * 5);
        }
        
        else
        {
            float velocityPower = Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.z);

            rb.velocity = (rb.velocity + movement);  // sollte sich bei hoher Geschwindigkeit verstärken ; Wert von ca 5



            //rb.AddForce(movement, ForceMode.Force);
        }

    }

    void Bounce()
    {

    }

    void ControlVelocity()
    {
        
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
     * 
    // void basic boost
    [SerializeField] float boostDuration = 0.1f;
    bool boostButtonPressedInLastFrame = false;
    bool allowBoost = false;
    float timerBoost;
    [SerializeField] float boostForce = 1;
    public bool boosting;
   
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
        
        /*
        if (collision.gameObject.tag == "Wall")
        {
            float bouncyness = 0.5f;

            rb.velocity = new Vector3(-rb.velocity.x * bouncyness, rb.velocity.y, -rb.velocity.z * bouncyness);

        }
        */
        
    }


}

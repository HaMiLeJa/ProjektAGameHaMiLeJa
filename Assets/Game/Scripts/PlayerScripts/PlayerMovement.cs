using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Video;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    #region Inspector
    [HideInInspector] public Rigidbody rb;
    GameManager gameMng;
    MovementCurves movCurves;

    [HideInInspector] public bool OnBoostForwardHex;
    [HideInInspector] public float CurrentHexFowardForce;
    [HideInInspector] public bool OnChangeDirectionHex;
    [HideInInspector] public bool OnBoostInDirectionHex;
    [HideInInspector] public float CurrentHexInDirectionForce;
    [HideInInspector] public Vector3 HexInDirectionDirection;
    //public float currentHexChangeDirectionForce;
    

    [Tooltip("Speed with which the player can influence the movement")]
   // public float StandardMovementSpeed = 10;

    [HideInInspector]public Vector3 MovementDirection;

    // void ControlVelocity
    public float SlowDownMultiplicator = 0.99f;

    
    // void Basic Jump
    bool jumping = false;
    [SerializeField] float forceJump = 3;
    float jumpDuration = 0.1f;
    float timerJump;
    bool jumpButtonPressedInLastFrame = false;
    bool allowJump = false;

    [Space]
    // void GroundCheck und Gravity
    public bool OnGround = false;
    float distanceToGround;
    [SerializeField] float fallDownSpeed = 20;
    [Range(0, 10)]
    [SerializeField] float distance = 1.6f;
    [SerializeField] LayerMask hexMask;
    [SerializeField] LayerMask worldMask;
    [SerializeField] LayerMask levelMask;

    public Vector3 Velocity; //Debug
    
    [Space]
    [Header("Max Velocity")]
    [Tooltip("Was könnte der Spieler maximal erreichen erreichen")] [SerializeField] private float maxSpeed = 500;
    [Tooltip("Ab wann wird die geschwindigkeit begrenzt (ein klein wenig Kontrollverlust)")] 
    [SerializeField] private int maxSpeedLimitStartClamping= 420;
    [Space]
    
    ShadowDash shadowDash;
    PlayerBoost playerBoost;

    [Space]
    // Trampolin
    public bool rebounded = false;

    //  [HideInInspector] public bool InNoInputZone = false;

    [SerializeField] float totalVelocity;
    [SerializeField] float velocityInfluence = 0.1f;
    [Space]

    // Hight Control
    private float highControlForce = 5;
    [Tooltip("Choose max Hight")] [Range (10, 60)] [SerializeField] float maxHight = 30;

    [Space]
    [SerializeField] AudioSource jumpAudioSource;
    [SerializeField] AudioSource basedrumAudioSource;
    AudioManager audManager;
    

    #endregion

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        gameMng = GameManager.Instance;
        shadowDash = this.GetComponent<ShadowDash>();
        playerBoost = this.GetComponent<PlayerBoost>();
        movCurves = this.GetComponent<MovementCurves>();
        audManager = AudioManager.Instance;
    }


    void FixedUpdate()
    {
        MaxVelocity();
        Velocity = rb.velocity; //Debug

        ControlVelocity();

        GroundCheck();
        Gravity();
        HightControl();
        totalVelocity = Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.y) + Mathf.Abs(Velocity.z);


        if (gameMng.AllowMovement == false) return;

        CorrectMovement();
        BasicJump();
    }

    public Vector3 ForwardDirection;
    
    
    void MaxVelocity()
    {
        if ((math.abs(rb.velocity.x) + math.abs(rb.velocity.z)) > maxSpeedLimitStartClamping)
        {
            Debug.Log("Enter limit velocity");
            float xVelocityMax = Mathf.Min(Mathf.Abs(rb.velocity.x), maxSpeed) * Mathf.Sign(rb.velocity.x);
            float zVelocityMax = Mathf.Min(Mathf.Abs(rb.velocity.z), maxSpeed) * Mathf.Sign(rb.velocity.z);

            rb.velocity = new Vector3(xVelocityMax, rb.velocity.y, zVelocityMax);
        }
        
    }
    void CorrectMovement()
    {
        //Bewegung
        Vector3 strafeMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");

        MovementDirection = forwardMovement + strafeMovement; //Richtung, die gerade durch Controller angegeben wird inkl "Eigenen Geschwindigkeit" abhängig von der Stärke der Neigung der Joysticks
        //Vector3 movement = MovementDirection * Time.deltaTime * StandardMovementSpeed;


        if (shadowDash.currentShadowDashForce != 0f)
        {
            rb.AddForce(MovementDirection.normalized * shadowDash.currentShadowDashForce * 5 * Time.deltaTime);

        }
        if(playerBoost.currentBoostforce != 0f)
        {
            rb.AddForce(MovementDirection.normalized * playerBoost.currentBoostforce * 100 * Time.deltaTime);
        }

        if (OnBoostForwardHex == true)
        {
            
            rb.AddForce(ForwardDirection.normalized * CurrentHexFowardForce * 200 * Time.deltaTime);
        }
        
        if (OnBoostInDirectionHex == true)
        {
            rb.AddForce(HexInDirectionDirection * CurrentHexInDirectionForce * Time.deltaTime);
        }
        
        
        if(OnChangeDirectionHex == true)
        {
            rb.AddForce(rb.velocity.normalized * 20 * Time.deltaTime); //*currentHexChangeDirectionForce 


        }
        
        
        if(OnGround == false)
        {
            totalVelocity = Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.z);
            float velocityPower = totalVelocity * velocityInfluence/2 * Time.deltaTime;

            rb.velocity = (rb.velocity + (MovementDirection * velocityPower));
        }

        /*
        else
        {
            if (rebounded == true) //eig unnötig
                return;

            totalVelocity = Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.z);
            float velocityPower = totalVelocity * velociyInfluence;

            rb.velocity = (rb.velocity + (movement * velocityPower));  // sollte sich bei hoher Geschwindigkeit verstärken ; Wert von ca 5



            //rb.AddForce(movement, ForceMode.Force);
        }
        */

    }

    void Bounce()
    {

    }

    void ControlVelocity()
    {
        if(totalVelocity > 300) //von total Velocity abhängig machen
        {
          rb.velocity = new Vector3(rb.velocity.x * 1.1f, rb.velocity.y, rb.velocity.z * 1.0001f * Time.deltaTime);
        }

        
        if(Velocity == Vector3.zero)
        {
            if (basedrumAudioSource.isPlaying == false && audManager.allowAudio == true)
                basedrumAudioSource.Stop();
        }
        else
        {
            if (basedrumAudioSource.isPlaying == false && audManager.allowAudio == true)
                basedrumAudioSource.Play();
        }
        
        

    }

    void BasicJump()
    {
        if (Input.GetButton(gameMng.Jump))
        {
            if (OnGround == true && jumpButtonPressedInLastFrame == false) //OnGround == true &&
            {
                allowJump = true;

                if (jumpAudioSource.isPlaying == false && audManager.allowAudio == true)
                    jumpAudioSource.Play();
            }

            jumpButtonPressedInLastFrame = true;


            if (allowJump == true && timerJump < jumpDuration)
            {
                jumping = true;
                timerJump += Time.deltaTime;
                rb.AddForce(this.transform.up * forceJump * Time.deltaTime, ForceMode.Impulse);
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
        if(this.rb.position.y >= maxHight)
        {
            rb.AddForce(Vector3.down * highControlForce * rb.transform.position.y * Time.deltaTime);

           //rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -100f, -0.01f ), rb.velocity.y);

            //Idee: stattdessen eine force nach unten? das sollte flüssigeren übergang machen


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
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10, hexMask)) //LayerMask.GetMask("Hex")
        {
            distanceToGround = hit.distance;

            if (distanceToGround <= distance) //Wert müsste evt über den Spielverlauf hin angepasst werden 1.6
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }
        }

        
        else if(Physics.Raycast(transform.position, -transform.up, out hit, 10, worldMask)) //LayerMask.GetMask("World")
        {
            distanceToGround = hit.distance;

            if (distanceToGround <= 1.6f) //Wert müsste evt über den Spielverlauf hin angepasst werden
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }
        }
        else if(Physics.Raycast(transform.position, -transform.up, out hit, 10, levelMask)) //LayerMask.GetMask("Level")
        {
            distanceToGround = hit.distance;

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

   void Gravity()
    {
        //if (movCurves.OnCurve == true) return;

        if (OnGround == false && jumping == false) //&&rebounding == false
        {
            //Vector3 direction = new Vector3(rb.velocity.x, -1, rb.velocity.z);
            //direction = direction.normalized;

            rb.AddForce((rb.velocity.normalized + Vector3.down) * fallDownSpeed * Time.deltaTime);
        }
        else if (OnGround == false && rebounded == false) //Trampolin
        {
            rb.AddForce((rb.velocity.normalized + Vector3.down) * fallDownSpeed * Time.deltaTime);
        }
    }

    //CollectEnergy
    /*
      private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnergyGenerator")
        {
            energyMng.Energy += collision.gameObject.GetComponent<EnergyGenerator>().GeneratedEnergy;

            collision.gameObject.GetComponent<EnergyGenerator>().GeneratedEnergy = 0;

        }
    }
    */
}

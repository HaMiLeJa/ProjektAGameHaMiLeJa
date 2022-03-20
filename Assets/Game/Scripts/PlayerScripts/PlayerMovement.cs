using UnityEngine;
using Unity.Mathematics;
public class PlayerMovement : MonoBehaviour
{
    #region Inspector
    [HideInInspector] public Rigidbody rb;
    GameManager gameMng;
    HexMovements hexMov;
    //[Tooltip("Speed with which the player can influence the movement")] public float StandardMovementSpeed = 10;
   [HideInInspector]public Vector3 MovementDirection;
   public float constspeed = 60;
    [SerializeField] private bool constSpeedAllowed = true;
    [HideInInspector] public float originalContspeed;
    bool jumping;// void Basic Jump
    [SerializeField] float forceJump = 3;
    float jumpDuration = 0.1f, timerJump;
    bool jumpButtonPressedInLastFrame, allowJump;
    [Space]    // void GroundCheck und Gravity
    public bool OnGround;
    public static float distanceToGround;
    [SerializeField] float fallDownSpeed = 20;
    [Range(0, 10)]
    [SerializeField] float distance = 1.6f;
    [SerializeField] LayerMask hexMask;
    [SerializeField] LayerMask worldMask; //Disabled during shadowdash
    [SerializeField] LayerMask levelMask;
    public bool DisableGravity;
    public Vector3 Velocity; //Debug
    [Space]
    [Header("Max Velocity - Spitzen abfangen")]
    [Tooltip("Was könnte der Spieler maximal erreichen erreichen")]  private float maxSpeed = 700;
    [Tooltip("Ab wann wird die geschwindigkeit begrenzt (ein klein wenig Kontrollverlust)")] [SerializeField] private int maxSpeedLimitStartClamping= 600;
    [Space]
    public float TotalVelocity;
    [SerializeField] float velocityInfluence = 0.1f;
    [Space]
    private float highControlForce = 5;    // Hight Control
    [Tooltip("Choose max Hight")] [Range (10, 60)] [SerializeField] float maxHight = 30;
    [Space]
    [SerializeField] AudioSource jumpAudioSource;
    #endregion
    void Start()
    {
        rb = ReferenceLibary.RigidbodyPl;
        gameMng = ReferenceLibary.GameMng;
        hexMov = ReferenceLibary.HexMov;
        originalContspeed = constspeed;
    }
    void FixedUpdate()
    {
        if (GameStateManager.gameState == GameStateManager.GameState.Start) return;
        MinVelocity(); MaxVelocity();
        Velocity = rb.velocity; //Debug
        ControlVelocity(); GroundCheck();
        Gravity(); HightControl();
        TotalVelocity = Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.y) + Mathf.Abs(Velocity.z);
        if (!gameMng.AllowMovement) return;
        CalculateMovementDirection();
        //BasicJump();
    }
    void MaxVelocity()
    {
        if ((math.abs(rb.velocity.x) + math.abs(rb.velocity.z)) > maxSpeedLimitStartClamping)
        {
            Debug.Log("Enter limit velocity");
            float xVelocityMax = Mathf.Min(Mathf.Abs(rb.velocity.x), maxSpeed) * Mathf.Sign(rb.velocity.x),
                   zVelocityMax = Mathf.Min(Mathf.Abs(rb.velocity.z), maxSpeed) * Mathf.Sign(rb.velocity.z);
            rb.velocity = new Vector3(xVelocityMax, rb.velocity.y, zVelocityMax);
        }
    }
    void MinVelocity()
    {
        if (!gameMng.AllowMovement) return;
        float horizontalInput = Input.GetAxis("Horizontal"), 
               verticalInput = Input.GetAxis("Vertical");
        if (math.abs(horizontalInput) > 0.3f || math.abs(verticalInput) > 0.3f)
            rb.AddForce(MovementDirection.normalized * 30f);
        if (ReferenceLibary.Dash.IsBoosting|| ReferenceLibary.SuperDash.isSuperDashing || ReferenceLibary.ShadowDashPl.isShadowDashing) return;
        if (constSpeedAllowed && Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z) < constspeed)
        {
            var normalizeSpeed  = (rb.velocity.normalized);
            rb.velocity = new Vector3(normalizeSpeed.x * constspeed , rb.velocity.y, normalizeSpeed.z * constspeed);
        }
    }
    Vector3 strafeMovement, forwardMovement;
    void CalculateMovementDirection()
    {     //Bewegung
        strafeMovement = transform.right * Input.GetAxis("Horizontal");
         forwardMovement = transform.forward * Input.GetAxis("Vertical");
         MovementDirection = forwardMovement + strafeMovement; //Richtung, die gerade durch Controller angegeben wird inkl "Eigenen Geschwindigkeit" abhängig von der Stärke der Neigung der Joysticks
        //Vector3 movement = MovementDirection * Time.deltaTime * StandardMovementSpeed;
        //  if (shadowDash.currentShadowDashForce != 0f) rb.AddForce(MovementDirection.normalized * shadowDash.currentShadowDashForce * 50 * Time.fixedDeltaTime);
        if (OnGround == false)
        {
            TotalVelocity = Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.z);
            float velocityPower = TotalVelocity * velocityInfluence/2 * Time.deltaTime;
            rb.velocity = (rb.velocity + (MovementDirection * velocityPower));
        }
        /*  else
         {
             if (rebounded) return; //eig unnötig
             totalVelocity = Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.z);
             float velocityPower = totalVelocity * velociyInfluence;
             rb.velocity = (rb.velocity + (movement * velocityPower));  // sollte sich bei hoher Geschwindigkeit verstärken ; Wert von ca 5
             //rb.AddForce(movement, ForceMode.Force);
         }*/
    }
    void ControlVelocity()
    {
        if(TotalVelocity > 300) //von total Velocity abhängig machen
            rb.velocity = new Vector3(rb.velocity.x * 1.1f, rb.velocity.y, rb.velocity.z * 1.0001f * Time.deltaTime);
    }
    void BasicJump()
    {
        if (Input.GetButton(gameMng.Jump))
        {
            if (OnGround == true && jumpButtonPressedInLastFrame == false) //OnGround == true &&
            {
                allowJump = true;
                if (!jumpAudioSource.isPlaying) jumpAudioSource.Play();
            }
            jumpButtonPressedInLastFrame = true;
            if (allowJump && timerJump < jumpDuration)
            {
                jumping = true;
                timerJump += Time.deltaTime;
                rb.AddForce(this.transform.up * forceJump * Time.fixedDeltaTime, ForceMode.Impulse);
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
        if(rb.position.y >= maxHight) rb.AddForce(Vector3.down * highControlForce * rb.transform.position.y * Time.deltaTime);
            //rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -100f, -0.01f ), rb.velocity.y);
           //Idee: stattdessen eine force nach unten? das sollte flüssigeren übergang machen
            //rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, -1 , 0.1f), rb.velocity.y);
    }
    void GroundCheck()
    {   //GroundControl
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10, hexMask)) //LayerMask.GetMask("Hex")
        {
            distanceToGround = hit.distance;
            if (distanceToGround <= distance)OnGround = true;//Wert müsste evt über den Spielverlauf hin angepasst werden 1.6
            else OnGround = false;
        }
        else if(Physics.Raycast(transform.position, -transform.up, out hit, 10, worldMask)) //LayerMask.GetMask("World")
        {
            distanceToGround = hit.distance;
            if (distanceToGround <= 1.6f) OnGround = true; //Wert müsste evt über den Spielverlauf hin angepasst werden
            else OnGround = false;
        }
        else if(Physics.Raycast(transform.position, -transform.up, out hit, 10, levelMask)) //LayerMask.GetMask("Level")
        {
            distanceToGround = hit.distance;
            if (distanceToGround <= 1.6f) OnGround = true; //Wert müsste evt über den Spielverlauf hin angepasst werden
            else OnGround = false;
        }
    }
    void Gravity()
   {
        if (hexMov.OnTrampolinHex) return;
        if (DisableGravity == true) return;
        if (OnGround == false && jumping == false) //&&rebounding == false
            rb.AddForce((rb.velocity.normalized + Vector3.down) * fallDownSpeed, ForceMode.Acceleration);
        else if (OnGround == false && hexMov.rebounded == false) //Trampolin
            rb.AddForce((rb.velocity.normalized + Vector3.down) * fallDownSpeed, ForceMode.Acceleration);
   }
   // private void OnCollisionEnter(Collision collision)
     // {
          //CollectEnergy
          /*
              if (collision.gameObject.tag == "EnergyGenerator")
              {
                  energyMng.Energy += collision.gameObject.GetComponent<EnergyGenerator>().GeneratedEnergy;
                  collision.gameObject.GetComponent<EnergyGenerator>().GeneratedEnergy = 0;
              }
              */
      //}
}
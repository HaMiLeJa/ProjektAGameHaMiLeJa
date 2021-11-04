using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    Rigidbody rb;
    public GameObject Planet;


    public float StandardMovementSpeed = 3;
    Vector3 movement; //Umbennen
    public float CurrentEnergy = 1;
    public float BoostMultiplicator = 1;

    [SerializeField] float smallSlowDown = 0.9999f;
    public float SlowDownMultiplicator = 0.99f;
    public float ReduceEnergyMulitplicator = 0.9f;


    [SerializeField] float forceJump;
    [SerializeField] float jumpDuration;
    float timerJump;
    bool jumpButtonPressedInLastFrame = false;
    bool allowJump = false;

    [SerializeField] float gravity = 9.8f;
    [SerializeField] bool OnGround = false;
    float distanceToGround;
    Vector3 Groundnormal;


    

    [SerializeField] float boostDuration;
    bool boostButtonPressedInLastFrame = false;
    bool allowBoost = true;
    float timerBoost;
    [SerializeField] float boostPower = 5;

    public Vector3 velocity;


    public bool boosting;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    void FixedUpdate()
    {
        velocity = rb.velocity;

        Movement();
        Boost();
        GroundCheck();
        Jump();
        Gravity();


    }

    void Movement()
    {
        //Bewegung
        Vector3 strafeMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");

        movement = forwardMovement + strafeMovement; //Richtung, die gerade durch Controller angegeben wird inkl "Eigenen Geschwindigkeit" abhängig von der Stärke der Neigung der Joysticks
        movement = movement * Time.deltaTime * StandardMovementSpeed * CurrentEnergy * BoostMultiplicator;

        rb.velocity = rb.velocity + movement;


        //Abhanme der Volicity zur Begrenzung :D
        if (Mathf.Abs(rb.velocity.x) > 0.2f && Mathf.Abs(rb.velocity.z) > 0.2f)
        {
            rb.velocity = new Vector3(rb.velocity.x * smallSlowDown, rb.velocity.y, rb.velocity.z * smallSlowDown);
        }

        //Abhname der Velocity über bei kein Input:

        if (Mathf.Abs(rb.velocity.x) < 0.001f && Mathf.Abs(rb.velocity.z) > 0.001f) return;
        if (strafeMovement == Vector3.zero && forwardMovement == Vector3.zero) //Wenn kein Input
        {

            float x = rb.velocity.x;
            float z = rb.velocity.z;

            x = x * SlowDownMultiplicator; //Passiert eh regelmäßig wegen fixedUpdate
            z = z * SlowDownMultiplicator;

            rb.velocity = new Vector3(x, rb.velocity.y, z);

            CurrentEnergy *= ReduceEnergyMulitplicator;
            CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0.4f, 10);
        }
       

        // Begrenzung der Velocity

        //Bremsen: einfach in die Entgegengesetze richtung steuern ne?


        //SpeedMultiplicator durch Wand-Anstupsen (zeitweise) erhöhen

    }

    void Boost()
    {
        if (Input.GetButton("BoostJanina"))
        {
            
            if(boostButtonPressedInLastFrame == false)
            {
                allowBoost = true;
            }

            boostButtonPressedInLastFrame = true;

            if (allowBoost == true & timerBoost < boostDuration)
            {
                timerBoost += Time.deltaTime;
                BoostMultiplicator = boostPower; // *Gespeicherte Energie für super boost später oder so (maybe ist das aber dann auch ne eigenen Methode)
                boosting = true;
            }
            else
            {
                boosting = false;
                BoostMultiplicator = 1;
            }
        }
        else
        {
            timerBoost = 0;
            boostButtonPressedInLastFrame = false;
            allowBoost = false;
            BoostMultiplicator = 1;
        }
    }


    void GroundCheck()
    {
        //GroundControl
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10, LayerMask.GetMask("World")))
        {
            distanceToGround = hit.distance;
            Groundnormal = hit.normal; //verwendet bei Gravity

            if (distanceToGround <= 0.5f)
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }
        }
    }

    void Jump()
    {

        if (Input.GetButton("JumpJanina"))
        {

            if (OnGround == true && jumpButtonPressedInLastFrame == false)
            {
                allowJump = true;
            }

            jumpButtonPressedInLastFrame = true;


            if (allowJump == true && timerJump < jumpDuration)
            {
                timerJump += Time.deltaTime;
                rb.AddForce(this.transform.up * forceJump); //15, 0.1
            }

        }
        else
        {
            timerJump = 0;
            jumpButtonPressedInLastFrame = false;
            allowJump = false;
        }



    }

    void Gravity()
    {
        //Gravity and rotation
        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        if (OnGround == false)
        {
            rb.AddForce((gravDirection * -gravity));

        }

        // Quat
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.tag == "Wall")
        {
            Debug.Log("Collision with wall");

            CurrentEnergy += collision.gameObject.GetComponent<WallEnergy>().energy;

            collision.gameObject.GetComponent<WallEnergy>().energy = 0;

        }


    }

}

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
    public float SlowDownMultiplicator = 0.99f;
    public float ReduceEnergyMulitplicator = 0.9f;


    [SerializeField] float forceJump;
    [SerializeField] float jumpDuration;
    [SerializeField] bool jumpButtonReleased = true;
    [SerializeField] bool jumping = false;
    float timer;

    bool jumpBottonPressedInLastFrame = false;
    bool allowJump = false;

    [SerializeField] float gravity = 9.8f;
    [SerializeField] bool OnGround = false;
    float distanceToGround;
    Vector3 Groundnormal;


    public Vector3 velocity;
    



    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    void FixedUpdate()
    {
        velocity = rb.velocity;

        Movement();
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
        movement = movement * Time.deltaTime * StandardMovementSpeed * CurrentEnergy;

        rb.velocity = rb.velocity + movement;


        //Abhname der Velocity über zeit:
        if (strafeMovement == Vector3.zero && forwardMovement == Vector3.zero) //Wenn kein Input
        {
            //Eigentlich müsste man den SpeedMultiplicator (auch) verringern, damit im Ganzen Energie verloren geht
            


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


            if (OnGround == true && jumpBottonPressedInLastFrame == false)
            {
                allowJump = true;
            }

            jumpBottonPressedInLastFrame = true;


            if (allowJump == true && timer < jumpDuration)
            {
                timer += Time.deltaTime;
                rb.AddForce(this.transform.up * forceJump); //15, 0.1
            }

        }
        else
        {
            timer = 0;
            jumpBottonPressedInLastFrame = false;
            allowJump = false;
        }



    }

    void Gravity()
    {

        if (jumping == true) return; //eig unnötig

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    Rigidbody rb;
    public GameObject Planet;

    public float movementSpeed = 150f; //umbenennen

    Vector3 movement; //Umbennen
    public float speedMulitplicator;


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

    

    



    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    void FixedUpdate()
    {


        Movement();
        GroundCheck();
        Jump();
        Gravity();


    }

    void Movement()
    {
        //Variante 1 ("Original")
        
        Vector3 strafeMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");

        movement = forwardMovement + strafeMovement; //Richtung, die gerade durch Controller angegeben wird
        movement = movement * Time.deltaTime * movementSpeed;

        rb.velocity = rb.velocity + movement;


        //Variante 2 (Angepasst auf Velocity-Konzept)
        /*
        Vector3 strafeMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");

        movement = forwardMovement + strafeMovement; //Richtung, die gerade durch Controller angegeben wird
        movement = movement.normalized;

        rb.velocity = rb.velocity + movement* speedMulitplicator;
        */






        //Abhname der Velocity über zeit:


        // Begrenzung der Velocity

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


}

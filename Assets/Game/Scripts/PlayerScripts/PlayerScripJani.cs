using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripJani : MonoBehaviour
{
    public GameObject Planet;

    public float speed = 150f;
    //[SerializeField] float lerpSpeed = 0.2f;

    public float JumpHeight = 1.2f;

    [SerializeField] float forceJump;
    [SerializeField] float jumpDuration;
    [SerializeField] bool jumpButtonReleased = true;
    float timer;

    [SerializeField] float gravity = 100;
    [SerializeField] bool OnGround = false;
    [SerializeField] bool jumping = false;

    float distanceToGround;
    Vector3 Groundnormal;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    void Update()
    {

       // Vector3 strafeMovement = transform.right * Input.GetAxis("HorizontalJoystickAxis");
       // rb.MovePosition(transform.position + strafeMovement * Time.deltaTime * speed);


        GroundCheck();
        Jump();
        Gravity();


    }

    void GroundCheck()
    {
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
        //Jump
        /*
         if (Input.GetKeyDown(KeyCode.Space))
         {
            rb.AddForce(transform.up* 40000 * JumpHeight * Time.deltaTime);
         }
        */
        if (Input.GetButton("JumpJanina") && OnGround == true && timer < jumpDuration && jumpButtonReleased == true)
        {
            jumping = true;

            timer += Time.deltaTime;
            rb.AddForce(Vector2.up * forceJump, ForceMode.Impulse);
        }
        else
        {
            jumping = false;

            jumpButtonReleased = false;

        }

        if(Input.GetButton("JumpJanina") == false) //Check if Button was released, before allowing new jump
        {
            jumpButtonReleased = true;
            timer = 0;
        }



    }

    void Gravity()
    {
        if (jumping == true) return;

        //Gravity and rotation
            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        if (OnGround == false)
        {
            rb.AddForce(gravDirection * -gravity);

        }

        // Quat
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject Planet;

    public float speed = 4;
    public float JumpHeight = 1.2f;

    [SerializeField] float forceJump;
    [SerializeField] float jumpDuration;
    float timer;

    float gravity = 100;
    [SerializeField] bool OnGround = false;

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

        // movement script
        float x = Input.GetAxis("Horizontal")*Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical")*Time.deltaTime * speed;
        

        transform.Translate(x,0,z);

        //Local rotation
        if(Input.GetKey(KeyCode.E))
         {

            transform.Rotate(0, 150 * Time.deltaTime,0);
         }


        if(Input.GetKey(KeyCode.Q))
        {

           transform.Rotate(0, -150 * Time.deltaTime,0);
        }

        //Jump
        /*
         if (Input.GetKeyDown(KeyCode.Space))
         {
            rb.AddForce(transform.up* 40000 * JumpHeight * Time.deltaTime);
         }
        */
        if (Input.GetButton("JumpJanina") && OnGround == true && timer < jumpDuration)
        {
            timer += Time.deltaTime;
            rb.AddForce(Vector2.up * forceJump);
        }
        else
        {
            timer = 0;
        }


        //GroundControl
        RaycastHit hit = new RaycastHit();
         if(Physics.Raycast(transform.position, -transform.up, out hit, 10))
         {
            distanceToGround = hit.distance;
            Groundnormal= hit.normal;

            if (distanceToGround <= 0.2f)
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }
         }

         
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

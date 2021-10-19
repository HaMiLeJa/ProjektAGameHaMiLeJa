using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Planet;
    public GameObject CameraManager;

    public float speed = 4;
    public float JumpHeight = 1.2f;

    float gravity = 100;
    bool OnGround = false;

    bool force = true;
    float direction = 1f;

    float distanceToGround;
    Vector3 Groundnormal;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {

        // movement script

        float x = Input.GetAxis("Horizontal")*Time.deltaTime * speed;
     //   float z = Input.GetAxis("Vertical")*Time.deltaTime * speed;
      
if(Input.GetKey(KeyCode.S))
        {
             direction = -1f;
}
if(Input.GetKey(KeyCode.W))
        {
             direction = 1f;
}


        transform.Translate(x,0,0);

        Vector3 movement = direction *(transform.forward * Time.deltaTime * speed*0.5f);
        rb.MovePosition(transform.position + movement);

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

         if (Input.GetKeyDown(KeyCode.Space))

         {
            rb.AddForce(transform.up* 40000 * JumpHeight * Time.deltaTime);
         }



         //GroundControl

         RaycastHit hit = new RaycastHit();
         if(Physics.Raycast(transform.position, -transform.up, out hit, 10))
         {
            distanceToGround = hit.distance;
            Groundnormal= hit.normal;

            if (distanceToGround <= 0.1f)
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
            rb.AddForce(gravDirection * -gravity *0.3f);

        }

        // Quat

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;


   
    }

         void OnTriggerEnter(Collider collision)
        {
           if(collision.transform != Planet.transform)
           {
               Planet = collision.transform.gameObject;
               Vector3 gravDirection = (transform.position - Planet.transform.position).normalized *Time.fixedDeltaTime;

               Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) *transform.rotation;
               transform.rotation = toRotation;

               rb.velocity =Vector3.zero;
               if(force){
               rb.AddForce(gravDirection * gravity * 0.3f);
               force = false;

               }
               force = true;
               CameraManager.GetComponent<CameraManager>().NewPlanet(Planet);
           } 
        }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour
{
    [SerializeField] Vector3 velocity;
    [SerializeField] Vector3 direction;
    [SerializeField] Vector3 VelocityForRebound;
    Rigidbody playerRb;

    bool collided;
    bool savedVelocity = false;

    private void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>(); ;
    }

    private void Update()
    {
        /*
        
        if(savedVelocity == false)
        {
            velocity = playerRb.velocity;
            
        }

        //playerRb.velocity = velocity;
        */
    }

    private void OnCollisionEnter(Collision collision)
    {

        /*
        savedVelocity = true;

        if (collision.gameObject.tag == "Player")
        {
            if( collided == false)
            {

                //collided = true;

                GameObject player = collision.gameObject;

                //velocity = playerRb.velocity;

                VelocityForRebound = new Vector3(Mathf.Abs(velocity.x), 0, Mathf.Abs(velocity.z)); //Mathf.Abs(velocity.y)
                 
                
                direction = velocity.normalized;

                playerRb.velocity = new Vector3(direction.x * velocity.x, direction.y * velocity.y, direction.z * velocity.z);
                Debug.Log("CangesDirection");



               

            }
           
        }

        savedVelocity = false;
        */
    }
}

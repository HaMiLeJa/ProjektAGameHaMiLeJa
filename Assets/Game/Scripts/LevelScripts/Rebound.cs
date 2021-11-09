using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour
{
    [SerializeField] Vector3 velocity;
    [SerializeField] Vector3 direction;
    [SerializeField] Vector3 VelocityForRebound;

    [SerializeField] float force = 1;

    Rigidbody playerRb;

    bool collided;
    bool savedVelocity = false;

    float timer;
    [SerializeField] float duration = 0.2f;

    private void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>(); ;
    }

    private void FixedUpdate()
    {

        //Addforce Impulse in ddie Richtung
        if (collided == true)
        {
            timer += Time.deltaTime;

            if(timer < duration)
                playerRb.AddForce(-direction * force, ForceMode.Impulse);
            else
            {
                timer = 0;
                collided = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {


        //über Addforce
        if (collision.gameObject.tag == "Player")
        {


            //Vector von wand zum Player -> richtung
            if (collided == false)
            {
                collided = true;
                //playerRb.velocity = Vector3.zero;
                direction = this.transform.position - playerRb.transform.position;
                direction.Normalize();

            }




            
        }

        


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    
    float timer;
    float reboundDuration = 0.2f;
    [SerializeField]  float force = 15f;
    //[SerializeField] float velocityInfluence = 0.5f;

    Rigidbody playerRb;
    PlayerMovement playerMov;

    //float yReboundVelocity;

    Vector3 direction;
    Vector3 ReboundMovement;

    private void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        playerMov = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {

        if(playerMov.rebounded == true)
        {
            timer += Time.deltaTime;
            
            if (timer < reboundDuration)
            {
                playerRb.AddForce(ReboundMovement, ForceMode.Impulse);
                Debug.Log("2");
                //playerRb.velocity *= 0.8f;
            }
            else 
            {
                playerMov.rebounded = false;
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {


            playerMov.rebounded = true;

            //yReboundVelocity = Mathf.Abs(playerRb.velocity.y * velocityInfluence);
            //yReboundVelocity = Mathf.Clamp(yReboundVelocity, 1f, 5);

            

            direction = Vector3.up;

            ReboundMovement = direction * (force * 10) * Time.deltaTime; //new Vector3(0, direction.y * yReboundVelocity, 0) * force;

            Debug.Log("1");
            playerRb.velocity = new Vector3(playerRb.velocity.x * 0.1f, playerRb.velocity.y, playerRb.velocity.z * 0.1f);


            //playerRb.velocity = new Vector3(playerRb.velocity.x / 2, playerRb.velocity.y, playerRb.velocity.z / 2);


            //ReboundMovement.y = Mathf.Clamp(ReboundMovement.y, 1f, 6f);
        }
    }


    

}

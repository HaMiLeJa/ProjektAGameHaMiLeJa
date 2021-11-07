using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    
    float timer;
    [SerializeField] float reboundDuration = 0.2f;
    [SerializeField] float force = 0.3f;
    [SerializeField] float velocityInfluence = 0.5f;

    Rigidbody playerRb;
    PlayerMovement playerMov;

    float yReboundVelocity;

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
            }
            else 
            {
                playerMov.rebounded = false;
                Debug.Log("2");

                timer = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {


            playerMov.rebounded = true;

            yReboundVelocity = Mathf.Abs(playerRb.velocity.y * velocityInfluence);
            //yReboundVelocity = Mathf.Clamp(yReboundVelocity, 5, 9);


            direction = new Vector3(0, 1, 0);  //Input.GetAxis("Horizontal")



            ReboundMovement = new Vector3(0, direction.y * yReboundVelocity, 0) * force;

            ReboundMovement.y = Mathf.Clamp(ReboundMovement.y, 1f, 2.5f);
        }
    }
}

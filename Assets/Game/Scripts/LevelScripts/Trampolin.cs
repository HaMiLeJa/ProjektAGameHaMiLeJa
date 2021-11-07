using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    
    float timer;
    [SerializeField] float reboundDuration = 0.2f;
    [SerializeField] float force = 0.2f;

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

    private void Update()
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

            //richtung bestimmen, aus der der Spieler kommt



            playerMov.rebounded = true;

            yReboundVelocity = Mathf.Abs(playerRb.velocity.y);

            direction = new Vector3(0, 1, 0);
            ReboundMovement = new Vector3(0, direction.y, 0) * force;
        }
    }
}

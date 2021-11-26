using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsTriggerCollider : MonoBehaviour
{
    GameObject player;
    Rigidbody playerRb;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = playerRb.transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");

        if (other.gameObject.layer == LayerMask.GetMask("World"))
        {
            Debug.Log("CollidingT");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("World"))
        {
            Debug.Log("CollidingC");
        }
    }
}

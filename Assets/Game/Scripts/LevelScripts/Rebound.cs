using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;

            Debug.Log(player.GetComponent<Rigidbody>().velocity);
            player.GetComponent<Rigidbody>().velocity *= -1;
        }
    }
}

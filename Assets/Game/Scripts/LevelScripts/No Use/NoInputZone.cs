using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoInputZone : MonoBehaviour
{
    private void Start()
    {

    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().InNoInputZone = true;
            Debug.Log("NoInputZone Enter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerMovement>().InNoInputZone = false;
            Debug.Log("NoInputZone Exit");
        }
    }
    */
}

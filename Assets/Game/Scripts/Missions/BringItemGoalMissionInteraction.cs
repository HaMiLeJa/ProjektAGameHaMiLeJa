using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringItemGoalMissionInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {

            Debug.Log("1");
            ReferenceLibary.MissionMng.ActiveMissionState.BringItemDelivered(this.gameObject);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject == ReferenceLibary.Player)
        {

            Debug.Log("1");
            ReferenceLibary.MissionMng.ActiveMissionState.BringItemDelivered(this.gameObject);

        }
    }
}
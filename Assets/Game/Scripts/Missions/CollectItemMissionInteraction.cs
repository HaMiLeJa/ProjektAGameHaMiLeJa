using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemMissionInteraction : MonoBehaviour
{
   


    bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            MissionStateActiveMission.ItemCollected(this.gameObject);
           


           
        }
    }
   
}

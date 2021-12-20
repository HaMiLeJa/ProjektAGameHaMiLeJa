using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemMissionInteraction : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {
            ReferenceLibary.MissionMng.ActiveMissionState.ItemCollected(this.gameObject);




        }
    }

    

    



}

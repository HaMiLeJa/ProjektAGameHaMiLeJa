using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemMissionInteraction : MonoBehaviour
{



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {
            ReferenceLibary.MissionMng.ActiveMissionState.ItemCollected(this.gameObject);




        }
    }

    



}

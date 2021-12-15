using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissionItemInteraction : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


    bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            MissionStateActiveMission.ItemCollected(this.gameObject);
           


                //Coroutine mit effekten

           
        }
    }
   
}

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



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "MissionItem")
        {
            MissionManager.Progress++;
            //Coroutine mit effekten
            Destroy(other.gameObject);
        }
    }
}

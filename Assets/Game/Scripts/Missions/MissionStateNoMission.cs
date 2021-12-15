using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateNoMission : MonoBehaviour
{
    //Wait for time to pass to get new Mission

    float timer;

    bool randomDurationSet = false;
    int duration = 0;

    public void UpdateNoMission()
    {
        if(randomDurationSet == false)
        {
            duration = GetRandomDuration();
            randomDurationSet = true;
        }

        timer += Time.deltaTime;

        if(timer>= duration)
        {
            randomDurationSet = false;
            ReferenceLibary.MissionMng.SwitchToFindMissionState();
            duration = 0;
            Debug.Log("NewMission, time is up");
            
        }
    }


    int GetRandomDuration()
    {
       int i = Random.Range(10, 100);
       return i;
    }

}

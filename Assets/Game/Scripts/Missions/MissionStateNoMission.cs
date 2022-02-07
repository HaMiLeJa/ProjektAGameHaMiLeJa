using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateNoMission : MonoBehaviour
{
    bool randomDurationSet = false;
    public static float duration = 0;

    public void UpdateNoMission()
    {
        if(randomDurationSet == false)
        {
            randomDurationSet = true;
            int d = GetRandomDuration();
            duration = d;
        }

        duration -= Time.deltaTime;

        if(duration <= 0)
        {
            randomDurationSet = false;
            ReferenceLibary.MissionMng.SwitchToFindMissionState();
            duration = 0;
            ReferenceLibary.UIMng.DeactivateNoMissionUI();
        }
        else
            ReferenceLibary.UIMng.TimerUntilNexMission();
    }


    int GetRandomDuration()
    {
        int i = Random.Range  (2,5);//(30, 60);
       return i;
    }


}

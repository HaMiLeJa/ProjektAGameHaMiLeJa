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
            Debug.Log("New Mission, time is up");
            UIManager.Instance.DeactivateNoMissionUI();
        }
        else
            UIManager.Instance.TimerUntilNexMission();
    }


    int GetRandomDuration()
    {
       int i = Random.Range(10, 20);
       return i;
    }

}

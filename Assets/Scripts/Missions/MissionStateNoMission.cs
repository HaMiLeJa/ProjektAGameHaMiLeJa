using UnityEngine;
public class MissionStateNoMission : MonoBehaviour
{
    bool randomDurationSet = false;
    public static float duration = 0;
    private void Start()
    {
        randomDurationSet = true;
        duration = 50;
        ReferenceLibrary.UIMng.TimerUntilNexMission();
    }
    public void UpdateNoMission()
    {
        if(!randomDurationSet)
        {
            randomDurationSet = true;
            int d = GetRandomDuration();
            duration = d;
        }
        duration -= Time.deltaTime;
        if(duration <= 0)
        {
            randomDurationSet = false;
            ReferenceLibrary.MissionMng.SwitchToFindMissionState();
            duration = 0;
            ReferenceLibrary.UIMng.DeactivateNoMissionUI();
        }
        else ReferenceLibrary.UIMng.TimerUntilNexMission();
    }
    int GetRandomDuration()
    {
        int i = Random.Range(40, 80);
        return i;
    }
}
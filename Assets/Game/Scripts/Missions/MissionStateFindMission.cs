using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateFindMission : MonoBehaviour
{
    public void FindMission()
    {
        int missionIndex = 0;
        int maxRange = ReferenceLibary.MissLib.Missions.Count;

        missionIndex = Random.Range(0, maxRange);

        MissionManager.CurrentMission = ReferenceLibary.MissLib.Missions[missionIndex];
        ReferenceLibary.MissLib.Missions.RemoveAt(missionIndex);
    }
}

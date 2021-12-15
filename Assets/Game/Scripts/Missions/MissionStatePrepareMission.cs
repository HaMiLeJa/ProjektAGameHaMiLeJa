using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStatePrepareMission : MonoBehaviour
{
    public void PrepareMission()
    {
        ReferenceLibary.ItemSpawner.SpawnCollectItem();
        PrepareMissionUI();
        MissionManager.MissionTimeLeft = MissionManager.CurrentMission.time;
        MissionManager.Progress = 0;
    }

    public void PrepareMissionUI()
    {
        switch (MissionManager.CurrentMission.missionType)
        {
            case MissionInformation.MissionType.CollectItem:
                ActivateCollectItemUI();
                break;
            case MissionInformation.MissionType.DestroyObjs:
                break;
            case MissionInformation.MissionType.CollectXPoints:
                break;
            case MissionInformation.MissionType.BringFromAToB:
                break;
            default:
                break;

        }

    }

    void ActivateCollectItemUI()
    {
        UIManager.Instance.ActivateBasicMissionUI();
        UIManager.Instance.ActivateCollectItemUI();
    }
}

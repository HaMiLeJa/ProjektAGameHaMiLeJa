using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateActiveMission : MonoBehaviour
{
    
     public void UpdateActiveMission()
     {
        switch (MissionManager.CurrentMission.missionType)
        {
            case MissionInformation.MissionType.CollectItem:
                UpdateCollectItem();
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


    void UpdateCollectItem()
    {
        MissionTimer();


        UIManager.Instance.UpdateBasicMissionUI();
        UIManager.Instance.UpdateCollectItemUI(); //TO DO

        CheckForCollectItemEnd();
    }

   

    void CheckForCollectItemEnd()
    {
        if(MissionManager.Progress == MissionManager.CurrentMission.Amount)
        {
            ReferenceLibary.MissionMng.SwitchToCompletedMissionState();
        }
        else if (MissionManager.MissionTimeLeft <= 0)
        {
            ReferenceLibary.MissionMng.SwitchToUncompletedMissionState();
        }
    }


    void MissionTimer()
    {
        MissionManager.MissionTimeLeft -= Time.deltaTime;
    }

}

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
                UpdateDestroyObj();
                break;
            case MissionInformation.MissionType.CollectXPoints:
                break;
            case MissionInformation.MissionType.BringFromAToB:
                break;
            default:
                break;

        }
     }

    void MissionTimer()
    {
        MissionManager.MissionTimeLeft -= Time.deltaTime;
    }

    void CheckForEndEnd()
    {
        if (MissionManager.Progress == MissionManager.CurrentMission.Amount)
        {
            ReferenceLibary.MissionMng.SwitchToCompletedMissionState();
        }
        else if (MissionManager.MissionTimeLeft <= 0)
        {
            ReferenceLibary.MissionMng.SwitchToUncompletedMissionState();
        }
    }

    #region Collect Item
    void UpdateCollectItem()
    {
        MissionTimer();


        UIManager.Instance.UpdateBasicMissionUI();
        UIManager.Instance.UpdateCollectItemUI(); //TO DO

        CheckForEndEnd();
    }

   

    public static void ItemCollected(GameObject item)
    {
        MissionManager.Progress++;
        MissionItemSpawner.CurrentMissionItems.Remove(item);
        Destroy(item);
    }
    #endregion


    #region Destroy Objs

    void UpdateDestroyObj()
    {
        MissionTimer();

        UIManager.Instance.UpdateBasicMissionUI();
        UIManager.Instance.UpdateDestroObjUI();

        CheckForEndEnd();
    }


    public static void ObjDestroyed()
    {
        MissionManager.Progress++;
    }

    #endregion
}

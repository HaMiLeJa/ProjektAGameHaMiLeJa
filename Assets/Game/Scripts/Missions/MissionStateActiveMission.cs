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
            case MissionInformation.MissionType.CollectPoints:
                UpdateCollectPointsProgress();
                UpdateCollectPoints();
                break;
            case MissionInformation.MissionType.BringFromAToB:
                UpdateBringItem();
                break;
            default:
                break;

        }
     }

    void MissionTimer()
    {
        MissionManager.MissionTimeLeft -= Time.deltaTime;
    }

    void CheckForEnd()
    {
        if (MissionManager.Progress == MissionManager.CurrentMission.Amount)
        {
            ReferenceLibary.MissionMng.SwitchToCompletedMissionState();
            MissionManager.CompletedMissions++;
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


        ReferenceLibary.UIMng.UpdateBasicMissionUI();
        ReferenceLibary.UIMng.UpdateCollectItemUI();

        CheckForEnd();
    }

    public void ItemCollected(GameObject item)
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

        ReferenceLibary.UIMng.UpdateBasicMissionUI();
        ReferenceLibary.UIMng.UpdateDestroObjUI();

        CheckForEnd();
    }


    public static void ObjDestroyed()
    {
        MissionManager.Progress++;
    }

    #endregion

    #region Collect Points

    void UpdateCollectPoints()
    {
        MissionTimer();

        ReferenceLibary.UIMng.UpdateBasicMissionUI();
        ReferenceLibary.UIMng.UpdateCollectPointsUI();

        CheckForCollectPointsEnd();
    }

    void UpdateCollectPointsProgress()
    {
        float differenz = MissionManager.EndPoints - ScoreManager.CurrentScore;
        MissionManager.Progress = MissionManager.CurrentMission.Amount - differenz;
    }

    void CheckForCollectPointsEnd()
    {
        if(ScoreManager.CurrentScore >= MissionManager.EndPoints)
        {
            MissionManager.CompletedMissions++;
            ReferenceLibary.MissionMng.SwitchToCompletedMissionState();
        }
        else if (MissionManager.MissionTimeLeft <= 0)
        {
            ReferenceLibary.MissionMng.SwitchToUncompletedMissionState();
        }
    }

    #endregion

    #region Bring Item

    void UpdateBringItem()
    {
        MissionTimer();
        ReferenceLibary.UIMng.UpdateBasicMissionUI();
        CheckForBringItemEnd();
    }

    void CheckForBringItemEnd()
    {
        if (MissionManager.ItemDelivered == true)
        {
            MissionManager.CompletedMissions++;
            Debug.Log("ItemDelivered CheckForEnd");
            ReferenceLibary.MissionMng.SwitchToCompletedMissionState();
        }
        else if (MissionManager.MissionTimeLeft <= 0)
        {
            ReferenceLibary.MissionMng.SwitchToUncompletedMissionState();
        }

    }

    public void BringItemCollected(GameObject item)
    {
        MissionManager.ItemCollected = true;
        MissionItemSpawner.CurrentMissionItems.Remove(item);
        Destroy(item);
        ReferenceLibary.UIMng.ChangeProgressState1();
    }

    public void BringItemDelivered(GameObject item)
    {
       if(MissionManager.ItemCollected == true)
       {
            ReferenceLibary.UIMng.ChangeProgressState2();
            Debug.Log("2");
            MissionManager.ItemDelivered = true;

            MissionItemSpawner.CurrentMissionItems.Remove(item);
            Destroy(item);

       }
    }

    #endregion
}

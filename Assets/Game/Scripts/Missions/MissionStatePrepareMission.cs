using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStatePrepareMission : MonoBehaviour
{
   

    public void PrepareMission()
    {
        switch (MissionManager.CurrentMission.missionType)
        {
            case MissionInformation.MissionType.CollectItem:
                PrepareBasicMission();
                PrepareCollectItem();
                ActivateCollectItemUI();
                break;
            case MissionInformation.MissionType.DestroyObjs:
                PrepareBasicMission();
                PrepareDestroyObj();
                ActivateDestoryObjUi();
                break;
            case MissionInformation.MissionType.CollectPoints:
                PrepareBasicMission();
                PrepareCollectPoints();
                ActivateCollectPointsUI();
                break;
            case MissionInformation.MissionType.BringFromAToB:
                break;
            default:
                break;

        }

    }

    public void PrepareBasicMission()
    {
        MissionManager.MissionTimeLeft = MissionManager.CurrentMission.time;
        MissionManager.Progress = 0;
    }

    #region Collect Item
    void PrepareCollectItem()
    {
        ReferenceLibary.ItemSpawner.SpawnCollectItem();
    }

    void ActivateCollectItemUI()
    {
        UIManager.Instance.ActivateBasicMissionUI();
        UIManager.Instance.ActivateCollectItemUI();
    }
    #endregion

    #region Destroy Obj

    void PrepareDestroyObj()
    {
        //ggf spawn obj
        // oder check if genügeng vorhanden und wenn nicht dann spawn
    }

    void ActivateDestoryObjUi()
    {
        UIManager.Instance.ActivateBasicMissionUI();
        UIManager.Instance.ActivateDestroyObjUI();
    }

    #endregion

    #region CollectPoints

    void PrepareCollectPoints()
    {
        float startPoints = ScoreManager.CurrentScore + 0;
        MissionManager.EndPoints = startPoints + MissionManager.CurrentMission.Amount;
    }

    void ActivateCollectPointsUI()
    {
        UIManager.Instance.ActivateBasicMissionUI();
        UIManager.Instance.ActivateCollectPointsUI();
    }

    #endregion
}

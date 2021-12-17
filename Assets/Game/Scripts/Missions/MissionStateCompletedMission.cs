using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateCompletedMission : MonoBehaviour
{



   public void UpdateCompletedMission()
   {
        Debug.Log("MissionCompleted");

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

        //Effects, Sound
   }

    void UpdateScore()
    {
        ScoreManager.OnMultiplicatorUpdate(MissionManager.CurrentMission.multiplicator);
    }

    #region Collect Item
    void UpdateCollectItem()
    {
        UIManager.Instance.DeactivateCollectItemUI();
        UIManager.Instance.DeactivateBasicMissionUI();
        UpdateScore();
        RemoveRemainingCollectables();
    }

    void RemoveRemainingCollectables()
    {
        MissionItemSpawner.ClearCurrentMissionItemList();
    }
    #endregion

    #region DestroyObjects

    void UpdateDestroyObj()
    {
        UIManager.Instance.DeactivateBasicMissionUI();
        UIManager.Instance.DeactivateDestroyObjUI();
        UpdateScore();
    }

    #endregion


}

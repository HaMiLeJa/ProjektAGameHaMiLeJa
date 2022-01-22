using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateCompletedMission : MonoBehaviour
{
   public void UpdateCompletedMission()
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
                UpdateCollectPoints();
                break;
            case MissionInformation.MissionType.BringFromAToB:
                UpdateBringItem();
                break;
            default:
                break;

        }

        //Effects, Sound
   }

    void UpdateMultiplicator()
    {
        ScoreManager.OnPermanentMultiplicatorUpdate(MissionManager.CurrentMission.multiplicator);
    }

    #region Collect Item
    void UpdateCollectItem()
    {
        UIManager.Instance.DeactivateCollectItemUI();
        UIManager.Instance.DeactivateBasicMissionUI();
        UpdateMultiplicator();
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
        UpdateMultiplicator();
    }

    #endregion


    #region Collect Points
    void UpdateCollectPoints()
    {
        UIManager.Instance.DeactivateBasicMissionUI();
        UIManager.Instance.DeactivateCollectPointsUI();
        UpdateMultiplicator();
    }
    #endregion

    #region Bring Item

    void UpdateBringItem()
    {
        UIManager.Instance.DeactivateBasicMissionUI();
        UIManager.Instance.DeactivateBringItemUI();

        UpdateMultiplicator();
    }

    #endregion

}

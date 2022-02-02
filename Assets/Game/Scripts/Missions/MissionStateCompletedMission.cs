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
                UpdateEnergy();
                UpdateCollectItem();
                break;
            case MissionInformation.MissionType.DestroyObjs:
                UpdateEnergy();
                UpdateDestroyObj();
                break;
            case MissionInformation.MissionType.CollectPoints:
                UpdateEnergy();
                UpdateCollectPoints(); //Hier
                break;
            case MissionInformation.MissionType.BringFromAToB:
                UpdateEnergy();
                UpdateBringItem();
                break;
            default:
                break;

        }

        //Effects, Sound
   }

    void UpdateMultiplicator()
    {
        Debug.Log("Update Multiplicator");
        ScoreManager.OnPermanentMultiplicatorUpdate(MissionManager.CurrentMission.multiplicator);

       
    }

    private void UpdateEnergy()
    {
        EnergyManager.energyGotHigher = true;
        StartCoroutine(ReferenceLibary.EnergyMng.ModifyEnergy(20));
    }


    #region Collect Item
    void UpdateCollectItem()
    {
        ReferenceLibary.UIMng.DeactivateCollectItemUI();
        ReferenceLibary.UIMng.DeactivateBasicMissionUI();
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
        ReferenceLibary.UIMng.DeactivateBasicMissionUI();
        ReferenceLibary.UIMng.DeactivateDestroyObjUI();
        UpdateMultiplicator();
    }

    #endregion


    #region Collect Points
    void UpdateCollectPoints()
    {
        ReferenceLibary.UIMng.DeactivateBasicMissionUI();
        ReferenceLibary.UIMng.DeactivateCollectPointsUI();
        UpdateMultiplicator();
    }
    #endregion

    #region Bring Item

    void UpdateBringItem()
    {
        ReferenceLibary.UIMng.DeactivateBasicMissionUI();
        ReferenceLibary.UIMng.DeactivateBringItemUI();

        UpdateMultiplicator();
    }

    #endregion

}

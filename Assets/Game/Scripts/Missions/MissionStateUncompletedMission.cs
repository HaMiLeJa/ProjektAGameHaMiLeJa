using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateUncompletedMission : MonoBehaviour
{
    public void UpdateUncompletedMission()
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




        Debug.Log("MissionUncomplete");

       
        //Mission evt zur AllMission Liste zurückadden, wenn wir wollen, dass player jede mission erfüllen müssen
        //Effects, Sound
    }

    #region Collect Item
    void UpdateCollectItem()
    {
        ReferenceLibary.UIMng.DeactivateCollectItemUI();
        ReferenceLibary.UIMng.DeactivateBasicMissionUI();
        RemoveCollectables();
    }

    void RemoveCollectables()
    {
        MissionItemSpawner.ClearCurrentMissionItemList();
    }
    #endregion

    #region Destroy Obj
    void UpdateDestroyObj()
    {
        ReferenceLibary.UIMng.DeactivateDestroyObjUI();
        ReferenceLibary.UIMng.DeactivateBasicMissionUI();
    }
    #endregion


    #region Collect Points
    void UpdateCollectPoints()
    {
        ReferenceLibary.UIMng.DeactivateBasicMissionUI();
        ReferenceLibary.UIMng.DeactivateCollectPointsUI();
    }
    #endregion

    #region Bring Item

    void UpdateBringItem()
    {
        ReferenceLibary.UIMng.DeactivateBasicMissionUI();
        ReferenceLibary.UIMng.DeactivateBringItemUI();

       
    }

    #endregion
}

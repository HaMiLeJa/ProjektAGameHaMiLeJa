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

       
        //Mission evt zur AllMission Liste zur�ckadden, wenn wir wollen, dass player jede mission erf�llen m�ssen

        //Effects, Sound
    }

    #region Collect Item
    void UpdateCollectItem()
    {
        UIManager.Instance.DeactivateCollectItemUI();
        UIManager.Instance.DeactivateBasicMissionUI();
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
        UIManager.Instance.DeactivateDestroyObjUI();
        UIManager.Instance.DeactivateBasicMissionUI();
    }
    #endregion


    #region Collect Points
    void UpdateCollectPoints()
    {
        UIManager.Instance.DeactivateBasicMissionUI();
        UIManager.Instance.DeactivateCollectPointsUI();
    }
    #endregion

    #region Bring Item

    void UpdateBringItem()
    {
        UIManager.Instance.DeactivateBasicMissionUI();
        UIManager.Instance.DeactivateBringItemUI();

       
    }

    #endregion
}
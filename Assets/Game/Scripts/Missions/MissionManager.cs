using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    //  IDEE
    //Copy MissLib and delete Entries wehn mis is done. >If empty recopy missLib

    MissionLibary missLib;

    public static MissionInformation CurrentMission;

    public float MissionTimePassed;

    static MissionState missionState = MissionState.findMission;
    enum MissionState
    {
        noMission,
        findMission,
        activeMission,
        prepareMission
    }

    void Start()
    {
        missLib = ReferenceLibary.MissLib;
    }

   
    void Update()
    {
       switch (missionState)
       {
            case MissionState.noMission:
                break;
            case MissionState.findMission:
                FindMission();
                break;
            case MissionState.prepareMission:
                PrepareMissionUI();
                break;
            case MissionState.activeMission:
                break;
            default:
                break;
       }

    }

    #region FindMission;

    void FindMission()
    {
        int missionIndex = 0;
        int maxRange = missLib.Missions.Count;

        missionIndex = Random.Range(0, maxRange);

        CurrentMission = missLib.Missions[missionIndex];
        missLib.Missions.RemoveAt(missionIndex);

        missionState = MissionState.prepareMission;
    }

    #endregion

    #region PrepareMission

    void PrepareMissionUI()
    {
        switch(CurrentMission.missionType)
        {
            case MissionInformation.MissionType.CollectItem:
                ActivateCollectItemUI();
                ReferenceLibary.ItemSpawner.SpawnCollectItem();
                SwitchToActiveMissionState();
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

   

    void SwitchToActiveMissionState()
    {
        missionState = MissionState.activeMission;
    }

    #region CollectItem

    void ActivateCollectItemUI()
    {
        UIManager.Instance.ActivateBasicMissionUI();
        UIManager.Instance.ActivateCollectItemUI();
    }

   

    #endregion

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    MissionStateNoMission NoMissionMissionState;
    MissionStateFindMission FindMissionState;
    MissionStatePrepareMission PrepareMissionState;
    public MissionStateActiveMission ActiveMissionState;
    MissionStateCompletedMission CompletedMissionState;
    MissionStateUncompletedMission UncompletedMissionState;

    public static MissionInformation CurrentMission;
    
    // For Active State
    public static float MissionTimeLeft;
    public static float Progress;

    //For CollectPoints
    public static float EndPoints;

    //For Bring Item
    public static bool ItemCollected = false;
    public static bool ItemDelivered = false;
    public float BringItemDistance = 0;

    static MissionState missionState = MissionState.noMissionsLeft;
    enum MissionState
    {
        noMission,
        findMission,
        prepareMission,
        activeMission,
        CompletedMission,
        UncompletedMission,
        noMissionsLeft
    }

    void Start()
    {
        NoMissionMissionState = GetComponentInChildren<MissionStateNoMission>();
        FindMissionState = GetComponentInChildren<MissionStateFindMission>();
        PrepareMissionState = GetComponentInChildren<MissionStatePrepareMission>();
        ActiveMissionState = GetComponentInChildren<MissionStateActiveMission>();
        CompletedMissionState = GetComponentInChildren<MissionStateCompletedMission>();
        UncompletedMissionState = GetComponentInChildren<MissionStateUncompletedMission>();
    }


    void Update()
    {
        if (GameManager.Instance.GameOver == true) return;

        switch (missionState)
        {
            case MissionState.noMission:
                NoMissionMissionState.UpdateNoMission();
                break;
            case MissionState.findMission:
                FindMissionState.FindMission();
                SwitchToPrepareMissionState();
                break;
            case MissionState.prepareMission:
                PrepareMissionState.PrepareMission();
                SwitchToActiveMissionState();
                break;
            case MissionState.activeMission:
                ActiveMissionState.UpdateActiveMission();
                break;
            case MissionState.CompletedMission:
                CompletedMissionState.UpdateCompletedMission();
                CheckForAllMissionsDone();
                CollectableManager.OnRespawnCollectables?.Invoke();
                break;
            case MissionState.UncompletedMission:
                UncompletedMissionState.UpdateUncompletedMission();
                CheckForAllMissionsDone();
                CollectableManager.OnRespawnCollectables?.Invoke();
                break;
            case MissionState.noMissionsLeft:
                break;
            default:
                break;
        }

    }




    #region Switch State
    void SwitchToActiveMissionState()
    {
        missionState = MissionState.activeMission;
    }

    void SwitchToPrepareMissionState()
    {
        missionState = MissionState.prepareMission;
    }

    public void SwitchToCompletedMissionState()
    {
        missionState = MissionState.CompletedMission;
    }

    public void SwitchToUncompletedMissionState()
    {
        missionState = MissionState.UncompletedMission;
    }

    public void SwitchToNoMissionState()
    {
        UIManager.Instance.ActivateNoMissionUI();
        missionState = MissionState.noMission;
    }

     public void SwitchToFindMissionState()
     {
        missionState = MissionState.findMission;
     }

    void SwitchToNoMissionLeftState()
    {
        missionState = MissionState.noMissionsLeft;
    }
    #endregion




    //Check if alle missiosn erfüllt wurden oder nicht, dann mission restart
    public void CheckForAllMissionsDone()
    {
        if(ReferenceLibary.MissLib.Missions.Count == 0)
        {
            SwitchToNoMissionLeftState();
        }
        else
        {
            SwitchToNoMissionState();
        }
    }


}

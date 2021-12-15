using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    MissionStateFindMission FindMissionState;
    MissionStatePrepareMission PrepareMissionState;
    MissionStateActiveMission ActiveMissionState;
    MissionStateCompletedMission CompletedMissionState;
    MissionStateUncompletedMission UncompletedMissionState;

    public static MissionInformation CurrentMission;

    // For Active State
    public static float MissionTimeLeft;
    public static float Progress;

    //For No Mission

    static MissionState missionState = MissionState.findMission;
    enum MissionState
    {
        noMission,
        findMission,
        activeMission,
        prepareMission,
        CompletedMission,
        UncompletedMission
    }

    void Start()
    {
        FindMissionState = GetComponentInChildren<MissionStateFindMission>();
        PrepareMissionState = GetComponentInChildren<MissionStatePrepareMission>();
        ActiveMissionState = GetComponentInChildren<MissionStateActiveMission>();
        CompletedMissionState = GetComponentInChildren<MissionStateCompletedMission>();
        UncompletedMissionState = GetComponentInChildren<MissionStateUncompletedMission>();
    }


    void Update()
    {
        switch (missionState)
        {
            case MissionState.noMission:
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
                SwitchToNoMissionState();
                break;
            case MissionState.UncompletedMission:
                UncompletedMissionState.UpdateUncompletedMission();
                SwitchToNoMissionState();
                break;
            default:
                break;
        }

    }





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

    void SwitchToNoMissionState()
    {
        missionState = MissionState.noMission;
    }

     public void SwitchToFindMissionState()
     {
        missionState = MissionState.findMission;
     }
}

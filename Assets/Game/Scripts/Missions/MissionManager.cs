using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MissionManager : MonoBehaviour
{
    public static int MissionAmount;
    public static int CompletedMissions;
    public static int MissionRound = 0;

    MissionStateNoMission NoMissionMissionState;
    MissionStateFindMission FindMissionState;
    MissionStatePrepareMission PrepareMissionState;
    [HideInInspector] public MissionStateActiveMission ActiveMissionState;
    MissionStateCompletedMission CompletedMissionState;
    MissionStateUncompletedMission UncompletedMissionState;
    [HideInInspector]public MissionStateNoMissionsLeft NoMissionLeft;

    public static MissionInformation CurrentMission;

    bool lastMissionSuccesfull = false;
    
    // For Active State
    public static float MissionTimeLeft;
    public static float Progress;

    //For CollectPoints
    public static float EndPoints;

    //For Bring Item
    public static bool ItemCollected = false;
    public static bool ItemDelivered = false;
    public float BringItemDistance = 0;

    //For NoMissionsLeft State
    public static bool StartNewMissionRoundAllowed = false;

    [Header("Audio")]
    [SerializeField] AudioClip newMissionClip;
    [SerializeField] AudioMixerGroup newMissionGroup;

    [SerializeField] AudioClip successfullClip;
    [SerializeField] AudioMixerGroup successfullGroup;

    [SerializeField] AudioClip unsuccesfullClip;
    [SerializeField] AudioMixerGroup unsuccessfullGroup;

    public AudioClip missionCollectalbeClip;
    public AudioMixerGroup missionCollectalbeGroup;

    AudioManager audioMng;

    static MissionState missionState = MissionState.noMission;
    enum MissionState
    {
        noMission,
        findMission,
        prepareMission,
        activeMission,
        CompletedMission,
        UncompletedMission,
        noMissionsLeft,
        transitionCase
    }

    void Start()
    {
        audioMng = ReferenceLibary.AudMng;

        missionState = MissionState.noMission;
        NoMissionMissionState = GetComponentInChildren<MissionStateNoMission>();
        FindMissionState = GetComponentInChildren<MissionStateFindMission>();
        PrepareMissionState = GetComponentInChildren<MissionStatePrepareMission>();
        ActiveMissionState = GetComponentInChildren<MissionStateActiveMission>();
        CompletedMissionState = GetComponentInChildren<MissionStateCompletedMission>();
        UncompletedMissionState = GetComponentInChildren<MissionStateUncompletedMission>();
        NoMissionLeft = GetComponentInChildren<MissionStateNoMissionsLeft>();
    }


    void Update()
    {
        if (GameStateManager.GameOver == true) return;

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
                lastMissionSuccesfull = true;
                CheckForAllMissionsDone(); //Switch State, Play Sound
                CollectableManager.OnRespawnCollectables?.Invoke();
                break;
            case MissionState.UncompletedMission:
                UncompletedMissionState.UpdateUncompletedMission();
                lastMissionSuccesfull = false;
                CheckForAllMissionsDone(); //Switch State, Play Sound
                CollectableManager.OnRespawnCollectables?.Invoke();
                break;
            case MissionState.noMissionsLeft:
                ReferenceLibary.WinconMng.CheckForWinConMission();
                missionState = MissionState.transitionCase;
                break;
            case MissionState.transitionCase:
                CheckForReactivation();
                break;
            default:
                break;
        }

        /*
        if(Input.GetKeyDown(KeyCode.A))
            audioMng.PlayMissionSound(newMissionClip, newMissionGroup);

        if (Input.GetKeyDown(KeyCode.B))
            audioMng.PlayMissionSound(successfullClip, successfullGroup);

        if (Input.GetKeyDown(KeyCode.C))
            audioMng.PlayMissionSound(unsuccesfullClip, unsuccessfullGroup);

        if (Input.GetKeyDown(KeyCode.D))
            audioMng.PlayMissionSound(missionCollectalbeClip, missionCollectalbeGroup);
        */
    }




    #region Switch State
    void SwitchToActiveMissionState()
    {
        audioMng.PlayMissionSound(newMissionClip, newMissionGroup);
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
        ReferenceLibary.UIMng.ActivateNoMissionUI();
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




    
    public void CheckForAllMissionsDone()
    {
        if(ReferenceLibary.MissLib.Missions.Count == 0) // Alle Missionen wurden durchlaufen
        {
            SwitchToNoMissionLeftState();
        }
        else
        {
            SwitchToNoMissionState();

            if(lastMissionSuccesfull == true)
            {
                 audioMng.PlayMissionSound(unsuccesfullClip, unsuccessfullGroup);
            }
            else
            {
                audioMng.PlayMissionSound(missionCollectalbeClip, missionCollectalbeGroup);
            }
        }
    }



    void CheckForReactivation() //Used, when The first Mission Round is over
    {
        if(StartNewMissionRoundAllowed == true)
        {
            StartNewMissionRoundAllowed = false;
            NoMissionLeft.ReactiveMissions();
        }
    }
}

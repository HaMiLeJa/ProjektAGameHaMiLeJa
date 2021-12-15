using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionLibary : MonoBehaviour
{
    [SerializeField] List<MissionInformation> AllMissions = new List<MissionInformation>();
    [HideInInspector] public List<MissionInformation> Missions = new List<MissionInformation>();


    private void Awake()
    {
        CopyMissionLists();
    }


    public void CopyMissionLists()
    {
        Missions = AllMissions;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateNoMissionsLeft : MonoBehaviour
{
   
    


    public void ReactiveMissions()
    {

        if (MissionManager.MissionRound <= 4) return;
        ReferenceLibary.MissLib.CopyMissionLists();
        ReferenceLibary.MissionMng.SwitchToNoMissionState();

    }

}

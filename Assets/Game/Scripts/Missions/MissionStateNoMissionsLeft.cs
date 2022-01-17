using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateNoMissionsLeft : MonoBehaviour
{
   


    // wv missisonen gab es? wv davon wurden erfüllt?
    //hexteil gefunden?/Wincon erfüllt?

    //Missionen wiederhohlen + UIMeldung



    //if mission round 1 = false, direct kopieren und wieder anfangen.


    public void ReactiveMissions()
    {
       

        ReferenceLibary.MissLib.CopyMissionLists();
        ReferenceLibary.MissionMng.SwitchToNoMissionState();

    }

}

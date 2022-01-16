using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateNoMissionsLeft : MonoBehaviour
{
   


    // wv missisonen gab es? wv davon wurden erf�llt?
    //hexteil gefunden?/Wincon erf�llt?

    //Missionen wiederhohlen + UIMeldung



    //if mission round 1 = false, direct kopieren und wieder anfangen.


    public void CheckForWinConMission()
    {
        if(MissionManager.MissionRound ==1)
        {
            if (MissionManager.CompletedMissions == MissionManager.MissionAmount)
            {
                //ALLE MISSIONEN GESCHAFFT!

                if (PlayerPrefs.GetInt("WinConMissions") == 0)
                {
                    PlayerPrefs.SetInt("WinConMissions", 1);
                    StartCoroutine(ReferenceLibary.UIMng.UIHexUnlocked());
                }
                else
                {
                    StartCoroutine(ReferenceLibary.UIMng.UIHexAlreadyUnlocked());
                }

            }
            else
            {
                StartCoroutine(ReferenceLibary.UIMng.UIHexUnlockedFailed());
            }
        }


        ReferenceLibary.MissLib.CopyMissionLists();
        //Change State

    }
}

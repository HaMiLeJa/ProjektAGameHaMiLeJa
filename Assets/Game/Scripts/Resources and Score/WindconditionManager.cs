using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindconditionManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void CheckForWinConMission()
    {
        if (MissionManager.MissionRound == 1)
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

    }


}

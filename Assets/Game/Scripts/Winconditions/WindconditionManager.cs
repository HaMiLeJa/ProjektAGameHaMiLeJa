using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindconditionManager : MonoBehaviour
{

    public int PointsForWinCon = 20000;

    [HideInInspector] public int WinConPoints = 0;
    [SerializeField] CollectableHex CollectableHex;
    
    void Awake()
    {
        WinConPoints = PlayerPrefs.GetInt("WinConPoints");
    }
    private void Start()
    {
        if (CollectableHex == null)
            CollectableHex = GameObject.FindObjectOfType<CollectableHex>();

        if (PlayerPrefs.GetInt("WinConHex") == 1)
        {
            Destroy(CollectableHex.gameObject);
        }
               
    
       // InstantiateWindConHexItem();
    }


    public void CheckForWinConMission() //Über MissionManager State NO MIssions left
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



    public void CheckForWinConPoints(float value)
    {
        if(ScoreManager.CurrentScore >= PointsForWinCon)
        {
            WinConPoints = 1;
            ScoreManager.OnScoring -= CheckForWinConPoints;
            PlayerPrefs.SetInt("WinConPoints", 1);

            StartCoroutine(ReferenceLibary.UIMng.WinConPointsCoroutine());
            Debug.Log("Win Con Points fullfilled");
            
        }
    }


    public void CheckForWinConHex()
    {
        PlayerPrefs.SetInt("WinConHex", 1);

        StartCoroutine(ReferenceLibary.UIMng.WinConHexCoroutine());
        //Effect

        Destroy(CollectableHex.gameObject);

    }

    void InstantiateWindConHexItem()
    {

    }
}

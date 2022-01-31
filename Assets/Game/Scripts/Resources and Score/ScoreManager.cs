using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    GameManager gameMng;

    public static float CurrentScore = 0;
    public static float CurrentMultiplicator = 1;



    #region Events

    public delegate void Scoring(float value);
    public static Scoring OnScoring;

    public delegate void MultiplicatorUpdate(float value);
    public static MultiplicatorUpdate OnPermanentMultiplicatorUpdate;

    public delegate void TemporaryMultiplicatorUpdate(float value);
    public static TemporaryMultiplicatorUpdate OnTemporaryMultiplicatorUpdate;

    #endregion

   
    void Start()
    {
        gameMng = ReferenceLibary.GameMng;

        OnScoring += UpdateScore;
        OnScoring += ReferenceLibary.UIMng.UpdateScore;
        OnScoring += ReferenceLibary.UIMng.PointsStarter;

        if (ReferenceLibary.WinconMng.WinConPoints == 0)
            OnScoring += ReferenceLibary.WinconMng.CheckForWinConPoints;


        OnPermanentMultiplicatorUpdate += UpdateMultiplicator;
        OnPermanentMultiplicatorUpdate += ReferenceLibary.UIMng.UpdateMultiplicatorUI;
        OnPermanentMultiplicatorUpdate += ReferenceLibary.UIMng.PermanentMulitplicatorStarter;

        OnTemporaryMultiplicatorUpdate += UpdateMultiplicator;
        OnTemporaryMultiplicatorUpdate += ReferenceLibary.UIMng.UpdateMultiplicatorUI;
        OnTemporaryMultiplicatorUpdate += ReferenceLibary.UIMng.UpdateTemporaryMultiplicator;

        CurrentMultiplicator = 1;
        CurrentScore = 0;

    }

    
    void Update()
    {
        
    }

    void UpdateScore(float value)
    {
        CurrentScore += Mathf.RoundToInt((value * CurrentMultiplicator));
        
        
    }

    void UpdateMultiplicator(float value)
    {
        CurrentMultiplicator += value;
    }




    public bool CheckForNewHighscore()
    {
        Debug.Log("CheckForNewHighscore");
        if (PlayerPrefs.GetFloat("Highscore") < CurrentScore)
        {
            Debug.Log("Highscorechek true");
            return true;

        }
        else
        {
            Debug.Log("Highscorechek false");
            return false;
        }
    }

    public void SetNewHighscore()
    {
        PlayerPrefs.SetFloat("Highscore", CurrentScore);
    }


}

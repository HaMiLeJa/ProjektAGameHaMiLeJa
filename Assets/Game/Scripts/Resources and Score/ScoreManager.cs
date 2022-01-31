using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static float CurrentScore = 0;
    public static float CurrentMultiplicator = 1;
    public float DebugScore;


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

        OnScoring = null;
        OnScoring += UpdateScore;
        OnScoring += ReferenceLibary.UIMng.UpdateUIScore;
        OnScoring += ReferenceLibary.UIMng.PointsStarter;


        if (ReferenceLibary.WinconMng.WinConPoints == 0)
            OnScoring += ReferenceLibary.WinconMng.CheckForWinConPoints;

        OnPermanentMultiplicatorUpdate = null;
        OnPermanentMultiplicatorUpdate += UpdateMultiplicator;
        OnPermanentMultiplicatorUpdate += ReferenceLibary.UIMng.UpdateMultiplicatorUI;
        OnPermanentMultiplicatorUpdate += ReferenceLibary.UIMng.PermanentMulitplicatorStarter;

        OnPermanentMultiplicatorUpdate = null;
        OnTemporaryMultiplicatorUpdate += UpdateMultiplicator;
        OnTemporaryMultiplicatorUpdate += ReferenceLibary.UIMng.UpdateMultiplicatorUI;
        OnTemporaryMultiplicatorUpdate += ReferenceLibary.UIMng.UpdateTemporaryMultiplicator;

        CurrentMultiplicator = 1;
        CurrentScore = 0;

    }

    
    void Update()
    {
        DebugScore = CurrentScore;
    }

    public void UpdateScore(float value)
    {
        CurrentScore += Mathf.RoundToInt((value * CurrentMultiplicator));
        //Debug.Log("Update Score");
    }

    public void UpdateMultiplicator(float value)
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

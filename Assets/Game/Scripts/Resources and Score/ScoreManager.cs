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

    #region Singleton
    public static ScoreManager Instance;
    private void Awake()
    {
        if (ScoreManager.Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            ScoreManager.Instance = this;
        }

    }
    #endregion

    void Start()
    {
        gameMng = GameManager.Instance;

        OnScoring += UpdateScore;
        OnScoring += UIManager.Instance.UpdateScore;
        OnScoring += ReferenceLibary.UIMng.PointsStarter;

        if (ReferenceLibary.WinconMng.WinConPoints == 0)
            OnScoring += ReferenceLibary.WinconMng.CheckForWinConPoints;


        OnPermanentMultiplicatorUpdate += UpdateMultiplicator;
        OnPermanentMultiplicatorUpdate += UIManager.Instance.UpdateMultiplicatorUI;
        OnPermanentMultiplicatorUpdate += ReferenceLibary.UIMng.PermanentMulitplicatorStarter;

        OnTemporaryMultiplicatorUpdate += UpdateMultiplicator;
        OnTemporaryMultiplicatorUpdate += UIManager.Instance.UpdateMultiplicatorUI;
        OnTemporaryMultiplicatorUpdate += ReferenceLibary.UIMng.UpdateTemporaryMultiplicator;

        CurrentMultiplicator = 1;
        CurrentScore = 0;

    }

    
    void Update()
    {
        
    }

    void UpdateScore(float value)
    {
        CurrentScore += (value * CurrentMultiplicator);
        
        
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

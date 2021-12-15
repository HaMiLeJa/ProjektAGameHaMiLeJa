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
    public static MultiplicatorUpdate OnMultiplicatorUpdate;

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

        OnMultiplicatorUpdate += UpdateMultiplicator;
        OnMultiplicatorUpdate += UIManager.Instance.UpdateMultiplicatorUI;

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
}

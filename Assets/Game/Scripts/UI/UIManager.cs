using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager gameMng;

    [SerializeField] TMPro.TMP_Text score;
    [SerializeField] TMPro.TMP_Text multiplicator;
    [SerializeField] TMPro.TMP_Text CurrentEnergy;

    [SerializeField] GameObject EndMessage;



    #region Singleton
    public static UIManager Instance;
    private void Awake()
    {
        if (UIManager.Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            UIManager.Instance = this;
        }
    }
    #endregion

    void Start()
    {
        gameMng = GameManager.Instance;

        //gameMng.onUIEnergyChange += UpdateEnergyUI;

        
        EndMessage.SetActive(false);

        UpdateScore(1);
        UpdateMultiplicatorUI(1);

    }
   

  
    void Update()
    {
      //  float energyDisplay = (EnergyManager.Instance.CurrentUIEnergy - 1);
       /* if(energyDisplay <0)
        {
            energyDisplay = 0;
        }*/


       CurrentEnergy.text = "Energy: " + EnergyManager.CurrentEnergy.ToString();


    }


    public void UpdateScore(float value)
    {
        score.text = ScoreManager.CurrentScore.ToString();
    }

    public void UpdateMultiplicatorUI(float value)
    {
        multiplicator.text = "x" + ScoreManager.CurrentMultiplicator.ToString();
    }


    void UpdateEnergy()
    {

    }

    public void ShowEndMessage()
    {
        EndMessage.SetActive(true);
    }

  }

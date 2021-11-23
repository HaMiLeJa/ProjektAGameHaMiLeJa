using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager gameMng;

    public TMPro.TMP_Text DestroyablePoints;
    public TMPro.TMP_Text CurrentEnergy;
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

        gameMng.onDestroyableDestroyed += UpdateDestroyableUI;
        //gameMng.onUIEnergyChange += UpdateEnergyUI;

        //UpdateEnergyUI(1);
        EndMessage.SetActive(false);
    }
   

  
    void Update()
    {

       CurrentEnergy.text = "Energy: " + ResourceManager.Instance.CurrentUIEnergy.ToString();
    }


    void UpdateDestroyableUI(float value)
    {
        Debug.Log("EventCalled");
        DestroyablePoints.text = ResourceManager.Instance.DestroyablePoints.ToString();
    }


    public void UpdateEnergyUI(float value)
    {
        CurrentEnergy.text = "Energy: " + ResourceManager.Instance.CurrentUIEnergy.ToString();
    }


    public void ShowEndMessage()
    {
        EndMessage.SetActive(true);
    }

  }

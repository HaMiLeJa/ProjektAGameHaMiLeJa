using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameManager gameMng;

    [SerializeField] TMPro.TMP_Text score;
    [SerializeField] TMPro.TMP_Text multiplicator;
    //[SerializeField] TMPro.TMP_Text CurrentEnergy;
    [SerializeField] Image CurrentEnergy;

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
        UpdateEnergyUI();

        ActivateNoMissionUI();
        DeactivateBasicMissionUI();
        DeactivateCollectItemUI();
        DeactivateDestroyObjUI();

    }

    #region Missions

    #region Basic UI
    [Header("UI Of All Missions")]
    [SerializeField] GameObject BasicMissionUI;
    [SerializeField] TMPro.TMP_Text missionTimeTxt;
    [SerializeField] TMPro.TMP_Text progressTxt;
    
    public void UpdateBasicMissionUI()
    {
        BasicMissionUI.SetActive(true);
        progressTxt.text = MissionManager.Progress + "/" + MissionManager.CurrentMission.Amount;
        missionTimeTxt.text = "Time remaining: " + MissionManager.MissionTimeLeft.ToString();
    }
    public void ActivateBasicMissionUI()
    {
        progressTxt.text = "0/" + MissionManager.CurrentMission.Amount;
        missionTimeTxt.text = "Time remaining: " + MissionManager.CurrentMission.time;
    }

  

    public void DeactivateBasicMissionUI()
    {
        progressTxt.text = "";
        missionTimeTxt.text = "";
        BasicMissionUI.SetActive(false);
    }
    #endregion

    #region No Mission
    [Header("No Mission UI")]
    [SerializeField] TMPro.TMP_Text nextMissionTxt;
    [SerializeField] GameObject noMissionParent;
    public void TimerUntilNexMission()
    {
        nextMissionTxt.text = "Next Mission in: " + MissionStateNoMission.duration;
    }

    public void ActivateNoMissionUI()
    {
        noMissionParent.SetActive(true);
    }
    public void DeactivateNoMissionUI()
    {
        noMissionParent.SetActive(false);
        Debug.Log("Deactivated UI");
    }

    #endregion


    #region Collect Item
    [Header("CollectItemUI")]
    [SerializeField] GameObject collectItemParent;
    [SerializeField] Image collectItemImage;
    [SerializeField] Sprite collectItem1;
    [SerializeField] Sprite collectItem2;
    
    public void ActivateCollectItemUI()
    {
        collectItemParent.SetActive(true);
       
        if (MissionManager.CurrentMission.missionItem == MissionInformation.Item.CollectItem1)
        {
            collectItemImage.sprite = collectItem1;
        }
        else if (MissionManager.CurrentMission.missionItem == MissionInformation.Item.CollectItem2)
        {
            collectItemImage.sprite = collectItem2;
        }

    }
    public void DeactivateCollectItemUI()
    {
        collectItemParent.SetActive(false);
    }

    public void UpdateCollectItemUI()
    {
        //Effekt beim Collecten?
    }

    #endregion

    #region DestroyObj
    [Header("Destory Obj")]
    [SerializeField] GameObject destroyItemParent;
    [SerializeField] Image destroyObjImage;
    [SerializeField] Sprite destroyObj1;

    public void ActivateDestroyObjUI()
    {
        destroyItemParent.SetActive(true);
        //evt sprite wie bei collect Item
    }

    public void DeactivateDestroyObjUI()
    {
        destroyItemParent.SetActive(false);
    }

    public void UpdateDestroObjUI()
    {

    }

    #endregion

    #endregion

    #region Energy
    public void UpdateEnergyUI()
    {
        CurrentEnergy.fillAmount = EnergyManager.CurrentEnergy / EnergyManager.Instance.MaxEnergyAmount;
    }
    #endregion

    #region Score and Multiplicator
    public void UpdateScore(float value)
    {
        score.text = ScoreManager.CurrentScore.ToString();
    }

    public void UpdateMultiplicatorUI(float value)
    {
        multiplicator.text = "x" + ScoreManager.CurrentMultiplicator.ToString();
    }
    #endregion

    #region EndScreen
    public void ShowEndMessage()
    {
        EndMessage.SetActive(true);
    }
    #endregion
}

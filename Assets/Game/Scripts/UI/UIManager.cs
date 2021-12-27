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

    public GameObject PauseCanvas;
    public GameObject GameOverCanvas;
    public GameObject IngameCanvas;


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
        IngameCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);
        PauseCanvas.SetActive(false);


        UpdateScore(1);
        UpdateMultiplicatorUI(1);
        UpdateEnergyUI();

        ActivateNoMissionUI();
        DeactivateBasicMissionUI();
        DeactivateCollectItemUI();
        DeactivateDestroyObjUI();
        DeactivateCollectPointsUI();
        DeactivateBringItemUI();

    }

    #region Missions

    #region Basic UI
    [Header("UI Of All Missions")]
    [SerializeField] GameObject BasicMissionUI;
    [SerializeField] TMPro.TMP_Text missionTimeTxt;
    
    public void UpdateBasicMissionUI()
    {
        BasicMissionUI.SetActive(true);
        missionTimeTxt.text = "Time remaining: " + MissionManager.MissionTimeLeft.ToString();
    }
    public void ActivateBasicMissionUI()
    {
       
        missionTimeTxt.text = "Time remaining: " + MissionManager.CurrentMission.time;
    }

  

    public void DeactivateBasicMissionUI()
    {
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
    [SerializeField] TMPro.TMP_Text collectItemProgressTxt;

    public void ActivateCollectItemUI()
    {
        collectItemParent.SetActive(true);
        collectItemProgressTxt.text = "0/" + MissionManager.CurrentMission.Amount;


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
        collectItemProgressTxt.text = MissionManager.Progress + "/" + MissionManager.CurrentMission.Amount;
    }

    #endregion

    #region DestroyObj
    [Header("Destory Obj")]
    [SerializeField] GameObject destroyItemParent;
    [SerializeField] Image destroyObjImage;
    [SerializeField] Sprite destroyObj1;
    [SerializeField] TMPro.TMP_Text destroyObjProgressTxt;

    public void ActivateDestroyObjUI()
    {
        destroyItemParent.SetActive(true);
        destroyObjProgressTxt.text = "0/" + MissionManager.CurrentMission.Amount;
        //evt sprite wie bei collect Item
    }

    public void DeactivateDestroyObjUI()
    {
        destroyItemParent.SetActive(false);
    }

    public void UpdateDestroObjUI()
    {
        destroyObjProgressTxt.text = MissionManager.Progress + "/" + MissionManager.CurrentMission.Amount;
    }

    #endregion

    #region CollectPoints
    [Header ("Collect Points")]
    [SerializeField] GameObject collectPointsParent;
    [SerializeField] TMPro.TMP_Text collectPointsProgressTxt;
    public void ActivateCollectPointsUI()
    {
        collectPointsParent.SetActive(true);
        collectPointsProgressTxt.text = "0/" + MissionManager.CurrentMission.Amount;
    }

    public void DeactivateCollectPointsUI()
    {
        collectPointsParent.SetActive(false);
    }

    public void UpdateCollectPointsUI()
    {
        collectPointsProgressTxt.text = MissionManager.Progress + "/" + MissionManager.CurrentMission.Amount;
    }
    #endregion

    #region BringItem
    [SerializeField] GameObject BringItemParent;
    [SerializeField] Image BringItemItem;
    [SerializeField] Image BringItemGoal;
    [SerializeField] TMPro.TMP_Text BringItemProgressTxt;

    public void ActivateBringItemUI()
    {
        BringItemParent.SetActive(true);
        BringItemProgressTxt.text = "";
    }
    public void DeactivateBringItemUI()
    {
        BringItemParent.SetActive(false);
    }

    public void ChangeProgressState1()
    {
        BringItemProgressTxt.text = "Item collected!";
    }

    public void ChangeProgressState2()
    {
        BringItemProgressTxt.text = "Item delivered!";
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
        GameOverCanvas.SetActive(true);
        EndMessage.SetActive(true);
    }
    #endregion
}

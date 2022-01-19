using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameManager gameMng;

    [Header ("Basic UI")]
    [SerializeField] TMPro.TMP_Text score;
    [SerializeField] TMPro.TMP_Text multiplicator;
    //[SerializeField] TMPro.TMP_Text CurrentEnergy;
    [SerializeField] Image CurrentEnergy;

    public GameObject PauseCanvas;

    public GameObject IngameCanvas;
    

    [Header("GameOver")]
    public GameObject GameOverCanvas;
    [SerializeField] GameObject GameOverMessage;
    [SerializeField] TMPro.TMP_Text PointsMessage;
    [SerializeField] GameObject NewHighscoreMessage;
    [SerializeField] TMPro.TMP_Text CurrentHighscoreMessage;


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
        WinConMissions.SetActive(false);

    }

    

    #region Missions

    #region Basic UI
    [Header("UI Of All Missions")]
    [SerializeField] GameObject BasicMissionUI;
    [SerializeField] TMPro.TMP_Text missionTimeTxt;
    
    public void UpdateBasicMissionUI()
    {

        int seconds = (int)MissionManager.MissionTimeLeft;
        missionTimeTxt.text = "Time left: " + seconds.ToString();
    }
    public void ActivateBasicMissionUI()
    {
        BasicMissionUI.SetActive(true);
        int seconds = (int)MissionManager.CurrentMission.time;
        missionTimeTxt.text = "Time left: " + seconds;
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

        int seconds = (int)MissionStateNoMission.duration ;

        nextMissionTxt.text = "Next Mission: " + seconds;
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

    #region WinConMissions
    [Header("WinConMissions")]
    [SerializeField] GameObject WinConMissions;
    [SerializeField] TMPro.TMP_Text AllCompleted;
    [SerializeField] TMPro.TMP_Text MissionHexUnlocked;
    [SerializeField] TMPro.TMP_Text AlreadyUnlocked;
    [SerializeField] TMPro.TMP_Text Failed;
    [SerializeField] TMPro.TMP_Text MoreMissions;

    public IEnumerator UIHexUnlocked()
    {
        WinConMissions.SetActive(true);
        AllCompleted.gameObject.SetActive(true);
        MissionHexUnlocked.gameObject.SetActive(true);
        

        yield return new WaitForSeconds(2f);
       
        AllCompleted.CrossFadeAlpha(0, 0.5f, true); //Ausfaden
        MissionHexUnlocked.CrossFadeAlpha(0, 0.5f, true);

        yield return new WaitForSeconds(3f);

        AllCompleted.gameObject.SetActive(false);
        MissionHexUnlocked.gameObject.SetActive(false);

        MoreMissions.gameObject.SetActive(true);
        ReferenceLibary.MissionMng.NoMissionLeft.ReactiveMissions();

        yield return new WaitForSeconds(3f);
        MoreMissions.CrossFadeAlpha(0, 0.5f, true);

        yield return new WaitForSeconds(2f);

        MoreMissions.gameObject.SetActive(false);
        WinConMissions.SetActive(false);

        yield return null;
    }


    public IEnumerator UIHexAlreadyUnlocked()
    {
        WinConMissions.SetActive(true);
        AllCompleted.gameObject.SetActive(true);
        AlreadyUnlocked.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        AllCompleted.CrossFadeAlpha(0, 0.5f, true); //Ausfaden
        AlreadyUnlocked.CrossFadeAlpha(0, 0.5f, true);

        yield return new WaitForSeconds(3f);

        AllCompleted.gameObject.SetActive(false);
        AlreadyUnlocked.gameObject.SetActive(false);

        MoreMissions.gameObject.SetActive(true);
        ReferenceLibary.MissionMng.NoMissionLeft.ReactiveMissions();

        yield return new WaitForSeconds(3f);
        MoreMissions.CrossFadeAlpha(0, 0.5f, true);

        yield return new WaitForSeconds(2f);

        MoreMissions.gameObject.SetActive(false);
        WinConMissions.SetActive(false);


        yield return null;
    }

    public IEnumerator UIHexUnlockedFailed()
    {
        WinConMissions.SetActive(true);
        Failed.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        Failed.CrossFadeAlpha(0, 0.5f, true); //Ausfaden
        

        yield return new WaitForSeconds(3f);

        Failed.gameObject.SetActive(false);
        

        MoreMissions.gameObject.SetActive(true);
        ReferenceLibary.MissionMng.NoMissionLeft.ReactiveMissions();

        yield return new WaitForSeconds(3f);
        MoreMissions.CrossFadeAlpha(0, 0.5f, true);

        yield return new WaitForSeconds(2f);

        MoreMissions.gameObject.SetActive(false);
        WinConMissions.SetActive(false);
        yield return null;
    }


    #endregion

    #region WinConPoints
    [Header("WinConPoints")]
    [SerializeField] GameObject WinConPoints;
    [SerializeField] TMPro.TMP_Text AllPointsCollected;
    [SerializeField] TMPro.TMP_Text PointHexUnlocked;

    public IEnumerator WinConPointsCoroutine()
    {
        WinConPoints.SetActive(true);
        AllPointsCollected.gameObject.SetActive(true);
        PointHexUnlocked.gameObject.SetActive(true);

        AllPointsCollected.text = ReferenceLibary.WinconMng.PointsForWinCon + " points collected!";

        yield return new WaitForSeconds(2f);

        AllPointsCollected.CrossFadeAlpha(0, 0.5f, true); //Ausfaden
        PointHexUnlocked.CrossFadeAlpha(0, 0.5f, true);

        yield return new WaitForSeconds(2f);
        WinConPoints.SetActive(false);


        yield return null;
    }


    #endregion


    #region WinConHex
    [Header("WinConHex")]
    [SerializeField] GameObject WinConHex;
    [SerializeField] TMPro.TMP_Text HexCollected;
    [SerializeField] TMPro.TMP_Text HexHexUnlocked;

    public IEnumerator WinConHexCoroutine()
    {
        WinConHex.SetActive(true);
        HexCollected.gameObject.SetActive(true);
        PointHexUnlocked.gameObject.SetActive(true);


        yield return new WaitForSeconds(2f);

        HexCollected.CrossFadeAlpha(0, 0.5f, true); //Ausfaden
        HexHexUnlocked.CrossFadeAlpha(0, 0.5f, true);

        yield return new WaitForSeconds(2f);
        WinConHex.SetActive(false);


        yield return null;
    }




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
        //EndMessage.SetActive(true);
    }


    public IEnumerator TestCoroutine()
    {
        Debug.Log("TestCoroutine");
        StartCoroutine(GameOverCoroutine());
        yield return null;
    }


   public IEnumerator GameOverCoroutine()
   {
        //bool UpdateUI = true;

        yield return new WaitForSeconds(3f);
        
        
        
        IngameCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
        NewHighscoreMessage.SetActive(false);
        PointsMessage.text = ScoreManager.CurrentScore + " points";
        CurrentHighscoreMessage.text = "CurrentHighscore: " + PlayerPrefs.GetFloat("Highscore");
        
      /*  while (UpdateUI)
        {


            //Effects

        }*/
        
        yield return null;

   }

    public IEnumerator GameOverNewHighscoreCoroutine()
    {
        //bool UpdateUI = true;
       

        yield return new WaitForSeconds(3f);

        IngameCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
        PointsMessage.text = ScoreManager.CurrentScore + " points";
        NewHighscoreMessage.SetActive(true);
        CurrentHighscoreMessage.text = "";

        /*
        while (UpdateUI)
        {

            //Effects


        }
        */
        yield return null;

    }


    #endregion
}

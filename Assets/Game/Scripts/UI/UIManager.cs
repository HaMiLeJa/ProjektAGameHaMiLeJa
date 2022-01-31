using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [Header ("Basic UI")]
    [SerializeField] TMPro.TMP_Text score;
    [SerializeField] TMPro.TMP_Text multiplicator;
    //[SerializeField] Image CurrentEnergy;

    public GameObject PauseCanvas;

    public GameObject IngameCanvas;
    

    [Header("GameOver")]
    public GameObject GameOverCanvas;
  //  [SerializeField] GameObject GameOverMessage;
    [SerializeField] TMPro.TMP_Text PointsMessage;
    [SerializeField] GameObject NewHighscoreMessage;
    [SerializeField] TMPro.TMP_Text CurrentHighscoreMessage;

    

    void Start()
    {
        //StopAllCoroutines();

        //gameMng.onUIEnergyChange += UpdateEnergyUI;
        IngameCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);
        PauseCanvas.SetActive(false);


        UpdateUIScore(1);
        UpdateMultiplicatorUI(1);
        //UpdateEnergyUI();

        ActivateNoMissionUI();
        DeactivateBasicMissionUI();
        DeactivateCollectItemUI();
        DeactivateDestroyObjUI();
        DeactivateCollectPointsUI();
        DeactivateBringItemUI();
        WinConMissions.SetActive(false);

        pointsParent.SetActive(true);
        TMPro.TMP_Text[] FindPointTMPs;
        FindPointTMPs = pointsParent.GetComponentsInChildren<TMPro.TMP_Text>();

        allPointsTexMeshs.Clear();
        foreach (TMPro.TMP_Text obj in FindPointTMPs)
        {
            obj.text = "";
            obj.gameObject.SetActive(true);

            PointsAndState points = new PointsAndState();
            points.textMesh = obj;
            points.inUse = false;
            

            allPointsTexMeshs.Add(points);
        }
        freeTexmeshes = allPointsTexMeshs.Count;


        temporaryTxt.gameObject.SetActive(true);
        temporaryTxt.text = "";
        permanentTxt.gameObject.SetActive(true);
        permanentTxt.text = "";

    }

    

    #region Missions

    #region Basic UI
    [Header("UI Of All Missions")]
    [SerializeField] GameObject BasicMissionUI;
    [SerializeField] TMPro.TMP_Text missionTimeTxt;
    [SerializeField] TMPro.TMP_Text missionTimeNmbr;

    public void UpdateBasicMissionUI()
    {
        int seconds = (int)MissionManager.MissionTimeLeft;
        missionTimeNmbr.text = seconds.ToString();
    }
    public void ActivateBasicMissionUI()
    {
        BasicMissionUI.SetActive(true);
        missionTimeTxt.gameObject.SetActive(true);
    }

    public void DeactivateBasicMissionUI()
    {
        missionTimeTxt.gameObject.SetActive(false);
        BasicMissionUI.SetActive(false);
    }
    #endregion

    #region No Mission
    [Header("No Mission UI")]
    [SerializeField] TMPro.TMP_Text nextMissionTxt;
    [SerializeField] TMPro.TMP_Text nextMissionNbr;
    [SerializeField] GameObject noMissionParent;
    public void TimerUntilNexMission()
    {

        int seconds = (int)MissionStateNoMission.duration ;
        nextMissionNbr.text = seconds.ToString();
    }

    public void ActivateNoMissionUI()
    {
        noMissionParent.SetActive(true);
        nextMissionTxt.gameObject.SetActive(true);
    }
    public void DeactivateNoMissionUI()
    {
        noMissionParent.SetActive(false);
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

    #region WinCons

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
        yield return new WaitForSeconds(2f);

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
        yield return new WaitForSeconds(2f);

        WinConMissions.SetActive(true);
        AllCompleted.gameObject.SetActive(true);
       

        yield return new WaitForSeconds(2f);

        AllCompleted.CrossFadeAlpha(0, 0.5f, true); //Ausfaden
       

        //yield return new WaitForSeconds(1f);

        
        AlreadyUnlocked.gameObject.SetActive(true);
        // + Multiplikator
        ScoreManager.OnPermanentMultiplicatorUpdate(2f);


        yield return new WaitForSeconds(2f);
        AllCompleted.gameObject.SetActive(false);
        AlreadyUnlocked.CrossFadeAlpha(0, 0.5f, true);

        yield return new WaitForSeconds(3f);

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
        yield return new WaitForSeconds(2f);

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
    #endregion

    #region Energy
    /*
    public void UpdateEnergyUI()
    {
        CurrentEnergy.fillAmount = EnergyManager.CurrentEnergy / EnergyManager.Instance.MaxEnergyAmount;
    }*/
    #endregion


    #region Update Score and Multiplicator
    public void UpdateUIScore(float value)
    {
        score.text = ScoreManager.CurrentScore.ToString();
       // Debug.Log("Update Score UI");
    }

    public void UpdateMultiplicatorUI(float value)
    {
        multiplicator.text = "x" + ScoreManager.CurrentMultiplicator.ToString();
       // Debug.Log("Multiplicator");
    }
    #endregion

    #region Show points you currently collected

    [Header("Scoring UI")]
    [SerializeField] GameObject pointsParent;

    [SerializeField] float MaxFontSizePoints = 36;
    [Range(1, 20)]
    [SerializeField] int StartFontSizePoints = 15;
    [Range(30, 150)]
    [SerializeField] float sizeModfierPoints = 110;

  

     List<PointsAndState> allPointsTexMeshs = new List<PointsAndState>();
     int freeTexmeshes;
   

    public void PointsStarter(float value)  //find textmesh, set color, startscale etc
    {
        
        if (freeTexmeshes == 0) return;
        PointsAndState myTxt = SetTextmesh();
        
        SetStartValues(myTxt, value);
        
        StartCoroutine(AnimatePoints(myTxt));
        

    }

    PointsAndState SetTextmesh()
    {
        foreach (var obj in allPointsTexMeshs)
        {
            if (obj.inUse == true)
            {
                continue;
            }
            else
            {
                freeTexmeshes--;
                obj.inUse = true;
                return obj;
            }

        }

        return null;
    }


    void SetStartValues(PointsAndState txt, float value)
    {
        txt.textMesh.text = Mathf.RoundToInt(value * ScoreManager.CurrentMultiplicator) + " points!";

    }

    public IEnumerator AnimatePoints(PointsAndState txt)
    {

        float fontSize = StartFontSizePoints;
        
        while(fontSize <= MaxFontSizePoints)
        {

            fontSize = fontSize + (sizeModfierPoints * Time.deltaTime);
            txt.textMesh.fontSize = fontSize;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.4f);

         while (fontSize >= 5)
         {

            fontSize = fontSize - (sizeModfierPoints * Time.deltaTime);
            txt.textMesh.fontSize = fontSize;

            yield return new WaitForEndOfFrame();

         }

        txt.textMesh.text = "";
        txt.inUse = false;
        freeTexmeshes++; // Problemstelle
         
        yield return null;
    }

    #endregion

    #region show Multiplicator modifications

    #region temporary
    [Header ("Temporary Multiplicator Modification")]
    [SerializeField] TMPro.TMP_Text temporaryTxt;

    [HideInInspector] public float temporaryModification;

    public void UpdateTemporaryMultiplicator(float value)
    {
        temporaryModification += value;

        temporaryTxt.text = "+" + temporaryModification;

        if(temporaryModification <= 0)
        {
            temporaryTxt.text = "";
        }
    }
    #endregion

    #region permanent
    [Header ("Permanent Mulitplikator Modification")]
    [SerializeField] TMPro.TMP_Text permanentTxt;

    [SerializeField] float MaxFontSizeMulti = 22;
    [Range(1, 20)]
    [SerializeField] int StartFontSizeMulti = 15;
    [Range(30, 150)]
    [SerializeField] float sizeModfierMulti = 110;

    public void PermanentMulitplicatorStarter(float value)
    {
        permanentTxt.text = "+" + value + " permanent!";
        //StopCoroutine(AnimatePermanentMultiplicator());
        StartCoroutine(AnimatePermanentMultiplicator());
    }


    IEnumerator AnimatePermanentMultiplicator()
    {

        float fontSize = StartFontSizeMulti;

        while (fontSize <= MaxFontSizeMulti)
        {

            fontSize = fontSize + (sizeModfierMulti * Time.deltaTime);
            permanentTxt.fontSize = fontSize;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.6f);

        while (fontSize >= 5)
        {

            fontSize = fontSize - (sizeModfierMulti * Time.deltaTime);
            permanentTxt.fontSize = fontSize;

            yield return new WaitForEndOfFrame();

        }

        permanentTxt.text = "";

        yield return null;
    }

    #endregion

    #endregion

    #region EndScreen
   /* public void ShowEndMessage()
    {
        GameOverCanvas.SetActive(true);
        //EndMessage.SetActive(true);
    }


    public IEnumerator TestCoroutine()
    {
        Debug.Log("TestCoroutine");
        StartCoroutine(GameOverCoroutine());
        yield return null;
    }*/


   public IEnumerator GameOverCoroutine()
   {
        //bool UpdateUI = true;

        yield return new WaitForSeconds(3f);
        
        
        
        IngameCanvas.SetActive(false);
        GameOverCanvas.SetActive(true);
        NewHighscoreMessage.SetActive(false);
        PointsMessage.text = ScoreManager.CurrentScore + " points";
        CurrentHighscoreMessage.text = PlayerPrefs.GetFloat("Highscore").ToString();
        
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

[System.Serializable]
public class PointsAndState
{
    public TMPro.TMP_Text textMesh;

    public bool inUse = false;
}

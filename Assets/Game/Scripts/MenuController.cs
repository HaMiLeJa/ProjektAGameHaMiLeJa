using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Levels to load")] 
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSaveGameDialog = null;

    private void Start()
    {
        amount = PlayerPrefs.GetInt("WinConPoints") + PlayerPrefs.GetInt("WinConHex") + PlayerPrefs.GetInt("WinConMissions");

        ManageHextileAmount(ProgressLv2, 3, Level2Image);
        ManageHextileAmount(ProgressLv3, 6, Level3Image);
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }


    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene((levelToLoad));
        }
        else
        {
            noSaveGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }


    [Header ("Choose Level")]
    [SerializeField] TMPro.TMP_Text ProgressLv2;
    [SerializeField] TMPro.TMP_Text ProgressLv3;

    [SerializeField] Image Level2Image;
    [SerializeField] Image Level3Image;

    private int amount;

    void ManageHextileAmount(TMPro.TMP_Text lv, int goalAmount, Image levelimage)
    {
       

        if(amount <= goalAmount-1)
        {
            lv.text = amount + "/"+ goalAmount + "Hextiles";
        }
        else if(amount >= goalAmount)
        {

            levelimage.color = Color.white;
            //Color tempColor = levelimage.color;
            // tempColor.a = 1f;

            // lv.text = "Not available yet";
            lv.gameObject.SetActive(false);
        }
       
    }

    [SerializeField] TMPro.TMP_Text Message;

    public void Level2AvailabilityMessage()
    {
        Message.gameObject.SetActive(true);

        if (amount <= 2)
            Message.text = "Collect Hextiles to unlock this Level";
        else
        {
            Message.text = "Currently only Level 1 is available";
        }
    }

    public void Level3AvailabilityMessage()
    {
        Message.gameObject.SetActive(true);

        if (amount <= 6)
            Message.text = "Collect Hextiles to unlock this Level";
        else
        {
            Message.text = "Currently only Level 1 is available";
        }
    }
}
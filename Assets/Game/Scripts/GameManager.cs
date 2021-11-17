using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Keys on Controller
    [Tooltip ("Controller Input: Use X, Y, A or B.")]
    public string Jump = "B";
    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string Dash = "X";
    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string ShadowDash = "Y";
    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string SuperDash = "A";

    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string DownDash = "RightBumper";

    #endregion




    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(currentScene);
        }
    }


}

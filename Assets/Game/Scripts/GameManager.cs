using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public string SaveEnergy = "LeftBumper";

    #endregion


    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

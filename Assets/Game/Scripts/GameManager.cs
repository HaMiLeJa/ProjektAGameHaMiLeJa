using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Keys on Controller
    [Tooltip ("Controller Input: Use X, Y, A or B.")]
    public string Jump;
    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string Dash;
    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string ShadowDash;
    [Tooltip("Controller Input: Use X, Y, A or B.")]
    public string SuperDash;

    [Tooltip("For Debug use")]
    public string Stop;
    #endregion


    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypMenuManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    public void OnButtonA()
    {
        SceneManager.LoadScene("LevelprototypJanina");
    }

    public void OnButtonB()
    {
        SceneManager.LoadScene("LevelPrototypLei");

    }

    public void OnButtonKamera()
    {
        SceneManager.LoadScene("Kameraszene");

    }

    public void OnButtonCurves()
    {
        SceneManager.LoadScene("Kurvenszene");

    }
}

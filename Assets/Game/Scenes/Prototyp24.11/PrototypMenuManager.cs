using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypMenuManager : MonoBehaviour
{
    
    public void OnButtonA()
    {
        SceneManager.LoadScene("PrototyplevelJanina");
    }

    public void OnButtonB()
    {
        SceneManager.LoadScene("PrototypLevelLei");

    }

    public void OnButtonKamera()
    {
        SceneManager.LoadScene("Kamerszene");

    }

    public void OnButtonCurves()
    {
        SceneManager.LoadScene("Kurvenszene");

    }
}

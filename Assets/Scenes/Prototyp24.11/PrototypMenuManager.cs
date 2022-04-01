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
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestingGround");
    }

    public void OnButtonB()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestingGround");

    }

    public void OnButtonKamera()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestingGround");

    }

    public void OnButtonCurves()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestingGround");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterimMenuManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    public void OnButtonA()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelInterimJanina");
    }

   
}

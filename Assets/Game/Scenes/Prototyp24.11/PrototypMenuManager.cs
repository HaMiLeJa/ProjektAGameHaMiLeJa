using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypMenuManager : MonoBehaviour
{
    
    public void OnButtonA()
    {
        SceneManager.LoadScene("ArbeitsszeneJanina");
    }

    public void OnButtonB()
    {
        SceneManager.LoadScene("ArbeitsszeneLeiPrototypeCorrectWorldSetup");

    }
}

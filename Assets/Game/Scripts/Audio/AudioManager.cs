using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   

    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("masterVolume");
    }

   
    

    
}

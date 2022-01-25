using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public HexEffectAudioManager HexAudMng;


    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("masterVolume");
        HexAudMng = this.GetComponentInChildren<HexEffectAudioManager>();

    }

   
    

    
}

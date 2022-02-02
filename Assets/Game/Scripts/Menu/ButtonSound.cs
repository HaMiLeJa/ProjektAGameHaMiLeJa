using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
   [SerializeField] AudioSource source;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


    public void PlayClickSound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}

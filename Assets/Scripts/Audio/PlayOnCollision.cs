using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnCollision : MonoBehaviour
{
    AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == ReferenceLibrary.Player)
        {
            if (myAudioSource.isPlaying == false)
                myAudioSource.Play();
        }
    }
}

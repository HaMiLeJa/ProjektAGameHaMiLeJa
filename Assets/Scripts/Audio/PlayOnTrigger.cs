using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnTrigger : MonoBehaviour
{
    [SerializeField] AudioSource myAudioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ReferenceLibrary.Player)
        {
            if (myAudioSource.isPlaying == false)
                myAudioSource.Play();
        }
    }
}

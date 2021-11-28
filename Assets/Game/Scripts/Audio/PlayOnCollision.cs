using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnCollision : MonoBehaviour
{
    AudioSource myAudioSource;
    [SerializeField] GameObject player;

    private void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            if (myAudioSource.isPlaying == false && AudioManager.Instance.allowAudio == true)
                myAudioSource.Play();
        }
    }
}

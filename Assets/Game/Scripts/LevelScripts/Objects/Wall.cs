using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public ScriptableLevelObject settings;
    int hitCounter;
    [SerializeField] AudioSource myAudioSource;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {
            if (hitCounter >= 20)
            {
                ScoreManager.OnScoring?.Invoke(settings.value/20);
                return;
            }
            

            float scoreValue = ((hitCounter * 0.05f)) * settings.value;

            ScoreManager.OnScoring?.Invoke(settings.value - scoreValue);
            hitCounter++;

            if(myAudioSource.isPlaying == false)
            {
                myAudioSource.Play();
            }

        }

    }

}

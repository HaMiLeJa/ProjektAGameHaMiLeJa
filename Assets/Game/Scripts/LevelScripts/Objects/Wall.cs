using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public ScriptableLevelObject settings;
    int hitCounter;
    [SerializeField] AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource.clip = settings.Clip;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {
            if (hitCounter >= 15)
            {
                ScoreManager.OnScoring?.Invoke(settings.value/15);
                return;
            }
            

            float scoreValue = ((hitCounter * 0.05f)) * settings.value; //bei 0.05 sind total 20 schritte möglich

            ScoreManager.OnScoring?.Invoke(settings.value - scoreValue);
            hitCounter++;

            if(myAudioSource.isPlaying == false)
            {
                myAudioSource.Play();
            }

        }

    }

}

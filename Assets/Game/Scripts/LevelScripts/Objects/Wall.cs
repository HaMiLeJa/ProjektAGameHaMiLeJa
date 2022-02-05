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
        //myAudioSource.pitch = 1;
       myAudioSource.clip = settings.Clip;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {

            //Debug.Log("Collision");

            if (hitCounter >= 15)
            {
                float points = settings.value / 15;
                if (points < 1) points = 1;

                ScoreManager.OnScoring?.Invoke(points);

                
            }
            else
            {
                float scoreValue = ((hitCounter * 0.05f)) * settings.value; //bei 0.05 sind total 20 schritte möglich

                float points = settings.value - scoreValue;
                if (points < 1) points = 1;

                ScoreManager.OnScoring?.Invoke(points);
            }
            

           


            hitCounter++;

            if(myAudioSource.isPlaying == false)
            {
                myAudioSource.pitch = Random.Range(0.8f, 1.6f);
                myAudioSource.Play();
            }

        }

    }

}

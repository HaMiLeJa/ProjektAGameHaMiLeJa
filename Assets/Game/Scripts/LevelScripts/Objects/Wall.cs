using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public ScriptableLevelObject settings;
    int hitCounter;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {
            if (hitCounter >= 10) return;

            

            float scoreValue = ((hitCounter * 0.1f)) * settings.value;

            ScoreManager.OnScoring?.Invoke(settings.value - scoreValue);
            hitCounter++;
            //Effect
            //music
        }

    }

}

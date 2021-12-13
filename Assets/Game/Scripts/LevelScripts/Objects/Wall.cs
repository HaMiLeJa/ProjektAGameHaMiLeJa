using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public ScriptableLevelObject settings;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {
            ScoreManager.OnScoring?.Invoke(settings.value);
            //Effect
            //music
        }

    }

}

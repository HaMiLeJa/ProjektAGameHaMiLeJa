using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public bool allowAudio = true;


    private void Start()
    {

    }

    #region Singleton
    public static AudioManager Instance;
   
    private void Awake()
    {
        if (AudioManager.Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            AudioManager.Instance = this;
        }
    }
    #endregion
}

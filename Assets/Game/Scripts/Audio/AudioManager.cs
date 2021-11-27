using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    bool allowAudio = true;

    [SerializeField] bool allowAudioInspector = true;

    private void Start()
    {
#if UNITY_EDITOR
        allowAudio = allowAudioInspector;
#endif
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public HexEffectAudioManager HexAudMng;


    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("masterVolume");
        HexAudMng = this.GetComponentInChildren<HexEffectAudioManager>();

        AllMissionAudioSources = MissionAudio.GetComponents<AudioSource>();
    }





    //Mission Audio

   
    [SerializeField] GameObject MissionAudio;
    AudioSource[] AllMissionAudioSources;

    public void PlayMissionSound(AudioClip clip, AudioMixerGroup group)
    {
        AudioSource source = FindFreeMissionAudioSource();

        if (source == null) return;

        source.outputAudioMixerGroup = group;
        source.clip = clip;
        source.Play();

    }

    AudioSource FindFreeMissionAudioSource()
    {
        foreach (AudioSource source in AllMissionAudioSources)
        {
            if (source.isPlaying == true)
                continue;
            else
                return source;

        }

        return null;

    }

    [Space]
    [SerializeField] AudioSource gameStateSource;
    public void PlayGameStateSound(AudioClip clip, AudioMixerGroup group)
    {
        gameStateSource.outputAudioMixerGroup = group;
        gameStateSource.clip = clip;
        gameStateSource.Play();
    }

}

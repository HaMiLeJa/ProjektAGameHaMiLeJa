using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HexEffectAudioManager : MonoBehaviour
{

    public List<StringAudiofileClass> AllHexClips = new List<StringAudiofileClass>();
    Dictionary<HexType, AudioClip> AllHexTypesAndClips = new Dictionary<HexType, AudioClip>();


    public List<HexTypeAndMixerGroup> AllOutputGroups = new List<HexTypeAndMixerGroup>();
    Dictionary<HexType, AudioMixerGroup> AllOutputGroupsForHextypes = new Dictionary<HexType, AudioMixerGroup>();

    //int freeAudioSources;

    [System.Serializable]
    public class HexTypeAndMixerGroup
    {
        public HexType type;
        public AudioMixerGroup group;
    }


    AudioSource[] AllHexAudioSources;

    void Start()
    {
        AllHexAudioSources = this.GetComponents<AudioSource>();
        
      
       // freeAudioSources = AllHexAudioSources.Length;

        foreach(HexTypeAndMixerGroup file in AllOutputGroups)
        {
            AllOutputGroupsForHextypes.Add(file.type, file.group);
        }


        foreach (StringAudiofileClass file in AllHexClips)
        {
            AllHexTypesAndClips.Add(file.type, file.clip);
        }
    }

   


    public void PlayHex(HexType type)
    {
       

        AudioSource mySource = SetAudioSource();


        if (mySource == null) return;

        mySource.outputAudioMixerGroup = AllOutputGroupsForHextypes[type];
        mySource.pitch = UnityEngine.Random.Range(0.8f, 1.6f);
        mySource.clip = AllHexTypesAndClips[type];
        mySource.Play();

        // audiosource.play(AllHextypes.type) oder so
    }

    AudioSource SetAudioSource()
    {
        foreach(AudioSource obj in AllHexAudioSources)
        {
            if(obj.isPlaying == true)
            {
                continue;
            }
            else
            {
                //freeAudioSources++;
                
                

                return obj;
            }

        }

        Debug.Log("Not Enough Hex Audio Sources");
        return null;
    }

}

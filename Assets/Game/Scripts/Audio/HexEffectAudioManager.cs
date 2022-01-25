using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexEffectAudioManager : MonoBehaviour
{

    public List<StringAudiofileClass> AllHexClips = new List<StringAudiofileClass>();
   // List<AudioSource> allHexAudioSources = new List<AudioSource>();

    Dictionary<HexType, AudioClip> AllHexTypesAndClips = new Dictionary<HexType, AudioClip>();
   // int freeAudioSources;



    AudioSource[] AllHexAudioSources;

    void Start()
    {
        AllHexAudioSources = this.GetComponents<AudioSource>();
        
      
       // freeAudioSources = AllHexAudioSources.Length;



        foreach (StringAudiofileClass file in AllHexClips)
        {
            AllHexTypesAndClips.Add(file.type, file.clip);
        }
    }

   


    public void PlayHex(HexType type)
    {
       

        AudioSource mySource = SetAudioSource();


        if (mySource == null) return;
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

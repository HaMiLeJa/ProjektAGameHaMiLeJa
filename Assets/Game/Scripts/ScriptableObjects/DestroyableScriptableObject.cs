using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class DestroyableScriptableObject : ScriptableObject
{  
    public  GameObject BrokenPrefab;
    public AudioClip CollisionClip;
    public  AudioClip DestructionClip;
    [Space]
    public  float ExplosiveForce = 1000;
   public float ExplosiveRadius = 2;
   public  float FadeSpeed = 3.1f;
   public  float DestroyDelay = 5f;
   public  float SleepCheckDelay = 0.5f;
    [Space]
   public bool Respawn = true;
   public float resetTimer = 3; 
    [Space]
   public float DestroyValue = 50;

    public float CollisionValue = 50;
    
    [Space]
    [Tooltip("Allow Destruction after \"Hitamount\" hits ")]
    public bool AllowAutomatedDestruction = true;
    public int HitAmount = 3;

    [Space]
    public bool ChangeMaterial = false;
    public Material Material01;
    public Material Material02;
    public Material Material03;
    public Material Material04;
    [Space]
    public AudioMixerGroup CollisionGroup;
    public AudioMixerGroup DestroyGroup;

}

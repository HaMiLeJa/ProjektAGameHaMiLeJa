using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Jobs;

public class CollectableManager : MonoBehaviour
{
    public delegate void RespawnCollectables();
    public static RespawnCollectables OnRespawnCollectables;
    public static bool rotateCollectablesInEditor = true;
    [InfoBox("The list will empty when the game starts. This is intended because it used native Container for multithreading and better cpu cache lining/ reading")]
    [BoxGroup("Debug")] [Tooltip("PreMade Array that has all the Transforms that then gets mem copied by the native Transformaccessarray")] 
    [SerializeField] public Transform[] hasAllTheCollectableHexParentTransformsBeforeStart;
    [BoxGroup("Debug")] [Tooltip("PreMade Array that has all the bools at the start. When the Object is there but disabled it safes it and hands its data over a native Byte array")] 
    [SerializeField] public byte[] hasAllTheCollectableActiveBoolsBeforeStart;
    [BoxGroup("Debug")]  [Tooltip("To safe some Bytes on the Objects, we have the relevant Hex components in an array. This gets hands over to the static variant")] 
    [SerializeField] public Hex[] haslAllTheHexScriptsForCollectablesUseBeforeStart;
    [BoxGroup("Debug")][SerializeField] private byte[] hasAllTheRandomSpeedsBeforeStart;
    [BoxGroup("Debug")] [SerializeField] public Transform[] hasAllTheCollectablesTransformsBeforeStart;
    [InfoBox("Automaticly updates when slider change. Randomizes between those numbers so every Collectable gets a different rotation. Gets converted to bytes, so everything after the . gets cut")]
    [BoxGroup("Configure")] [MinMaxSlider(0,255)] [SerializeField]  private Vector2 rotationRandomBetween = new Vector2(30, 120);
    
    
    
    private static NativeQueue<int> hasAllTheValidSpawnAbleHexID;
    TransformAccessArray hasAllTheCollectableHexParentTransforms;
    private TransformAccessArray hasAllTheCollectablesTransforms;
    NativeArray<byte> hasAllTheCollectableActiveBools;
    private static Hex[] haslAllTheHexScriptsForCollectablesUse;
    private NativeArray<byte> hasAllTheRandomSpeeds;
    private JobHandle rotationJOB;
    private void Awake() => SetNativeContainer();

    void Start()
    {
        OnRespawnCollectables = null;
        OnRespawnCollectables += spawnCollectableObjects;
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        for (int i = 0; i < hasAllTheRandomSpeedsBeforeStart.Length; i++) hasAllTheRandomSpeedsBeforeStart[i] = (byte)UnityEngine.Random.Range((int)rotationRandomBetween.x, (byte)rotationRandomBetween.y);
    }

    [NaughtyAttributes.Button()] public void fillCollectableListsBeforeStart()
    {
        //------------ Fill local scope Lists for easier handling ---------//
        HexAutoTiling hexAutoTiling = FindObjectOfType<HexAutoTiling>();   //get all the hex Objectgs
        HashSet<Collectable> collectablesHashSet = new HashSet<Collectable>();   // make a new local hashset, to guarantee everything we add is unique
        
        foreach (Transform hexTransform in hexAutoTiling.hasAllTheHexGameObjectsTransformsBeforeStart)
        {
            foreach (Collectable collectable in hexTransform.GetComponentsInChildren<Collectable>())
                collectablesHashSet.Add(collectable);  //adding process to hashset
        }
        List<Collectable> collectableList = collectablesHashSet.ToList();  //convert that hashset to a list. Not needed but its easier this way and it is a editor tool anayway 
        
        
        
        //------------ Resize Arrays ---------//
        int newSize = collectableList.Count;
           Array.Resize(ref hasAllTheCollectableHexParentTransformsBeforeStart, newSize);
          Array.Resize(ref haslAllTheHexScriptsForCollectablesUseBeforeStart, newSize);
          Array.Resize(ref hasAllTheCollectableActiveBoolsBeforeStart, newSize);
          Array.Resize(ref hasAllTheCollectablesTransformsBeforeStart, newSize);
          Array.Resize(ref hasAllTheRandomSpeedsBeforeStart, newSize);

          for (int i = 0; i < newSize; i++) hasAllTheRandomSpeedsBeforeStart[i] = (byte)UnityEngine.Random.Range((int)rotationRandomBetween.x, (byte)rotationRandomBetween.y);

          for (var index = 0; index < collectableList.Count; index++)   //Transforms for rotationRandomBetween
              hasAllTheCollectablesTransformsBeforeStart[index] = collectableList[index].gameObject.transform;
          

          //------------ Fill Serialized Container that then will get converted to Native Container ---------//
        int counter = 0;  //index id counter. The Collectable List stays fixed
        foreach (Collectable collectableScript in collectableList) 
        {
           Hex hexScript = collectableScript.GetComponentInParent<Hex>();
           hasAllTheCollectableHexParentTransformsBeforeStart[counter] = hexScript.gameObject.transform;  //Fill with HexObject Transform
           haslAllTheHexScriptsForCollectablesUseBeforeStart[counter] = hexScript;   //fill with the Script "Hex" so we have a premade List and dont need get component anymore
           if (collectableScript.enabled)  //check if the script is enabled
               hasAllTheCollectableActiveBoolsBeforeStart[counter] = 1;    //if yes, we add true to the bool list (needed for multithreading later)
           else hasAllTheCollectableActiveBoolsBeforeStart[counter] = 0;  //if not active we add false
           counter++;   //we add one more to the indexid counter
        }

        //------------ Change the Serialized Propertys---------//
        SerializedObject serializedHex;  // for setting the Collectable 
        SerializedObject serializedCollectable; // for setting the array index, named CollectableIndexID
        
        for (int i = 0; i < haslAllTheHexScriptsForCollectablesUseBeforeStart.Length; i++)
        {
            serializedHex = new SerializedObject(haslAllTheHexScriptsForCollectablesUseBeforeStart[i]);
            if (haslAllTheHexScriptsForCollectablesUseBeforeStart[i].gameObject)
            {
                serializedHex.FindProperty("MyCollectable").objectReferenceValue = collectableList[i].gameObject; 
                serializedHex.ApplyModifiedPropertiesWithoutUndo();
                
                serializedCollectable = new SerializedObject(collectableList[i]);
                serializedCollectable.FindProperty("CollectableIndexID").intValue = i;
                serializedCollectable.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        //------------ Disable all deactived collectables Gameobjects---------//
        for (int i = 0; i < hasAllTheCollectableActiveBoolsBeforeStart.Length; i++)
        {   //not reaaaally needed it is just a safety check
            if(hasAllTheCollectableActiveBoolsBeforeStart[i] == 0) collectableList[i].gameObject.SetActive(false);
        }
    }
    [NaughtyAttributes.Button()] public void ToogleRotationCollectableInEditor()
    {
      rotateCollectablesInEditor = !rotateCollectablesInEditor;
    }
  
    private void OnDrawGizmos()
    {
        if (Application.isPlaying || !rotateCollectablesInEditor ) return;
        if (rotationJOB.IsCompleted == false) return;
            NativeArray<byte> RandomSpeedsForEditor =
                new NativeArray<byte>(hasAllTheRandomSpeedsBeforeStart, Allocator.TempJob);
            rotateCollectablesJob rotateJob = new rotateCollectablesJob
            {
                RandomSpeed =  RandomSpeedsForEditor,
                deltaTime = Time.deltaTime
            };
            TransformAccessArray JustForJob = new TransformAccessArray(hasAllTheCollectablesTransformsBeforeStart,12);
            rotationJOB = rotateJob.Schedule(JustForJob);
            rotationJOB.Complete();
            RandomSpeedsForEditor.Dispose();
            JustForJob.Dispose();
        
        
    }
#endif
    void Update()
    {  
        if (Input.GetKeyDown(KeyCode.M)) spawnCollectableObjects();
        if (rotationJOB.IsCompleted == false) return;
        rotateCollectablesJob rotateJob = new rotateCollectablesJob
            {
                deltaTime = Time.deltaTime,
                RandomSpeed =  hasAllTheRandomSpeeds
            };
            rotationJOB = rotateJob.Schedule(hasAllTheCollectablesTransforms);
            rotationJOB.Complete();

    }

    private void SetNativeContainer()
    {
        if (!Application.isPlaying) return;
        hasAllTheValidSpawnAbleHexID= new NativeQueue<int>(Allocator.Persistent);  //Queue has the Objects that are Valid for Spawn
        hasAllTheCollectableHexParentTransforms = new TransformAccessArray(hasAllTheCollectableHexParentTransformsBeforeStart, 12); //Native Transforms are much faster than normal
        hasAllTheCollectableActiveBools = new NativeArray<byte>(hasAllTheCollectableActiveBoolsBeforeStart, Allocator.Persistent); // Keep a native bool list for checking if ValidSpawn
        haslAllTheHexScriptsForCollectablesUse = haslAllTheHexScriptsForCollectablesUseBeforeStart;  //better have an static List premade than having a getcomponend of ref type
        hasAllTheCollectablesTransforms = new TransformAccessArray(hasAllTheCollectablesTransformsBeforeStart, 12);
        hasAllTheRandomSpeeds = new NativeArray<byte>(hasAllTheRandomSpeedsBeforeStart,Allocator.Persistent);
        
        hasAllTheCollectablesTransformsBeforeStart = null;
        hasAllTheCollectableHexParentTransformsBeforeStart = null;   //null not needed  Serialized Container
        hasAllTheCollectableActiveBoolsBeforeStart = null; //  since we have native Containter
        haslAllTheHexScriptsForCollectablesUseBeforeStart = null;
        hasAllTheRandomSpeedsBeforeStart = null;
    } 
    private void OnDestroy()
    {
        if (!Application.isPlaying) return;
   
        //Dispose all NativeContainer
        hasAllTheValidSpawnAbleHexID.Dispose();
        hasAllTheCollectableHexParentTransforms.Dispose();
        hasAllTheCollectableActiveBools.Dispose();
        hasAllTheCollectablesTransforms.Dispose();
        hasAllTheRandomSpeeds.Dispose();
    }
    public void CollectableCollected( float energyValue , int collectableIndexID)
    {   //Invoked by the Collectable when the Collectable gets collected 
        hasAllTheCollectableActiveBools[collectableIndexID] = 0;  
        EnergyManager.energyGotHigher = true;
        StartCoroutine(ReferenceLibrary.EnergyMng.ModifyEnergy(energyValue));
        ReferenceLibrary.AudMng.HexAudMng.PlayHex(HexType.DefaultCollectable);
        hasAllTheCollectablesTransforms[collectableIndexID].gameObject.SetActive(false);
    }
    public void spawnCollectableObjects()
    {
        HexCollectablePosJob spawnCheckJob = new HexCollectablePosJob
        {
            hasAllTheHexCollectableActiveBoolsJob = hasAllTheCollectableActiveBools,
            hasAllTheValidSpawnAbleHexesIDJob =  hasAllTheValidSpawnAbleHexID.AsParallelWriter(),
            PlayerPosJob = ReferenceLibrary.PlayerPosition
        };
       JobHandle spawnCheckhandle = spawnCheckJob.Schedule(hasAllTheCollectableHexParentTransforms);
       spawnCheckhandle.Complete();
       
       Debug.Log("counterCollectableRespawned " + hasAllTheValidSpawnAbleHexID.Count);
       
       while (!hasAllTheValidSpawnAbleHexID.IsEmpty())
           haslAllTheHexScriptsForCollectablesUse[hasAllTheValidSpawnAbleHexID.Dequeue()].MyCollectable.SetActive(true);
    }
}
[BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
struct rotateCollectablesJob : IJobParallelForTransform
{
    [Unity.Collections.ReadOnly] public float deltaTime;
    [Unity.Collections.ReadOnly] public NativeArray<byte> RandomSpeed;
    public void Execute(int index, TransformAccess transform)
    {
        transform.localRotation *= Quaternion.Euler(0, RandomSpeed[index]*deltaTime, 0);
    }
}

[BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
public struct HexCollectablePosJob : IJobParallelForTransform
{
    public NativeArray<byte> hasAllTheHexCollectableActiveBoolsJob;
    [NativeDisableParallelForRestriction] [WriteOnly] public NativeQueue<int>.ParallelWriter hasAllTheValidSpawnAbleHexesIDJob;
    [Unity.Collections.ReadOnly] public Vector3 PlayerPosJob;
    private Vector3 Verbindungsvector;
    public void Execute(int index, TransformAccess hasAllTheHexCollectableTransform)
    {
        Verbindungsvector = PlayerPosJob - hasAllTheHexCollectableTransform.position;
        if (hasAllTheHexCollectableActiveBoolsJob[index] == 0 && Mathf.Sqrt(Mathf.Pow(Verbindungsvector.x, 2) + Mathf.Pow(Verbindungsvector.y, 2) + Mathf.Pow(Verbindungsvector.z, 2)) >= 100f)
        {
            hasAllTheValidSpawnAbleHexesIDJob.Enqueue(index);
            hasAllTheHexCollectableActiveBoolsJob[index] = 1;
        }
    }
}
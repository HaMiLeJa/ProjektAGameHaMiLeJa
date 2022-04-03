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
    [NaughtyAttributes.InfoBox("The list will empty when the game starts. This is intended because it used native Container for multithreading and better cpu cache lining/ reading")]
    [BoxGroup("Debug")] [Tooltip("PreMade Array that has all the Transforms that then gets mem copied by the native Transformaccessarray")] 
    [SerializeField] public Transform[] hasAllTheCollectableHexParentTransformsBeforeStart;
    [BoxGroup("Debug")] [Tooltip("PreMade Array that has all the bools at the start. When the Object is there but disabled it safes it and hands its data over a native Byte array")] 
    [SerializeField] public byte[] hasAllTheCollectableActiveBoolsBeforeStart;
    [BoxGroup("Debug")]  [Tooltip("To safe some Bytes on the Objects, we have the relevant Hex components in an array. This gets hands over to the static variant")] 
    [SerializeField] public Hex[] haslAllTheHexScriptsForCollectablesUseBeforeStart;

    private static NativeQueue<int> hasAllTheValidSpawnAbleHexID;
    TransformAccessArray hasAllTheCollectableHexParentTransforms;
    NativeArray<byte> hasAllTheCollectableActiveBools;
    private static Hex[] haslAllTheHexScriptsForCollectablesUse;
    
    private void Awake() => SetNativeContainer();
    void Start()
    {
        OnRespawnCollectables = null;
        OnRespawnCollectables += spawnCollectableObjects;
    }
    
#if UNITY_EDITOR
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
        Array.Resize(ref hasAllTheCollectableHexParentTransformsBeforeStart, collectableList.Count);
          Array.Resize(ref haslAllTheHexScriptsForCollectablesUseBeforeStart, collectableList.Count);
          Array.Resize(ref hasAllTheCollectableActiveBoolsBeforeStart, collectableList.Count);
         
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) spawnCollectableObjects();
    }
#endif
    
    private void SetNativeContainer()
    {
        hasAllTheValidSpawnAbleHexID= new NativeQueue<int>(Allocator.Persistent);  //Queue has the Objects that are Valid for Spawn
        hasAllTheCollectableHexParentTransforms = new TransformAccessArray(hasAllTheCollectableHexParentTransformsBeforeStart); //Native Transforms are much faster than normal
        hasAllTheCollectableActiveBools = new NativeArray<byte>(hasAllTheCollectableActiveBoolsBeforeStart, Allocator.Persistent); // Keep a native bool list for checking if ValidSpawn
        haslAllTheHexScriptsForCollectablesUse = haslAllTheHexScriptsForCollectablesUseBeforeStart;  //better have an static List premade than having a getcomponend of ref type

        hasAllTheCollectableHexParentTransformsBeforeStart = null;   //null not needed  Serialized Container
        hasAllTheCollectableActiveBoolsBeforeStart = null; //  since we have native Containter
        haslAllTheHexScriptsForCollectablesUseBeforeStart = null;  
    } 
    private void OnDestroy()
    {   //Dispose all NativeContainer
        hasAllTheValidSpawnAbleHexID.Dispose();
        hasAllTheCollectableHexParentTransforms.Dispose();
        hasAllTheCollectableActiveBools.Dispose();
    }
    public void CollectableCollected(GameObject item, float energyValue , int collectableIndexID)
    {   //Invoked by the Collectable when the Collectable gets collected 
        hasAllTheCollectableActiveBools[collectableIndexID] = 0;  
        EnergyManager.energyGotHigher = true;
        StartCoroutine(ReferenceLibrary.EnergyMng.ModifyEnergy(energyValue));
        ReferenceLibrary.AudMng.HexAudMng.PlayHex(HexType.DefaultCollectable);
        item.SetActive(false);
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


[BurstCompile]
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
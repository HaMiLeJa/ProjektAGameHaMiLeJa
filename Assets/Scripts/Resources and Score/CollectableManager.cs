using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] public Transform[] hasAllTheHexCollectablePositionBeforeStart;
    [SerializeField] public byte[] hasAllTheHexCollectableActiveBoolsBeforeStart;
    [SerializeField] public Hex[] haslAllTheHexScriptsForCollectablesBeforeStart;

    private static NativeQueue<int> hasAllTheValidSpawnAbleHexesID;
    TransformAccessArray hasAllTheHexCollectablePosition;
    NativeArray<byte> hasAllTheHexCollectableActiveBools;
    private static Hex[] haslAllTheHexScriptsForCollectables;
    
    private void Awake() => SetNativeContainer();
    void Start()
    {
        OnRespawnCollectables = null;
        OnRespawnCollectables += spawnCollectableObjects;
    }
    
#if UNITY_EDITOR
    [NaughtyAttributes.Button()] public void fillDictionaryBeforeStart()
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
        Array.Resize(ref hasAllTheHexCollectablePositionBeforeStart, collectableList.Count);
          Array.Resize(ref haslAllTheHexScriptsForCollectablesBeforeStart, collectableList.Count);
          Array.Resize(ref hasAllTheHexCollectableActiveBoolsBeforeStart, collectableList.Count);
         
        //------------ Fill Serialized Container that then will get converted to Native Container ---------//
        int counter = 0;  //index id counter. The Collectable List stays fixed
        foreach (Collectable collectableScript in collectableList) 
        {
           Hex hexScript = collectableScript.GetComponentInParent<Hex>();
           hasAllTheHexCollectablePositionBeforeStart[counter] = hexScript.gameObject.transform;  //Fill with HexObject Transform
           haslAllTheHexScriptsForCollectablesBeforeStart[counter] = hexScript;   //fill with the Script "Hex" so we have a premade List and dont need get component anymore
           if (collectableScript.enabled)  //check if the script is enabled
               hasAllTheHexCollectableActiveBoolsBeforeStart[counter] = 1;    //if yes, we add true to the bool list (needed for multithreading later)
           else hasAllTheHexCollectableActiveBoolsBeforeStart[counter] = 0;  //if not active we add false
           counter++;   //we add one more to the indexid counter
        }

        //------------ Change the Serialized Propertys---------//
        SerializedObject serializedHex;  // for setting the Collectable 
        SerializedObject serializedCollectable; // for setting the array index, named CollectableIndexID
        
        for (int i = 0; i < haslAllTheHexScriptsForCollectablesBeforeStart.Length; i++)
        {
            serializedHex = new SerializedObject(haslAllTheHexScriptsForCollectablesBeforeStart[i]);
            if (haslAllTheHexScriptsForCollectablesBeforeStart[i].gameObject)
            {
                serializedHex.FindProperty("MyCollectable").objectReferenceValue = collectableList[i].gameObject; 
                serializedHex.ApplyModifiedPropertiesWithoutUndo();
                
                serializedCollectable = new SerializedObject(collectableList[i]);
                serializedCollectable.FindProperty("CollectableIndexID").intValue = i;
                serializedCollectable.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        //------------ Disable all deactived collectables Gameobjects---------//
        for (int i = 0; i < hasAllTheHexCollectableActiveBoolsBeforeStart.Length; i++)
        {   //not reaaaally needed it is just a safety check
            if(hasAllTheHexCollectableActiveBoolsBeforeStart[i] == 0) collectableList[i].gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) spawnCollectableObjects();
    }
    
#endif
    
    private void SetNativeContainer()
    {
        hasAllTheValidSpawnAbleHexesID= new NativeQueue<int>(Allocator.Persistent);  //Queue has the Objects that are Valid for Spawn
        hasAllTheHexCollectablePosition = new TransformAccessArray(hasAllTheHexCollectablePositionBeforeStart); //Native Transforms are much faster than normal
        hasAllTheHexCollectableActiveBools = new NativeArray<byte>(hasAllTheHexCollectableActiveBoolsBeforeStart, Allocator.Persistent); // Keep a native bool list for checking if ValidSpawn
        haslAllTheHexScriptsForCollectables = haslAllTheHexScriptsForCollectablesBeforeStart;  //better have an static List premade than having a getcomponend of ref type

        hasAllTheHexCollectablePositionBeforeStart = null;   //null not needed  Serialized Container
        hasAllTheHexCollectableActiveBoolsBeforeStart = null; //  since we have native Containter
        haslAllTheHexScriptsForCollectablesBeforeStart = null;  
    } 
    private void OnDestroy()
    {   //Dispose all NativeContainer
        hasAllTheValidSpawnAbleHexesID.Dispose();
        hasAllTheHexCollectablePosition.Dispose();
        hasAllTheHexCollectableActiveBools.Dispose();
    }

    public void CollectableCollected(GameObject item, float energyValue , int collectableIndexID)
    {   //Invoked by the Collectable when the Collectable gets collected 
        hasAllTheHexCollectableActiveBools[collectableIndexID] = 0;  
        EnergyManager.energyGotHigher = true;
        StartCoroutine(ReferenceLibrary.EnergyMng.ModifyEnergy(energyValue));
        ReferenceLibrary.AudMng.HexAudMng.PlayHex(HexType.DefaultCollectable);
        item.SetActive(false);
    }
    public void spawnCollectableObjects()
    {
        HexCollectablePosJob spawnCheckJob = new HexCollectablePosJob
        {
            hasAllTheHexCollectableActiveBoolsJob = hasAllTheHexCollectableActiveBools,
            hasAllTheValidSpawnAbleHexesIDJob =  hasAllTheValidSpawnAbleHexesID,
            PlayerPosJob = ReferenceLibrary.PlayerPosition
        };
       JobHandle spawnCheckhandle = spawnCheckJob.Schedule(hasAllTheHexCollectablePosition);
       spawnCheckhandle.Complete();
       
       Debug.Log("counterCollectableRespawned " + hasAllTheValidSpawnAbleHexesID.Count);
       
       while (!hasAllTheValidSpawnAbleHexesID.IsEmpty())
           haslAllTheHexScriptsForCollectables[hasAllTheValidSpawnAbleHexesID.Dequeue()].MyCollectable.SetActive(true);
    }
}

[BurstCompile]
public struct HexCollectablePosJob : IJobParallelForTransform
{
    public NativeArray<byte> hasAllTheHexCollectableActiveBoolsJob;
    [NativeDisableParallelForRestriction] [WriteOnly] public NativeQueue<int> hasAllTheValidSpawnAbleHexesIDJob;
    [ReadOnly] public Vector3 PlayerPosJob;
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
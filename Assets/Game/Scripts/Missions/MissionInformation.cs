using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.SerializableAttribute]
public class MissionInformation 
{

    public MissionType missionType;

    [Tooltip("Time in Seconds")]
    public float time = 180f;
    public float multiplicator = 1;
    //public bool done = false;
    public float Amount = 5f;

    [Header("For Collect, Destroy and Bring")]
    public Item missionItem;


   




    public enum MissionType
    {
        CollectItem,
        DestroyObjs,
        CollectXPoints,
        BringFromAToB
    }


    public enum Item
    {
        none,
        CollectItem1,
        CollectItem2,
        Destroyables,
        BringItem1,
        BringItem2,
    }
    
}

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
    public int Amount = 5;

    [Header("For Collect, Destroy and Bring")]
    public Item missionItem;


   

    public enum MissionType
    {
        CollectItem,
        DestroyObjs,
        CollectPoints,
        BringFromAToB
    }


    public enum Item
    {
        none,
        CollectItem1,
        CollectItem2,
        Destroyable1,
        BringItem1,
        BringItem2,
    }
    
}

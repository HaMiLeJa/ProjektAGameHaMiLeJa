using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject CollectItem1;
    [SerializeField] GameObject CollectItem2;
    [SerializeField] GameObject Destroyable;
    [SerializeField] GameObject BringItem1;
    [SerializeField] GameObject BringItem2;

    [SerializeField] GameObject currentItem;


    [SerializeField] List<GameObject> AllItemSpawnPositions;
    [SerializeField] List<GameObject> PositionListInUse;
    int spawnCounter = 0;

    public static List<GameObject> CurrentMissionItems = new List<GameObject>();

    private void Awake()
    {
        AllItemSpawnPositions = new List<GameObject>(GameObject.FindGameObjectsWithTag("ItemPos"));
    }


    #region Collect Item
    public void SpawnCollectItem()
    {
        PrepareSpawn();

        FindCollectItem();

        Spawn();
    }

    void PrepareSpawn()
    {
        PositionListInUse.Clear();

        foreach (GameObject pos in AllItemSpawnPositions)
            PositionListInUse.Add(pos);

        currentItem = CollectItem1; //Sonst meckert er meh
        ClearCurrentMissionItemList();
    }

    void FindCollectItem()
    {
        if (MissionManager.CurrentMission.missionItem == MissionInformation.Item.CollectItem1)
        {
            currentItem = CollectItem1;
        }
        else if (MissionManager.CurrentMission.missionItem == MissionInformation.Item.CollectItem2)
        {
            currentItem = CollectItem2;
        }
    }

    void Spawn()
    {

        for (int i = 0; i < MissionManager.CurrentMission.Amount; i++) //i <= MissionManager.CurrentMission.Amount - 1 - spawnCounter;
        {
           // spawnCounter++;

            int random = Random.Range(0, PositionListInUse.Count);
            Debug.Log("Random: " + random);

            Vector3 itemPos = PositionListInUse[random].transform.position;

            Transform parentHex = PositionListInUse[random].transform.parent;

            GameObject item = Instantiate(currentItem, itemPos, Quaternion.identity);
            item.transform.parent = parentHex.transform;

            CurrentMissionItems.Add(item);

            PositionListInUse.RemoveAt(random);

            //i--;
        }
    }

    #endregion

    public static void ClearCurrentMissionItemList()
    {
        if (CurrentMissionItems.Count == 0)
        {
            CurrentMissionItems.Clear();
            return;
        }

        for (int i = CurrentMissionItems.Count-1; i >=0 ; i--)
        {
            Destroy(CurrentMissionItems[i]);
        }
        CurrentMissionItems.Clear();
    }
}

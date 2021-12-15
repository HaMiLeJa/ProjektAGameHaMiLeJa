using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateUncompletedMission : MonoBehaviour
{
    public void UpdateUncompletedMission()
    {
        Debug.Log("MissionUncomplete");

        UIManager.Instance.DeactivateCollectItemUI();

        RemoveCollectables();
        //Mission evt zur AllMission Liste zurückadden, wenn wir wollen, dass player jede mission erfüllen müssen

        //Effects, Sound
    }


    void RemoveCollectables()
    {
        MissionItemSpawner.ClearCurrentMissionItemList();
    }
}

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
        //Mission evt zur AllMission Liste zur�ckadden, wenn wir wollen, dass player jede mission erf�llen m�ssen

        //Effects, Sound
    }


    void RemoveCollectables()
    {
        MissionItemSpawner.ClearCurrentMissionItemList();
    }
}

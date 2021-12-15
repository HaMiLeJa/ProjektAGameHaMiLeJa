using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateCompletedMission : MonoBehaviour
{
   public void UpdateCompletedMission()
   {
        Debug.Log("MissionCompleted");

        ScoreManager.OnMultiplicatorUpdate(MissionManager.CurrentMission.multiplicator);

        UIManager.Instance.DeactivateCollectItemUI();
        UIManager.Instance.DeactivateBasicMissionUI();

        RemoveRemainingCollectables();

        //Effects, Sound
   }



    void RemoveRemainingCollectables()
    {
        MissionItemSpawner.ClearCurrentMissionItemList();
    }



}

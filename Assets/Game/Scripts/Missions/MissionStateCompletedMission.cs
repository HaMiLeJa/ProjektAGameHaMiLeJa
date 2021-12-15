using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionStateCompletedMission : MonoBehaviour
{
   public void UpdateCompletedMission()
   {
        ScoreManager.OnMultiplicatorUpdate(MissionManager.CurrentMission.multiplicator);

        UIManager.Instance.DeactiveCollectItemUI();

        //Effects, Sound
   }

    
}

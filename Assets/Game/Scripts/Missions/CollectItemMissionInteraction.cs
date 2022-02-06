using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemMissionInteraction : MonoBehaviour
{

    #region Inspector
    [SerializeField] float rotation = 100;
    #endregion


    void Update()
    {

        transform.Rotate(new Vector3(0, rotation * Time.deltaTime, 0));

       

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {
            ReferenceLibary.MissionMng.ActiveMissionState.ItemCollected(this.gameObject);




        }
    }

    

    



}

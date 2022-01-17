using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHex : MonoBehaviour
{
    [SerializeField] float rotation = 20;

    [HideInInspector] public GameObject ParentHex;


    void Update()
    {
        #region forward mov + rotation

       
        transform.Rotate(new Vector3(0, rotation * Time.deltaTime, 0));

        #endregion

    }



    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {
            ReferenceLibary.WinconMng.CheckForWinConHex();

        }
    }
}


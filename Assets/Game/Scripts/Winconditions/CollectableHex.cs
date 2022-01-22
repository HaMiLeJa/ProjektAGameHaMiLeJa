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
        Debug.Log("2");
        if (collision.gameObject == ReferenceLibary.Player)
        {
            ReferenceLibary.WinconMng.CheckForWinConHex();
            Debug.Log("1");

        }
    }
}


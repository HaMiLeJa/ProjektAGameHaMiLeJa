using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpawnHexCollectableInEditor : MonoBehaviour
{
    public GameObject ObjectToSpawn;

   // public bool spawnObjectInEditor = true;
   // public bool objectActive = false;

    public GameObject CurrentItem;

    [SerializeField] Hex myHex;
    [SerializeField] GameObject MyProps;

    //CollectableReferences colRef;
    public bool testB = false;

    void Start()
    {
       

    }


    void Update()
    {
        

       // if (Application.isPlaying == false) return;
        
       
         

        EditModeSpawnAndDeletion();
           

    }

    #region Editor

    bool CheckForSpawnAlloance()
    {
        if (MyProps.GetComponentInChildren<Collectable>() == true)
        {
            CurrentItem = MyProps.GetComponentInChildren<Collectable>().gameObject;
            return false;
        }
        else
        {
            return true;
        }

    }



    void EditModeSpawnAndDeletion()
    {
        if (myHex.hexType == HexType.DefaultCollectable)
        {
            if(CheckForSpawnAlloance())
            {
                SpawnObjectInEditMode();
            }
        }
        else if (myHex.hexType != HexType.DefaultCollectable && CurrentItem != null)
        {
            DestroyImmediate(CurrentItem);

        }
    }

    void SpawnObjectInEditMode()
    {
        //objectActive = true;
        Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);

        CurrentItem = Instantiate(ObjectToSpawn, position, Quaternion.identity);
        CurrentItem.transform.parent = MyProps.transform;
    }

    #endregion


   


   
}

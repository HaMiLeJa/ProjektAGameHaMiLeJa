using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpawnHexCollectableInEditor : MonoBehaviour
{
    public GameObject ObjectToSpawn;

    public GameObject CurrentItem;

    [SerializeField] Hex myHex;
    [SerializeField] GameObject MyProps;

    public bool DebugActiveObj = false;
    public bool DebugIsRunning = false;

    void Start()
    {
       

    }


    void Update()
    {
        if (CollectableManager.StopEditorScript == true) return;

        // if (Application.isPlaying == false) return;


        DebugIsRunning = true;
        EditModeSpawnAndDeletion();

        //myHex.myCollectable = CurrentItem;
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
            if (CheckForSpawnAlloance())
            {
                DebugActiveObj = true;
                SpawnObjectInEditMode();
            }
        }
        else if (myHex.hexType != HexType.DefaultCollectable && CurrentItem != null)
        {
            DestroyImmediate(CurrentItem);
            DebugActiveObj = false;
        }
    }

    void SpawnObjectInEditMode()
    {
        Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);

        CurrentItem = Instantiate(ObjectToSpawn, position, Quaternion.identity);
        CurrentItem.transform.parent = MyProps.transform;
    }

    #endregion


   


   
}

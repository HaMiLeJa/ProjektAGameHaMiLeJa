using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpawnHexCollectableInEditor : MonoBehaviour
{
    public GameObject ObjectToSpawn;

    public bool spawnObjectInEditor = true;
    bool objectActive;

    [HideInInspector] public GameObject CurrentItem;

    [SerializeField] Hex myHex;
    [SerializeField] GameObject MyProps;

    [SerializeField] CollectableType collectableType = CollectableType.Type1;
    Mode mode = Mode.EditMode;

    CollectableReferences colRef;

    void Start()
    {
        if (Application.isPlaying == false) return;
        
         mode = Mode.Initialise;

        if (mode == Mode.Initialise)
            InitializePlayMode();

    }


    void Update()
    {

        switch (mode)
        {
            case Mode.EditMode:
                EditModeSpawnAndDeletion();
                break;
            case Mode.PlayMode:
                UpdatePlaymode();
                break;
            default:
                break;
        }

    }

    #region Editor

    bool CheckForSpawnAlloance()
    {
        if (MyProps.GetComponentInChildren<Collectable>() == true)
        {
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

            /*
            if (objectActive == false)
                SpawnObjectInEditMode();
            */
        }
        else if (myHex.hexType != HexType.DefaultCollectable && CurrentItem != null)
        {
            objectActive = false;
            DestroyImmediate(CurrentItem);

        }
    }

    void SpawnObjectInEditMode()
    {
        objectActive = true;
        Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);

        CurrentItem = Instantiate(ObjectToSpawn, position, Quaternion.identity);
        CurrentItem.transform.parent = MyProps.transform;
    }
    #endregion

    #region Initialize PlayMode

   void InitializePlayMode()
   {
        if (myHex.hexType == HexType.DefaultCollectable)
        {
            CurrentItem = MyProps.GetComponentInChildren<Collectable>().gameObject;


            CurrentItem.GetComponent<Collectable>().ParentHex = this.gameObject;

            colRef.Collectable = CurrentItem;
            colRef.activeCollectable = true;
            colRef.Hex = this.gameObject;
            colRef.HexCollectableScript = this.gameObject.GetComponent<SpawnHexCollectableInEditor>();


            if (objectActive == true)
            {
                CollectableManager.AllCollectables.Add(this.gameObject, colRef);
            }
            else
            {
                colRef.activeCollectable = false;
                CollectableManager.AllCollectables.Add(this.gameObject, colRef);
            }

        }

        mode = Mode.PlayMode;
    }

    #endregion

    #region Playmode

    void UpdatePlaymode()
    {
        if (myHex.hexType != HexType.DefaultCollectable) return;
    }


    public void SpawnCollectable()
    {
        Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + 4, this.transform.position.z);

        CurrentItem = Instantiate(ObjectToSpawn, position, Quaternion.identity);
        CurrentItem.transform.parent = MyProps.transform;


        //Add to List
        colRef.Collectable = CurrentItem;
        colRef.activeCollectable = true;
        // colRef.Hex = this.gameObject;
        // colRef.HexCollectableScript = this.gameObject.GetComponent<SpawnHexCollectable>();

        CollectableManager.AllCollectables[this.gameObject].activeCollectable = true;
        CollectableManager.AllCollectables[this.gameObject].Collectable = CurrentItem;

    }

    #endregion

    enum Mode
    {
        EditMode,
        Initialise,
        PlayMode
    }

    enum CollectableType
    {
        Type1,
        Type2,
    }
}

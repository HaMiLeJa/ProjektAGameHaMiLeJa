using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnHexObjectsInEditor : MonoBehaviour
{
    public GameObject ObjectToSpawn, CurrentItem;
    [SerializeField] Hex myHex;
    [SerializeField] GameObject MyProps;
    public bool DebugActiveObj, DebugIsRunning;
    ChangeDirectionProp[] propsCDChildren;
    BoostForwardProp[] propsBFChildren;
    BoostInDirectionProp[] propsBIDChildren;
    TrampolinProp[] propsTChildren;
    SlowDownProp[] propsSDChildren;
    Collectable[] propsCChildren;
    HexType currentHexType;
    [SerializeField] GameObject BoostForwardObj, TrampolinObj, BoostInDirectionObj, SlowDownObj, ChangeDirectionObj, CollectableObj;
    private void Start() => currentHexType = myHex.hexType;

    private void OnDrawGizmos() => UpdateMeshes();
    private void UpdateMeshes()
    {
        if (!SpawnHexPropsManager.AllowEditorHexObjSpawn) return;
        if (GameManager.DisableSpawnHexObjectsInEditMode) return;
        EditModeSpawnAndDeletion();
    }
    void EditModeSpawnAndDeletion()
    {
        switch (myHex.hexType)
        {
            case HexType.BoostForward: BoostForward(); break;
            case HexType.ChangeDirection: ChangeDirection(); break;
            case HexType.BoostInDirection: BoostInDirection(); break;
            case HexType.Trampolin: Trampolin(); break;
            case HexType.SlowDown: SlowDown(); break;
            case HexType.DefaultCollectable: CollectableCase(); break;
            default: Default(); break; //delete obj of all Types!
        }
    }
    void SetNewValues(GameObject obj) => ObjectToSpawn = obj;
    void ClearFalseObj()
    {
        if (currentHexType != myHex.hexType)
        {
            DestroyImmediate(CurrentItem);
            currentHexType = myHex.hexType;
        }
    }
    void Default()
    {
        ObjectToSpawn = null; ClearFalseObj();
        currentHexType = myHex.hexType;
    }
    #region boostForward
    void BoostForward()
    {
        SetNewValues(BoostForwardObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps,SpawnComponentTypes.BoostForward);
        if(CheckForSpawnAllowanceBoostForward())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.26f);
        }
    }
    bool CheckForSpawnAllowanceBoostForward()
    {
        propsBFChildren = MyProps.GetComponentsInChildren<BoostForwardProp>();
        if (propsBFChildren.Length == 0) return true;
        return false;
    }
    #endregion
    #region ChangeDirection
    void ChangeDirection()
    {
        SetNewValues(ChangeDirectionObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps, SpawnComponentTypes.ChangeDirection);
        if (CheckForSpawnAllowanceChangeDirection())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.24f);
        }
    }
    bool CheckForSpawnAllowanceChangeDirection()
    {
        propsCDChildren = MyProps.GetComponentsInChildren<ChangeDirectionProp>();
        if (propsCDChildren.Length == 0) return true; return false;
    }
    #endregion
    #region BoostInDirection
    void BoostInDirection()
    {
        SetNewValues(BoostInDirectionObj); ClearFalseObj(); CurrentItem =  ResetCurrentItem(MyProps,SpawnComponentTypes.BoostInDirection);
        if (CheckForSpawnAllowanceBoostInDirection())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.24f);
            CurrentItem.GetComponent<BoostInDirectionProp>().MyHex = myHex;
        }
    }
    bool CheckForSpawnAllowanceBoostInDirection()
    {
        propsBIDChildren = MyProps.GetComponentsInChildren<BoostInDirectionProp>();
        if (propsBIDChildren.Length == 0) return true; return false;
    }
    #endregion
    #region Trampolin
    void Trampolin()
    {
        SetNewValues(TrampolinObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps,SpawnComponentTypes.Trampolin);
        if (CheckForSpawnAllowanceTrampolin())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.78f);
        }
    }
    bool CheckForSpawnAllowanceTrampolin()
    {
        propsTChildren = MyProps.GetComponentsInChildren<TrampolinProp>();
        if (propsTChildren.Length == 0) return true; return false;
    }
    #endregion
    #region SlowDown
    void SlowDown()
    {
        SetNewValues(SlowDownObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps, SpawnComponentTypes.SlowDown);
        if (CheckForSpawnAllowanceSlowDown())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.25f);
        }
    }
    bool CheckForSpawnAllowanceSlowDown()
    {
        propsSDChildren = MyProps.GetComponentsInChildren<SlowDownProp>();
        if (propsSDChildren.Length == 0) return true; return false;
    }
    #endregion
    #region Collectable
    void CollectableCase()
    {
        SetNewValues(CollectableObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps, SpawnComponentTypes.Collectable);
        if (CheckForSpawnAllowanceCollectable())
        {
            currentHexType = myHex.hexType;
            SpawnCollectableInEditMode(4.63f);
        }
    }
    bool CheckForSpawnAllowanceCollectable()
    {
        propsCChildren = MyProps.GetComponentsInChildren<Collectable>();
        if (propsCChildren.Length == 0) return true; return false;
    }
    void SpawnCollectableInEditMode(float y)
    {
        Collectable col = null;
        CurrentItem = spawnObjectWithPrefabConnection(y,CurrentItem,gameObject, ObjectToSpawn, MyProps);
        col = CurrentItem.GetComponent<Collectable>();
        col.ParentHex = gameObject;
        col.colRef.HexScript = GetComponent<Hex>();
        col.colRef.ActiveCollectable = true;
        gameObject.GetComponent<Hex>().MyCollectable = CurrentItem;
    }
    #endregion
    void SpawnObjectInEditMode(float y) =>  spawnObjectWithPrefabConnection(y, CurrentItem, gameObject, ObjectToSpawn, MyProps);
    public static GameObject spawnObjectWithPrefabConnection(float y, GameObject Item, GameObject hex, GameObject ObjectToSpawn, GameObject MyProps)
    {
        Vector3 position = new Vector3(hex.transform.position.x, hex.transform.position.y + y, hex.transform.position.z);
#if UNITY_EDITOR
        Item = (GameObject)PrefabUtility.InstantiatePrefab(ObjectToSpawn);
#endif
        Item.transform.position = position;
        Item.transform.rotation = Quaternion.identity;
        Item.transform.parent = MyProps.transform;
        return Item;
    }
    public static GameObject ResetCurrentItem(GameObject MyProps, SpawnComponentTypes spawnComponentType)
    {
        if (spawnComponentType == SpawnComponentTypes.Collectable && MyProps.GetComponentInChildren<Collectable>()) return MyProps.GetComponentInChildren<Collectable>().gameObject;
        if (spawnComponentType == SpawnComponentTypes.SlowDown && MyProps.GetComponentInChildren<SlowDownProp>()) return MyProps.GetComponentInChildren<SlowDownProp>().gameObject;
        if (spawnComponentType == SpawnComponentTypes.Trampolin &&MyProps.GetComponentInChildren<TrampolinProp>()) return MyProps.GetComponentInChildren<TrampolinProp>().gameObject;
        if (spawnComponentType == SpawnComponentTypes.ChangeDirection && MyProps.GetComponentInChildren<ChangeDirectionProp>()) return MyProps.GetComponentInChildren<ChangeDirectionProp>().gameObject;
        if (spawnComponentType == SpawnComponentTypes.BoostForward && MyProps.GetComponentInChildren<BoostForwardProp>()) return MyProps.GetComponentInChildren<BoostForwardProp>().gameObject;
        if (spawnComponentType == SpawnComponentTypes.BoostInDirection && MyProps.GetComponentInChildren<BoostInDirectionProp>()) return MyProps.GetComponentInChildren<BoostInDirectionProp>().gameObject;
        return MyProps;
    }
}
public enum SpawnComponentTypes
{
    Collectable,
    SlowDown,
    Trampolin,
    BoostForward,
    BoostInDirection,
    ChangeDirection
}

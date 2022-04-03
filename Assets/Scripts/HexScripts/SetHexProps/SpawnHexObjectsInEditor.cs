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
    private void Awake()
    {
        if (Application.isPlaying) return;
        currentHexType = myHex.hexType;
    }

    private void OnDrawGizmos() => UpdateMeshes();
    private void UpdateMeshes()
    {        if (Application.isPlaying) return;
        if (!SpawnHexPropsManager.AllowEditorHexObjSpawn) return;
        if (GameManager.DisableSpawnHexObjectsInEditMode) return;
        EditModeSpawnAndDeletion();
    }
    void EditModeSpawnAndDeletion()
    {      if (Application.isPlaying) return;
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
    void SetNewValues(GameObject obj)
    {   if (Application.isPlaying) return;
        ObjectToSpawn = obj;
    }

    void ClearFalseObj()
    {      if (Application.isPlaying) return;
        if (currentHexType != myHex.hexType)
        {
            DestroyImmediate(CurrentItem);
            currentHexType = myHex.hexType;
        }
    }
    void Default()
    {   if (Application.isPlaying) return;
        ObjectToSpawn = null; ClearFalseObj();
        currentHexType = myHex.hexType;
    }
    #region boostForward
    void BoostForward()
    {   if (Application.isPlaying) return;
        SetNewValues(BoostForwardObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps,SpawnComponentTypes.BoostForward);
        if(CheckForSpawnAllowanceBoostForward())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.26f);
        }
    }
    bool CheckForSpawnAllowanceBoostForward()
    {
        if (!Application.isPlaying)
        {
            propsBFChildren = MyProps.GetComponentsInChildren<BoostForwardProp>();
            if (propsBFChildren.Length == 0) return true;
        }
        return false;
    }
    #endregion
    #region ChangeDirection
    void ChangeDirection()
    {   if (Application.isPlaying) return;
        SetNewValues(ChangeDirectionObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps, SpawnComponentTypes.ChangeDirection);
        if (CheckForSpawnAllowanceChangeDirection())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.24f);
        }
    }
    bool CheckForSpawnAllowanceChangeDirection()
    {
        if (!Application.isPlaying)
        {
            propsCDChildren = MyProps.GetComponentsInChildren<ChangeDirectionProp>();
            if (propsCDChildren.Length == 0) return true; 
        }
        return false;
    }
    #endregion
    #region BoostInDirection
    void BoostInDirection()
    {   if (Application.isPlaying) return;
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
        if (!Application.isPlaying)
        {
            propsBIDChildren = MyProps.GetComponentsInChildren<BoostInDirectionProp>();
            if (propsBIDChildren.Length == 0) return true; 
        }
        return false;
    }
    #endregion
    #region Trampolin
    void Trampolin()
    {   if (Application.isPlaying) return;
        SetNewValues(TrampolinObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps,SpawnComponentTypes.Trampolin);
        if (CheckForSpawnAllowanceTrampolin())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.78f);
        }
    }
    bool CheckForSpawnAllowanceTrampolin()
    {
        if (Application.isPlaying)
        {
            propsTChildren = MyProps.GetComponentsInChildren<TrampolinProp>();
            if (propsTChildren.Length == 0) return true; 
        }
        return false;
    }
    #endregion
    #region SlowDown
    void SlowDown()
    {   if (Application.isPlaying) return;
        SetNewValues(SlowDownObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps, SpawnComponentTypes.SlowDown);
        if (CheckForSpawnAllowanceSlowDown())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.25f);
        }
    }
    bool CheckForSpawnAllowanceSlowDown()
    {
        if (!Application.isPlaying)
        {
            propsSDChildren = MyProps.GetComponentsInChildren<SlowDownProp>();
            if (propsSDChildren.Length == 0) return true;
        }
        return false;
    }
    #endregion
    #region Collectable
    void CollectableCase()
    {   if (Application.isPlaying) return;
        SetNewValues(CollectableObj); ClearFalseObj(); CurrentItem = ResetCurrentItem(MyProps, SpawnComponentTypes.Collectable);
        if (CheckForSpawnAllowanceCollectable())
        {
            currentHexType = myHex.hexType;
            SpawnCollectableInEditMode(4.63f);
        }
    }
    bool CheckForSpawnAllowanceCollectable()
    {   if (!Application.isPlaying)
        {
            propsCChildren = MyProps.GetComponentsInChildren<Collectable>();
            if (propsCChildren.Length == 0) return true; 
        }
        return false;
    }
    void SpawnCollectableInEditMode(float y)
    {   if (Application.isPlaying) return;
        CurrentItem = spawnObjectWithPrefabConnection(y,CurrentItem,gameObject, ObjectToSpawn, MyProps);
        //------------------- Update Collectable Index ID if Possible --------------//
        CollectableManager colManager = FindObjectOfType<CollectableManager>();
        if(colManager.hasAllTheHexCollectablePositionBeforeStart == null) return;
        for (int i = 0; i < colManager.hasAllTheHexCollectablePositionBeforeStart.Length; i++)
        {
            if (colManager.hasAllTheHexCollectablePositionBeforeStart[i] == gameObject.transform)
            {
                SerializedObject serializedCollectable = new SerializedObject(GetComponentInChildren<Collectable>());
                serializedCollectable.FindProperty("CollectableIndexID").intValue = i;
                serializedCollectable.ApplyModifiedPropertiesWithoutUndo();
            }
        }
        //------------------- Update Parent ref  --------------//
        SerializedObject serializedHex = new SerializedObject(gameObject.GetComponent<Hex>());
        serializedHex.FindProperty("MyCollectable").objectReferenceValue =
            GetComponentInChildren<Collectable>().gameObject;
        serializedHex.ApplyModifiedPropertiesWithoutUndo();
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

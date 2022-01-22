using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpawnHexObjectsInEditor : MonoBehaviour
{
    public GameObject ObjectToSpawn;

    public GameObject CurrentItem;

    [SerializeField] Hex myHex;
    [SerializeField] GameObject MyProps;

    public bool DebugActiveObj = false;
    public bool DebugIsRunning = false;

    //Transform[] propsChildren;
    ChangeDirectionProp[] propsCDChildren;
    BoostForwardProp[] propsBFChildren;
    BoostInDirectionProp[] propsBIDChildren;
    TrampolinProp[] propsTChildren;
    SlowDownProp[] propsSDChildren;

    Collectable[] propsCChildren;

    HexType currentHexType;

    [SerializeField] GameObject BoostForwardObj;
    [SerializeField] GameObject TrampolinObj;
    [SerializeField] GameObject BoostInDirectionObj;
    [SerializeField] GameObject SlowDownObj;
    [SerializeField] GameObject ChangeDirectionObj;
    [SerializeField] GameObject CollectableObj;

    private void Start()
    {
        /*
        HexEffectsAndProps.Add(BoostForwardObj);
        HexEffectsAndProps.Add(TrampolinObj);
        HexEffectsAndProps.Add(BoostInDirectionObj);
        HexEffectsAndProps.Add(SlowDownObj);
        HexEffectsAndProps.Add(ChangeDirectionObj);
        */

        // if(GameManager.Instance.DisableExecuteAlwaysScripts != true)
         //ResetCurrentItem(); //searchs for current Item (eg after change to edit mode)
         currentHexType = myHex.hexType;

    }

    void Update()
    {
        //if (ReferenceLibary.GameMng.DisableExecuteAlwaysScripts == true) return;


        //propsChildrenLength = propsChildren.Length;

        if (GameManager.DisableSpawnHexObjectsInEditMode == true) return;

        EditModeSpawnAndDeletion();

        
    }

    void EditModeSpawnAndDeletion()
    {
        switch (myHex.hexType)
        {
            case HexType.BoostForward:
                BoostForward();
                break;
            case HexType.ChangeDirection:
                ChangeDirection();
                break;
            case HexType.BoostInDirection:
                BoostInDirection();
                break;
            case HexType.Trampolin:
                Trampolin();
                break;
            case HexType.SlowDown:
                SlowDown();
                break;
            case HexType.DefaultCollectable:
                CollectableCase();
                break;
            default: //delete obj of all Types!
                Default();
                break;
        }
    }

    void SetNewValues(GameObject obj)
    {
        ObjectToSpawn = obj;
    }


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
        //if (CurrentItem = null) return;
        ObjectToSpawn = null;
        ClearFalseObj();
        currentHexType = myHex.hexType;
    }

    #region boostForward
    void BoostForward()
    {
       

        SetNewValues(BoostForwardObj);

        ClearFalseObj(); //ToTest

        ResetCurrentItemBoostFoward();


        if(CheckForSpawnAllowanceBoostForward())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(4f);
        }
    }

   

    void ResetCurrentItemBoostFoward() //hier evt list einbauen und alles bis auf 1. löschen oder so
    {
        
       if (MyProps.GetComponentInChildren<BoostForwardProp>() == true)
       {
           CurrentItem = MyProps.GetComponentInChildren<BoostForwardProp>().gameObject;

       }
       
        /*
        for (int i = 0; i < propsChildren.Length; i++)
        {
            if (propsChildren[i].tag == "HexEffectObj")
            {
                if(propsChildren[i].GetComponentInChildren<BoostForwardProp>() && CurrentItem == null)
                {
                    CurrentItem = propsChildren[i].GetComponentInChildren<BoostForwardProp>().gameObject;
                    
                }
                else
                {
                    DestroyImmediate(propsChildren[i].gameObject);
                    i--;
                }

            }
        }
        */

    }

    

    bool CheckForSpawnAllowanceBoostForward()
    {
        propsBFChildren = MyProps.GetComponentsInChildren<BoostForwardProp>();

        if (propsBFChildren.Length == 0)
        {
            return true;
        }
        else
            return false;

        /*  propsChildren = MyProps.GetComponentsInChildren<Transform>();

          if (propsChildren.Length == 0)
          {
              return true;
          }

          foreach (Transform obj in propsChildren)
          {
              if(obj.gameObject.GetComponent<BoostForwardProp>() == true)
              {
                  //hier könnte ich auch current item setzen
                  return false;
              }
          }

          return true; */
    }

    #endregion

    #region ChangeDirection

    void ChangeDirection()
    {
        SetNewValues(ChangeDirectionObj);

        ClearFalseObj(); //ToTest

        ResetCurrentItemChangeDirection();


        if (CheckForSpawnAllowanceChangeDirection())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(3.3f);
        }
    }



    void ResetCurrentItemChangeDirection() //hier evt list einbauen und alles bis auf 1. löschen oder so
    {

        if (MyProps.GetComponentInChildren<ChangeDirectionProp>() == true)
        {
            CurrentItem = MyProps.GetComponentInChildren<ChangeDirectionProp>().gameObject;

        }

        

    }

    bool CheckForSpawnAllowanceChangeDirection()
    {

        propsCDChildren = MyProps.GetComponentsInChildren<ChangeDirectionProp>();

        if (propsCDChildren.Length == 0)
        {
            return true;
        }
        else
            return false;

        /*

        propsChildren = MyProps.GetComponentsInChildren<Transform>();

        if (propsChildren.Length == 0)
        {
            return true;
        }

        foreach (Transform obj in propsChildren)
        {
            if (obj.gameObject.GetComponent<BoostInDirectionProp>() == true)
            {
                //hier könnte ich auch current item setzen
                return false;
            }
        }

        return true;*/
    }

    #endregion


    #region BoostInDirection
    void BoostInDirection()
    {
        SetNewValues(BoostInDirectionObj);

        ClearFalseObj(); //ToTest

        ResetCurrentItemBoostInDirection();


        if (CheckForSpawnAllowanceBoostInDirection())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(0.21f);

            CurrentItem.GetComponent<BoostInDirectionProp>().MyHex = myHex;
        }
    }

    void ResetCurrentItemBoostInDirection() //hier evt list einbauen und alles bis auf 1. löschen oder so
    {

        if (MyProps.GetComponentInChildren<BoostInDirectionProp>() == true)
        {
            CurrentItem = MyProps.GetComponentInChildren<BoostInDirectionProp>().gameObject;

        }

    }

   

    bool CheckForSpawnAllowanceBoostInDirection()
    {

        propsBIDChildren = MyProps.GetComponentsInChildren<BoostInDirectionProp>();

        if (propsBIDChildren.Length == 0)
        {
            return true;
        }
        else
            return false;

        /*propsChildren = MyProps.GetComponentsInChildren<Transform>();

        if (propsChildren.Length == 0)
        {
            return true;
        }

        foreach (Transform obj in propsChildren)
        {
            if (obj.gameObject.GetComponent<BoostInDirectionProp>() == true)
            {
                //hier könnte ich auch current item setzen
                return false;
            }
        }

        return true;*/
    }

    

    #endregion

    #region Trampolin
    void Trampolin()
    {


        SetNewValues(TrampolinObj);

        ClearFalseObj(); //ToTest

        ResetCurrentItemTrampolin();


        if (CheckForSpawnAllowanceTrampolin())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(4f);
        }
    }



    void ResetCurrentItemTrampolin() //hier evt list einbauen und alles bis auf 1. löschen oder so
    {

        if (MyProps.GetComponentInChildren<TrampolinProp>() == true)
        {
            CurrentItem = MyProps.GetComponentInChildren<TrampolinProp>().gameObject;

        }

    }

    bool CheckForSpawnAllowanceTrampolin()
    {

        propsTChildren = MyProps.GetComponentsInChildren<TrampolinProp>();

        if (propsTChildren.Length == 0)
        {
            return true;
        }
        else
            return false;

        /*propsChildren = MyProps.GetComponentsInChildren<Transform>();

        if (propsChildren.Length == 0)
        {
            return true;
        }

        foreach (Transform obj in propsChildren)
        {
            if (obj.gameObject.GetComponent<TrampolinProp>() == true)
            {
                //hier könnte ich auch current item setzen
                return false;
            }
        }

        return true;*/
    }

    #endregion

    #region SlowDown

    void SlowDown()
    {
        SetNewValues(SlowDownObj);

        ClearFalseObj(); //ToTest

        ResetCurrentItemSlowDown();


        if (CheckForSpawnAllowanceSlowDown())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(4f);
        }
    }



    void ResetCurrentItemSlowDown() //hier evt list einbauen und alles bis auf 1. löschen oder so
    {

        if (MyProps.GetComponentInChildren<SlowDownProp>() == true)
        {
            CurrentItem = MyProps.GetComponentInChildren<SlowDownProp>().gameObject;

        }


    }

    bool CheckForSpawnAllowanceSlowDown()
    {
        propsSDChildren = MyProps.GetComponentsInChildren<SlowDownProp>();

        if (propsSDChildren.Length == 0)
        {
            return true;
        }
        else
            return false;

        /*propsChildren = MyProps.GetComponentsInChildren<Transform>();

        if (propsChildren.Length == 0)
        {
            return true;
        }

        foreach (Transform obj in propsChildren)
        {
            if (obj.gameObject.GetComponent<SlowDownProp>() == true)
            {
                return false;
            }
        }

        return true;*/
    }

    #endregion

    #region Collectable

    void CollectableCase()
    {
        SetNewValues(CollectableObj);

        ClearFalseObj(); //ToTest

        ResetCurrentItemCollectable();


        if (CheckForSpawnAllowanceCollectable())
        {
            currentHexType = myHex.hexType;
            SpawnObjectInEditMode(4f);

            //set collectable parent
        }
    }



    void ResetCurrentItemCollectable() //hier evt list einbauen und alles bis auf 1. löschen oder so
    {

        if (MyProps.GetComponentInChildren<Collectable>() == true)
        {
            CurrentItem = MyProps.GetComponentInChildren<Collectable>().gameObject;

        }


    }

    bool CheckForSpawnAllowanceCollectable()
    {
        propsCChildren = MyProps.GetComponentsInChildren<Collectable>();

        if (propsCChildren.Length == 0)
        {
            return true;
        }
        else
            return false;

        

        /* propsChildren = MyProps.GetComponentsInChildren<Transform>();

         if (propsChildren.Length == 0)
         {
             return true;
         }

         foreach (Transform obj in propsChildren)
         {
             if (obj.gameObject.GetComponent<Collectable>() == true)
             {
                 return false;
             }
         }

         return true; */
    }


    #endregion


    void SpawnObjectInEditMode(float y)
    {
        Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + y, this.transform.position.z);

        CurrentItem = Instantiate(ObjectToSpawn, position, Quaternion.identity);
        CurrentItem.transform.parent = MyProps.transform;
    }

}

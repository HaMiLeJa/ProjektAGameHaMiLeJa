using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour //for points and energy
{
   
    GameManager gameMng;

    [Tooltip ("How many points the player have") ]public float DestroyablePoints = 0;


    #region Singleton
    public static ResourceManager Instance;
    private void Awake()
    {
        if (ResourceManager.Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            ResourceManager.Instance = this;
        }
    }
    #endregion

    void Start()
    {
        DestroyablePoints = 0;
        gameMng = GameManager.Instance;

        gameMng.onDestroyableDestroyed += ResourceManager.Instance.UpdateDestroyablePoints;

    }

    
    void Update()
    {
        
    }



    public void UpdateDestroyablePoints(float value)
    {
        DestroyablePoints += value;
    }
}

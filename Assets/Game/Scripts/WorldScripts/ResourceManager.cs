using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour //for points and energy
{

    GameManager gameMng;
    GameObject player;
    PlayerStartDash startDash;

    [Tooltip("How many points the player have")] public float DestroyablePoints = 0;
    [Space]
    [SerializeField] float EnergyStartAmount = 10;
    public float CurrentEnergy;
    public float CurrentUIEnergy;
    [Tooltip("A limit of how many Energy the player can have")] [SerializeField] float MaxEnergyAmount = 20f;
   


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
        CurrentEnergy = EnergyStartAmount;
        CurrentUIEnergy = EnergyStartAmount;
        UIManager.Instance.UpdateEnergyUI(1);

        player = GameObject.FindGameObjectWithTag("Player");
        startDash = player.GetComponent<PlayerStartDash>();

        gameMng.onDestroyableDestroyed += UpdateDestroyablePoints;
        gameMng.onEnergyChange += ModifyEnergy;
        gameMng.onEnergyChange += CheckEnergyAmount;
        gameMng.onUIEnergyChange += ModifyUIEnergy;




    }

    
    void Update()
    {
        
    }

    void UpdateDestroyablePoints(float value)
    {
        DestroyablePoints += value;
    }

    void ModifyEnergy(float value)
    {
        CurrentEnergy += value;
        //falls die Energyanzeige Falsch ist, hier UI Update aufrufen statt über Event

        

    }

    void ModifyUIEnergy(float value)
    {
        CurrentUIEnergy += value;
        
    }

    void CheckEnergyAmount(float value)
    {

        if (CurrentEnergy <= 0)
        {
            //if (startDash.Boosting == true) return;

            gameMng.AllowMovement = false;
        }
        else
        {
            gameMng.AllowMovement = true;
        }

        if(CurrentEnergy >= MaxEnergyAmount)
        {
            CurrentEnergy = MaxEnergyAmount;
        }
    }
}

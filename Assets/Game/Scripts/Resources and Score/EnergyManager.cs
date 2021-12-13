using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour //for points and energy
{
   

    GameManager gameMng;

    public bool DisableEnergyCosts = false;
    [Space]
    [Tooltip("How many points the player have")] public float DestroyablePoints = 0;
    [Space]
    [SerializeField] float EnergyStartAmount = 10;
    public float CurrentEnergy;
    public float CurrentUIEnergy;
    [Tooltip("A limit of how many Energy the player can have")] [SerializeField] float MaxEnergyAmount = 20f;
   


    #region Singleton
    public static EnergyManager Instance;
    private void Awake()
    {
        if (EnergyManager.Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            EnergyManager.Instance = this;
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

        

        ;
        gameMng.onEnergyChange += ModifyEnergy;
        gameMng.onUIEnergyChange += ModifyUIEnergy;
        gameMng.onEnergyChange += CheckEnergyAmount;




    }

    
    void Update()
    {
        if (DisableEnergyCosts == true)
        {
            CurrentEnergy = 25;
            CurrentUIEnergy = 25;
        }
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

        if (CurrentEnergy <= 1)
        {
            //if (startDash.Boosting == true) return;

            gameMng.AllowMovement = false;
            Debug.Log("Energy 0");
        }
        else
        {
            gameMng.AllowMovement = true;
        }

        if(CurrentEnergy >= MaxEnergyAmount)
        {
            CurrentEnergy = MaxEnergyAmount;
            CurrentUIEnergy = MaxEnergyAmount;
        }
    }
}

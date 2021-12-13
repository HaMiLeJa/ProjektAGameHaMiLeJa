using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour //for points and energy
{
    GameManager gameMng;


    public bool DisableEnergyCosts = false;
    [Space]
    [SerializeField] float EnergyStartAmount = 10;
    public static float CurrentEnergy;

    [Tooltip("A limit of how many Energy the player can have")] 
    [SerializeField] float MaxEnergyAmount = 20f;

    #region Events

    public delegate void EnergyChange(float value); //Managing the Energy Value (gain and loss)
    public static EnergyChange onEnergyChange;

    #endregion

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
        gameMng = GameManager.Instance;
        CurrentEnergy = EnergyStartAmount;
       
        onEnergyChange += ModifyEnergy;
        onEnergyChange += CheckEnergyAmount;

    }

    
    void Update()
    {
        if (DisableEnergyCosts == true)
        {
            CurrentEnergy = 25;
        }
    }

   

    void ModifyEnergy(float value)
    {
        CurrentEnergy += value;
        //falls die Energyanzeige Falsch ist, hier UI Update aufrufen statt über Event
        

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
           
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    GameManager gameMng;

    [Tooltip ("How much Energy the Player currently has")]
    [Range(0.5f, 5)] public float Energy = 1;

    [HideInInspector] public float MaxEnergy = 5;

    public float SavedEnergy = 0;

    [HideInInspector] public float EnergyMovementValue;
    [HideInInspector] public float EnergyBoostValue;
   // [HideInInspector] public float EnergyShadowDashValue;
    [HideInInspector] public float EnergySuperBoostValue;


    [HideInInspector] public float ReduceEnergyMulitplicator = 0.9999f;

    bool savingEnergy = false;
    bool regenerateMinimalEnergy = false;

    private void Start()
    {
        gameMng = GameManager.Instance;
    }

    void Update()
    {
       // EnergyValues();

        // WEnn ernergie 0 ist x sekuden warten und wieder etwas hoch setzten, damit man sich wieder bewegen aknn   (Akutell wird die Energy in PlayerMovement geclampt, sodass nie nie kleiner als 0,4 wird)

        
        if (Energy > MaxEnergy)
        {
            Energy = MaxEnergy;
        }


       

       


       
    }


    //[Tooltip("if you reduce de PlayerMovementSpeed you have to increase this value")]
    //[SerializeField] float MinimalRequiredEnergyForMovement = 0.7f;

    /*
    void EnergyValues()
    {   
        EnergyMovementValue = Energy * 1f;
        if (EnergyMovementValue < MinimalRequiredEnergyForMovement)
        {
            EnergyMovementValue = MinimalRequiredEnergyForMovement;
        }

        EnergyBoostValue = Energy * 1.2f;
        EnergySuperBoostValue = Energy * 1.4f;

        //EnergyShadowDashValue = Energy * 1.5f;
    }
    */

    /*
    public void ReduceEnergy()
    {
        Energy *= ReduceEnergyMulitplicator;
        Energy = Mathf.Clamp(Energy, 0f, MaxEnergy);

    }
    */
}

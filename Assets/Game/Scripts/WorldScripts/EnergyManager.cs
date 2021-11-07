using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;

    [Tooltip ("How much Energy the Player currently has")]
    [Range(0.1f, 10)] public float Energy = 1;

    [HideInInspector] public float EnergyMovementValue;
    [HideInInspector] public float EnergyBoostValue;
   // [HideInInspector] public float EnergyShadowDashValue;
    [HideInInspector] public float EnergySuperBoostValue;


    public float ReduceEnergyMulitplicator = 0.9f;

    private void Awake()
    {
        if(EnergyManager.Instance == null)
        {
            EnergyManager.Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    void Update()
    {
        EnergyValues();

        // WEnn ernergie 0 ist x sekuden warten und wieder etwas hoch setzten, damit man sich wieder bewegen aknn   (Akutell wird die Energy in PlayerMovement geclampt, sodass nie nie kleiner als 0,4 wird)

        if (Energy > 10f)
        {
            Energy = 10f;
        }

        

        

        /*
        if (Energy > 0 && Energy < 0.7)
        {
            EnergyBoostValue = Energy * 1.2f; //
        }
        else if (Energy >= 0.7 && Energy < 1.5)
        {
            EnergyBoostValue = Energy * 1f; //
        }
        else if ( Energy >=1.5 && Energy < 3)
        {
            EnergyBoostValue = Energy * 0.8f; //
        }
        else if (Energy >= 3 && Energy < 4.5) 
        {
            EnergyBoostValue = Energy * 0.5f;//
        }
        else if (Energy >= 4.5 && Energy < 7)
        {
            EnergyBoostValue = Energy * 0.4f;//
        }
        else if (Energy >= 7 && Energy <= 8.5)
        {
            EnergyBoostValue = Energy * 0.3f;//
        }
        else if (Energy >= 8.5 && Energy <= 10)
        {
            EnergyBoostValue = Energy * 0.2f;//
        }
        */
    }


    void EnergyValues()
    {   
        EnergyMovementValue = Energy * 1f;
        if (EnergyMovementValue < 0.7f) EnergyMovementValue = 0.7f;

        EnergyBoostValue = Energy * 1f;
        EnergySuperBoostValue = Energy * 1.4f;

        //EnergyShadowDashValue = Energy * 1.5f;
    }

    public void ReduceEnergy()
    {
        Energy *= ReduceEnergyMulitplicator;
        Energy = Mathf.Clamp(Energy, 0.1f, 10);

        Debug.Log("ReduceEnergy");
    }
}

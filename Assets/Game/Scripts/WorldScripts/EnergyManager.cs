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
        gameMng = this.GetComponent<GameManager>();
    }

    void Update()
    {
        EnergyValues();

        // WEnn ernergie 0 ist x sekuden warten und wieder etwas hoch setzten, damit man sich wieder bewegen aknn   (Akutell wird die Energy in PlayerMovement geclampt, sodass nie nie kleiner als 0,4 wird)

        if (Energy > MaxEnergy)
        {
            Energy = MaxEnergy;
        }


        //Benötiger Energiwert als vorraussetzung?
        SaveEnergy();

       


        #region stuff
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
        #endregion
    }


    #region SaveEnergy (Abbrechbar)
    void SaveEnergy()
{
        if (Input.GetButton(gameMng.SaveEnergy))
        {
             savingEnergy = true;
        }
         else
            savingEnergy = false; //WEnn alle energie gespeichert werden soll, dann doch eine Coroutine draus machen.




        if (savingEnergy == true)
        {
            if (Energy >= 0)
            {
                float transferedEnergy = 0;

                transferedEnergy = Time.deltaTime;
                Energy = Energy - Time.deltaTime;
                SavedEnergy = SavedEnergy + transferedEnergy;
            }
            else
            {
                savingEnergy = false;
            }
        }
    }
    #endregion

    #region SaveEnergy über Coroutine (nicht abbrechbar)

    bool buttonPressedinLastFrame = false;
    void SaveEnergy1()
    {
        if (Input.GetButton(gameMng.SaveEnergy))
        {

            if (buttonPressedinLastFrame == false)
            {
                buttonPressedinLastFrame = true;
                savingEnergy = true;
                StartCoroutine(SaveAndRegenerateEnergy());
            }
        }
        else if (Input.GetButton(gameMng.SaveEnergy) && buttonPressedinLastFrame == true && savingEnergy == false)
        {
            savingEnergy = false;
            buttonPressedinLastFrame = false;
        }

        if (Input.GetButton(gameMng.SaveEnergy) == false && buttonPressedinLastFrame == true)
        {
            StopCoroutine(SaveAndRegenerateEnergy());
            Debug.Log("Stop");
            savingEnergy = false;
            buttonPressedinLastFrame = false;
        }






    }

    IEnumerator SaveAndRegenerateEnergy()
    {
        
        //SaveEnergy
        while (Energy >= 0)
        {
            float transferedEnergy = 0;

            transferedEnergy = Time.deltaTime;
            Energy = Energy - Time.deltaTime;
            SavedEnergy = SavedEnergy + transferedEnergy;

            yield return null;
        }


        /*
        yield return new WaitForSeconds(0.4f);

        //Regenerate Minmal Energy
        while (Energy >= 0.5f)
        {
            Energy = Energy +  Time.deltaTime * 0.1f;

            yield return null;
        }

        Energy = Mathf.Clamp(Energy, 0.5f, 5);


        yield return null;
        Debug.Log("Q");
        */
    }

    #endregion


  

    void EnergyValues()
    {   
        EnergyMovementValue = Energy * 1f;
        if (EnergyMovementValue < 0.7f)
        {
            EnergyMovementValue = 0.7f;
        }

        EnergyBoostValue = Energy * 1.2f;
        EnergySuperBoostValue = Energy * 1.4f;

        //EnergyShadowDashValue = Energy * 1.5f;
    }

    public void ReduceEnergy()
    {
        //Energy *= ReduceEnergyMulitplicator;
        Energy = Mathf.Clamp(Energy, 0f, MaxEnergy);

    }
}

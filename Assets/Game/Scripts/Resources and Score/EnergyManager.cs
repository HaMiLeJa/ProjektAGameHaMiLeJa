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
    public float MaxEnergyAmount = 20f;
    [SerializeField] float currentEnergyForInspector;

    [SerializeField] float stepSize = 0.1f;


    #region Singleton and Set Energy
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

        CurrentEnergy = EnergyStartAmount;
    }
    #endregion

    void Start()
    {
        gameMng = GameManager.Instance;
       
       

    }

    
    void Update()
    {
        if (DisableEnergyCosts == true)
        {
            CurrentEnergy = 25;
        }

        currentEnergyForInspector = CurrentEnergy;

        CheckEnergyAmount();
    }


    /*
    void ModifyEnergy(float value)
    {

        
        float step = stepSize * Mathf.Sign(value);
        float stepsDone = 0;

        float timer = 0;

        while (stepsDone < Mathf.Abs(value))
        {
            timer = Time.deltaTime;

            if(timer > 0.1f)
            {
                CurrentEnergy += step;
                stepsDone += Mathf.Abs(step);
                timer = 0;
            }
           
        }
        
        
        //CurrentEnergy += value;

    }
    */

    public IEnumerator ModifyEnergy(float value)
    {
        float absValue = Mathf.Abs(value);

        float step = stepSize * Mathf.Sign(value);
        float stepsDone = 0;

        while (stepsDone < absValue)
        {
            CurrentEnergy += step;
            stepsDone += stepSize;

            UIManager.Instance.UpdateEnergyUI();

            yield return new WaitForFixedUpdate();
        }


        yield return null;
    }


    void CheckEnergyAmount()
    {
        if (CurrentEnergy <= 0)
        {
            //if (startDash.Boosting == true) return;

            gameMng.AllowMovement = false;

            StopAllCoroutines();
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


    public bool CheckForRequiredEnergyAmount(float value)
    {
        if (value <= CurrentEnergy)
            return true;
        else
            return false;
            //Sound machen
    }

}
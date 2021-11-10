using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    EnergyManager energyMng;
    [SerializeField] TMPro.TMP_Text energy;

    [SerializeField] TMPro.TMP_Text savedEnergy;


    void Start()
    {
        energyMng = FindObjectOfType<EnergyManager>();
    }
   

  
    void Update()
    {
        //Ordentlich machen
        float displayedEnergy = Mathf.Clamp(energyMng.Energy, 0, energyMng.MaxEnergy);

        energy.text = ("Energy: " + displayedEnergy);
        savedEnergy.text = ("Saved Energy: " + energyMng.SavedEnergy);

    }


    // Schreibt hier bitte Methoden, in denen das UI geupdatet wird und Ruft diese dann in den anderen Scripten auf (z.b. �ber Events), damit das UI nicht permanet unn�tig geupdated wird
    // das hier hat ein Singleton pattern, also k�nnt ihr in anderen Scripten mit UIManager.Instance.methodenname darauf zugreifen
}

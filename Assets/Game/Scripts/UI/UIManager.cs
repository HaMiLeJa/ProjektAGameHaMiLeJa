using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    EnergyManager energyMng;
    [SerializeField] TMPro.TMP_Text energy;


    void Start()
    {
        energyMng = FindObjectOfType<EnergyManager>();
    }
   

  
    void Update()
    {
        float displayedEnergy = Mathf.Clamp(energyMng.Energy, 0, 5);

        energy.text = ("Energy: " + displayedEnergy);

    }


    // Schreibt hier bitte Methoden, in denen das UI geupdatet wird und Ruft diese dann in den anderen Scripten auf (z.b. über Events), damit das UI nicht permanet unnütig geupdated wird
    // das hier hat ein Singleton pattern, also könnt ihr in anderen Scripten mit UIManager.Instance.methodenname darauf zugreifen
}

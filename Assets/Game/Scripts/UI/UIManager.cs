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
        
        energy.text = ("Energy: " + energyMng.Energy);

    }


    // Schreibt hier bitte Methoden, in denen das UI geupdatet wird und Ruft diese dann in den anderen Scripten auf (z.b. �ber Events), damit das UI nicht permanet unn�tig geupdated wird
    // das hier hat ein Singleton pattern, also k�nnt ihr in anderen Scripten mit UIManager.Instance.methodenname darauf zugreifen
}

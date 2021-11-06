using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMPro.TMP_Text energy;


    private void Awake()
    {
        if (UIManager.Instance == null)
        {
            UIManager.Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        
    }

  
    void Update()
    {
        
        energy.text = ("Energy: " + EnergyManager.Instance.Energy);
    }


    // Schreibt hier bitte Methoden, in denen das UI geupdatet wird und Ruft diese dann in den anderen Scripten auf (z.b. über Events), damit das UI nicht permanet unnütig geupdated wird
    // das hier hat ein Singleton pattern, also könnt ihr in anderen Scripten mit UIManager.Instance.methodenname darauf zugreifen
}

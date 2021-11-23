using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager gameMng;

    public TMPro.TMP_Text DestroyablePoints;


    void Start()
    {
        gameMng = GameManager.Instance;

        gameMng.onDestroyableDestroyed += UpdateDestroyableUI;
    }
   

  
    void Update()
    {
        

    }


    void UpdateDestroyableUI(float value)
    {
        Debug.Log("EventCalled");
        DestroyablePoints.text = ResourceManager.Instance.DestroyablePoints.ToString();
    }

    // Schreibt hier bitte Methoden, in denen das UI geupdatet wird und Ruft diese dann in den anderen Scripten auf (z.b. über Events), damit das UI nicht permanet unnütig geupdated wird
    // das hier hat ein Singleton pattern, also könnt ihr in anderen Scripten mit UIManager.Instance.methodenname darauf zugreifen
}

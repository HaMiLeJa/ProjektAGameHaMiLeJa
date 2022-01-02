using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{

    // IDEE 1
    // Liste mit allen enum collectable Hexagonen (die subsciben sich selber in die liste rein)

    //Counter: mit aktueller  collectable anzhal. Wenn counter kleiner ist, als max, dann wird ein Hex
    //aus liste gesucht und und da gespawnt


    //Idee 2
    // Dictionary mit enum collectable Hexagonen(die subsciben sich selber in die liste rein); hex und activeCollectable? abspeichern


    //  ALLGEMEINES

    //nur collectables mit einer bestimmten distanz zum spieler werden gespant (damit er es nicht sieht



    //  MIRO SCHREIBT:
    //wenn du willst das 3 stück auf einmal spawnen sollen, kannst du entweder eine verkettete liste nutzen oder ein event trigger.
    //Wenn eins von denen spawnt, check ob da schon ein Objekt ist und falls nicht spawne die auch.

    //du kannst ne boolean abfrage machen. sobald was eingesammelt wurde, triggert es einen timer der erst beim ablaufen  wieder das feld als "bespawnable" macht

    // Hex und References
    public static Dictionary<GameObject, CollectableReferences> AllCollectables = new Dictionary<GameObject, CollectableReferences>();



    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            StartCollectableSpawn();
    }



    void StartCollectableSpawn()
    {
        foreach (KeyValuePair <GameObject, CollectableReferences> hex in AllCollectables)
        {
            if(hex.Value.activeCollectable == false)
            {
                hex.Value.HexCollectableScript.SpawnCollectable();
            }


        }
    }

    public void CollectableCollected(GameObject item, GameObject parent)
    {
        AllCollectables[parent].activeCollectable = false;
        AllCollectables[parent].Collectable = null;

        //Effekte, Sound


        Destroy(item);
    }
}

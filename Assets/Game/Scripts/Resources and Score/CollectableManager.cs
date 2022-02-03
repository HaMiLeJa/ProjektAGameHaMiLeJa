using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{

    public delegate void RespawnCollectables();
    public static RespawnCollectables OnRespawnCollectables;

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

    // Hex und References(Hex und bool)
    public static Dictionary<GameObject, CollectableReferences> AllCollectables;

    public static bool StopEditorScript = false;

    public CollectableReferences refff;

    // private bool addAll = true;

    Vector3 collectalbeOriginalScale;

    private void Awake()
    {
        StopEditorScript = true;
        AllCollectables =  new Dictionary<GameObject, CollectableReferences>();
    }
    void Start()
    {
        collectalbeOriginalScale = GameObject.FindObjectOfType<Collectable>().transform.localScale;
        //if (addAll == true)
        // {
             OnRespawnCollectables = null;
             OnRespawnCollectables += StartCollectableSpawn;
            // evt noch ui benachrichtigung

            if (AllCollectables != null)
            {
                foreach (KeyValuePair<GameObject, CollectableReferences> hex in AllCollectables)
                {
                    hex.Value.HexScript = hex.Key.GetComponent<Hex>();
                    SetCollectableReferencesAtStart(hex.Value.HexScript);
                }
            }

         //   addAll = false;
        //}
    }

    void SetCollectableReferencesAtStart(Hex hex)
    {
        if(hex.myProps.GetComponentInChildren<Collectable>())
        {
            Collectable colScript = hex.myProps.GetComponentInChildren<Collectable>();
            hex.MyCollectable = colScript.gameObject;
            hex.colRef.ActiveCollectable = true;

            colScript.ParentHex = hex.gameObject;
        }
        else
        {
            hex.SpawnCollectable();
        }
        
    }


    
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.J))
            OnRespawnCollectables?.Invoke();

    }

    public void StartCollectableSpawn()
    {
        foreach (KeyValuePair <GameObject, CollectableReferences> hex in AllCollectables)
        {
            if(hex.Value.ActiveCollectable == false && DistanceToPlayer(hex.Key) >= 100f) //&& if Distance to player ist höher als was auch immer
            {
                hex.Value.HexScript.SpawnCollectable();

            }
        }
    }

    float DistanceToPlayer(GameObject hex)
    {
        Vector3 Verbindungsvector = ReferenceLibary.Player.transform.position - hex.transform.position;
        float distance = Mathf.Sqrt(Mathf.Pow(Verbindungsvector.x, 2) + Mathf.Pow(Verbindungsvector.y, 2) + Mathf.Pow(Verbindungsvector.z, 2));
        
        return distance;
    }


    public void CollectableCollected(GameObject item, GameObject hex)
    {
        AllCollectables[hex].ActiveCollectable = false;


        EnergyManager.energyGotHigher = true;
        StartCoroutine(ReferenceLibary.EnergyMng.ModifyEnergy(1));
        ReferenceLibary.AudMng.HexAudMng.PlayHex(HexType.DefaultCollectable);

        item.SetActive(false);
        //Destroy(item);
    }

    IEnumerator DecreaseItemSize(GameObject Item) // not testet yet
    {
        Vector3 scale = Item.transform.localScale;
        float goal = 0.1f;
        float t = 0.5f;
        while (Item.transform.localScale.x >= goal)
        {
            Item.transform.localScale = new Vector3(Mathf.Lerp(Item.transform.localScale.x, goal, t),
            Mathf.Lerp(Item.transform.localScale.y, goal, t),
            Mathf.Lerp(Item.transform.localScale.z, goal, t));

            yield return null;
        }
        yield return null;
    }

}

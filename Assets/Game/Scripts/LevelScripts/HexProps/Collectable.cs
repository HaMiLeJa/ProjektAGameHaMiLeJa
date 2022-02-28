using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    #region Inspector
    [SerializeField] float speed = 0;
    [SerializeField] float rotation = 400;

    public GameObject ParentHex; //Wird assigned in CollectalbeManager Start -> SetCollectableReferencesAtStart

    public ScriptableLevelObject settings;
    public CollectableReferences colRef;
   #endregion
   
    void Start()
    {

        if (ParentHex == null)
        {
            Debug.Log("I have no parent Hex :(");
            this.gameObject.SetActive(false);


            if (this.GetComponentInParent<Hex>().gameObject != null)
            {
                ParentHex = this.GetComponentInParent<Hex>().gameObject;

                colRef.HexScript = ParentHex.GetComponent<Hex>();
                colRef.HexScript = ParentHex.GetComponent<Hex>();
            }
            else
            {
                Debug.Log("still no parent Hex");
            }
        }

        if (ParentHex != null)
        {
            CollectableManager.AllCollectables.Add(ParentHex, colRef);
            ParentHex.GetComponent<Hex>().MyCollectable = this.gameObject;
        }
        else
            this.gameObject.SetActive(false);
    }

    void Update()
    {
        
        transform.Rotate(new Vector3(0, rotation * Time.deltaTime,0));

        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {
            ScoreManager.OnScoring?.Invoke(settings.value);


            ReferenceLibary.ColMng.CollectableCollected(this.gameObject, settings.secondValue ,ParentHex); //hier drin sind auch sounds;
            
        }
    }
}
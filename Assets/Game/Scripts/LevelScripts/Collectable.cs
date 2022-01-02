using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    #region Inspector
    [SerializeField] float speed = 0;
    [SerializeField] float rotation = 20;

    [HideInInspector] public GameObject ParentHex;

    public ScriptableLevelObject settings;
   #endregion
   
    void Start()
    {
        
    }

    void Update()
    {
        #region forward mov + rotation

       // transform.position   += transform.forward *Time.deltaTime * speed;
        transform.Rotate(new Vector3(0, rotation * Time.deltaTime,0));

        #endregion
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {
            ScoreManager.OnScoring?.Invoke(settings.value);

            ReferenceLibary.ColMng.CollectableCollected(this.gameObject, ParentHex);



            Destroy(this.gameObject);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    #region Inspector
    [SerializeField] float speed = 0;
    [SerializeField] float rotation = 0;
   
    Rigidbody2D CollectableRigidbody;
    GameObject Player;
   #endregion
   
    void Start()
    {
        CollectableRigidbody = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
    }

 #region Destroy
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Player)
        {  
            CollectingManager.counterCaught++;
            Destroy(this.gameObject);
          
        }
	}
 #endregion
    void Update()
    {
        #region forward mov + rotation

        transform.position   += transform.forward *Time.deltaTime * speed;
        transform.Rotate(new Vector3(0, rotation * Time.deltaTime,0));

        #endregion
        
    }
}
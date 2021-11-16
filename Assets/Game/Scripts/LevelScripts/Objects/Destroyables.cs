using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyables : MonoBehaviour
{
    [SerializeField] float lifeStartAmount = 100;
    [Tooltip ("Dont change. Its Set to lifeStartAmount in the code")] [SerializeField] float currentLife;
    


    void Start()
    {
        currentLife = lifeStartAmount;
        
    }

    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //evt boost bools abfragen, um mehr abzuziehen

            GameObject player = collision.gameObject;

            if(player == null)
            {
                return;
            }


            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            float totalVelocity = Mathf.Abs(playerRb.velocity.x) + Mathf.Abs(playerRb.velocity.y) + Mathf.Abs(playerRb.velocity.z);

            float multiplicator = 0.4f;

            currentLife -= totalVelocity * multiplicator;


            if (currentLife <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyables : MonoBehaviour
{
    [SerializeField] float lifeStartAmount = 5;
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

            if(player.GetComponent<PlayerBoost>().dealDamage == true)
            {
                currentLife -= 1;
            }

            if (player.GetComponent<PlayerStartDash>().dealDamage == true)
            {
                currentLife -= 4;
            }

            if(currentLife <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

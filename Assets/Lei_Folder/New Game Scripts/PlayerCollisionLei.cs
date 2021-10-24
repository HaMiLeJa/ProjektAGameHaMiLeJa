using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollisionLei : MonoBehaviour
{
    //Leis Code
    public GameObject collisionPrefabOne;
    public GameObject collisionPrefabTwo;
    int lives = 3;
    public Text livesDisplay;
    Vector3 spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// Code von Lei
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            Vector3 targetPos = coll.gameObject.transform.position;
            Destroy(coll.gameObject);
            Instantiate(collisionPrefabOne, targetPos, Quaternion.identity);



        }
        else if (coll.gameObject.CompareTag("Defense"))
        {
            Destroy(coll.gameObject);
            Instantiate(collisionPrefabTwo, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            //lives--;
            // livesDisplay.text = "Lives: " + lives;

            //if (lives > 0)
            // Invoke(nameof(NextLife), 3);

        }


    }

    /*void NextLife()
    {
        gameObject.SetActive(true);

        //Respawn();
        
        transform.position += new Vector3(0, 22,0);
    }

    void SaveNewPosition()
    {
        spawnPoint = transform.position;
    }

    void Respawn()
    {
        transform.position = spawnPoint;
    }*/
}

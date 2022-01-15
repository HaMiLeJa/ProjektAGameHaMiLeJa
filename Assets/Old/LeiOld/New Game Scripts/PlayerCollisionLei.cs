using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCollisionLei : MonoBehaviour
{
    //Leis Code
    public GameObject collisionPrefabOne;
    public GameObject collisionPrefabTwo;
    int lives = 3;
    public Text livesDisplay;
    bool gameStarted = false;
    Vector3 spawnPoint;
    // Start is called before the first frame update
   

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
            gameStarted = false;
            //lives--;
            // livesDisplay.text = "Lives: " + lives;

            //if (lives > 0)
            // Invoke(nameof(NextLife), 3);

        }
    }
        public void ApplicationEndButton_Click()
        {
            Application.Quit();
        }

        public void RestartGame()
        {
             UnityEngine.SceneManagement.SceneManager.LoadScene("Enemy_Movement");
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

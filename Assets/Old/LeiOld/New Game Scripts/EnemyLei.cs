using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLei : MonoBehaviour
{
    public GameObject player;
    //PlayerScript playerClass;

    public GameObject defensePrefab;
    bool reload = false;

    public int distanceForce;
    public int magnitudeDistance;
    public int invokeTime;

    int points = 0;
    int pointsScore;
    public Text pointDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // transform.position = new Vector3(Random.Range(20, 980), 50, Random.Range(20, 980));
        // playerClass = player.GetComponent<PlayerScript>();

    }

    // Update is called once per frame

    private void Update()
    {
        Vector3 distance = player.transform.position - transform.position;
        if (!reload && distance.magnitude < magnitudeDistance)
        {
            GameObject defense = (GameObject)Instantiate
            (defensePrefab, transform.position, Quaternion.identity);

            defense.GetComponent<Rigidbody>().AddForce(distanceForce * distance);
            reload = true;
            Invoke(nameof(ReloadEnded), invokeTime);
        }
    }

    void ReloadEnded()
    {
        reload = false;
   
    }

  

    /*
  public void EnterGameMode()
  {
      gameObject.AddComponent<Rigidbody>();
      //Adds Rigidbody to gameObject
      void ReloadFinished()
  {
      reload = false;
  }*/

    //Checking(gameObject);

    /*public void Checking(GameObject go)
    {
        if (go.transform.position.y < 0)
            go.transform.position += new Vector3(0, 50, 0);

        if (go.transform.position.x < 0)
            go.transform.position = new Vector3
                (0, go.transform.position.y, go.transform.position.z);

        else if (go.transform.position.x > 1000)
            go.transform.position = new Vector3
                (1000, go.transform.position.y, go.transform.position.z);

        if (go.transform.position.z < 0)
            go.transform.position = new Vector3
            (go.transform.position.x, go.transform.position.y, 0);

        else if (go.transform.position.z > 1000)
            go.transform.position = new Vector3
                (go.transform.position.x, go.transform.position.y, 1000);

    }*/

}
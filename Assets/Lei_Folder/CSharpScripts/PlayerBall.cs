using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBall : MonoBehaviour
{
    public GameObject cameras;
    public GameObject winning;
    int points = 0;
    public Text pointsDisplay;
    public int cameraY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameras.transform.position = new Vector3(0, cameraY, transform.position.z - 10); 
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            transform.position = new Vector3(transform.position.x, transform.position.y + 0.001f, transform.position.z);
        }
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Win"))
        {
            points++;
            pointsDisplay.text = "Points: " + points;
            winning.SetActive(false);
            Invoke(nameof(NextWinning), 2);
        }
        else if (coll.gameObject.CompareTag("Winning"))
        {
            points--;
            pointsDisplay.text = "Points: " + points;
        }
    }

    void NextWinning()
    {

        winning.SetActive(true);
        winning.transform.localPosition = new Vector3(Random.Range(-0.3f, 0.3f), 1.55f, Random.Range(-.3f, 0.3f));

    }

  

}

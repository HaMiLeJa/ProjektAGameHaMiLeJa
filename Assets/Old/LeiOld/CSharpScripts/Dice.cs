using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public GameObject dicePrefab;
    List<GameObject> diceList;
    Rigidbody rb;

    bool end = false;
    public Material[] mat = new Material[4];

    int amounts = 0;
    public int moveNumber;
    public Text amountsDisplay;
    public int diceAmount;
    public float diceGroundMin;
    public float diceGroundMax;
    private void Start()
    {
        diceList = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (end) return;

        float xNew = transform.position.x;
        float zNew = transform.position.z;

        if (Input.GetKeyDown(KeyCode.L))
        {
            xNew++;
            if (xNew > moveNumber) xNew = moveNumber;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            xNew--;
            if (xNew < -moveNumber) xNew = -moveNumber;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            zNew++;
            if (zNew > moveNumber) zNew = moveNumber;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            zNew--;
            if (zNew < -moveNumber) zNew = -moveNumber;
        }
        transform.position = new Vector3(xNew, transform.position.y, zNew);
    }

    private void OnCollisionEnter(Collision coll)
    {
        Vector3 positionOld = transform.position;

        if (positionOld.y < diceAmount)
        {
            Material materialOld = GetComponent<MeshRenderer>().material;
            GetComponent<MeshRenderer>().material = mat[Random.Range(0, 4)];

            transform.position = new Vector3(0, 6, 0);
            rb.drag *= 0.98f;

            Object objectReference = Instantiate(dicePrefab, positionOld, Quaternion.identity);
            GameObject gameObjectReference = (GameObject)objectReference;
            gameObjectReference.GetComponent<MeshRenderer>().material = materialOld;
            diceList.Add(gameObjectReference);

            Check();
        }
        else
            end = true;
    }

    void Check()
    {
        int counter = 0;
        for (int k = 0; k < diceList.Count; k++)
            if (diceList[k].transform.position.y >= diceGroundMin &&
                diceList[k].transform.position.y <= diceGroundMax)
            {
                counter++;
            }

        if (counter == diceAmount)
        {
            amounts++;
            amountsDisplay.text = "Amounts: " + amounts;


            for (int k = diceList.Count - 1; k >= 0; k--)
                if (diceList[k].transform.position.y >= diceGroundMin &&
                    diceList[k].transform.position.y <= diceGroundMax)
                {
                    Destroy(diceList[k]);
                    diceList.RemoveAt(k);
                }
        }
    }
}
 

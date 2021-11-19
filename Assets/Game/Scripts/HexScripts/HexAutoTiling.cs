using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexAutoTiling : HexGrid
{

    public static List<GameObject> HexesToBeMoved = new List<GameObject>();
    GameObject playerLocation;
    // Start is called before the first frame update
    void Awake()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player");
        if(HexesToBeMoved != null){
        HexesToBeMoved.AddRange(GameObject.FindGameObjectsWithTag("Hex"));
        }
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if( !(Input.GetKeyDown(KeyCode.P)))
        {
        foreach (GameObject hex in HexesToBeMoved )
        {
            if(playerLocation.transform.position.z -205 > hex.transform.position.z)
                hex.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y, hex.transform.position.z+298);
            
            if(playerLocation.transform.position.z +205 < hex.transform.position.z)
                hex.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y, hex.transform.position.z-298);
            
            
            if(playerLocation.transform.position.x +205< hex.transform.position.x)
                hex.transform.position = new Vector3(hex.transform.position.x-344.5f, hex.transform.position.y, hex.transform.position.z);
            
            if(playerLocation.transform.position.x -205 > hex.transform.position.x)
                hex.transform.position = new Vector3(hex.transform.position.x+344.5f, hex.transform.position.y, hex.transform.position.z);

        }
        }
        
        
    }

}

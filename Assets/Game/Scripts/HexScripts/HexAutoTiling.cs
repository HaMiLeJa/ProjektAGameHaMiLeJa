using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HexAutoTiling : HexGrid
{

    public static List<GameObject> HexesToBeMoved = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        HexesToBeMoved.AddRange(GameObject.FindGameObjectsWithTag("Hex"));
        Debug.Log(HexesToBeMoved);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject hex in HexesToBeMoved )
        {
            if(player.transform.position.z -205 > hex.transform.position.z)
                hex.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y, hex.transform.position.z+298);
            
            if(player.transform.position.z +205 < hex.transform.position.z)
                hex.transform.position = new Vector3(hex.transform.position.x, hex.transform.position.y, hex.transform.position.z-298);
            
            
            if(player.transform.position.x +205< hex.transform.position.x)
                hex.transform.position = new Vector3(hex.transform.position.x-344.5f, hex.transform.position.y, hex.transform.position.z);
            
            if(player.transform.position.x -205 > hex.transform.position.x)
                hex.transform.position = new Vector3(hex.transform.position.x+344.5f, hex.transform.position.y, hex.transform.position.z);

        }
        
    
        
        
    }

  
}

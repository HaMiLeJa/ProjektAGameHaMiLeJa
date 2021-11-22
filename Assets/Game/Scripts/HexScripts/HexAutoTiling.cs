using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexAutoTiling : HexGrid
{
    public static List<GameObject> HexesToBeMoved = new List<GameObject>();
    private int startTilingTreshhold = 65;
    private bool playerHasMoved = true;
    [SerializeField] GameObject playerLocation;

    private float xPlusSnapShotPos;
    private float xMinusSnapShotPos;
    private float zPlusSnapShotPos;
    private float zMinusSnapShotPos;

    private float tilingTreshold = 307.5f;
    private float zTilingDistance = 438;
    private float xTilingDistance = 517;
    
    

    // Start is called before the first frame update
    void Awake()
    {   HexesToBeMoved.Clear();
        HexesToBeMoved.AddRange(GameObject.FindGameObjectsWithTag("Hex"));
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHasMoved)
        {
             xPlusSnapShotPos = playerLocation.transform.position.x + startTilingTreshhold;
             xMinusSnapShotPos = playerLocation.transform.position.x - startTilingTreshhold;
            
             zPlusSnapShotPos = playerLocation.transform.position.z + startTilingTreshhold;
             zMinusSnapShotPos = playerLocation.transform.position.z - startTilingTreshhold;

             playerHasMoved = false;
        }
        
        if (playerLocation.transform.position.x > xPlusSnapShotPos  ||
            playerLocation.transform.position.x < xMinusSnapShotPos ||
            playerLocation.transform.position.z > zPlusSnapShotPos  ||
            playerLocation.transform.position.z < zMinusSnapShotPos )
        {
            
        foreach (GameObject hex in HexesToBeMoved)
        {   if ( hex == null)
            return;
            if (playerLocation.transform.position.z + tilingTreshold < hex.transform.position.z)
                hex.transform.position = new Vector3(
                    hex.transform.position.x,
                    hex.transform.position.y,
                    hex.transform.position.z - zTilingDistance);
            
            if (playerLocation.transform.position.z - tilingTreshold > hex.transform.position.z)
                hex.transform.position = new Vector3(
                    hex.transform.position.x,
                    hex.transform.position.y,
                    hex.transform.position.z + zTilingDistance);
            
            if (playerLocation.transform.position.x + tilingTreshold < hex.transform.position.x)
                hex.transform.position = new Vector3(
                    hex.transform.position.x - xTilingDistance,
                    hex.transform.position.y,
                    hex.transform.position.z);
            
            if (playerLocation.transform.position.x - tilingTreshold > hex.transform.position.x)
                hex.transform.position = new Vector3(
                    hex.transform.position.x + xTilingDistance,
                    hex.transform.position.y,
                    hex.transform.position.z);

            playerHasMoved = true;
        }
        
        }
    }
        
}



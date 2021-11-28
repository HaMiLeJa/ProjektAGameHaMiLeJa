using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class HexAutoTiling : MonoBehaviour
{
    public static List<GameObject> HexesToBeMoved = new List<GameObject>();
    private int startTilingTreshhold = 130;
    float treshholdMoveBackToOrigin = 1300;
    

     [SerializeField] GameObject playerLocation;
     
     private float xPlusSnapShotPos;
    private float xMinusSnapShotPos;
    private float zPlusSnapShotPos;
    private float zMinusSnapShotPos;

    private float xOriginPosition;
    private float zOriginPosition;

    [Tooltip("ab wann soll er das Tiling anfangen?")] public float tilingTreshold = 307.5f; //default 307.5
    [Tooltip("wie weit soll er die Tiles nach z verschieben?") ] public static float zTilingDistance = 598; //default 438
    [Tooltip("wie weit soll er die Tiles nach xverschieben")] public static  float xTilingDistance = 691; //default 517
    
    

    // Start is called before the first frame update
    void Awake()
    
    {   
   
        HexesToBeMoved.Clear();
        HexesToBeMoved.AddRange(GameObject.FindGameObjectsWithTag("Hex"));
        
    }

    void Start()
    {
        xOriginPosition = playerLocation.transform.position.x;
        zOriginPosition = playerLocation.transform.position.z;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (HexCoordinates.playerHasMoved)
        {
             xPlusSnapShotPos = playerLocation.transform.position.x + startTilingTreshhold;
             xMinusSnapShotPos = playerLocation.transform.position.x - startTilingTreshhold;
            
             zPlusSnapShotPos = playerLocation.transform.position.z + startTilingTreshhold;
             zMinusSnapShotPos = playerLocation.transform.position.z - startTilingTreshhold;

             HexCoordinates.playerHasMoved = false;
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
            
                 

            HexCoordinates.playerHasMoved = true;
        }
        
        }
    }

    private void LateUpdate()
    {
        
        if (playerLocation.transform.position.x > treshholdMoveBackToOrigin  
            || playerLocation.transform.position.x < -treshholdMoveBackToOrigin
            ||  playerLocation.transform.position.z > treshholdMoveBackToOrigin  
            || playerLocation.transform.position.z < -treshholdMoveBackToOrigin  )
        {
            Vector3 moveEveryThingBack = new Vector3(
                playerLocation.transform.position.x - (xOriginPosition),
                0,
                playerLocation.transform.position.z - (zOriginPosition)
            );

            int numVcams = CinemachineCore.Instance.VirtualCameraCount;
            for (int i = 0; i < numVcams; ++i)
                CinemachineCore.Instance.GetVirtualCamera(i).OnTargetObjectWarped(
                    playerLocation.transform, -moveEveryThingBack);
            
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                foreach (GameObject allParentObjects in SceneManager.GetSceneAt(j).GetRootGameObjects())
                {
                    allParentObjects.transform.position -= moveEveryThingBack;
                    HexCoordinates.playerHasMoved = true;
                }
            }
        }
    }
    
}



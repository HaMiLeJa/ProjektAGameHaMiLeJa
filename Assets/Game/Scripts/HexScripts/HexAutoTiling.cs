using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class HexAutoTiling : MonoBehaviour
{
    private List<Vector3> hasAllTheHexPosCopy = new List<Vector3>();
    private List<Vector3> hasAllTheHexPos = new List<Vector3>();
    private  static Dictionary<float, GameObject> hasAllTheHexesDic = new Dictionary<float, GameObject>();
    
    private int startTilingTreshhold = 130;
    float treshholdMoveBackToOrigin = 1600;
    GameObject playerLocation;
     
     private float xPlusSnapShotPos;
    private float xMinusSnapShotPos;
    private float zPlusSnapShotPos;
    private float zMinusSnapShotPos;

    private float xOriginPosition;
    private float zOriginPosition;
    
    private bool leftMove = true;
    private bool rightMove = true;
    private bool topMove = true;
    private bool bottomMove = true;
    private float treshHoldBoth = 60;
    private int shortCircutToOrginCounter = 0;
    private int shortCircutTreshhold = 5;
    
    [Tooltip("ab wann soll er das Tiling anfangen?")] public float tilingTreshold = 307.5f; //default 307.5
    [Tooltip("wie weit soll er die Tiles nach z verschieben?") ] public static float zTilingDistance = 598; //default 438
    [Tooltip("wie weit soll er die Tiles nach xverschieben")] public static  float xTilingDistance = 691; //default 517
    void Awake()
    {
        playerLocation = GameObject.FindWithTag("Player");
        fillHexDic();
        fillHexPosArray();
    }
    void Start()
    {
        xOriginPosition = playerLocation.transform.position.x;
        zOriginPosition = playerLocation.transform.position.z;
    }
    void Update()
    {
        limitTiling();
        setFlags();
        if (bottomMove || topMove || leftMove || rightMove)
            moveHexes();
    }
    void fillHexDic()
    {
        int i = 1;
        foreach (GameObject hex in GameObject.FindGameObjectsWithTag("Hex"))
        {
            hasAllTheHexesDic[i] = hex;
            i++;
        }
    }
    void fillHexPosArray()
    {
        hasAllTheHexPos.Clear();
        foreach (KeyValuePair <float, GameObject> hex in hasAllTheHexesDic)
        {
            float xPos = hex.Value.transform.position.x;
            float zPos= hex.Value.transform.position.z;
            float key = hex.Key;
            Vector3 addToList = new Vector3(xPos, key, zPos);
            hasAllTheHexPos.Add(addToList );
        }
    }

    void setFlags()
    {    //only one if is less heavy + make sure that everything gets checked once
        if(
            playerLocation.transform.position.x > xPlusSnapShotPos ||
            playerLocation.transform.position.x < xMinusSnapShotPos ||
            playerLocation.transform.position.z > zPlusSnapShotPos ||
            playerLocation.transform.position.z < zMinusSnapShotPos
            )
        {
            if (playerLocation.transform.position.x > xPlusSnapShotPos)
            {
                rightMove = true;
                leftMove = false;
                compareTopBottom();
            }
            
            if (playerLocation.transform.position.x < xMinusSnapShotPos)
            {
                leftMove = true;
                rightMove = false;
                compareTopBottom();
            }

            if (playerLocation.transform.position.z > zPlusSnapShotPos)
            { 
                topMove = true;
                bottomMove = false;
                compareLeftRight();
            }
            
            if (playerLocation.transform.position.z < zMinusSnapShotPos)
            {
                bottomMove = true;
                topMove = false;
                compareLeftRight();
            }
        }
    }
    void compareTopBottom()
    {
        float zPlus = Mathf.Abs(zPlusSnapShotPos - playerLocation.transform.position.z);
        float zMinus = Mathf.Abs(zMinusSnapShotPos - playerLocation.transform.position.z);
        bool noSide = zPlus > treshHoldBoth && zMinus > treshHoldBoth;
        if ( noSide || zPlus < zMinus )
            topMove = true;
        if (noSide || zPlus > zMinus )
            bottomMove = true;
    }
    void compareLeftRight()
    {
        float xPlus = Mathf.Abs(xPlusSnapShotPos - playerLocation.transform.position.x);
        float xMinus = Mathf.Abs(xMinusSnapShotPos - playerLocation.transform.position.x);
        bool noSide = xPlus > treshHoldBoth && xMinus < treshHoldBoth;
        if ( noSide || xPlus< xMinus)
            rightMove = true;
        if ( noSide || xPlus > xMinus)
             leftMove = true;
    }
    void setAllFalse()
    {
        bottomMove = false;
        topMove = false;
        rightMove = false;
        leftMove = false;
    }
    void limitTiling()
    {
        //snapshot position so it only needs to update at certain distance
        if (HexCoordinates.playerHasMoved)
        {
            xPlusSnapShotPos = playerLocation.transform.position.x + startTilingTreshhold;
            xMinusSnapShotPos = playerLocation.transform.position.x - startTilingTreshhold;

            zPlusSnapShotPos = playerLocation.transform.position.z + startTilingTreshhold;
            zMinusSnapShotPos = playerLocation.transform.position.z - startTilingTreshhold;
            HexCoordinates.playerHasMoved = false;
            shortCircutToOrginCounter++;
        }
    }
    void moveHexes()
    {
             int vectorIndex = 0;
        
            hasAllTheHexPosCopy.Clear();
            hasAllTheHexPosCopy.AddRange(hasAllTheHexPos);
         
            foreach(Vector3 hexPos in hasAllTheHexPosCopy)
           {  
                if (bottomMove && playerLocation.transform.position.z + tilingTreshold < hasAllTheHexPos[vectorIndex].z)
                {
                    hasAllTheHexesDic[hasAllTheHexPos[vectorIndex].y].transform.position = new Vector3(hasAllTheHexPos[vectorIndex].x,
                        hasAllTheHexesDic[hasAllTheHexPos[vectorIndex].y].transform.position.y, hasAllTheHexPos[vectorIndex].z - zTilingDistance);
                    hasAllTheHexPos[vectorIndex] = new Vector3(hasAllTheHexPos[vectorIndex].x,
                        hasAllTheHexPos[vectorIndex].y,
                        hasAllTheHexPos[vectorIndex].z - zTilingDistance);
                    topMove = false;
                }

                if (topMove && playerLocation.transform.position.z - tilingTreshold > hasAllTheHexPos[vectorIndex].z)
                {
                    hasAllTheHexesDic[hasAllTheHexPos[vectorIndex].y].transform.position = new Vector3(hasAllTheHexPos[vectorIndex].x,
                        hasAllTheHexesDic[hasAllTheHexPos[vectorIndex].y].transform.position.y, hasAllTheHexPos[vectorIndex].z + zTilingDistance);
                    hasAllTheHexPos[vectorIndex] = new Vector3(
                        hasAllTheHexPos[vectorIndex].x,
                        hasAllTheHexPos[vectorIndex].y,
                        hasAllTheHexPos[vectorIndex].z + zTilingDistance);
                    bottomMove = false;
                }

                if (leftMove && playerLocation.transform.position.x + tilingTreshold < hasAllTheHexPos[vectorIndex].x)
                {
                    hasAllTheHexesDic[hasAllTheHexPos[vectorIndex].y].transform.position = new Vector3(hasAllTheHexPos[vectorIndex].x - xTilingDistance,
                        hasAllTheHexesDic[hasAllTheHexPos[vectorIndex].y].transform.position.y, hasAllTheHexPos[vectorIndex].z);
                    hasAllTheHexPos[vectorIndex] = new Vector3(
                        hasAllTheHexPos[vectorIndex].x - xTilingDistance,
                        hasAllTheHexPos[vectorIndex].y,
                        hasAllTheHexPos[vectorIndex].z);
                   rightMove = false;
                }
                
                if (rightMove && playerLocation.transform.position.x - tilingTreshold > hasAllTheHexPos[vectorIndex].x)
                {
                    hasAllTheHexesDic[hasAllTheHexPos[vectorIndex].y].transform.position = new Vector3(hasAllTheHexPos[vectorIndex].x + xTilingDistance,
                        hasAllTheHexesDic[hasAllTheHexPos[vectorIndex].y].transform.position.y, hasAllTheHexPos[vectorIndex].z);
                    hasAllTheHexPos[vectorIndex] = new Vector3(
                        hasAllTheHexPos[vectorIndex].x + xTilingDistance,
                        hasAllTheHexPos[vectorIndex].y,
                        hasAllTheHexPos[vectorIndex].z);
                    leftMove = false;
                }
                HexCoordinates.playerHasMoved = true;
                vectorIndex++;
           }
            setAllFalse();
    }
    private void LateUpdate()
    {
        ///return to origin
        if (shortCircutToOrginCounter > shortCircutTreshhold &&
            playerLocation.transform.position.x > treshholdMoveBackToOrigin  
            || playerLocation.transform.position.x < -treshholdMoveBackToOrigin
            || playerLocation.transform.position.z > treshholdMoveBackToOrigin  
            || playerLocation.transform.position.z < -treshholdMoveBackToOrigin  
            )
        {Debug.Log("Sir, I Moved everything back to Origin");
            
            Vector3 moveEveryThingBack = new Vector3(
                playerLocation.transform.position.x - (xOriginPosition),
                0,
                playerLocation.transform.position.z - (zOriginPosition)
            );
            int numVcams = CinemachineCore.Instance.VirtualCameraCount;
            HexCoordinates.playerHasMoved = true;
            for (int i = 0; i < numVcams; ++i)
                CinemachineCore.Instance.GetVirtualCamera(i).OnTargetObjectWarped(
                    playerLocation.transform, -moveEveryThingBack);

            for (int j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount; j++)
            {
                foreach (GameObject allParentObjects in UnityEngine.SceneManagement.SceneManager.GetSceneAt(j).GetRootGameObjects())
                {
                    allParentObjects.transform.position -= moveEveryThingBack;
                }
            }
            shortCircutToOrginCounter = 0;
            StartCoroutine(camUpdateAll());
        }
    }
    IEnumerator camUpdateAll()
    {
        yield return new WaitForSeconds(0.7f);
        fillHexPosArray();
        moveHexes();
        HexCoordinates.playerHasMoved = true;
        limitTiling();
    }
}



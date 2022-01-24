using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
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
    //bool markSortList = true;
   
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
        /*if (Input.GetKeyDown(KeyCode.J))
        {
            var test = hasAllTheHexPos.ToArray();
            sortVec3Array(test, 1200);
            hasAllTheHexPos = test.ToList();
        }

        if (Input.GetKey(KeyCode.K))
        {
            for(int i = 0; i < 20; i++)
            {
                Debug.Log(hasAllTheHexPos[i]);
            }
        }*/
        
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
              // markSortList = true;
                compareTopBottom();
            }
            
            if (playerLocation.transform.position.x < xMinusSnapShotPos)
            {
                leftMove = true;
                rightMove = false;
              //  markSortList = true;
                compareTopBottom();
            }

            if (playerLocation.transform.position.z > zPlusSnapShotPos)
            { 
                topMove = true;
                bottomMove = false;
               // markSortList = false;
                compareLeftRight();
            }
            
            if (playerLocation.transform.position.z < zMinusSnapShotPos)
            {
                bottomMove = true;
                topMove = false;
                
                  //  markSortList = false;
                
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
            // if(markSortList)
            // StartCoroutine(sortListCo());
        }
    }
    void moveHexes()
    {
             int vectorIndex = 0;
        
            hasAllTheHexPosCopy.Clear();
            hasAllTheHexPosCopy.AddRange(hasAllTheHexPos);

            float hor= 0, hor2 = 0, vert = 0, vert2 = 0;
            bool markDirtyVector = false;
            foreach(Vector3 hexPos in hasAllTheHexPosCopy)
           {  //&& vectorIndex > 13100
               if (leftMove  && playerLocation.transform.position.x + tilingTreshold < hexPos.x)
               {
                   hor = xTilingDistance;
                   rightMove = false;
                   markDirtyVector = true;
               }
                //&&  vectorIndex < 2800 
               if (rightMove && playerLocation.transform.position.x - tilingTreshold > hexPos.x)
               {
                   hor2 = xTilingDistance;
                   leftMove = false;
                   markDirtyVector = true;
               }
               
                if (bottomMove && playerLocation.transform.position.z + tilingTreshold < hexPos.z)
                {
                    vert = zTilingDistance;
                    topMove = false;
                    markDirtyVector = true;
                }

                if (topMove && playerLocation.transform.position.z - tilingTreshold > hexPos.z)
                {
                    vert2 = zTilingDistance;
                    bottomMove = false;
                    markDirtyVector = true;
                }
                
                if (markDirtyVector)
                { 
                    hasAllTheHexesDic[hexPos.y].transform.position = new Vector3(hexPos.x  - hor +hor2, 
                        hasAllTheHexesDic[hexPos.y].transform.position.y, hexPos.z - vert+vert2);
                    hasAllTheHexPos[vectorIndex] = new Vector3(hexPos.x - hor +hor2,
                        hexPos.y, hexPos.z - vert+vert2);
                    
                    hor = 0; vert = 0; hor2 = 0; vert2 = 0;
                    markDirtyVector = false;
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
    void setEverythingTrue()
    {
        topMove = true;
        bottomMove = true;
        leftMove = true;
        rightMove = true;
    }
    // IEnumerator sortListCo()
    // {
    //     yield return new WaitForSeconds(0.3f);
    //     hasAllTheHexPos = hasAllTheHexPos.OrderBy(x => x.x).ToList();
    // }
    IEnumerator camUpdateAll()
    {
        yield return new WaitForSeconds(0.25f);
        fillHexPosArray();
        setEverythingTrue();
        moveHexes();
        HexCoordinates.playerHasMoved = true;
        limitTiling();
    }

    
    /*static void sortVec3Array(Vector3[] arrayToSort, int stopAt)
    {
        int i, j;
        for (i = 1; i < stopAt; i++)
        {
            float item = arrayToSort[i].x;
            int ins = 0;
            for (j = i - 1; j >= 0 && ins != 1;)
            {
                if (item < arrayToSort[j].x)
                {
                    arrayToSort[j + 1].x = arrayToSort[j].x;
                    j--;
                    arrayToSort[j + 1].x = item;
                }
                else ins = 1;
            }
        }
    }*/

     

}



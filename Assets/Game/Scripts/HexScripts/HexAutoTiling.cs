#region Imports
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
#endregion
public class HexAutoTiling : MonoBehaviour
{
    #region Dictionarys
    [HideInInspector]
    public List<Vector3> hasAllTheHexPos = new List<Vector3>();
   
    private List<Vector3> hasAllTheHexPosHelperCopy = new List<Vector3>();
    [HideInInspector] public Dictionary<float, GameObject> hasAllTheHexesDic = new Dictionary<float, GameObject>();
    #endregion
    
    #region PrivateVariables
    private int startTilingTreshhold = 130;
    float moveBackToOriginTreshhold = 1600;
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
    private float allowBothSidesTreshhold = 60;
    private int shortCircutToOrginCounter = 0;
    private int shortCircutTreshhold = 5;

    private bool playerHasMoved = true;
    //bool markSortList = true;
    #endregion
    
    #region Inspector
    [Tooltip("ab wann soll er das Tiling anfangen?")] public float tilingTreshold = 307.5f; //default 307.5
    [Tooltip("wie weit soll er die Tiles nach z verschieben?") ] public static float zTilingDistance = 598; //default 438
    [Tooltip("wie weit soll er die Tiles nach xverschieben")] public static  float xTilingDistance = 691; //default 517
    #endregion

    #region Expressionbodys
    private bool tilingDistanceCheck => playerLocation.transform.position.x > xPlusSnapShotPos
                                        || playerLocation.transform.position.x < xMinusSnapShotPos
                                        || playerLocation.transform.position.z > zPlusSnapShotPos
                                        || playerLocation.transform.position.z < zMinusSnapShotPos;
    private bool returnToOriginDistanceCheck => shortCircutToOrginCounter > shortCircutTreshhold &&
                                        playerLocation.transform.position.x > moveBackToOriginTreshhold
                                        || playerLocation.transform.position.x < -moveBackToOriginTreshhold
                                        || playerLocation.transform.position.z > moveBackToOriginTreshhold
                                        || playerLocation.transform.position.z < -moveBackToOriginTreshhold;
    #endregion
    
    #region UnityUpdates                                  
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
    private void LateUpdate()
    {
        if (returnToOriginDistanceCheck)
        { 
            moveEverythingBackToOrigin();
            StartCoroutine(updateAllAfterOrigin());
        }
    }
    #endregion
    
    #region StartMethods
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
            float xPos = hex.Value.transform.position.x, 
                  zPos= hex.Value.transform.position.z,
                  key = hex.Key;
            Vector3 addToList = new Vector3(xPos, key, zPos);
            hasAllTheHexPos.Add(addToList );
        }
    }
    #endregion

    #region TilingRules
    void setFlags()
    {   
        if(tilingDistanceCheck)  //only one if is less heavy + make sure that everything gets checked once
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
                compareRightLeft(); 
                // markSortList = false;
            }
            
            if (playerLocation.transform.position.z < zMinusSnapShotPos)
            {
                bottomMove = true;
                topMove = false;
                //  markSortList = false;
                  compareRightLeft();
            }
        }
    }
    void compareTopBottom()
    {
        float zPlus = Mathf.Abs(zPlusSnapShotPos - playerLocation.transform.position.z);
        float zMinus = Mathf.Abs(zMinusSnapShotPos - playerLocation.transform.position.z);
        bool noSide = zPlus > allowBothSidesTreshhold && zMinus > allowBothSidesTreshhold;
        if ( noSide || zPlus < zMinus )
            topMove = true;
        if (noSide || zPlus > zMinus )
            bottomMove = true;
    }
    void compareRightLeft()
    {
        float xPlus = Mathf.Abs(xPlusSnapShotPos - playerLocation.transform.position.x);
        float xMinus = Mathf.Abs(xMinusSnapShotPos - playerLocation.transform.position.x);
        bool noSide = xPlus > allowBothSidesTreshhold && xMinus < allowBothSidesTreshhold;
        if ( noSide || xPlus< xMinus)
            rightMove = true;
        if ( noSide || xPlus > xMinus)
             leftMove = true;
    }
    void setAllFalse()
    {
        bottomMove = false; topMove = false; 
        rightMove = false; leftMove = false;
    }
    void limitTiling()
    {
        //snapshot position so it only needs to update at certain distance
        if (playerHasMoved)
        {
            xPlusSnapShotPos = playerLocation.transform.position.x + startTilingTreshhold;
            xMinusSnapShotPos = playerLocation.transform.position.x - startTilingTreshhold;
            zPlusSnapShotPos = playerLocation.transform.position.z + startTilingTreshhold;
            zMinusSnapShotPos = playerLocation.transform.position.z - startTilingTreshhold;
            playerHasMoved = false;
            shortCircutToOrginCounter++;
            // if(markSortList)
            // StartCoroutine(sortListCo());
        }
    }
    void moveHexes()
    {
             int vectorIndex = 0;
             hasAllTheHexPosHelperCopy.Clear();
             hasAllTheHexPosHelperCopy.AddRange(hasAllTheHexPos);

            float hor= 0, hor2 = 0, vert = 0, vert2 = 0;
            bool markDirtyVector = false;
            
            foreach(Vector3 hexPos in hasAllTheHexPosHelperCopy)
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
                    //update dic
                    hasAllTheHexesDic[hexPos.y].transform.position = new Vector3(hexPos.x - hor +hor2, 
                        hasAllTheHexesDic[hexPos.y].transform.position.y, hexPos.z - vert+vert2);
                    //update V3 List
                    hasAllTheHexPos[vectorIndex] = new Vector3(hexPos.x - hor +hor2,
                        hexPos.y, hexPos.z - vert+vert2);
                    
                    hor = 0; vert = 0; hor2 = 0; vert2 = 0;
                    markDirtyVector = false; 
                }
                playerHasMoved = true;
                vectorIndex++;
           }
            setAllFalse();
    }
    #endregion

    #region  OriginMethods
    void moveEverythingBackToOrigin()
    {
        //calculte Distances
        Vector3 moveEveryThingBack = new Vector3(
            playerLocation.transform.position.x - (xOriginPosition),
            0,
            playerLocation.transform.position.z - (zOriginPosition)); 
        
        //Informs vcams
        int numVcams = CinemachineCore.Instance.VirtualCameraCount;
        for (int i = 0; i < numVcams; ++i)
            CinemachineCore.Instance.GetVirtualCamera(i).OnTargetObjectWarped(
                playerLocation.transform, -moveEveryThingBack);
        
        //moves everything back
        for (int j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount; j++)
        { 
            foreach (GameObject allParentObjects in UnityEngine.SceneManagement.SceneManager.GetSceneAt(j).GetRootGameObjects())
                allParentObjects.transform.position -= moveEveryThingBack;
        }
        //sets flags
        shortCircutToOrginCounter = 0;
        Debug.Log("Sir, I Moved everything back to Origin");
    }
    IEnumerator updateAllAfterOrigin()
    { 
        yield return new WaitForSeconds(0.25f);
        fillHexPosArray();
        setEverythingTrue();
        moveHexes();
        playerHasMoved = true;
        limitTiling();
    }
    void setEverythingTrue()
    {
        topMove = true; bottomMove = true;
        leftMove = true; rightMove = true;
    }
    // IEnumerator sortListCo()
    // {
    //     yield return new WaitForSeconds(0.3f);
    //     hasAllTheHexPos = hasAllTheHexPos.OrderBy(x => x.x).ToList();
    // }
    /*static void sortVec3Array(Vector3[] arrayToSort, int startAt, int stopAt)
    {
        int i, j;
        for (i = startAt+1; i < stopAt; i++)
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
    #endregion
}



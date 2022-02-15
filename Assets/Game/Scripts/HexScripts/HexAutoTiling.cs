#region Imports
using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using System.Linq;
#endregion
public class HexAutoTiling : MonoBehaviour
{
    #region Arrays
    public HexPos[] hasAllTheHexPos= new HexPos[HEXCOUNT];
   [HideInInspector] public Transform[] hasAllTheHexesTransforms = new Transform[HEXCOUNT];
    #endregion
    
    #region PrivateVariables
    private const ushort HEXCOUNT = 16384, //später im Inspector bei mehr Level: default 16384
                         FIRSTQUARTER = 4096, //später im Inspector bei mehr Level: default 4096
                         LASTQUARTER = 12288; //später im Inspector bei mehr Level: default 12288
    
    GameObject playerLocation;
    
    private float xPlusSnapShotPos, xMinusSnapShotPos, 
                  zPlusSnapShotPos, zMinusSnapShotPos,
                  xOriginPosition, zOriginPosition;
                  
                
    private bool leftMove = true, rightMove = true, topMove = true, bottomMove = true,
                 playerHasMoved = true, markSortList = true;
    
    private byte    startTilingTreshhold = 130, //später im Inspector bei mehr Level: default 150
                    declineBothSidesTreshhold = 10, //später im Inspector bei mehr Level: default 10
                    shortCircutToOrginCounter = 0, 
                    shortCircutTreshhold = 8; //später im Inspector bei mehr Level: default 8
    
    [Tooltip("ab wann soll er das Tiling anfangen?")] [SerializeField] private float tilingTreshold = 307.5f; //default 307.5
    public static ushort zTilingDistance = 598, //default 598
                          xTilingDistance = 691, //default 691
                          moveBackToOriginTreshhold = 1600; //später im Inspector bei mehr Level: default 1600
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
        setFlags();
        if (bottomMove || topMove || leftMove || rightMove)
            moveHexes();
        limitTiling();
    }
    private void LateUpdate()
    {
        if (returnToOriginDistanceCheck)
        { 
            moveEverythingBackToOrigin();
            StartCoroutine(updateAllAfterOrigin(0.2f));
            StartCoroutine(sortListCo(0.12f));
        }
    }
    #endregion
    
    #region StartMethods
    void fillHexDic() 
    {
        ushort i = 0;
        foreach (GameObject hex in GameObject.FindGameObjectsWithTag("Hex"))
        {
            hasAllTheHexesTransforms[i] = hex.transform;
            i++;
        } 
    }
    void fillHexPosArray()
    {
        Array.Clear(hasAllTheHexPos,0,HEXCOUNT-1);
        ushort i = 0;
        foreach (Transform hex in hasAllTheHexesTransforms)
        { 
            hasAllTheHexPos[i] = new HexPos(hex.transform.position.x, i, hex.transform.position.z);
                i++;
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
                rightMove = true; compareTopBottom();
            }
            
            if (playerLocation.transform.position.x < xMinusSnapShotPos)
            {
                leftMove = true; compareTopBottom();
            }

            if (playerLocation.transform.position.z > zPlusSnapShotPos)
            { 
                topMove = true; compareRightLeft(); 
            }
            
            if (playerLocation.transform.position.z < zMinusSnapShotPos)
            {
                bottomMove = true; compareRightLeft();
            }
        }
    }
    void compareTopBottom()
    {
        float zPlus = Mathf.Abs(zPlusSnapShotPos - playerLocation.transform.position.z);
        float zMinus = Mathf.Abs(zMinusSnapShotPos - playerLocation.transform.position.z);
        bool noSide = zPlus > declineBothSidesTreshhold;
        
        if ( noSide && zPlus < zMinus ) topMove = true;
        if (noSide && zPlus > zMinus) bottomMove = true;
    }
    void compareRightLeft()
    {
        float xPlus = Mathf.Abs(xPlusSnapShotPos - playerLocation.transform.position.x);
        float xMinus = Mathf.Abs(xMinusSnapShotPos - playerLocation.transform.position.x);
        bool noSide = xPlus > declineBothSidesTreshhold;
        
        if ( noSide && xPlus < xMinus) rightMove = true;
        if ( noSide && xPlus > xMinus) leftMove = true;
    }
    void setAllFalse()
    {
        bottomMove = false; topMove = false; 
        rightMove = false; leftMove = false;
    }
    void limitTiling() //snapshot position so it only needs to update at certain distance
    {
        if (playerHasMoved)
        {
            xPlusSnapShotPos = playerLocation.transform.position.x + startTilingTreshhold;
            xMinusSnapShotPos = playerLocation.transform.position.x - startTilingTreshhold;
            zPlusSnapShotPos = playerLocation.transform.position.z + startTilingTreshhold;
            zMinusSnapShotPos = playerLocation.transform.position.z - startTilingTreshhold;
            StartCoroutine(sortListCo(0.15f));
            playerHasMoved = false;
            shortCircutToOrginCounter++;
        }
    }
    void moveHexes()
    {
        ushort vectorIndex = 0, hor = 0, hor2 = 0, vert = 0, vert2 = 0;
         bool markDirtyVector = false;
         foreach (HexPos hexPos in hasAllTheHexPos)
           {
               if (rightMove && vectorIndex <= FIRSTQUARTER && playerLocation.transform.position.x - tilingTreshold > hexPos.xPos)
               {
                   hor2 = xTilingDistance;
                   leftMove = false;
                   markDirtyVector = true;
               }
               if (bottomMove && playerLocation.transform.position.z + tilingTreshold < hexPos.zPos)
               {
                   vert = zTilingDistance;
                   topMove = false;
                   markDirtyVector = true;
               }
               if (topMove && playerLocation.transform.position.z - tilingTreshold > hexPos.zPos)
               {
                   vert2 = zTilingDistance;
                   bottomMove = false;
                   markDirtyVector = true;
               }
               if (leftMove && vectorIndex >= LASTQUARTER && playerLocation.transform.position.x + tilingTreshold < hexPos.xPos)
               {
                   hor = xTilingDistance;
                   rightMove = false;
                   markDirtyVector = true;
               }
               if (markDirtyVector)
               {
                   ushort dicKey = hexPos.dicKey; 
                   hasAllTheHexesTransforms[dicKey].transform.position = new Vector3(hexPos.xPos - hor + hor2,//update dic
                       hasAllTheHexesTransforms[dicKey].transform.position.y, hexPos.zPos - vert + vert2);
                 
                   hasAllTheHexPos[vectorIndex] = new HexPos(hexPos.xPos - hor + hor2,   //update Array v3
                       hexPos.dicKey, hexPos.zPos - vert + vert2);
                   
                   hor = 0; vert = 0; hor2 = 0; vert2 = 0;
                   markDirtyVector = false;
               }
               vectorIndex++;
           }
           playerHasMoved = true;
           setAllFalse();
    }
    #endregion
    
    #region  OriginMethods
    IEnumerator sortListCo(float sec)
    {
        yield return new WaitForSeconds(sec);
        hasAllTheHexPos.TimSort((pos1, pos2) => pos1.xPos.CompareTo(pos2.xPos));
    }
    void moveEverythingBackToOrigin()
    {
        //calculte Distances
        Vector3 moveEveryThingBack = new Vector3(
            playerLocation.transform.position.x - (xOriginPosition),
            0,
            playerLocation.transform.position.z - (zOriginPosition)); 
        
        //Informs vcams
        int numVcams = CinemachineCore.Instance.VirtualCameraCount;
        for (byte i = 0; i < numVcams; ++i)
            CinemachineCore.Instance.GetVirtualCamera(i).OnTargetObjectWarped(
                playerLocation.transform, -moveEveryThingBack);
        
        //moves everything back
        for (ushort j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount; j++)
        { 
            foreach (GameObject allParentObjects in UnityEngine.SceneManagement.SceneManager.GetSceneAt(j).GetRootGameObjects())
                allParentObjects.transform.position -= moveEveryThingBack;
        }
        //sets flags
        shortCircutToOrginCounter = 0;
        Debug.Log("Sir, I Moved everything back to Origin");
        fillHexPosArray();
    }
   
    IEnumerator updateAllAfterOrigin(float sec)
    { 
        yield return new WaitForSeconds(sec);
        setEverythingTrue();
        moveHexes();
        playerHasMoved = true;
    }
    void setEverythingTrue()
    {
        topMove = true; bottomMove = true;
        leftMove = true; rightMove = true;
    }
    #endregion
}

#region HexPosStruct
public struct HexPos 
{
    public float xPos;
    public ushort dicKey;
    public float zPos;
    public HexPos(float xPos, ushort dicKey, float zPos)
    {
        this.xPos = xPos;
        this.dicKey = dicKey;
        this.zPos = zPos;
    }
}
#endregion

  
     /*public const int RUN = 32;
     public static void insertionSort(Vector3[] arr, int left, int right)  
    {  
        for (int i = left + 1; i <= right; i++)  
        {  
            float temp = arr[i].x;  
            int j = i - 1;  
            while (arr[j+1].x > temp && j >= left)  
            {  
                arr[j+1].x = arr[j].x;  
                j--;  
            }  
            arr[j+1].x = temp;  
        }  
    }  
     
    public static void merge(Vector3[] arr, int l, int m, int r)  
    {
        int len1 = m - l + 1, len2 = r - m;  
        Vector3[] left = new Vector3[len1]; 
        Vector3[] right = new Vector3[len2];  
        for (int x = 0; x < len1; x++) 
            left[x] = arr[l + x];  
        for (int x = 0; x < len2; x++)  
            right[x] = arr[m + 1 + x];  
        
        int i = 0;  
        int j = 0;  
        int k = l;  
        
        while (i < len1 && j < len2)  
        {  
            if (left[i].x <= right[j].x)  
            {  
                arr[k].x = left[i].x;  
                i++;  
            }  
            else
            {  
                arr[k].x = right[j].x;  
                j++;  
            }  
            k++;  
        }     
        while (i < len1)  
        {  
            arr[k].x = left[i].x;  
            k++;  
            i++;  
        }  
        
        while (j < len2)  
        {  
            arr[k].x = right[j].x;  
            k++;  
            j++;  
        }  
    }  
    public static void timSort(Vector3[] arr, int n)  
    {  
        for (int i = 0; i < n; i+=RUN)  
            insertionSort(arr, i, Math.Min((i+31), (n-1)));  
        
        for (int size = RUN; size < n; size = 2*size)  
        {  
            for (int left = 0; left < n; left += 2*size)  
            {  
                // find ending point of left sub array  
                // mid+1 is starting point of right sub array  
                int mid = left + size - 1;  
                int right = Math.Min((left + 2*size - 1), (n-1));  
        
                // merge sub array arr[left.....mid] &  
                // arr[mid+1....right]  
                merge(arr, left, mid, right);  
            }  
        }  
    }  */
      

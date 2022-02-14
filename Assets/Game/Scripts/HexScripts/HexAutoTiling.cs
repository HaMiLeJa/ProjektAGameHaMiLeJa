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
    public List<HexPos> hasAllTheHexPos = new List<HexPos>();
    private List<HexPos> hasAllTheHexPosHelperCopy = new List<HexPos>();
    //  public Dictionary<ushort, GameObject> hasAllTheHexesDic = new Dictionary<ushort, GameObject>();
  public List<HextileObjects> hasAllTheHexesDic = new List<HextileObjects>();
    #endregion
    
    #region PrivateVariables
    private byte startTilingTreshhold = 130;
    ushort moveBackToOriginTreshhold = 1600;
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
    private byte declineBothSidesTreshhold = 10;
    private byte shortCircutToOrginCounter = 0;
    private byte shortCircutTreshhold = 8;

    private bool playerHasMoved = true;
    private bool hasMovedOnce = false;
    bool markSortList = true;
    private bool switchListValues = false;
    #endregion
    
    #region Inspector
    [Tooltip("ab wann soll er das Tiling anfangen?")] public float tilingTreshold = 307.5f; //default 307.5
    [Tooltip("wie weit soll er die Tiles nach z verschieben?") ] public static ushort zTilingDistance = 598; //default 438
    [Tooltip("wie weit soll er die Tiles nach xverschieben")] public static ushort xTilingDistance = 691; //default 517
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
            StartCoroutine(updateAllAfterOrigin());
            StartCoroutine(sortListCo());
        }
    }
    #endregion
    
    #region StartMethods
    void fillHexDic() 
    {
        ushort i = 0;
        foreach (GameObject hex in GameObject.FindGameObjectsWithTag("Hex"))
        {
            HextileObjects addToList = new HextileObjects(i, hex);
            hasAllTheHexesDic.Add(addToList);
            i++;
        } 
    }

    void fillHexPosArray()
    {
        hasAllTheHexPos.Clear();
        foreach (HextileObjects hex in hasAllTheHexesDic)
        {
            float xPos = hex.hextile.transform.position.x,
                zPos = hex.hextile.transform.position.z;
               ushort key = hex.dicKey;
            HexPos addToList = new HexPos(xPos, key, zPos);
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
    void limitTiling()
    {
        //snapshot position so it only needs to update at certain distance
        if (playerHasMoved)
        {
            xPlusSnapShotPos = playerLocation.transform.position.x + startTilingTreshhold;
            xMinusSnapShotPos = playerLocation.transform.position.x - startTilingTreshhold;
            zPlusSnapShotPos = playerLocation.transform.position.z + startTilingTreshhold;
            zMinusSnapShotPos = playerLocation.transform.position.z - startTilingTreshhold;
            StartCoroutine(sortListCo());
            playerHasMoved = false;
            shortCircutToOrginCounter++;
        }
    }
    void moveHexes()
    {
        ushort vectorIndex = 0;
        hasAllTheHexPosHelperCopy.Clear();
        hasAllTheHexPosHelperCopy.AddRange(hasAllTheHexPos);

       ushort hor= 0, hor2 = 0, vert = 0, vert2 = 0;
       bool markDirtyVector = false;
      
           foreach (HexPos hexPos in hasAllTheHexPosHelperCopy)
           {
               if (rightMove && vectorIndex <= 4096 && playerLocation.transform.position.x - tilingTreshold > hexPos.xPos)
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
               
               if (leftMove && vectorIndex >= 12288 && playerLocation.transform.position.x + tilingTreshold < hexPos.xPos)
               {
                   hor = xTilingDistance;
                   rightMove = false;
                   markDirtyVector = true;
               }

              

               if (markDirtyVector)
               {
                   //update dic
                   ushort dicKey = hexPos.dicKey;
                   hasAllTheHexesDic[dicKey].hextile.transform.position = new Vector3(hexPos.xPos - hor + hor2,
                       hasAllTheHexesDic[dicKey].hextile.transform.position.y, hexPos.zPos - vert + vert2);
                   //update V3 List
                   hasAllTheHexPos[vectorIndex] = new HexPos(hexPos.xPos - hor + hor2,
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

    IEnumerator sortListCo()
    {
        yield return new WaitForSeconds(0.15f);
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
   
    IEnumerator updateAllAfterOrigin()
    { 
        yield return new WaitForSeconds(0.25f);
        setEverythingTrue();
        moveHexes();
        hasAllTheHexPos.TimSort((pos1, pos2) => pos1.xPos.CompareTo(pos2.xPos));
        playerHasMoved = true;
    }
    void setEverythingTrue()
    {
        topMove = true; bottomMove = true;
        leftMove = true; rightMove = true;
    }

     
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

public struct HextileObjects
{
    public ushort dicKey;
    public GameObject hextile;
    public HextileObjects(ushort dicKey, GameObject hextile)
    {
        this.dicKey = dicKey;
        this.hextile = hextile;
    }
}
#endregion



#region Imports
using System.Collections;
using UnityEngine;
using Cinemachine;
using Unity.Burst;
using UnityEngine.Jobs;
#endregion
public class HexAutoTiling : MonoBehaviour
{
    #region Arrays
    public static TransformAccessArray hasAllTheHexesTransformsNative;
    #endregion
    
    #region PrivateVariables

    private const ushort HEXCOUNT = 16384; //später im Inspector bei mehr Level: default 16384

    private float xPlusSnapShotPos, xMinusSnapShotPos, 
                  zPlusSnapShotPos, zMinusSnapShotPos,
                  xOriginPosition, zOriginPosition,
                  zPlus, zMinus, xPlus, xMinus,
                  xMoveBack, zMoveback;
    
    private bool leftMove = true, rightMove = true, topMove = true, bottomMove = true, playerHasMoved = true, noSide;
    
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
    private bool tilingDistanceCheck => ReferenceLibary.Player.transform.position.x > xPlusSnapShotPos
                                        || ReferenceLibary.Player.transform.position.x < xMinusSnapShotPos
                                        || ReferenceLibary.Player.transform.position.z > zPlusSnapShotPos
                                        || ReferenceLibary.Player.transform.position.z < zMinusSnapShotPos;
    private bool returnToOriginDistanceCheck => shortCircutToOrginCounter > shortCircutTreshhold &&
                                        ReferenceLibary.Player.transform.position.x > moveBackToOriginTreshhold
                                        || ReferenceLibary.Player.transform.position.x < -moveBackToOriginTreshhold
                                        || ReferenceLibary.Player.transform.position.z > moveBackToOriginTreshhold
                                        || ReferenceLibary.Player.transform.position.z < -moveBackToOriginTreshhold;
    #endregion
    #region UnityUpdates                                  
    void Awake() => fillHexDic();
    void Start()
    {
        xOriginPosition = ReferenceLibary.Player.transform.position.x;
        zOriginPosition = ReferenceLibary.Player.transform.position.z;
    }
    void Update()
    {
        setFlags();
        if (bottomMove || topMove || leftMove || rightMove) moveHexes();
        limitTiling();
    }
    private void LateUpdate()
    {
        if (returnToOriginDistanceCheck)
        { 
            moveEverythingBackToOrigin();
            StartCoroutine(updateAllAfterOrigin(0.2f));
        }
    }
    #endregion
    #region StartMethods
    void fillHexDic() 
    {
        hasAllTheHexesTransformsNative  = new TransformAccessArray(HEXCOUNT);
        foreach (GameObject hex in GameObject.FindGameObjectsWithTag("Hex")) hasAllTheHexesTransformsNative.Add(hex.transform);
    }
    #endregion
    
    #region TilingRules
    void setFlags()
    {   
        if(tilingDistanceCheck)  //only one if is less heavy + make sure that everything gets checked once
        {
          
            if (ReferenceLibary.Player.transform.position.x > xPlusSnapShotPos)
            {
                rightMove = true; compareTopBottom();
            }
            
            if (ReferenceLibary.Player.transform.position.x < xMinusSnapShotPos)
            {
                leftMove = true; compareTopBottom();
            }

            if (ReferenceLibary.Player.transform.position.z > zPlusSnapShotPos)
            { 
                topMove = true; compareRightLeft(); 
            }
            
            if (ReferenceLibary.Player.transform.position.z < zMinusSnapShotPos)
            {
                bottomMove = true; compareRightLeft();
            }
        }
    }
    void compareTopBottom()
    {
         zPlus = Mathf.Abs(zPlusSnapShotPos - ReferenceLibary.Player.transform.position.z);
         zMinus = Mathf.Abs(zMinusSnapShotPos - ReferenceLibary.Player.transform.position.z);
         noSide = zPlus > declineBothSidesTreshhold;
        
        if ( noSide && zPlus < zMinus ) topMove = true;
        if (noSide && zPlus > zMinus) bottomMove = true;
    }
    void compareRightLeft()
    {
        xPlus = Mathf.Abs(xPlusSnapShotPos - ReferenceLibary.Player.transform.position.x);
        xMinus = Mathf.Abs(xMinusSnapShotPos - ReferenceLibary.Player.transform.position.x);
       noSide = xPlus > declineBothSidesTreshhold;
        
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
            xPlusSnapShotPos = ReferenceLibary.Player.transform.position.x + startTilingTreshhold;
            xMinusSnapShotPos = ReferenceLibary.Player.transform.position.x - startTilingTreshhold;
            zPlusSnapShotPos = ReferenceLibary.Player.transform.position.z + startTilingTreshhold;
            zMinusSnapShotPos = ReferenceLibary.Player.transform.position.z - startTilingTreshhold;
            playerHasMoved = false;
            shortCircutToOrginCounter++;
        }
    }
    void moveHexes()
    {
        HexPosJob hexTransformJob = new HexPosJob
        {
            xTilingDistanceJob =  xTilingDistance, 
            zTilingDistanceJob = zTilingDistance, tilingTreshholdJob = tilingTreshold,
            bottomMoveJob = bottomMove, rightMoveJob = rightMove, leftMoveJob = leftMove, topMoveJob = topMove,
            playerLocationXJob = ReferenceLibary.Player.transform.position.x,
            playerLocationZJob = ReferenceLibary.Player.transform.position.z,
            
        };
        hexTransformJob.Schedule(hasAllTheHexesTransformsNative);
         playerHasMoved = true;
           setAllFalse();
    }
    #endregion
    
    #region  OriginMethods
    void moveEverythingBackToOrigin()
    { 
        //calculte Distances
        xMoveBack = ReferenceLibary.Player.transform.position.x - (xOriginPosition);
         zMoveback = ReferenceLibary.Player.transform.position.z - (zOriginPosition);
      
        Vector3 moveEveryThingBack = new Vector3(
            xMoveBack,
            0,
            zMoveback);
        //Informs vcams
        int numVcams = CinemachineCore.Instance.VirtualCameraCount;
        for (byte i = 0; i < numVcams; ++i)
            CinemachineCore.Instance.GetVirtualCamera(i).OnTargetObjectWarped(
                ReferenceLibary.Player.transform, -moveEveryThingBack);
        //moves everything back
        for (ushort j = 0; j < UnityEngine.SceneManagement.SceneManager.sceneCount; j++)
        { 
            foreach (GameObject allParentObjects in UnityEngine.SceneManagement.SceneManager.GetSceneAt(j).GetRootGameObjects()) 
                allParentObjects.transform.position -= moveEveryThingBack;
        }
        //sets flags
        shortCircutToOrginCounter = 0;
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
[BurstCompile]
public struct HexPosJob : IJobParallelForTransform
{ 
    [Unity.Collections.ReadOnly] public ushort xTilingDistanceJob, zTilingDistanceJob ;
  [Unity.Collections.ReadOnly] public float tilingTreshholdJob;
  [Unity.Collections.ReadOnly]  public bool bottomMoveJob, rightMoveJob, leftMoveJob, topMoveJob;
  [Unity.Collections.ReadOnly]  public float playerLocationXJob, playerLocationZJob;
    public void Execute(int index, TransformAccess hasAllTheHexPosTransform)
    {
        ushort vectorindex = 0, hor = 0, hor2 = 0, vert = 0, vert2 = 0;
        bool markDirtyVector = false;
        if (rightMoveJob  && playerLocationXJob - tilingTreshholdJob > hasAllTheHexPosTransform.position.x)
            {
                hor2 = xTilingDistanceJob;
                markDirtyVector = true;
            }
            if (bottomMoveJob && playerLocationZJob + tilingTreshholdJob < hasAllTheHexPosTransform.position.z)
            {
                vert = zTilingDistanceJob;
                markDirtyVector = true;
            }
            if (topMoveJob && playerLocationZJob - tilingTreshholdJob > hasAllTheHexPosTransform.position.z)
            {
                vert2 = zTilingDistanceJob;
                markDirtyVector = true;
            }
            if (leftMoveJob  && playerLocationXJob + tilingTreshholdJob < hasAllTheHexPosTransform.position.x)
            {
                hor = xTilingDistanceJob;
                markDirtyVector = true;
            }
            if (markDirtyVector)
                hasAllTheHexPosTransform.position = new Vector3(
                    hasAllTheHexPosTransform.position.x - hor + hor2,
                    hasAllTheHexPosTransform.position.y,
                    hasAllTheHexPosTransform.position.z - vert + vert2);
    }
}
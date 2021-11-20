using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{   
    
  //  public GameObject plane;
  

    // private int radius = 5;
    // private int planeOffset = 10;

    private Vector3 startPos = Vector3.zero;

    // private int XPlayerMove => (int)(player.transform.position.x - startPos.x);
    // private int ZPlayerMove => (int)(player.transform.position.z - startPos.z);
    //
    // int XPlayerLocation => (int)Mathf.Floor(player.transform.position.x / planeOffset) * planeOffset;
    //  int ZPlayerLocation => (int)Mathf.Floor(player.transform.position.z / planeOffset) * planeOffset;
    
    // Hashtable tilePlane = new Hashtable();
    
    #region Dictonarys
    
    protected Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
    protected Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>();
    #endregion
   
    private void Start()
    {
        
       
        foreach (Hex hex in FindObjectsOfType<Hex>())
        {
            
            hexTileDict[hex.HexCoords] = hex;
        }

    }
#region GetNeighbours


public void Update()
{
 

    
    
    /*if (Input.GetKeyDown(KeyCode.N))
    {
        foreach (Hex hex in FindObjectsOfType<Hex>())
        {
            if(startPos.x-90 > hex.transform.position.x)
                hex.transform.position = new Vector3(hex.transform.position.x+298, hex.transform.position.y, hex.transform.position.z);
        }
    }
    
    if(Input.GetKeyDown(KeyCode.M))
    {
        foreach (Hex hex in FindObjectsOfType<Hex>())
        {
            if(startPos.x+205 < hex.transform.position.x)
                hex.transform.position = new Vector3(hex.transform.position.x-298, hex.transform.position.y, hex.transform.position.z);
        }
    }*/
}
// private bool hasPlayerMoved(int playerX, int playerZ) 
// {
//     if (Mathf.Abs(XPlayerMove) >= planeOffset || Mathf.Abs(ZPlayerMove) >= planeOffset){
//         return true;
//     }
//     return false;
// }



public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
    {
        if (hexTileDict.ContainsKey(hexCoordinates) == false)
            return new List<Vector3Int>();

        if (hexTileNeighboursDict.ContainsKey(hexCoordinates))
            return hexTileNeighboursDict[hexCoordinates];

        hexTileNeighboursDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach (Vector3Int direction in Direction.GetDirectionList(hexCoordinates.z))
        {
            if (hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                hexTileNeighboursDict[hexCoordinates].Add(hexCoordinates + direction);
            }
        }
        return hexTileNeighboursDict[hexCoordinates];
    }

    public Vector3Int GetClosestHex(Vector3 worldposition)
    {
        worldposition.y = 0;
        return HexCoordinates.ConvertPositionToOffset(worldposition);
    }
}

#endregion

#region EvenandUnvenNeighbours
public static class Direction
{
    public static List<Vector3Int> directionsOffsetOdd = new List<Vector3Int>
    {
        new Vector3Int(-1,0,1), //North 1
        new Vector3Int(0,0,1), //North 2
        new Vector3Int(1,0,0), //East
        new Vector3Int(0,0,-1), //South2 
        new Vector3Int(-1,0,-1), //South 1
        new Vector3Int(-1,0,0), //West
    };

    public static List<Vector3Int> directionsOffsetEven = new List<Vector3Int>
    {
        new Vector3Int(0,0,1), //North 1
        new Vector3Int(1,0,1), //North 2
        new Vector3Int(1,0,0), //East
        new Vector3Int(1,0,-1), //South2
        new Vector3Int(0,0,-1), //South 1
        new Vector3Int(-1,0,0), //West
    };

    public static List<Vector3Int> GetDirectionList(int z)
        => z % 2 == 0 ? directionsOffsetEven : directionsOffsetOdd;
   
}

#endregion

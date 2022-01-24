using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{   
    private HexCoordinates hexCoordinates;
    #region Dictonarys
    
    public  Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
   protected Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>();
    
    #endregion
    
    private void Start()
    {

       //  foreach (Hex hex in FindObjectsOfType<Hex>())
      //  {
        //     hexCoordinates = this.GetComponent<HexCoordinates>();
        //     hexTileDict[hex.HexCoords] = hex;
      //  }
        //
        // foreach (KeyValuePair<K, V> entry in dict) {
        //     Console.WriteLine(entry.Key + " : " + entry.Value);
        // }

    }
  
   // public Dictionary<Vector3Int, Hex> HexTileDict
  //  {
     //   get => hexTileDict;
     //   set => hexTileDict = value;
   // }
    #region GetNeighbours


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

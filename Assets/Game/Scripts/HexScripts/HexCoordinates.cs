using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    #region OffsetValues
    
    public static float xOffset = 2, yOffset = 4, zOffset = 2;
    internal Vector3Int GetHexCoords()
        => offsetCoordinates;
    
    [Header("Offset coordinates")] [SerializeField] private Vector3Int offsetCoordinates;
    
    #endregion

    

    private void Awake()
    {
        offsetCoordinates = ConvertPositionToOffset(transform.position);
    }

    #region OffsetHexesRound
    public static Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / xOffset);
        int y = Mathf.RoundToInt(position.y / yOffset);
        int z = Mathf.RoundToInt(position.z / zOffset);
        return new Vector3Int(x, y, z);
    }
    
    #endregion
}

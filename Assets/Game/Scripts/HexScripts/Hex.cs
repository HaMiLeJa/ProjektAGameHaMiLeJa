using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hex : MonoBehaviour
{
   #region Inspector
    [SerializeField] private GlowHighlight highlight;
    private HexCoordinates hexCoordinates;
    [SerializeField] private HexType hexType;
    #endregion
    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    
    // wie weit kann die Unit laufen
    public int GetCost()
        => hexType switch
        {
            HexType.Water => 20,
            HexType.Default => 10,
            HexType.Slow => 15,
            _ => throw new Exception($"Hex of type {hexType} not supported")
        };
    
    public bool IsObstacle()
    {
        return this.hexType == HexType.Obstacle;
    }

    private void Awake()
    {
        hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighlight>();
    }

    #region  HighlightHexs
    public void EnableHighlight()
    {
        highlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        highlight.ToggleGlow(false);
    }

    internal void ResetHighlight()
    {
        highlight.ResetGlowHighlight();
    }

    internal void HighlightPath()
    {
        highlight.HighlightValidPath();
    }
}
#endregion

public enum HexType
{
    None,
    Default,
    Slow,
    Water,
    Building,
    Obstacle
}
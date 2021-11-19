using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hex : MonoBehaviour
{
   #region Inspector
   GameObject Player;
   //Create a new cube primitive to set the color on
   [SerializeField] private GlowHighlight highlight;
    private HexCoordinates hexCoordinates;
    [SerializeField] protected HexType hexType;
    #endregion
    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    
    // wie weit kann die Unit laufen
    public int GetCost()
        => hexType switch
        {
            HexType.Water => 20,
            HexType.Default => 10,
            HexType.SlowDown => 15,
            _ => throw new Exception($"Hex of type {hexType} not supported")
        };
    
    public bool IsObstacle()
    {
        return this.hexType == HexType.Obstacle;
    }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
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
    
    
    
    #endregion
    
    #region OnTriggerHexTypes
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Player)
        {  
            if ((hexType == HexType.SlowDown))
            {
                ;
            }
            
            if ((hexType == HexType.Trampolin))
            {
                
            }
            
            if ((hexType == HexType.ChangeDirection))
            {
                
            }
            
            if ((hexType == HexType.ChangeDirection))
            {
                
            }
        }
        
        
    }
    
    
}
#endregion


public enum HexType
{
    None,
    Default,
    SlowDown,
    Trampolin,
    ChangeDirection,
    BoostForward,
    Water,
    Building,
    Obstacle
}



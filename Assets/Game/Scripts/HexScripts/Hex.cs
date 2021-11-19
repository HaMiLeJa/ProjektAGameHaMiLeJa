using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hex : MonoBehaviour
{
   #region Inspector
   [SerializeField]  GameObject Player;
   
    private GlowHighlight highlight;
    private HexCoordinates hexCoordinates;
    [SerializeField] protected HexType hexType;

    GameObject gameMng;
    SlowDown slowDown;
    BoostForward boostForward;
    ChangeDirection changeDirection;
    Trampolin trampolin;
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
        
        hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighlight>();

    }

    private void Start()
    {
        gameMng = GameObject.Find("GameManager");

        slowDown = gameMng.GetComponentInChildren<SlowDown>();
        boostForward = gameMng.GetComponentInChildren<BoostForward>();
        changeDirection = gameMng.GetComponentInChildren<ChangeDirection>();
        trampolin = gameMng.GetComponentInChildren<Trampolin>();
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
                slowDown.SlowDownStarter();
            }
            
            if ((hexType == HexType.Trampolin))
            {
                trampolin.TrampolinStarter();
            }
            
            if ((hexType == HexType.ChangeDirection))
            {
                changeDirection.ChangeDirectionStarter();
            }
            
            if ((hexType == HexType.BoostForward))
            {
                boostForward.BoostForwardStarter();
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



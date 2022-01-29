using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using NaughtyAttributes;

public class Waypoint : MonoBehaviour
{
    [InfoBox("Rebuild nur falls was schief läuft. Ist auch im Parent und wird bei instanziieren gemacht", EInfoBoxType.Normal)]
    public Waypoint NextPoint;
   public Waypoint  PreviousPoint;
  
   [Button()]
   void RebuildList()
   {
       this.transform.parent.GetComponent<Pathfinder>().AllChildsToList();
   }
}

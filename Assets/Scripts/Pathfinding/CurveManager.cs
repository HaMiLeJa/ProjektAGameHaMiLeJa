using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class CurveManager : MonoBehaviour
{
     #if UNITY_EDITOR
     public static bool drawPathfindingInEditMode = true;
     public static bool updateCurves = true;
     public static bool updatePathfinder;
     [Button("Toogle Pathfinding in Editor")] private void TogglePathfinding()
     {
          drawPathfindingInEditMode = !drawPathfindingInEditMode;
          if (drawPathfindingInEditMode) Debug.Log("Pathfinding now show up in the Editor");
          else Debug.Log("Pathfinding no longer show up in the Editor");
     }
   [Button()] public void UpdateCollider()
     {
          MeshColliderThatNeedRefresh?.Clear();
          foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BrueckeParent"))
          {
               foreach (Segment seg in obj.GetComponentsInChildren<Segment>())
               {
                    if (seg.gameObject.GetComponent<MeshCollider>() != null)
                         MeshColliderThatNeedRefresh.Add(seg.gameObject.GetComponent<MeshCollider>());
               }
          }
     }
#endif
     
     [SerializeField] private List<MeshCollider> MeshColliderThatNeedRefresh;
     private void Start()
     {
          foreach (MeshCollider meshCollider in MeshColliderThatNeedRefresh)
          {
               meshCollider.enabled = false;
               meshCollider.enabled = true;
          }
     }
}


using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class CurveManager : MonoBehaviour
{
     #if UNITY_EDITOR
     public static bool drawCurvesInEditMode = true;
     public static bool updateCurves = true;
     [Button("Toogle Curve in Editor")] private void ToggleCurves()
     {
          drawCurvesInEditMode = !drawCurvesInEditMode;
          if (drawCurvesInEditMode) Debug.Log("Curves now show up in the Editor");
          else Debug.Log("Curves no longer show up in the Editor");
     }
    public void UpdateCollider()
     {
          SegmentsThatNeedRefresh?.Clear();
          SegmentsThatNeedRefresh.AddRange(GameObject.FindGameObjectsWithTag("BrueckeParent"));
     }
     #endif
     
     [SerializeField] private List<GameObject> SegmentsThatNeedRefresh;
     private void Awake()
     {
          foreach (GameObject Seg in SegmentsThatNeedRefresh)
          {
               Seg.SetActive(false);
               Seg.SetActive(true);
          }
     }
     }


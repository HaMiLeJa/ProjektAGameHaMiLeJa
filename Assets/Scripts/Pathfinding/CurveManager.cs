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
#endif
 
}


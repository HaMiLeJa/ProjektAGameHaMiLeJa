using UnityEditor;
using UnityEngine;

public class RenderBoundsGizmos
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void DrawSceneGizmos(Waypoint waypoint, GizmoType gizmoType)
    {
        Gizmos.color = Color.magenta;
        
        foreach (var mf in RenderBounds.RenderBoundsList)
        {
            Gizmos.matrix = mf.transform.localToWorldMatrix;
            Mesh m = mf.sharedMesh;
            Gizmos.DrawWireCube(m.bounds.center, m.bounds.size);
        }
    }
    
    }


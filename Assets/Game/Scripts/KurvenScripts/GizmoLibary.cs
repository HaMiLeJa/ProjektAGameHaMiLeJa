using UnityEngine;
using UnityEditor;

//verschiedene Gizmos die wir immer wieder nutzen können
public static class GizmoLibary 
{
	// zeichnen einen  wireframe circle
	public static void DrawWireCircle( Vector3 pos, Quaternion rot, float radius, int detail = 32 ) 
	{
		Vector3[] points3D = new Vector3[detail];
		for( int i = 0; i < detail; i++ ) 
		{
			float t = i / (float)detail;
			float angRad = t * MathLibary.TAU;
			Vector2 point2D = MathLibary.GetUnitVectorByAngle( angRad ) * radius;
			points3D[i] = pos + rot * point2D;
		}

		// zeichne alle punkte all kleine dots
		for( int i = 0; i < detail - 1; i++ ) 
		{
			Gizmos.DrawLine( points3D[i], points3D[i + 1] );
		}
		Gizmos.DrawLine( points3D[detail - 1], points3D[0] );
	}

	// Zeichnet einen gizmoartigen Satz von drei farbigen Koordinatenachsenlinien 
	public static void DrawOrientedPoint( OrientedPoint op ) 
	{
		void DrawAxis( Color color, Vector3 axis ) 
		{
			Gizmos.color = color;
			Gizmos.DrawLine( op.pos, op.LocalToWorldPos( axis ) );
		}
		DrawAxis( Handles.xAxisColor, Vector3.right );
		DrawAxis( Handles.yAxisColor, Vector3.up );
		DrawAxis( Handles.zAxisColor, Vector3.forward );
		Gizmos.color = Color.white;
	}
}

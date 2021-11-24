using System.Collections.Generic;
using UnityEngine;

//In dieser Klasse werden die meshes (2d) extrudiert (3d).
public class MeshExtruder
{

	// Used when generating the mesh
	List<Vector3> verts = new List<Vector3>();
	List<Vector3> normals = new List<Vector3>();
	List<Vector2> uvs0 = new List<Vector2>();
	List<Vector2> uvs1 = new List<Vector2>();
	List<int> triIndices = new List<int>();

	
	public void Extrude( Mesh mesh, Mesh2D mesh2D, OrientedCubicBezier3D bezier, Ease rotationEasing, UVMode uvMode, Vector2 nrmCoordStartEnd, float edgeLoopsPerMeter, float tilingAspectRatio ) 
	{
		// Clear was vorher war um sauber zu starten
		mesh.Clear();
		verts.Clear();
		normals.Clear();
		uvs0.Clear();
		uvs1.Clear();
		triIndices.Clear();

		// UVs/Texture 
		LengthTable table = null;
		if(uvMode == UVMode.TiledWithFix)
			table = new LengthTable( bezier, 12 );
		float curveArcLength = bezier.GetArcLength();
		
		// Tiling von den uvs
		float tiling = tilingAspectRatio;
		if( uvMode == UVMode.Tiled || uvMode == UVMode.TiledWithFix ) 
		{
			float uSpan = mesh2D.CalcUspan(); // World space units werden zu UV Coordinaten umgerechnet
			tiling *= curveArcLength / uSpan;
			tiling = Mathf.Max( 1, Mathf.Round( tiling ) ); // Snap zum nächsten int um richtig zu tilen
		}

		// Generieren von vertices
		// Foreach edge loop
		// Berechnen der edge loops
		int targetCount = Mathf.RoundToInt( curveArcLength * edgeLoopsPerMeter );
		int edgeLoopCount = Mathf.Max( 2, targetCount ); // Müssen mindestens 2
		for( int ring = 0; ring < edgeLoopCount; ring++ ) 
		{
			float t = ring / (edgeLoopCount-1f);
			OrientedPoint op = bezier.GetOrientedPoint( t, rotationEasing );

			// Bereite UV coordinaten vor
			float tUv = t;
			if( uvMode == UVMode.TiledWithFix )
				tUv = table.ToPercentage( tUv );
			float uv0V = tUv * tiling;
			float uv1U = Mathf.Lerp( nrmCoordStartEnd.x, nrmCoordStartEnd.y, tUv ); // Normalizierte Coordinaten

			// Foreach vertex in der  2D shape
			for( int i = 0; i < mesh2D.VertexCount; i++ ) 
			{
				verts.Add( op.LocalToWorldPos( mesh2D.vertices[i].point ) );
				normals.Add( op.LocalToWorldVec( mesh2D.vertices[i].normal ) );
				uvs0.Add( new Vector2( mesh2D.vertices[i].u, uv0V ) );
				uvs1.Add( new Vector2( uv1U, 0 ) );
			}

		}

		// Hier werden die Triangles generiert. Ich habe den approach genommen, dass es über kreuz geht
		// Foreach edge loop (bis auf den letzen, da es ein step weiter schaut)
		for( int edgeLoop = 0; edgeLoop < edgeLoopCount - 1; edgeLoop++ )
		{
			int rootIndex = edgeLoop * mesh2D.VertexCount;
			int rootIndexNext = (edgeLoop+1) * mesh2D.VertexCount;

			// Foreach  linepaare verts im  2D shape
			for( int line = 0; line < mesh2D.LineCount; line += 2 )
			{
				int lineIndexA = mesh2D.lineIndices[line];
				int lineIndexB = mesh2D.lineIndices[line+1];
				int currentA = rootIndex + lineIndexA;
				int currentB = rootIndex + lineIndexB;
				int nextA = rootIndexNext + lineIndexA;
				int nextB = rootIndexNext + lineIndexB;
				triIndices.Add( currentA );
				triIndices.Add( nextA );
				triIndices.Add( nextB );
				triIndices.Add( currentA );
				triIndices.Add( nextB );
				triIndices.Add( currentB );
			}
		}

		// Simples assignen von allem
		mesh.SetVertices( verts );
		mesh.SetNormals( normals );
		mesh.SetUVs( 0, uvs0 );
		mesh.SetUVs( 1, uvs1 );
		mesh.SetTriangles( triIndices, 0 );

	}
}

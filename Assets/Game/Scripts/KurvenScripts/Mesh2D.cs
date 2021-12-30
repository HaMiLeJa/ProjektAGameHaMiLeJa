﻿using UnityEngine;

[CreateAssetMenu]
public class Mesh2D : ScriptableObject 
{
	// Ein 2d Vertex
	[System.Serializable]
	public class Vertex 
	{
		public Vector2 point;
		public Vector2 normal;
		public float u; // UVs aber nur u
		// vertex colors
		// tangents
	}

	// Genauso wie 3D-Meshes die Konnektivität mit Dreiecken durch Dreiergruppen von Indizes definieren,
	// wird unser 2D-Meshes die Konnektivität mit Linien durch Paare von Indizes definieren
	
	public int[] lineIndices;
	public Vertex[] vertices;

	public int VertexCount => vertices.Length;
	public int LineCount => lineIndices.Length; // Äquivalent der Dreiecksanzahl eines 2D-Meshes

	// Gesamtlänge, die von den U-Koordinaten im worldspace abgedeckt wird
	// Wird verwendet, um sicherzustellen, dass die Textur das richtige Seitenverhältnis hat
	public float CalcUspan() 
	{
		float dist = 0;
		for( int i = 0; i < LineCount; i+=2 ) 
		{
			Vector2 a = vertices[lineIndices[i]].point;
			Vector2 b = vertices[lineIndices[i+1]].point;
			dist += ( a - b ).magnitude;
		}
		return dist;
	}

}

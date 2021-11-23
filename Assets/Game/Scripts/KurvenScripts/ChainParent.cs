using UnityEngine;
using System.Linq;

// Das ist der ParentKontroller von allen Segmenten
[ExecuteInEditMode]
public class ChainParent : MonoBehaviour 
{

	[Tooltip("Die Form vom 2d Mesh das extrudiert wird als Scriptable Object")] public Mesh2D mesh2D = null; // Das 2d Shape, dass extrudiert wird
	[Tooltip("Falls das ganze Konstrukt geschlossen werden soll")] public bool closed = false; // Falls die Strecke geschlossen werden soll
	[Tooltip("Trianguliert das Mesh mehr. Generiert Edge Loops")] public float MeshDensity = 2; // Density vom Mesh
	[Tooltip("Die Art wie die UV's projeziert werden")]public UVMode uvMode = UVMode.TiledWithFix; // Das ist für das UV

	// Regenerate meshes beim instanziieren
	// Wenn du die meshes in der Scene speicherst, muss du das nicht machen, aber es geht relativ schnell 
	void Awake() => UpdateMeshes();

#if UNITY_EDITOR
	
	// To do an Zukunfsperson:  mach das nur, wenn sich auch etwas ändert. 
	void Update() => UpdateMeshes();
#endif

	// Iterriere durch alles Childs und update die Meshes
	public void UpdateMeshes() 
	{

		Segment[] allSegments = GetComponentsInChildren<Segment>();
		Segment[] segmentsWithMesh = allSegments.Where( s => s.HasValidNextPoint ).ToArray();
		Segment[] segmentsWithoutMesh = allSegments.Where( s => s.HasValidNextPoint == false ).ToArray();

		// Wir rechnen die komplette Länge der Strecke um eine normalisierte Koordinate zu kriegen für wie weit du am Track bist
		// 0 = Start im Track | 0.5 = Mitten im Track | 1.0  ist das Ende vom Track
		float[] lengths = segmentsWithMesh.Select( x => x.GetBezierRepresentation( Space.Self ).GetArcLength() ).ToArray();
		float totalRoadLength = lengths.Sum();

		float startDist = 0f;
		for( int i = 0; i < segmentsWithMesh.Length; i++ ) 
		{
			float endDist = startDist + lengths[i];
			Vector2 uvzStartEnd = new Vector2(
				startDist / totalRoadLength,		// Prozent am track Start
				endDist / totalRoadLength			// Prozent am  track Ende
			);
			segmentsWithMesh[i].UpdateMesh( uvzStartEnd );
			startDist = endDist;
		}

		// Clear alle Segmente ohne meshes
		foreach( Segment seg in segmentsWithoutMesh ) 
		{
			seg.UpdateMesh( Vector2.zero );
		}
	}
}

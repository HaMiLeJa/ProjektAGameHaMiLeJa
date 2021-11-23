﻿using UnityEngine;
// In dieser Klasse stellen wir sicher, dass wir der owner vom Mesh sind. 
// Damit wir verhindert das wir:
// 1. Ein existierendes Objekt duplizieren,
// 2. das neue Mesh zum alten asset referenziert
// 3. Wenn beide Meshes editieren werden, dass nicht beide das source Mesh nutzen.
// Wir nutzen eine Track Id, die hier erstellt wird für jedes object. Damit wird ein neues Mesh erstellt

public class UniqueMesh : MonoBehaviour 
{

	[HideInInspector][SerializeField] int ownerID;

	protected Mesh meshCached; // Das mesh assets das erstellt wird
	protected Mesh Mesh 
	{
		get 
		{
			bool isOwner = ownerID == gameObject.GetInstanceID();
			bool filterHasMesh = MeshFilter.sharedMesh != null ;
			bool filterHasCollider = MeshCollider.sharedMesh != null ;
			if( !filterHasMesh || !isOwner || !filterHasCollider ) 
			{
				MeshFilter.sharedMesh = meshCached = new Mesh(); // //mach neues mesh und assign filter
				MeshCollider.sharedMesh = meshCached; // noch die Collider
				ownerID = gameObject.GetInstanceID(); // Markiert sich selber als owner vom Mesh. Damit erhalten wir die gameobject id
				meshCached.name = "Segement Nummer: [" + ownerID + "]";
				meshCached.hideFlags = HideFlags.HideAndDontSave; // Stelle sicher, dass es nicht in der Scene ist. Das prevented memory leaks
				meshCached.MarkDynamic(); // Nur nützlich für realtime Bending. Mach das nicht an, wenn du eins generieren willst
			} 
			else if( isOwner && filterHasMesh && filterHasCollider && meshCached == null ) 
			{
				// Wenn das Mesh  die Referenez verliert, assign.  Das passiert manchmal bei assembly reloads
				meshCached = MeshFilter.sharedMesh;
				meshCached = MeshCollider.sharedMesh;
			}
			return meshCached;
		}
	}

	MeshFilter MeshFilter => GetComponent<MeshFilter>();
	MeshCollider MeshCollider => GetComponent<MeshCollider>();

}
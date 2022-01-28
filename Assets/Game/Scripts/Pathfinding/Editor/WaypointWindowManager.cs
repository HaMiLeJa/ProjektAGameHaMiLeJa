using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class WaypointWindowManager : EditorWindow
{
 [MenuItem("HaMiLeJa/Waypoint Creator")]
 private static void ShowWindow()
 {
  var window = GetWindow<WaypointWindowManager>();
  window.titleContent = new GUIContent("Waypoint Creator");
  window.Show();
 }

 public Transform WaypointParent;
 
 private void OnGUI()
 {
  var serializedEditorWindow = new SerializedObject(this);
  EditorGUILayout.PropertyField(serializedEditorWindow.FindProperty(nameof(WaypointParent)));

  if (!WaypointParent)
  {
   EditorGUILayout.HelpBox("Zieh erstmal ein Waypoint parent rein", MessageType.Info);
  }
  else
  
   {
    DrawButtons();
   }
   serializedEditorWindow.ApplyModifiedProperties();
  

 
 }
 
 
 private void DrawButtons()
 {
  if (GUILayout.Button("Create waypoint"))
  {
   CreateWaypoint();
  }
 }
 
 private void CreateWaypoint()
 {
  var waypoint = CreateNewWayPoint();
  waypoint.transform.parent.GetComponent<Pathfinder>().AllChildsToList();
  if (WaypointParent.childCount > 1)
  {
   waypoint.PreviousWaypoint =
    WaypointParent.GetChild(waypoint.transform.GetSiblingIndex() - 1).GetComponent<Waypoint>();

   waypoint.PreviousWaypoint.NextWaypoint = waypoint;
  }
  Selection.activeObject = waypoint.gameObject;
  
  
 }

 private Waypoint CreateNewWayPoint()
 {
  var waypointGameObject = new GameObject(
   $"Waypoint {WaypointParent.childCount +1}",
   typeof(Waypoint));
  waypointGameObject.transform.SetParent(WaypointParent.transform, false);
 
  return waypointGameObject.GetComponent<Waypoint>();
  
 }
}

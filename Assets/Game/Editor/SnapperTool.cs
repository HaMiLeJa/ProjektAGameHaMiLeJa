using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SnapperTool : EditorWindow
{
   [MenuItem("Tools/Snapper")]
   public static void OpenTheThing() => GetWindow<SnapperTool>("Snapper");

   private void OnEnable()
   {
      Selection.selectionChanged += Repaint;
      SceneView.duringSceneGui += DuringSceneGUI;
   }
   private void OnDisable()
   {
      Selection.selectionChanged -= Repaint;
      SceneView.duringSceneGui -= DuringSceneGUI;
   }

   void DuringSceneGUI(SceneView sceneView)
   {
      Handles.DrawLine( Vector3.zero, Vector3.up);
   }
   void OnGUI()
   {
      using (new EditorGUI.DisabledScope(Selection.gameObjects.Length == 0))
      {
         if (GUILayout.Button("Snap Selection"))
         {
            SnapSelection();
         }
      }
      
      
        void SnapSelection()
        {
           foreach (GameObject go in Selection.gameObjects)
         {
            Undo.RecordObject(go.transform, "snap Objects");
            go.transform.position = go.transform.position.Round();
         }
           
        }
      
   }
}

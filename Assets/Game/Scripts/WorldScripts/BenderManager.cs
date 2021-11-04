using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class BenderManager : MonoBehaviour
{
  
// Konstanten
  private const string BENDER = "ENABLE_BENDING";

  private const string PLANET = "ENABLE_BENDING_PLANET";
//Für Inspector
  private static readonly int BENDINGAMOUNT =
    Shader.PropertyToID("_BendingAmount");
  
  //Inspector
  [SerializeField]
  private bool enablePlanet = default;

  [SerializeField]
  [Range(0.005f, 0.1f)]
  private float bendingAmount = 0.015f;

  private float _prevAmount;
  
 

  private void Awake ()
  {
    if ( Application.isPlaying )
      Shader.EnableKeyword(BENDER);
    else
      Shader.DisableKeyword(BENDER);

    if ( enablePlanet )
      Shader.EnableKeyword(PLANET);
    else
      Shader.DisableKeyword(PLANET);

    UpdateBendingAmount();
  }
//SceneView
  private void OnEnable ()
  {
    if ( !Application.isPlaying )
      return;
    
    RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
  }

  private void Update ()
  {
    if ( Math.Abs(_prevAmount - bendingAmount) > Mathf.Epsilon )
      UpdateBendingAmount();
  }

  //SceneView
  private void OnDisable ()
  {
    RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
  }
  

  //methoden

  private void UpdateBendingAmount ()
  {
    _prevAmount = bendingAmount;
    Shader.SetGlobalFloat(BENDINGAMOUNT, bendingAmount);
  }

  private static void OnBeginCameraRendering (ScriptableRenderContext ctx,
                                              Camera cam)
  {
    cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99) *
                        cam.worldToCameraMatrix;
  }
  private static void OnEndCameraRendering (ScriptableRenderContext ctx,
                                            Camera cam)
  {
    cam.ResetCullingMatrix();
  }
  
}
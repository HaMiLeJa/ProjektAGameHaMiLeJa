using System;
using UnityEngine;
using UnityEngine.Rendering;
[ExecuteAlways]
public class BenderManager : MonoBehaviour
{
  private const string BENDER = "ENABLE_BENDING", PLANET = "ENABLE_BENDING_PLANET";
  private static readonly int BENDINGAMOUNT = Shader.PropertyToID("_BendingAmount");
  private bool enablePlanet = true;
  [SerializeField] [Range(-0.1f, 0.1f)] private float bendingAmount = 0.01f;
  private float _prevAmount;
  private void Awake ()
  {
    if ( Application.isPlaying ) Shader.EnableKeyword(BENDER);
    else Shader.DisableKeyword(BENDER);
    if ( enablePlanet ) Shader.EnableKeyword(PLANET);
    else Shader.DisableKeyword(PLANET);
    UpdateBendingAmount();
  }
  private void OnEnable ()
  {
    if ( !Application.isPlaying ) return;
    RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
  }
  private void Update ()
  {
    if ( Math.Abs(_prevAmount - bendingAmount) > Mathf.Epsilon ) UpdateBendingAmount();
  }
  private void OnDisable ()  //SceneView
  { 
    RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
  }
  private void UpdateBendingAmount ()  //methoden
  {
    _prevAmount = bendingAmount;
    Shader.SetGlobalFloat(BENDINGAMOUNT, bendingAmount);
  }
  private static void OnBeginCameraRendering (ScriptableRenderContext ctx, Camera cam)
  {
    cam.cullingMatrix = Matrix4x4.Ortho(-999, 999, -999, 999, 0.0001f, 999) *
                        cam.worldToCameraMatrix;
  }
  private static void OnEndCameraRendering (ScriptableRenderContext ctx, Camera cam)
  {
    cam.ResetCullingMatrix();
  }
}
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
  private static float cullingmatrix;
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
    if (Application.isPlaying) UpdateCullingMatrix();
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
  private static void OnBeginCameraRendering(ScriptableRenderContext ctx, Camera cam) =>
    cam.cullingMatrix = Matrix4x4.Ortho(-cullingmatrix, cullingmatrix, -cullingmatrix, cullingmatrix, 0.0001f, 150+cullingmatrix) *
                        cam.worldToCameraMatrix;
  private void UpdateCullingMatrix()
  {
    if (CameraZoomOut.vcamera.m_Lens.FieldOfView < 110)
      cullingmatrix = 20 + CameraZoomOut.vcamera.m_Lens.FieldOfView;
    else if (CameraZoomOut.vcamera.m_Lens.FieldOfView >= 110)
      cullingmatrix = DynamiclyScaleCulling(CameraZoomOut.vcamera);
  }
  private static float DynamiclyScaleCulling(Cinemachine.CinemachineVirtualCamera vcam) =>
    MathLibary.RemapClamped(110, 180, 140, 260, vcam.m_Lens.FieldOfView);
  private static void OnEndCameraRendering (ScriptableRenderContext ctx, Camera cam)=> cam.ResetCullingMatrix();
}
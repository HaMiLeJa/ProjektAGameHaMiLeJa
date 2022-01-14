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
  
  private bool enablePlanet = true;

  [SerializeField]
  [Range(-0.1f, 0.1f)]
  private float bendingAmount = 0.015f;

  private float _prevAmount;
  
 

  private void Awake ()
  {
    if ( Application.isPlaying )
    {
      Shader.EnableKeyword(BENDER);
      Shader.EnableKeyword("MY_KEYWORD");
    }
    else
    {
      Shader.DisableKeyword(BENDER);
    Shader.DisableKeyword("MY_KEYWORD");
    }

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
    cam.cullingMatrix = Matrix4x4.Ortho(-999, 999, -999, 999, 0.0001f, 999) *
                        cam.worldToCameraMatrix;
  }
  private static void OnEndCameraRendering (ScriptableRenderContext ctx,
                                            Camera cam)
  {
    cam.ResetCullingMatrix();
  }
  
}
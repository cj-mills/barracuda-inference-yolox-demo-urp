using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class CameraBlitPass : ScriptableRenderPass
{
    RTHandle m_CameraColorTarget;
    InferenceController inferenceController;

    public CameraBlitPass(InferenceController inferenceController)
    {
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        this.inferenceController = inferenceController;
    }

    public void SetTarget(RTHandle colorHandle)
    {
        m_CameraColorTarget = colorHandle;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        ConfigureTarget(m_CameraColorTarget);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cameraData = renderingData.cameraData;
        if (cameraData.camera.cameraType != CameraType.Game)
            return;

        if (inferenceController)
        {
            inferenceController.Infernece(m_CameraColorTarget.rt);
        }
        else
        {
            Debug.Log("inferenceController not assigned");
        }
    }
}
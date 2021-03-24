using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraController : MonoBehaviour
{

    void Start()
    {
        var vuforia = VuforiaARController.Instance;
        vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        vuforia.RegisterOnPauseCallback(OnPaused);
    }

    private void OnVuforiaStarted()
    {
        try
        {
            CameraSetSettings();
        }
        catch (System.Exception error)
        {
            Debug.LogError($"Error: {error}");
        }

    }

    private void OnPaused(bool paused)
    {
        try
        {
            if (!paused) // resumed
            {
                CameraSetSettings();
            }
        }
        catch (System.Exception error)
        {
            Debug.LogError($"Error: {error}");
        }
    }

    private void CameraSetSettings()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        CameraDevice.Instance.SetFrameFormat(PIXEL_FORMAT.GRAYSCALE, true);
    }

}

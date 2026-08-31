using System.Collections;
using UnityEngine;
using Yarn.Unity;

/* This script allows you to zoom out the main camera when a Yarn command is called.
*
* Add <<ZoomOutCamera(zoomAmount, zoomDuration)>>
* to your Yarn script to trigger the zoom out effect
*
* zoomAmount : how much to increase the camera's field of view (default 10)
* zoomDuration : how long the zoom out effect should take in seconds (default 1)
*
*                           \(°0°)/
*/

public class CameraZoomOut : MonoBehaviour
{

    // --- Configurable parameters for the zoom out effect ---

    [SerializeField] private AnimationCurve zoomCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    

    // --- Bridge between the static Yarn command and the live MonoBehaviour in the scene ---
    
    private static CameraZoomOut instance;

    private void Awake()
    {
        instance = this;
    }

    // --- Yarn command to trigger the camera zoom out effect ---

    [YarnCommand("ZoomOutCamera")]
    public static void ZoomOutCamera(float zoomAmount = 10f, float zoomDuration = 1f)
    {
        // Check if the CameraZoomOut instance exists in the scene
        if (instance == null)
        {
            Debug.LogError("ZoomOutCamera failed: no CameraZoomOut instance exists in the scene.");
            return;
        }

        // Coroutine let Unity run the zoom out effect over multiple frames without blocking the main thread
        instance.StartCoroutine(instance.ZoomOutRoutine(zoomAmount, zoomDuration));
    }

    // --- Coroutine that performs the zoom out effect over time ---
    private IEnumerator ZoomOutRoutine(float zoomAmount, float zoomDuration)
    {
        // Get the main camera in the scene
        var cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("ZoomOutCamera failed: no camera tagged MainCamera was found.");
            yield break;
        }

        // Store initial field of view and calculate target field of view
        float startFov = cam.fieldOfView;
        float targetFov = startFov + zoomAmount;
        
        // Track elapsed time for the zoom effect
        float elapsed = 0f;

        // Loop until the zoom duration has elapsed, updating the camera's field of view each frame
        while (elapsed < zoomDuration)
        {
            float t = elapsed / zoomDuration;
            float easedT = zoomCurve.Evaluate(t);
            cam.fieldOfView = Mathf.Lerp(startFov, targetFov, easedT);

            elapsed += Time.deltaTime;
            yield return null;
        }

        //cam.fieldOfView = targetFov;
    }
}
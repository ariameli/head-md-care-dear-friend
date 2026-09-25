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

    void Start()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            cam.usePhysicalProperties = true;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    // --- Yarn command to trigger the camera zoom out effect ---

    [YarnCommand("ZoomOutCamera")]
    public static IEnumerator ZoomOutCamera(
        float targetFov = 77.32661f,
        float lensShiftX = 0f,
        float lensShiftY = 0f,
        float zoomDuration = 1f,
        float targetRotationX = 0f,
        float targetRotationY = 0f,
        float targetRotationZ = 0f)

    {

        yield return instance.ZoomOutRoutine(
            targetFov,
            new Vector2(lensShiftX, lensShiftY),
            zoomDuration,
            targetRotationX,
            targetRotationY,
            targetRotationZ);
    }

    // --- Coroutine that performs the zoom out effect over time ---
    private IEnumerator ZoomOutRoutine(float targetFov, Vector2 targetLensShift, float zoomDuration, float targetRotationX, float targetRotationY, float targetRotationZ)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("ZoomOutCamera failed: no camera tagged MainCamera was found.");
            yield break;
        }

        float startFov = cam.fieldOfView;
        Quaternion startRotation = cam.transform.rotation;
        Vector2 startLensShift = cam.lensShift;
        Quaternion targetRotation = Quaternion.Euler(targetRotationX, targetRotationY, targetRotationZ);

        float elapsed = 0f;
        while (elapsed < zoomDuration)
        {
            float t = elapsed / zoomDuration;
            float easedT = zoomCurve.Evaluate(t);

            cam.fieldOfView = Mathf.Lerp(startFov, targetFov, easedT);
            cam.lensShift = Vector2.Lerp(startLensShift, targetLensShift, easedT);
            cam.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.fieldOfView = targetFov;
        cam.lensShift = targetLensShift;
        cam.transform.rotation = targetRotation;
    }
}
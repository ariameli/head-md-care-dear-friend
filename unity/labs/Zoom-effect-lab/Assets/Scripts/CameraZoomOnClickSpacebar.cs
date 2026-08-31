using UnityEngine;

// system engine
using UnityEngine.InputSystem;

public class CameraZoomOnClickSpacebar : MonoBehaviour
{
    //public Camera cam;
    public GameObject targetObject;

    // if space button is pressed, zoom out the camera
    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //cam.fieldOfView += 10f;
            Camera.main.fieldOfView += 10f;
        }

        // if target object is clicked, zoom out the camera
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == targetObject)
                {
                    //cam.fieldOfView += 10f;
                    Camera.main.fieldOfView += 10f;
                }
            }
        }

    }

}
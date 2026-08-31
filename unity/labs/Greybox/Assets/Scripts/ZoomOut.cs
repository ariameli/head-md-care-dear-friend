using UnityEngine;
// use new input system
using UnityEngine.InputSystem;

// use unity event system
using UnityEngine.EventSystems;

public class ZoomOut : MonoBehaviour, IPointerDownHandler
{
   // create 3 public game objects
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;


  

    // on pointer down 
    public void OnPointerDown(PointerEventData eventData)
    {
        // if game object is button1, zoom out the camera a little bit
        if (eventData.pointerCurrentRaycast.gameObject == button1)
        {            // zoom out by increasing the field of view of the camera
            Camera.main.fieldOfView += 1f;
            // clamp the field of view to a maximum value
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 100f);
            // translate the camera backwards to create a zoom out effect
            Camera.main.transform.Translate(Vector3.back * Time.deltaTime * 1f);
        }
        // if game object is button2, zoom out the camera a little bit
        if (eventData.pointerCurrentRaycast.gameObject == button2)
        {            // zoom out by increasing the field of view of the camera
            Camera.main.fieldOfView += 1f;
            // clamp the field of view to a maximum value
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 100f);
            // translate the camera backwards to create a zoom out effect
            Camera.main.transform.Translate(Vector3.back * Time.deltaTime * 1f);
        }
        // if game object is button3, zoom out the camera a little bit
        if (eventData.pointerCurrentRaycast.gameObject == button3)
        {            // zoom out by increasing the field of view of the camera
            Camera.main.fieldOfView += 1f;
            // clamp the field of view to a maximum value
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 100f);
            // translate the camera backwards to create a zoom out effect
            Camera.main.transform.Translate(Vector3.back * Time.deltaTime * 1f);
        }   




        /*
        // check if the player is pressing the zoom out button
        if (Keyboard.current.spaceKey.isPressed)
        {
            // zoom out by increasing the field of view of the camera
            Camera.main.fieldOfView += 1f;
            // clamp the field of view to a maximum value
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 20f, 100f);
            // translate the camera backwards to create a zoom out effect
            Camera.main.transform.Translate(Vector3.back * Time.deltaTime * 1f);
        }*/
        
    }


}

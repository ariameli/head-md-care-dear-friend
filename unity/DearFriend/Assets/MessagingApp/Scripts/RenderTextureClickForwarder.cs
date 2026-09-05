using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RenderTextureClickForwarder : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera messagingCamera;

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        if (hit.collider.gameObject != gameObject)
            return;

        Vector2 uv = hit.textureCoord;

        Vector2 rtPosition = new Vector2(
            uv.x * messagingCamera.pixelWidth,
            uv.y * messagingCamera.pixelHeight
        );

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = rtPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            ExecuteEvents.Execute(
                result.gameObject,
                pointerData,
                ExecuteEvents.pointerClickHandler
            );

            Debug.Log("UI clicked: " + result.gameObject.name);
            break;
        }
    }
}
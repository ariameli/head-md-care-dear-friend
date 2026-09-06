using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RenderTextureClickForwarder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera messagingCamera;
    [SerializeField] private GraphicRaycaster messagingRaycaster;

    private Collider screenCollider;
    private GameObject currentHover;

    private void Awake()
    {
        screenCollider = GetComponent<Collider>();

        // Important :
        // on empêche l'EventSystem normal de raycaster ce Canvas.
        messagingRaycaster.enabled = false;
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (!screenCollider.Raycast(ray, out RaycastHit hit, 1000f))
        {
            ClearHover();
            return;
        }

        Vector2 uv = hit.textureCoord;

        Vector2 rtPosition = new Vector2(
            uv.x * messagingCamera.targetTexture.width,
            uv.y * messagingCamera.targetTexture.height
        );

        PointerEventData pointerData =
            new PointerEventData(EventSystem.current);

        pointerData.position = rtPosition;
        pointerData.button = PointerEventData.InputButton.Left;

        List<RaycastResult> results = new List<RaycastResult>();

        // On active le GraphicRaycaster juste le temps
        // de faire NOTRE raycast avec les bonnes coordonnées.
        messagingRaycaster.enabled = true;
        messagingRaycaster.Raycast(pointerData, results);
        messagingRaycaster.enabled = false;

        GameObject hoverTarget = null;
        GameObject clickTarget = null;

        foreach (RaycastResult result in results)
        {
            if (hoverTarget == null)
            {
                hoverTarget =
                    ExecuteEvents.GetEventHandler<IPointerEnterHandler>(
                        result.gameObject
                    );
            }

            if (clickTarget == null)
            {
                clickTarget =
                    ExecuteEvents.GetEventHandler<IPointerClickHandler>(
                        result.gameObject
                    );
            }

            if (hoverTarget != null && clickTarget != null)
                break;
        }

        UpdateHover(hoverTarget, pointerData);

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            clickTarget != null)
        {
            ExecuteEvents.Execute(
                clickTarget,
                pointerData,
                ExecuteEvents.pointerClickHandler
            );

            Debug.Log("UI clicked: " + clickTarget.name);
        }
    }

    private void UpdateHover(
        GameObject newHover,
        PointerEventData pointerData)
    {
        if (newHover == currentHover)
            return;

        if (currentHover != null)
        {
            ExecuteEvents.Execute(
                currentHover,
                pointerData,
                ExecuteEvents.pointerExitHandler
            );
        }

        currentHover = newHover;

        if (currentHover != null)
        {
            ExecuteEvents.Execute(
                currentHover,
                pointerData,
                ExecuteEvents.pointerEnterHandler
            );
        }
    }

    private void ClearHover()
    {
        if (currentHover == null)
            return;

        PointerEventData pointerData =
            new PointerEventData(EventSystem.current);

        ExecuteEvents.Execute(
            currentHover,
            pointerData,
            ExecuteEvents.pointerExitHandler
        );

        currentHover = null;
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class DialogueGlobalClick : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public AudioDialoguePresenter audioDialoguePresenter;
    public Camera cam;

    void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (audioDialoguePresenter != null &&
            audioDialoguePresenter.audioSource != null &&
            audioDialoguePresenter.audioSource.isPlaying)
        {
            return;
        }

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (cam == null)
            cam = Camera.main;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.GetComponentInParent<DesktopItem>() != null ||
                hit.collider.GetComponentInParent<CloseWindow>() != null)
            {
                return;
            }
        }

        if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.RequestNextLine();
        }
    }
}
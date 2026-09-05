using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class DialogueGlobalClick : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public AudioDialoguePresenter audioDialoguePresenter;

    void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        // Ne pas avancer pendant que l'audio joue
        if (audioDialoguePresenter != null &&
            audioDialoguePresenter.audioSource != null &&
            audioDialoguePresenter.audioSource.isPlaying)
        {
            return;
        }

        if (dialogueRunner != null)
        {
            dialogueRunner.RequestNextLine();
        }
    }
}
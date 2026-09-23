using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueGlobalClick : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public AudioDialoguePresenter audioDialoguePresenter;
    public Camera cam;
    private Button continueButton;

    void Awake()
    {
        GameObject continueButtonObject = GameObject.Find("Continue Button");
        if (continueButtonObject != null)
        {
            continueButton = continueButtonObject.GetComponent<Button>();

            if (continueButton != null)
                continueButton.gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (continueButton == null || audioDialoguePresenter == null ||
            audioDialoguePresenter.audioSource == null)
        {
            return;
        }

        if (audioDialoguePresenter.audioSource.isPlaying)
            continueButton.interactable = false;
    }

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

        if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.RequestNextLine();
        }
    }
}
using UnityEngine;
using Yarn.Unity;

public class ErrorBoxFunctions : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public GameObject objectToShow;

    [YarnCommand("showWarning")]
    public void ShowWarning()
    {
        if (objectToShow != null)
        {
            foreach (Transform child in objectToShow.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        if (dialogueRunner != null)
        {
            StartCoroutine(StartWarningDialogueNextFrame());
        }
    }

    private System.Collections.IEnumerator StartWarningDialogueNextFrame()
    {
        yield return null;
        dialogueRunner.StartDialogue("WarningWindow");
    }
}

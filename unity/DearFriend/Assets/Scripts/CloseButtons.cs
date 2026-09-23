using UnityEngine;
using Yarn.Unity;

public class CloseButtons : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public string nodeNameAfterClose;

    public void CloseParentImage()
    {
        if (transform.parent != null)
        {
            DesktopItem.SetContentOpen(false);
            transform.parent.gameObject.SetActive(false);

            if (dialogueRunner != null && !string.IsNullOrEmpty(nodeNameAfterClose))
            {
                dialogueRunner.StartDialogue(nodeNameAfterClose);
            }
        }
    }
}
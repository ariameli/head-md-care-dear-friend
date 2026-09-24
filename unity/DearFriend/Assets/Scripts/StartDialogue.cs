using UnityEngine;
using Yarn.Unity;

public class DialogueButton : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string nodeName;

    public void StartDialogue()
    {
        dialogueRunner.StartDialogue(nodeName);
    }
}
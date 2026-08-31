using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;


public class ClickOnPassword : MonoBehaviour, IPointerDownHandler
{
   public DialogueRunner dialogueRunner;
   public string dialogueNodeName;

   public void OnPointerDown(PointerEventData eventData)
   {
       if (dialogueRunner != null && !dialogueRunner.IsDialogueRunning)
       {
           dialogueRunner.StartDialogue(dialogueNodeName);
       }
    }
}

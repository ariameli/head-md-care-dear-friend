using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;


public class ObjectAppears : MonoBehaviour
{
   public DialogueRunner dialogueRunner;
   public string dialogueNodeName;
   public GameObject objectToActivate;
    
   [YarnCommand("activateObject")]
   public static void ActivateObject(string objectName)
   {
      foreach (GameObject sceneObject in Resources.FindObjectsOfTypeAll<GameObject>())
      {
         if (sceneObject.name == objectName && sceneObject.scene.IsValid())
         {
            sceneObject.SetActive(true);
            return;
         }
      }

      Debug.LogWarning($"ObjectAppears: could not find scene object '{objectName}'.");
   }
}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class SearchController : MonoBehaviour
{
    [Header("Yarn")]
    [SerializeField] private DialogueRunner dialogueRunner;

    [SerializeField] private string[] dialogueNodes =
    {
        "Photo01Dialogue",
        "Photo02Dialogue",
        "Photo03Dialogue",
        "Photo04Dialogue"
    };

    [Header("Search")]
    [SerializeField] private TMP_Text searchText;
    [SerializeField] private string wordToType = "MEMENTO VIVERE";
    [SerializeField] private float typingSpeed = 0.06f;

    [Header("Results")]
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private RectTransform[] results;

    [Header("Scroll")]
    [SerializeField] private ScrollRect scrollRect;

    private int currentResult = 0;

    private void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null)
        {
            GameObject cameraObject = GameObject.Find("MessApp Camera");
            Camera messageAppCamera = cameraObject != null ? cameraObject.GetComponent<Camera>() : null;
            canvas.worldCamera = messageAppCamera != null
                ? messageAppCamera
                : Camera.main != null ? Camera.main : FindFirstObjectByType<Camera>();
        }

        if (canvas != null)
        {
            foreach (Transform child in canvas.GetComponentsInChildren<Transform>(true))
            {
                if (child.name != "Header")
                {
                    continue;
                }

                foreach (Graphic graphic in child.GetComponentsInChildren<Graphic>(true))
                {
                    graphic.raycastTarget = false;
                }

                break;
            }
        }

        EventSystem eventSystem = EventSystem.current;

        if (eventSystem == null)
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystem = eventSystemObject.AddComponent<EventSystem>();
        }

        if (eventSystem.GetComponent<BaseInputModule>() == null)
        {
#if ENABLE_INPUT_SYSTEM
            eventSystem.gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
            eventSystem.gameObject.AddComponent<StandaloneInputModule>();
#endif
        }
    }

    private void StartDialogueForCurrentResult()
    {
        if (dialogueRunner == null)
        {
            return;
        }

        if (currentResult < 0 || currentResult >= dialogueNodes.Length)
        {
            return;
        }

        dialogueRunner.StartDialogue(dialogueNodes[currentResult]);
    }

    public void StartSearch()
    {
        currentResult = 0;
        UpdateCounter();
        StartCoroutine(TypeSearch());
    }

    private IEnumerator TypeSearch()
    {
        searchText.text = "";

        foreach (char letter in wordToType)
        {
            searchText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

   private void GoToCurrentResult()
    {
        Canvas.ForceUpdateCanvases();

        RectTransform target = results[currentResult];
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        // Force le Vertical Layout Group à avoir ses positions définitives
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        Canvas.ForceUpdateCanvases();

        // Position réelle de la photo par rapport au Viewport
        Bounds targetBounds =
            RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, target);

        // Centre réel du Viewport
        float viewportCenterY = viewport.rect.center.y;

        // Centre réel de la photo
        float targetCenterY = targetBounds.center.y;

        // Distance nécessaire pour mettre les deux centres au même endroit
        float difference = viewportCenterY - targetCenterY;

        Vector2 newPosition = content.anchoredPosition;
        newPosition.y += difference;

        content.anchoredPosition = newPosition;

        scrollRect.StopMovement();
    }

    public void NextResult()
    {
        if (currentResult < results.Length - 1)
        {
            currentResult++;
        }

        UpdateCounter();
        GoToCurrentResult();
        StartDialogueForCurrentResult();
    }

    public void PreviousResult()
    {
        if (currentResult > 0)
        {
            currentResult--;
        }

        UpdateCounter();
        GoToCurrentResult();
    }
   

    private void UpdateCounter()
    {
        counterText.text = (currentResult + 1) + "/" + results.Length;
    }
}
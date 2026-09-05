using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class DesktopItem : MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public bool canClick = false;
    public bool canDrag = false;
    private bool deleteAfterClick = false;

    [Header("Audio")]
    public AudioClip inputSound;
    private AudioSource audioSource;

    [Header("Yarn")]
    public DialogueRunner dialogueRunner;

    public string clickNodeName;
    public string trashNodeName;
    public string trashAfterOpenNodeName;
    public string dropElsewhereNodeName;

    public Camera cam;

    [Header("Selection Scale")]
    public float selectedScaleMultiplier = 1.15f;

    [Header("Open File Object")]
    public GameObject fileContentObject;

    [Header("Trash Material")]
    public Material trashHoverMaterial;

    [Header("Trash Animation")]
    public Transform trashTransform;
    public float trashBigScale = 1.3f;
    public float trashPulseDuration = 1f;

    private Vector3 originalScale;
    private Vector3 dragStartPosition;
    private Vector3 dragOffset;

    private Renderer objectRenderer;
    private Material[] originalMaterials;

    private Vector3 trashOriginalScale;
    private Coroutine trashPulseCoroutine;

    private bool isDragging;
    private bool wasDragged;
    private bool isOverTrash;

    // Track first interaction for blocking initial trash attempt
    private bool isFirstInteraction = true;

    // Track whether the file has been opened at least once
    private bool hasOpenedFile = false;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        EnsurePointerCollider();

        originalScale = transform.localScale;

        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalMaterials = objectRenderer.materials;
        }

        if (trashTransform != null)
        {
            trashOriginalScale = trashTransform.localScale;
        }

        // Get AudioSource from this GameObject
        audioSource = GetComponent<AudioSource>();

        // Optional: use AudioSource clip automatically
        if (inputSound == null)
        {
            inputSound = audioSource.clip;
        }
    }

    void EnsurePointerCollider()
    {
        if (GetComponent<Collider>() != null)
        {
            return;
        }

        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            return;
        }

        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }
        // Scale up the selected file/folder
        transform.localScale = originalScale * selectedScaleMultiplier;

        // Start trash animation while player is holding this item
        StartTrashPulse();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }
        // Stop trash animation when player releases
        StopTrashPulse();

        // If it was only a click, scale back down
        if (!isDragging)
        {
            transform.localScale = originalScale;
        }
    }

    [YarnCommand("setCanClick")]
    public void canBeClicked(bool value)
    {
        canClick = value;
    }

    [YarnCommand("setCanDrag")]
    public void canBeDragged(bool value)
    {
        canDrag = value;
    }

    [YarnCommand("debugOpened")]
    public static void debugOpened()
    {
        var dialogueRunner = Object.FindFirstObjectByType<DialogueRunner>();

        if (dialogueRunner == null || dialogueRunner.VariableStorage == null)
        {
            Debug.LogWarning("Desktop: unable to read $opened because DialogueRunner or VariableStorage is missing.");
            return;
        }

        if (dialogueRunner.VariableStorage.TryGetValue("$opened", out float opened))
        {
            Debug.Log($"Desktop: opened = {opened}");
        }
        else
        {
            Debug.LogWarning("Desktop: variable $opened was not found.");
        }
    }

    [YarnCommand("deleteItem")]
    public void deleteItem()
    {
        deleteAfterClick = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick)
        {
            Debug.LogWarning($"DesktopItem: click blocked on '{name}' because canClick is false.");
            return;
        }
        // Mark the file as opened so trashing is no longer treated as the first interaction
        hasOpenedFile = true;
        isFirstInteraction = false;

        if (inputSound != null)
        {
            audioSource.clip = inputSound;
            audioSource.Play();

            Debug.Log("Played click sound");
        }

        // Prevent click from firing after drag
        if (wasDragged)
        {
            wasDragged = false;
            return;
        }

        // Activate the image/content inside the file
        if (fileContentObject != null)
        {
            for (var parent = fileContentObject.transform.parent; parent != null; parent = parent.parent)
            {
                parent.gameObject.SetActive(true);
            }

            fileContentObject.SetActive(true);
        }

        // Start Yarn dialogue for clicking/opening this file
        var nodeToPlay = clickNodeName;
        if (string.IsNullOrEmpty(nodeToPlay))
        {
            var match = Regex.Match(name, @"^DocumentIcon_(\d+)$");
            if (match.Success)
            {
                nodeToPlay = $"Fichier{match.Groups[1].Value}";
            }
        }

        if (dialogueRunner != null && !string.IsNullOrEmpty(nodeToPlay))
        {
            Debug.Log($"DesktopItem: OnPointerClick received, starting dialogue node '{nodeToPlay}'.");
            dialogueRunner.StartDialogue(nodeToPlay);
        }
        else
        {
            Debug.LogError($"DesktopItem: cannot open '{name}'. Assign a DialogueRunner and clickNodeName.");
        }

        if(deleteAfterClick)
        {
            Destroy(gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }
        isDragging = true;
        wasDragged = true;

        dragStartPosition = transform.position;

        // Create ray from mouse/touch position
        Ray ray = cam.ScreenPointToRay(eventData.position);

        // Drag on XY plane, keeping Z fixed
        Plane dragPlane = new Plane(Vector3.forward, dragStartPosition);

        if (dragPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);

            // Prevent object from snapping to pointer center
            dragOffset = transform.position - worldPoint;
        }

        transform.localScale = originalScale * selectedScaleMultiplier;

        // StartTrashPulse();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!canDrag)
        {
            return;
        }
        Ray ray = cam.ScreenPointToRay(eventData.position);

        // Drag on XY plane, keeping Z fixed
        Plane dragPlane = new Plane(Vector3.forward, dragStartPosition);

        if (dragPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance) + dragOffset;

            transform.position = new Vector3(
                worldPosition.x,
                worldPosition.y,
                dragStartPosition.z
            );
        }

        CheckTrashHover(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }
        isDragging = false;

        transform.localScale = originalScale;

        StopTrashPulse();

        if (IsPointerOverTrash(eventData))
        {
            // Block deletion only on the first trash attempt if the file has never been opened
            if (isFirstInteraction && !hasOpenedFile)
            {
                // Return file to original position
                transform.position = dragStartPosition;
                
                // Mark first interaction as complete
                isFirstInteraction = false;
                
                // Reset material and trigger the first-time trash response
                ResetMaterial();
                
                if (dialogueRunner != null && !string.IsNullOrEmpty(trashNodeName))
                {
                    dialogueRunner.StartDialogue(trashNodeName);
                }
                
                return;
            }

            // Choose the correct trash node after the file has been opened
            string nodeToPlay = trashNodeName;

            if (hasOpenedFile && !string.IsNullOrEmpty(trashAfterOpenNodeName))
            {
                nodeToPlay = trashAfterOpenNodeName;
            }

            if (dialogueRunner != null && !string.IsNullOrEmpty(nodeToPlay))
            {
                dialogueRunner.StartDialogue(nodeToPlay);
            }

            Destroy(gameObject);
            return;
        }

        // If dropped somewhere else, stay there
        ResetMaterial();

        if (dialogueRunner != null && !string.IsNullOrEmpty(dropElsewhereNodeName))
        {
            dialogueRunner.StartDialogue(dropElsewhereNodeName);
        }
    }

    void CheckTrashHover(PointerEventData eventData)
    {
        bool currentlyOverTrash = IsPointerOverTrash(eventData);

        if (currentlyOverTrash && !isOverTrash)
        {
            isOverTrash = true;
            SetAllMaterials(trashHoverMaterial);
        }
        else if (!currentlyOverTrash && isOverTrash)
        {
            isOverTrash = false;
            ResetMaterial();
        }
    }

    bool IsPointerOverTrash(PointerEventData eventData)
    {
        Ray ray = cam.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.CompareTag("Trash");
        }

        return false;
    }

    void SetAllMaterials(Material newMaterial)
    {
        if (objectRenderer == null || newMaterial == null)
        {
            return;
        }

        Material[] newMaterials = new Material[objectRenderer.materials.Length];

        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = newMaterial;
        }

        objectRenderer.materials = newMaterials;
    }

    void ResetMaterial()
    {
        if (objectRenderer != null && originalMaterials != null)
        {
            objectRenderer.materials = originalMaterials;
        }

        isOverTrash = false;
    }

    void StartTrashPulse()
    {
        if (trashTransform == null)
        {
            return;
        }

        if (trashPulseCoroutine != null)
        {
            StopCoroutine(trashPulseCoroutine);
        }

        trashPulseCoroutine = StartCoroutine(TrashPulseLoop());
    }

    void StopTrashPulse()
    {
        if (trashPulseCoroutine != null)
        {
            StopCoroutine(trashPulseCoroutine);
            trashPulseCoroutine = null;
        }

        if (trashTransform != null)
        {
            trashTransform.localScale = trashOriginalScale;
        }
    }

    IEnumerator TrashPulseLoop()
    {
        Vector3 bigScale = trashOriginalScale * trashBigScale;

        while (true)
        {
            yield return ScaleTrash(trashOriginalScale, bigScale, trashPulseDuration);
            yield return ScaleTrash(bigScale, trashOriginalScale, trashPulseDuration);
        }
    }

    IEnumerator ScaleTrash(Vector3 fromScale, Vector3 toScale, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            trashTransform.localScale = Vector3.Lerp(fromScale, toScale, t);

            yield return null;
        }

        trashTransform.localScale = toScale;
    }
}
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class DayManager : MonoBehaviour
{
    [SerializeField] private FadeController fadeController;
    [SerializeField] private RoomController roomController;

    [Range(1, 3)]
    [SerializeField] private int currentDay = 1;

    [Header("Dialogue")]
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string day1DialogueNode = "Start";
    [SerializeField] private string day2DialogueNode = "Part2";
    [SerializeField] private string day3DialogueNode = "Part3";
    [SerializeField] private bool startDialogueOnStart = true;

    private int lastObservedDay;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform day1CameraTarget;
    [SerializeField] private Transform day2CameraTarget;
    [SerializeField] private Transform day3CameraTarget;
    [SerializeField] private float day1FieldOfView = 60f;
    [SerializeField] private float day2FieldOfView = 45f;
    [SerializeField] private float day3FieldOfView = 50f;

    private void Start()
    {
        // Apply the initial room state.
        roomController.UpdateRoom(currentDay);
        ApplyCameraForDay(currentDay);
        lastObservedDay = currentDay;

        if (startDialogueOnStart)
            StartDialogueForDay(currentDay);
    }

    private void Update()
    {
        if (currentDay == lastObservedDay)
            return;

        currentDay = Mathf.Clamp(currentDay, 1, 3);
        roomController.UpdateRoom(currentDay);
        ApplyCameraForDay(currentDay);
        lastObservedDay = currentDay;
        StartDialogueForDay(currentDay);
    }

    [YarnCommand("Next_day")]
    public IEnumerator NextDay()
    {
        yield return fadeController.FadeOut();

        currentDay = Mathf.Clamp(currentDay + 1, 1, 3);

        roomController.UpdateRoom(currentDay);
        ApplyCameraForDay(currentDay);

        yield return new WaitForSeconds(0.5f);

        yield return fadeController.FadeIn();
    }

    [YarnCommand("Set_day")]
    public IEnumerator SetDay(int day)
    {
        yield return fadeController.FadeOut();

        currentDay = Mathf.Clamp(day, 1, 3);

        roomController.UpdateRoom(currentDay);
        ApplyCameraForDay(currentDay);

        yield return new WaitForSeconds(0.5f);

        yield return fadeController.FadeIn();
    }

    public int CurrentDay => currentDay;

    private void StartDialogueForDay(int day)
    {
        if (dialogueRunner == null)
        {
            Debug.LogWarning("DayManager: no DialogueRunner is assigned.");
            return;
        }

        string node = day switch
        {
            1 => day1DialogueNode,
            2 => day2DialogueNode,
            3 => day3DialogueNode,
            _ => null
        };

        if (string.IsNullOrWhiteSpace(node))
        {
            Debug.LogWarning($"DayManager: no dialogue node is configured for day {day}.");
            return;
        }

        dialogueRunner.StartDialogue(node);
    }

    private void ApplyCameraForDay(int day)
    {
        if (mainCamera == null)
            return;

        Transform target = day switch
        {
            1 => day1CameraTarget,
            2 => day2CameraTarget,
            3 => day3CameraTarget,
            _ => null
        };

        if (target != null)
        {
            mainCamera.transform.SetPositionAndRotation(
                target.position,
                target.rotation);
        }

        mainCamera.fieldOfView = day switch
        {
            1 => day1FieldOfView,
            2 => day2FieldOfView,
            3 => day3FieldOfView,
            _ => mainCamera.fieldOfView
        };
    }
}
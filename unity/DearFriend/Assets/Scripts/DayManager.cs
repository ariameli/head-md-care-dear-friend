using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class DayManager : MonoBehaviour
{
    [Header("Testing")]
    [SerializeField, Range(1, 3)]
    private int startingDay = 1;

    [Header("References")]
    [SerializeField] private FadeController fadeController;
    [SerializeField] private RoomController roomController;
    [SerializeField] private DialogueRunner dialogueRunner;

    private int currentDay;

    private void Start()
    {
        currentDay = startingDay;

        // Set up the room for this day
        roomController.UpdateRoom(currentDay);

        // Start the corresponding Yarn node
        StartDayDialogue();
    }

    private void StartDayDialogue()
    {
        string nodeName = "Day" + currentDay;

        dialogueRunner.StartDialogue(nodeName);
    }

    [YarnCommand("next_day")]
    public IEnumerator NextDay()
    {
        yield return fadeController.FadeOut();

        currentDay++;

        if (currentDay > 3)
        {
            currentDay = 3;
            yield return fadeController.FadeIn();
            yield break;
        }

        // Change the room
        roomController.UpdateRoom(currentDay);

        yield return fadeController.FadeIn();

        // Start the next day's Yarn node
        dialogueRunner.StartDialogue("Day" + currentDay);
    }

    public int CurrentDay => currentDay;
}
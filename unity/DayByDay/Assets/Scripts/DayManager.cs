using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class DayManager : MonoBehaviour
{
    [SerializeField] private FadeController fadeController;
    [SerializeField] private RoomController roomController;

    [SerializeField] private int currentDay = 1;

    private void Start()
    {
        // Apply the initial room state.
        roomController.UpdateRoom(currentDay);
    }

    [YarnCommand("Next_day")]
    public IEnumerator NextDay()
    {
        yield return fadeController.FadeOut();

        currentDay = Mathf.Clamp(currentDay + 1, 1, 3);

        roomController.UpdateRoom(currentDay);

        yield return new WaitForSeconds(0.5f);

        yield return fadeController.FadeIn();
    }

    [YarnCommand("Set_day")]
    public IEnumerator SetDay(int day)
    {
        yield return fadeController.FadeOut();

        currentDay = Mathf.Clamp(day, 1, 3);

        roomController.UpdateRoom(currentDay);

        yield return new WaitForSeconds(0.5f);

        yield return fadeController.FadeIn();
    }

    public int CurrentDay => currentDay;
}
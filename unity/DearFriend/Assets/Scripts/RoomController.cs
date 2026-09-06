using UnityEngine;

public class RoomController : MonoBehaviour
{
    private DayTransform[] dayObjects;

    private void Awake()
    {
        dayObjects = FindObjectsByType<DayTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    public void UpdateRoom(int day)
    {
        foreach (var obj in dayObjects)
        {
            obj.ApplyDay(day);
        }
    }
}
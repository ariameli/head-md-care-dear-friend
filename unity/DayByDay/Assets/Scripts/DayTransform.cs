using UnityEngine;

public class DayTransform : MonoBehaviour
{
    [System.Serializable]
    public class DayState
    {
        public Transform target;
        public bool visible = true;
    }

    [SerializeField] private DayState day1;
    [SerializeField] private DayState day2;
    [SerializeField] private DayState day3;

    public void ApplyDay(int day)
    {
        DayState state = day switch
        {
            1 => day1,
            2 => day2,
            3 => day3,
            _ => null
        };

        if (state == null)
            return;

        gameObject.SetActive(state.visible);

        if (state.visible && state.target != null)
        {
            transform.SetPositionAndRotation(
                state.target.position,
                state.target.rotation);
        }
    }
}
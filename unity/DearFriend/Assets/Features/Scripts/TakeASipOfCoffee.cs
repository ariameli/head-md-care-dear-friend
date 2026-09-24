using UnityEngine;
using UnityEngine.EventSystems;

public class TakeASipOfCoffee : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform assetToLower;
    [SerializeField] private float descentDistance = 0.05f;

    private const int MaxSips = 2;
    private int sipCount;

    private void Awake()
    {
        if (assetToLower == null && transform.childCount > 0)
        {
            assetToLower = transform.GetChild(0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (assetToLower == null || sipCount >= MaxSips)
        {
            return;
        }

        var position = assetToLower.localPosition;
        position.z -= descentDistance;
        assetToLower.localPosition = position;
        sipCount++;
    }
}

using UnityEngine;

public class KitchenTimer : MonoBehaviour
{
    [SerializeField] private Transform dialTop;
    [SerializeField] private float maxTime = 10f;
    [SerializeField] private float startAngle = 270f;

    private float timeRemaining;
    private bool running = true;

    private Quaternion initialRotation;

    void Start()
    {
        timeRemaining = maxTime;

        // On garde l'orientation originale de la pièce
        initialRotation = dialTop.localRotation;
    }

    void Update()
    {
        if (!running || dialTop == null)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            running = false;
            Debug.Log("DING!");
        }

        float progress = timeRemaining / maxTime;
        float angle = progress * startAngle;

        Quaternion timerRotation = Quaternion.Euler(0f, 0f, angle);

        dialTop.localRotation = initialRotation * timerRotation;
    }
}
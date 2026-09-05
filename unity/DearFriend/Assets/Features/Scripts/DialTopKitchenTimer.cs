using UnityEngine;
using Yarn.Unity;

public class KitchenTimer : MonoBehaviour
{
    [SerializeField] private Transform dialTop;
    [SerializeField] private float maxTime = 10f;
    [SerializeField] private float startAngle = 270f;

    private float timeRemaining;
    private bool running = false;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = dialTop.localRotation;
        timeRemaining = maxTime;
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

        Quaternion timerRotation =
            Quaternion.Euler(0f, 0f, angle);

        dialTop.localRotation =
            initialRotation * timerRotation;
    }

    // Commande appelée depuis Yarn
    [YarnCommand("Start_timer")]
    public void StartTimer()
    {
        timeRemaining = maxTime;
        running = true;

        Debug.Log("Timer démarré !");
    }
}
using UnityEngine;
using Yarn.Unity;

public class KitchenTimer : MonoBehaviour
{
    [SerializeField] private Transform dialTop;
    [SerializeField] private float rotationSpeed = 90f;

    private bool running = false;

    void Start()
    {
    }

    void Update()
    {
        if (!running || dialTop == null)
            return;

        dialTop.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);
    }

    // Commande appelée depuis Yarn
    [YarnCommand("Start_timer")]
    public void StartTimer()
    {
        running = true;

        Debug.Log("Timer démarré !");
    }

    [YarnCommand("Stop_timer")]
    public void StopTimer()
    {
        running = false;

        Debug.Log("Timer arrêté !");
    }
}
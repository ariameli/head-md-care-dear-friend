using System.Collections;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(AudioSource))]
public class AudioPlaySound : MonoBehaviour
{
    public AudioClip ComputerWarning;
    public AudioClip ComputerNotification;
    public AudioClip ComputerHumming;
    public AudioClip ComputerFansSpeedsUp;
    public AudioClip CameraZoomOut;
    public AudioClip KitchenTimerTicTac;
    public AudioClip KitchenTimerDing;

    [Header("Playback")]
    [SerializeField] private bool loopComputerWarning;
    [SerializeField] private bool loopComputerNotification;
    [SerializeField] private bool loopComputerHumming;
    [SerializeField] private bool loopComputerFansSpeedsUp;
    [SerializeField] private bool loopCameraZoomOut;
    [SerializeField] private bool loopKitchenTimerTicTac = true;
    [SerializeField] private bool loopKitchenTimerDing;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetClip(string soundName)
    {
        return soundName switch
        {
            "Warning"       => ComputerWarning,
            "Notification"  => ComputerNotification,
            "ZoomOut"       => CameraZoomOut,
            "ComputerFanSpeedsUp" => ComputerFansSpeedsUp,
            "ComputerHumming" => ComputerHumming,
            "KitchenTimerTicTac" => KitchenTimerTicTac,
            "KitchenTimerDing" => KitchenTimerDing,
            _               => null
        };
    }

    private bool ShouldLoop(string soundName)
    {
        return soundName switch
        {
            "Warning" => loopComputerWarning,
            "Notification" => loopComputerNotification,
            "ZoomOut" => loopCameraZoomOut,
            "ComputerFanSpeedsUp" => loopComputerFansSpeedsUp,
            "ComputerHumming" => loopComputerHumming,
            "KitchenTimerTicTac" => loopKitchenTimerTicTac,
            "KitchenTimerDing" => loopKitchenTimerDing,
            _ => false
        };
    }

    [YarnCommand("PlaySound")]
    public IEnumerator PlaySound(string soundName)
    {
        AudioClip clip = GetClip(soundName);

        if (clip == null)
        {
            Debug.LogWarning($"Sound not found: {soundName}");
            yield break;
        }

        audioSource.clip = clip;
        audioSource.loop = ShouldLoop(soundName);
        audioSource.Play();

    }

    [YarnCommand("StopSound")]
    public void StopSound(string soundName)
    {
        audioSource.Stop();
        audioSource.loop = false;
    }
}
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(AudioSource))]
public class AudioPlaySound : MonoBehaviour
{
    public AudioClip ComputerWarning;
    public AudioClip ComputerNotification;
    public AudioClip CameraZoomOut;

    public AudioClip DialogueOpen;
    public AudioClip DialogueClose;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [YarnCommand("PlaySound")]
    public void PlaySound(string soundName)
    {
        AudioClip clip = soundName switch
        {
            "Warning" => ComputerWarning,
            "Notification" => ComputerNotification,
            "ZoomOut" => CameraZoomOut,
            "DialogueOpen" => DialogueOpen,
            "DialogueClose" => DialogueClose,
            "ComputerFan" => ComputerNotification, // Placeholder for ComputerFan sound
            _ => null
        };

        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound not found: {soundName}");
        }
    }
}
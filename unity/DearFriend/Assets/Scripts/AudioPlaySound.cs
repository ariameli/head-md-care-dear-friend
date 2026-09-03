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
            _               => null
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
        audioSource.PlayOneShot(clip);

    }

    [YarnCommand("StopSound")]
    public void StopSound(string soundName)
    {
        audioSource.Stop();
    }
}
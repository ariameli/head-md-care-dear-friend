using UnityEngine;
using Yarn.Unity;

public class AudioDialoguePresenter : DialoguePresenterBase
{
    public AudioSource audioSource;

    public AudioClip DialogueSylvia_1;
    public AudioClip DialogueCamille_1;

    public override YarnTask OnDialogueStartedAsync()
    {
        return YarnTask.CompletedTask;
    }

    public override YarnTask OnDialogueCompleteAsync()
    {
        return YarnTask.CompletedTask;
    }

    private AudioClip GetClip(string soundName)
    {
        return soundName switch
        {
            "DialogueSylvia_1"  => DialogueSylvia_1,
            "DialogueCamille_1" => DialogueCamille_1,
            _ => null
        };
    }

    public override async YarnTask RunLineAsync(
        LocalizedLine line,
        LineCancellationToken token)
    {
        string soundName = null;

        foreach (string metadata in line.Metadata)
        {
            Debug.Log("Yarn metadata: " + metadata);

            if (metadata.StartsWith("sound:"))
            {
                soundName = metadata.Substring("sound:".Length);
                break;
            }
        }

        // Cette ligne n'a aucun son associé
        if (string.IsNullOrEmpty(soundName))
        {
            return;
        }

        AudioClip clip = GetClip(soundName);

        if (clip == null)
        {
            Debug.LogWarning(
                $"AudioDialoguePresenter: clip introuvable pour '{soundName}'"
            );
            return;
        }

        Debug.Log("Playing dialogue sound: " + soundName);

        audioSource.clip = clip;
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            await YarnTask.Yield();
        }
    }
}
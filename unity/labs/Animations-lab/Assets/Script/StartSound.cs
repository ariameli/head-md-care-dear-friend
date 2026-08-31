using System.Collections;
using UnityEngine;

public class AudioFadeIn : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 2f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(
                0f,
                1f,
                timer / fadeDuration
            );

            yield return null;
        }

        audioSource.volume = 1f;
    }
}
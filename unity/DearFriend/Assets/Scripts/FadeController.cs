using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // Don't block clicks when we're not fading.
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public IEnumerator FadeOut()
    {
        // Start blocking clicks immediately.
        canvasGroup.blocksRaycasts = true;

        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);

        // Stop blocking clicks once the fade is finished.
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        canvasGroup.alpha = start;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                start,
                end,
                elapsed / fadeDuration
            );

            yield return null;
        }

        canvasGroup.alpha = end;
    }
}
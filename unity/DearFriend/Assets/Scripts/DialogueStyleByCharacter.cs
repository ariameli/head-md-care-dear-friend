using UnityEngine;
using TMPro;

public class DialogueStyleByCharacter : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public RectTransform bubble;

    public Vector2 annaPosition;
    public Vector2 camillePosition;

    public Color annaColor = Color.white;
    public Color camilleColor = Color.white;

    public Color annaTextColor = Color.white;
    public Color camilleTextColor = Color.white;

    public UnityEngine.UI.Image bubbleImage;

    [Header("Nameplate Position")]
    public RectTransform nameplate;
    [Tooltip("Distance inward from the left or right edge of Line Presenter.")]
    public float nameplateHorizontalInset = 40f;
    [Tooltip("Distance above the top edge of Line Presenter.")]
    public float nameplateVerticalOffset = 30f;

    void LateUpdate()
    {
        if (characterName == null) return;

        string speaker = characterName.text.Trim();

        if (speaker == "Sylvia" || speaker == "Anna")
        {
            if (bubble) bubble.anchoredPosition = annaPosition;
            if (bubbleImage) bubbleImage.color = annaColor;
            if (characterName) characterName.color = annaTextColor;
            PositionNameplate(true);
        }
        else if (speaker == "Camille")
        {
            if (bubble) bubble.anchoredPosition = camillePosition;
            if (bubbleImage) bubbleImage.color = camilleColor;
            if (characterName) characterName.color = camilleTextColor;
            PositionNameplate(false);
        }
    }

    private void PositionNameplate(bool onRight)
    {
        if (nameplate == null) return;

        Vector2 topCorner = new Vector2(onRight ? 1f : 0f, 1f);
        nameplate.anchorMin = topCorner;
        nameplate.anchorMax = topCorner;
        nameplate.pivot = topCorner;
        nameplate.anchoredPosition = new Vector2(
            onRight ? -nameplateHorizontalInset : nameplateHorizontalInset,
            nameplateVerticalOffset);
    }
}

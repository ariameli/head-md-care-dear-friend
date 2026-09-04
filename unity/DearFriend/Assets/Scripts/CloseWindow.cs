using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class CloseWindow : MonoBehaviour, IPointerClickHandler
{
    public DialogueRunner dialogueRunner;
    public string clickNodeName;

    public float pulseAmount = 0.1f;
    public float pulseSpeed = 3f;

    public bool canClick = false;

    Vector3 originalScale;
    bool pulsing = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!pulsing) return;

        // Smoothly goes from 1.0 to 1.1 and back
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        transform.localScale = originalScale * scale;
    }

    [YarnCommand("startPulsating")]
    public void pulsatingButton()
    {
        pulsing = true;
    }

    void OnNodeComplete(string nodeName)
    {
        if (nodeName == clickNodeName)
            pulsing = true;
    }

    [YarnCommand("setCanClickForCloseButton")]
    public void canBeClicked(bool value)
    {
        canClick = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick) return;
        Destroy(transform.parent.gameObject);
        //start the node specified in clickNodeName
        if (dialogueRunner != null && !string.IsNullOrEmpty(clickNodeName))
        {
            dialogueRunner.StartDialogue(clickNodeName);
        }
    }
}
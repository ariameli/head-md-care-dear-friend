using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchController : MonoBehaviour
{
    [Header("Search")]
    [SerializeField] private TMP_Text searchText;
    [SerializeField] private string wordToType = "MEMENTO VIVERE";
    [SerializeField] private float typingSpeed = 0.06f;

    [Header("Results")]
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private RectTransform[] results;

    [Header("Scroll")]
    [SerializeField] private ScrollRect scrollRect;

    private int currentResult = 0;

    public void StartSearch()
    {
        currentResult = 0;
        UpdateCounter();
        StartCoroutine(TypeSearch());
    }

    private IEnumerator TypeSearch()
    {
        searchText.text = "";

        foreach (char letter in wordToType)
        {
            searchText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextResult()
    {
        if (currentResult < results.Length - 1)
        {
            currentResult++;
        }

        UpdateCounter();
    }

    public void PreviousResult()
    {
        if (currentResult > 0)
        {
            currentResult--;
    }

        UpdateCounter();
    }
   

    private void UpdateCounter()
    {
        counterText.text = (currentResult + 1) + "/" + results.Length;
    }
}
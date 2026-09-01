using System.Collections;
using TMPro;
using UnityEngine;

public class SearchController : MonoBehaviour
{
    [SerializeField] private TMP_Text searchText;
    [SerializeField] private string wordToType = "MEMENTO VIVERE";
    [SerializeField] private float typingSpeed = 0.06f;

    public void StartSearch()
    {
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
}
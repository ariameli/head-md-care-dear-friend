using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterText : MonoBehaviour
{
    [SerializeField] private float vitesse = 0.15f;
    [SerializeField] private string motDePasse = "1234";

    private TMP_Text texteUI;
    private Coroutine coroutine;

    private void Awake()
    {
        texteUI = GetComponent<TMP_Text>();
    }

    public void LancerTexte()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(Ecrire(motDePasse));
    }

    private IEnumerator Ecrire(string texte)
    {
        // Efface "TAPEZ le mot de passe"
        texteUI.text = "";

        // Écrit "1234" caractère par caractère
        foreach (char caractere in texte)
        {
            texteUI.text += caractere;
            yield return new WaitForSeconds(vitesse);
        }

        coroutine = null;
    }
}
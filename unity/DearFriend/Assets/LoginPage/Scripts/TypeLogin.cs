using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterText : MonoBehaviour
{
    [SerializeField] private float vitesse = 0.5f;
    [SerializeField] private string motDePasse = "1234";

    private TMP_Text texteTMP;
    private Text texteLegacy;
    private Coroutine coroutine;

    private void Awake()
    {
        texteTMP = GetComponent<TMP_Text>();
        texteLegacy = GetComponent<Text>();
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
        DefinirTexte("");

        // Écrit "1234" caractère par caractère
        foreach (char caractere in texte)
        {
            string texteActuel = texteTMP != null ? texteTMP.text : texteLegacy.text;
            DefinirTexte(texteActuel + caractere);
            yield return new WaitForSeconds(vitesse);
        }

        coroutine = null;
    }

    private void DefinirTexte(string texte)
    {
        if (texteTMP != null)
            texteTMP.text = texte;
        else if (texteLegacy != null)
            texteLegacy.text = texte;
    }
}
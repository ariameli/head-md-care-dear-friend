using UnityEngine;

public class AppController : MonoBehaviour
{
    [SerializeField] private GameObject accountScreen;
    [SerializeField] private GameObject conversationScreen;

    public void OpenConversation()
    {
        accountScreen.SetActive(true);
        conversationScreen.SetActive(true);
    }
}
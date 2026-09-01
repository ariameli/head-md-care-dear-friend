using UnityEngine;

public class AppController : MonoBehaviour
{
    [SerializeField] private GameObject accountScreen;
    [SerializeField] private GameObject conversationScreen;

    public void OpenConversation()
    {
        accountScreen.SetActive(false);
        conversationScreen.SetActive(true);
    }
}
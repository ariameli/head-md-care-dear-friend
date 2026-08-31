using UnityEngine;
// use unity event system 
using UnityEngine.EventSystems;
public class loginButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject profilePicture; // Reference to the profile picture GameObject
    public GameObject NewPage; // Reference to the new page GameObject
    public void OnPointerClick(PointerEventData eventData)
    {
        // Handle login logic here
        // hide profile picture object when login button is clicked
        profilePicture.SetActive(false);
        gameObject.SetActive(false); // Hide the login button after clicking
        NewPage.SetActive(true); // Show the new page
    }
}

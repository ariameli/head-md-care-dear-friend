using UnityEngine;

public class CloseButtons : MonoBehaviour
{
    public void CloseParentImage()
    {
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
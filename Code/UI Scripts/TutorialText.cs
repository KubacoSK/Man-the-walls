using UnityEngine;
using UnityEngine.UI;

public class IntroductionPopup : MonoBehaviour
{
    public Button closeButton;

    void Start()
    {
        // Attach a function to the close button
        closeButton.onClick.AddListener(ClosePopup);
    }

    void ClosePopup()
    {
        // Close or disable the pop-up window
        gameObject.SetActive(false);
    }
}
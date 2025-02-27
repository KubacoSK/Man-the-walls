using UnityEngine;
using UnityEngine.UI;

public class IntroductionPopup : MonoBehaviour
{
    public Button closeButton;

    void Start()
    {
        closeButton.onClick.AddListener(ClosePopup);
    }

    void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
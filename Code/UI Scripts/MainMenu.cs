
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource GearSound;
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Play()
    {
        animator.SetTrigger("Play");
        GearSound.Play();
    }
}

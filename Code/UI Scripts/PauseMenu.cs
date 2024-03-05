using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]public GameObject pauseMenu;
    [SerializeField] private AudioSource SettingsMenuSound;
    private static bool isPaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // pauses game when button is selected
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        // resumes it
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        MusicPlayer.Instance.ResumeBackgroundMusic();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        isPaused = false; 
    }
    public void PlaySettingsMenuSound()
    {
        MusicPlayer.Instance.PauseBackgroundMusic();
        SettingsMenuSound.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public static bool IsGamePaused()
    { 
        return isPaused; 
    }
}

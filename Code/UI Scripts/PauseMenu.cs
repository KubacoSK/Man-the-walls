
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]public GameObject pauseMenu;
    [SerializeField] private AudioSource SettingsMenuSound;
    private static bool isPaused;
    public static PauseMenu instance;

    private void Start()
    {
        if (instance != null)
        {
            Debug.LogError("There's more than one PauseMenu! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }
        instance = this;
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
        // zastaví hru
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
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        isPaused = false; 
    }
    public bool IsSettingsMenuPlayingSound()
    {
        return SettingsMenuSound.isPlaying;
    }
    public void ResumeGameMenuSound()
    {
        MusicPlayer.Instance.ResumeBackgroundMusic();
        SettingsMenuSound.Stop();
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

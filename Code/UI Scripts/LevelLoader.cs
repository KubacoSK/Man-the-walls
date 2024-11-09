using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI progressText;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;  // Prevents the scene from loading immediately

        loadingScreen.SetActive(true);

        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            // Calculate the loading progress
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            // Track the elapsed time
            elapsedTime += Time.deltaTime;

            // Allow scene activation only after 2 seconds and when loading is complete
            if (elapsedTime >= 2.5f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    // these are used for buttons in main menu screen
    public void SetDifficultyToEasy()
    {
        DifficultySetter.SetDifficulty("Easy");
    }

    public void SetDifficultyToMedium()
    {
        DifficultySetter.SetDifficulty("Medium");
    }

    public void SetDifficultyToHard()
    {
        DifficultySetter.SetDifficulty("Hard");
    }

    public void SetDifficultyToNightmare()
    {
        DifficultySetter.SetDifficulty("Nightmare");
    }
    
}

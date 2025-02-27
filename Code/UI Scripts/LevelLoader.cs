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
        operation.allowSceneActivation = false;  // Zabraňuje okamžitému načítaniu scény

        loadingScreen.SetActive(true);

        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            // Vypočítanie priebehu načítania
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

            // Sledovanie uplynutého času
            elapsedTime += Time.deltaTime;

            // Povolenie aktivácie scény až po 2,5 sekundách a pri dokončení načítania
            if (elapsedTime >= 2.5f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    // Tieto metódy sa používajú pre tlačidlá v hlavnom menu
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
}

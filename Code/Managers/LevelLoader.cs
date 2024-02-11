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
        StartCoroutine(LoadAsynchoronously(sceneIndex));
        
    }

    public void SetDifficultyToEasy()
    {
        DifficultySetter.Instance.SetDifficulty("Easy");
    }

    public void SetDifficultyToMedium()
    {
        DifficultySetter.Instance.SetDifficulty("Medium");
    }

    public void SetDifficultyToHard()
    {
        DifficultySetter.Instance.SetDifficulty("Hard");
    }

    public void SetDifficultyToNightmare()
    {
        DifficultySetter.Instance.SetDifficulty("Nightmare");
    }
    IEnumerator LoadAsynchoronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}

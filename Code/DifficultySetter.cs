using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    public static DifficultySetter Instance {  get; private set; }
    private string Difficulty = "Medium";

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ResourceManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public string GetDifficulty()
    {
        return Difficulty;
    }

    public void SetDifficulty(string SetDifficulty)
    {
        Difficulty = SetDifficulty;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DifficultySetter
{
    private static string Difficulty = "Medium";

    public static string GetDifficulty()
    {
        return Difficulty;
    }

    public static void SetDifficulty(string SetDifficulty)
    {
        Difficulty = SetDifficulty;
    }   
}

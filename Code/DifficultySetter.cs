using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DifficultySetter
{
    private static string Difficulty = "Medium";
    // class that remembers settings of difficulty throught scenes
    public static string GetDifficulty()
    {
        return Difficulty;
    }

    public static void SetDifficulty(string SetDifficulty)
    {
        Difficulty = SetDifficulty;
    }   
}

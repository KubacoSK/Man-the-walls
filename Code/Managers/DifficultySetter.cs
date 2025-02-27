

public static class DifficultySetter
{
    private static string Difficulty = "Medium";
    // táto trieda zodpovedá za nastavovanie obtiažnosti aj medzi scénami
    public static string GetDifficulty()
    {
        return Difficulty;
    }

    public static void SetDifficulty(string SetDifficulty)
    {
        Difficulty = SetDifficulty;
    }   
}

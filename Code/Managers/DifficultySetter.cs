

public static class DifficultySetter
{
    private static string Difficulty = "Medium";
    // t�to trieda zodpoved� za nastavovanie obtia�nosti aj medzi sc�nami
    public static string GetDifficulty()
    {
        return Difficulty;
    }

    public static void SetDifficulty(string SetDifficulty)
    {
        Difficulty = SetDifficulty;
    }   
}

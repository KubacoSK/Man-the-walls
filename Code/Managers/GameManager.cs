using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject victorymenu;
    [SerializeField] private GameObject losemenu;
    [SerializeField] private Zone CenterZone;
    static bool ?VictoryState = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Update()
    {  
        // základná kontrola či sme vyhrali alebo prehrali
        if (CenterZone.ReturnEnemyUnitsInZone().Count > 0) Lose();
        if (VictoryState == null) { return; }
        else if (VictoryState == true)
        {
            SceneManager.LoadScene("VictoryScreen");
        }
        else if (VictoryState == false)
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }

    public void Win()
    {
        VictoryState = true;
    }

    public void Lose()
    {
        VictoryState = false;
    }

}

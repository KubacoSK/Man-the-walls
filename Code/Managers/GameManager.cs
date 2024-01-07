using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject victorymenu;
    [SerializeField] private GameObject losemenu;
    [SerializeField] private Zone CenterZone;
    static bool ?VictoryState = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        if (CenterZone.ReturnEnemyUnitsInZone().Count > 0) Lose();
        if (VictoryState == null) { return; }
        else if (VictoryState == true)
        {
            victorymenu.SetActive(true);
        }
        else if (VictoryState == false)
        {
            losemenu.SetActive(true);
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

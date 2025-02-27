using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;                         
    [SerializeField] private TextMeshProUGUI turnNumberText;                // ukazuje počet kol ktory sa hra hrala
    [SerializeField] private GameObject EnemyTurnVisualObject;              // informuje ze je nepriatelske kolo
    [SerializeField] private TextMeshProUGUI numberOfEnemySoldierMovesLeft; // ukaze kolko nepriatelov spravilo svoje pohyby
    [SerializeField] private AudioSource menuRetractSound;
    [SerializeField] private AudioSource menuOpenSound;

    public static TurnSystemUI Instance;
    public void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one TurnSystemUi! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // idk
        endTurnButton.onClick.AddListener(() =>
        {
           
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
        UpdateEnemyUnitsMovesLeftNumber();
    }
    

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
        UpdateEnemyUnitsMovesLeftNumber();
    }
    private void UpdateTurnText()
    {
        if(TurnSystem.Instance.GetTurnNumber() < 10)                          // ukazuje pocet kôl s 0 na zaciatku pre lepší výzor
            turnNumberText.text = "00" + TurnSystem.Instance.GetTurnNumber();
        else if (TurnSystem.Instance.GetTurnNumber() < 100)
            turnNumberText.text = "0" + TurnSystem.Instance.GetTurnNumber();
        else turnNumberText.text = "" + TurnSystem.Instance.GetTurnNumber();
    }
    public void PlayRetractSound()
    {
        menuRetractSound.Play();
    }
    public void PlayOpenSound()
    {
        menuOpenSound.Play();
    }
    private void UpdateEnemyTurnVisual()
    {
        EnemyTurnVisualObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }
    private void UpdateEnemyUnitsMovesLeftNumber()
    {
        numberOfEnemySoldierMovesLeft.gameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }
    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
    public void UpdateEnemySoldiersTurnNumber(int numberOfEnemySoldiersUnfinished)
    {
        numberOfEnemySoldierMovesLeft.text = "Enemy moves left: " + numberOfEnemySoldiersUnfinished;
    }
}

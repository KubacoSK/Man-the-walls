using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;                          // button for ending turn
    [SerializeField] private TextMeshProUGUI turnNumberText;                // displays the number of turns game has played
    [SerializeField] private GameObject EnemyTurnVisualObject;              // just text object informing that its enemy turn

    public void Start()
    {
        // idk
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }
    

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }
    private void UpdateTurnText()
    {
        if(TurnSystem.Instance.GetTurnNumber() < 10)                          // its for better visual look by showing zeros before actual number
            turnNumberText.text = "00" + TurnSystem.Instance.GetTurnNumber();
        else if (TurnSystem.Instance.GetTurnNumber() < 100)
            turnNumberText.text = "0" + TurnSystem.Instance.GetTurnNumber();
        else turnNumberText.text = "" + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        EnemyTurnVisualObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;                          // button for ending turn
    [SerializeField] private TextMeshProUGUI turnNumberText;                // displays the number of turns game has played
    [SerializeField] private GameObject EnemyTurnVisualObject;              // just text object informing that its enemy turn
    [SerializeField] private TextMeshProUGUI numberOfEnemySoldierTurnsLeft; // shows how many enemy units havent done thier turn
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
        UpdateEnemyUnitsTurnsLeftNumber();
    }
    

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
        UpdateEnemyUnitsTurnsLeftNumber();
    }
    private void UpdateTurnText()
    {
        if(TurnSystem.Instance.GetTurnNumber() < 10)                          // its for better visual look by showing zeros before actual number
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
    private void UpdateEnemyUnitsTurnsLeftNumber()
    {
        numberOfEnemySoldierTurnsLeft.gameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }
    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
    public void UpdateEnemySoldiersTurnNumber(int numberOfEnemySoldiersUnifnished)
    {
        numberOfEnemySoldierTurnsLeft.text = "Enemy turns left: " + numberOfEnemySoldiersUnifnished;
    }
}

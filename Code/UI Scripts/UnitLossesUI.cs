using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitLossesUI : MonoBehaviour
{
    public static UnitLossesUI Instance;

    [SerializeField] private TextMeshProUGUI textTotalAlliedInfantryLosses;
    [SerializeField] private TextMeshProUGUI textTotalAlliedTankLosses;
    [SerializeField] private TextMeshProUGUI textTotalAlliedHorsemanLosses;
    [SerializeField] private TextMeshProUGUI textTotalAlliedBattleRobotLosses;
    [SerializeField] private TextMeshProUGUI textTotalEnemyInfantryLosses;

    [SerializeField] private TextMeshProUGUI textRecentAlliedInfantryLosses;
    [SerializeField] private TextMeshProUGUI textRecentAlliedTankLosses;
    [SerializeField] private TextMeshProUGUI textRecentAlliedHorsemanLosses;
    [SerializeField] private TextMeshProUGUI textRecentAlliedBattleRobotLosses;
    [SerializeField] private TextMeshProUGUI textRecentEnemyInfantryLosses;


    
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one TurnSystemUi! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        TurnSystem.Instance.OnTurnChanged += UnitLossesUI_OnTurnChanged;
        Unit.OnAnyUnitDead += UnitLossesUI_OnAnyUnitDead;
    }

    private void UnitLossesUI_OnTurnChanged(object sender, EventArgs e)
    {
        // updates number of losses that occured when turn changes
        textTotalAlliedInfantryLosses.text = LossesManager.Instance.totalAlliedInfantryLosses.ToString();
        textTotalAlliedTankLosses.text = LossesManager.Instance.totalAlliedTankLosses.ToString();
        textTotalAlliedHorsemanLosses.text = LossesManager.Instance.totalAlliedHorsemanLosses.ToString();
        textTotalAlliedBattleRobotLosses.text = LossesManager.Instance.totalAlliedBattleRobotLosses.ToString();
        textTotalEnemyInfantryLosses.text = LossesManager.Instance.totalEnemyInfantryLosses.ToString();

        textRecentAlliedInfantryLosses.text = LossesManager.Instance.recentAlliedInfantryLosses.ToString();
        textRecentAlliedTankLosses.text = LossesManager.Instance.recentAlliedTankLosses.ToString();
        textRecentAlliedHorsemanLosses.text = LossesManager.Instance.recentAlliedHorsemanLosses.ToString();
        textRecentAlliedBattleRobotLosses.text = LossesManager.Instance.recentAlliedBattleRobotLosses.ToString();
        textRecentEnemyInfantryLosses.text = LossesManager.Instance.recentEnemyInfantryLosses.ToString();
    }

    private void UnitLossesUI_OnAnyUnitDead(object sender, EventArgs e)
    {
        // updates number of losses that occured when turn changes
        textTotalAlliedInfantryLosses.text = LossesManager.Instance.totalAlliedInfantryLosses.ToString();
        textTotalAlliedTankLosses.text = LossesManager.Instance.totalAlliedTankLosses.ToString();
        textTotalAlliedHorsemanLosses.text = LossesManager.Instance.totalAlliedHorsemanLosses.ToString();
        textTotalAlliedBattleRobotLosses.text = LossesManager.Instance.totalAlliedBattleRobotLosses.ToString();
        textTotalEnemyInfantryLosses.text = LossesManager.Instance.totalEnemyInfantryLosses.ToString();

        textRecentAlliedInfantryLosses.text = LossesManager.Instance.recentAlliedInfantryLosses.ToString();
        textRecentAlliedTankLosses.text = LossesManager.Instance.recentAlliedTankLosses.ToString();
        textRecentAlliedHorsemanLosses.text = LossesManager.Instance.recentAlliedHorsemanLosses.ToString();
        textRecentAlliedBattleRobotLosses.text = LossesManager.Instance.recentAlliedBattleRobotLosses.ToString();
        textRecentEnemyInfantryLosses.text = LossesManager.Instance.recentEnemyInfantryLosses.ToString();
    }

}

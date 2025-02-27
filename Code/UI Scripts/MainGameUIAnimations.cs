using UnityEngine;

public class MainGameUIAnimations : MonoBehaviour
{
    [SerializeField] Animator battleReportAnimator;   // animácie pre menu vojakov a vylepšení
    [SerializeField] Animator upgradeSpawnMenuAnimator;  
    public void ShowBattleReport()
    {
        battleReportAnimator.SetBool("ShowBattleReport",true);
    }
    public void HideBattleReport()
    {
        battleReportAnimator.SetBool("ShowBattleReport", false);
    }
    public void ShowUpgradeMenu()
    {
        upgradeSpawnMenuAnimator.SetBool("UpgradeMenuTurnedOn", true);
        upgradeSpawnMenuAnimator.SetBool("SpawnMenuTurnedOn", false);
    }
    public void HideBothMenu()
    {
        upgradeSpawnMenuAnimator.SetBool("UpgradeMenuTurnedOn", false);
        upgradeSpawnMenuAnimator.SetBool("SpawnMenuTurnedOn", false);
    }
    public void ShowSpawnMenu()
    {
        upgradeSpawnMenuAnimator.SetBool("UpgradeMenuTurnedOn", false); // jedno vypne a jedno zapne tak aby to robili naraz
        upgradeSpawnMenuAnimator.SetBool("SpawnMenuTurnedOn", true);
    }
}

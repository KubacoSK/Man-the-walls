using UnityEngine;

public class MainGameUIAnimations : MonoBehaviour
{
    [SerializeField] Animator battleReportAnimator;   // animation for battle menu rollout and reverse
    [SerializeField] Animator upgradeSpawnMenuAnimator;  // animation for two menus to work together
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
        upgradeSpawnMenuAnimator.SetBool("UpgradeMenuTurnedOn", false); // turns one menu off so the other can slide in
        upgradeSpawnMenuAnimator.SetBool("SpawnMenuTurnedOn", true);
    }
}

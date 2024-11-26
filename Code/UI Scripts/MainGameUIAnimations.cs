using UnityEngine;

public class MainGameUIAnimations : MonoBehaviour
{
    [SerializeField] Animator battleReportAnimator;
    public void ShowBattleReport()
    {
        battleReportAnimator.SetBool("ShowBattleReport",true);
    }
    public void HideBattleReport()
    {
        battleReportAnimator.SetBool("ShowBattleReport", false);
    }
}

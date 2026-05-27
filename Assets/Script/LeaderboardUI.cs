using UnityEngine;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    public TextMeshProUGUI totalTrophiesText;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI lastRunTrophiesText;

    private void OnEnable()
    {
        if (TrophyManager.Instance == null)
        {
            Debug.LogError("TrophyManager가 없습니다!");
            return;
        }

        var data = TrophyManager.Instance.currentRun;

        totalTrophiesText.text = "총 트로피: " + data.totalTrophies + "개";
        
    }
}
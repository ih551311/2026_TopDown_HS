using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("UI")]
    public GameObject shopPanel;
    public TextMeshProUGUI trophyText;
    public TextMeshProUGUI statusText;

    [Header("업그레이드 가격")]
    public int speedCost = 8;
    public int dashSpeedCost = 12;
    public int extraTrophyCost = 20;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            Time.timeScale = 0f;
            UpdateUI();
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void UpdateUI()
    {
        // 트로피 표시
        if (trophyText != null && TrophyManager.Instance != null)
        {
            trophyText.text = $"트로피: {TrophyManager.Instance.currentRun.trophiesCollected}";
        }

        // 업그레이드 현황만 표시 (가격 제거)
        if (statusText != null)
        {
            string content = "=== 상점 ===\n\n";

            if (PermanentStatsManager.Instance != null)
            {
                var stats = PermanentStatsManager.Instance.stats;
                content += $"[현재 업그레이드]\n";
                content += $"• 이동 속도: +{stats.speedBonus:F1}\n";
                content += $"• 대쉬 속도: +{stats.dashSpeedBonus:F1}\n";
                content += $"• 추가 트로피 스폰: +{stats.extraTrophySpawn}";
            }
            else
            {
                content += "업그레이드 정보를 불러올 수 없습니다.";
            }

            statusText.text = content;
        }
    }

    // ==================== 구매 ====================
    public void BuySpeedUpgrade()
    {
        if (TrophyManager.Instance == null) return;
        if (TrophyManager.Instance.currentRun.trophiesCollected >= speedCost)
        {
            TrophyManager.Instance.currentRun.trophiesCollected -= speedCost;
            if (PermanentStatsManager.Instance != null)
                PermanentStatsManager.Instance.UpgradeSpeed(0.5f);
            UpdateUI();
        }
    }

    public void BuyDashSpeedUpgrade()
    {
        if (TrophyManager.Instance == null) return;
        if (TrophyManager.Instance.currentRun.trophiesCollected >= dashSpeedCost)
        {
            TrophyManager.Instance.currentRun.trophiesCollected -= dashSpeedCost;
            if (PermanentStatsManager.Instance != null)
                PermanentStatsManager.Instance.UpgradeDash(3f, 0f);
            UpdateUI();
        }
    }

    public void BuyExtraTrophySpawn()
    {
        if (TrophyManager.Instance == null) return;
        if (TrophyManager.Instance.currentRun.trophiesCollected >= extraTrophyCost)
        {
            TrophyManager.Instance.currentRun.trophiesCollected -= extraTrophyCost;
            if (PermanentStatsManager.Instance != null)
                PermanentStatsManager.Instance.AddExtraTrophySpawn(1);
            UpdateUI();
        }
    }

    // ==================== 기타 버튼 ====================
    public void ResetAllUpgrades()
    {
        if (PermanentStatsManager.Instance != null)
        {
            PermanentStatsManager.Instance.stats = new PermanentStats();
            PermanentStatsManager.Instance.SaveStats();
            UpdateUI();
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetStage()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentStage = 1;
            if (TrophyManager.Instance != null)
                TrophyManager.Instance.currentRun.trophiesCollected = 0;
            UpdateUI();
        }
    }
}
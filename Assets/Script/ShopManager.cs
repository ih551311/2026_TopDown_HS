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
            Time.timeScale = 0f;        // 시간 정지
            UpdateUI();
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Time.timeScale = 1f;        // 시간 재개
        }
    }

    private void UpdateUI()
    {
        if (trophyText != null && TrophyManager.Instance != null)
        {
            trophyText.text = $"트로피: {TrophyManager.Instance.currentRun.trophiesCollected}";
        }

        if (statusText != null)
        {
            string content = "=== 상점 ===\n\n";

            // 현재 업그레이드 현황
            if (PermanentStatsManager.Instance != null)
            {
                var stats = PermanentStatsManager.Instance.stats;
                content += $"[현재 업그레이드]\n";
                content += $"• 이동 속도: +{stats.speedBonus:F1}\n";
                content += $"• 대쉬 속도: +{stats.dashSpeedBonus:F1}\n";
                content += $"• 추가 트로피: +{stats.extraTrophySpawn}\n\n";
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
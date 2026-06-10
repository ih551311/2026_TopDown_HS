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
    public int dashCooldownCost = 10;

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
            UpdateUI();
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    private void UpdateUI()
    {
        if (trophyText != null && TrophyManager.Instance != null)
            trophyText.text = $"트로피: {TrophyManager.Instance.currentRun.trophiesCollected}";

        if (statusText != null && PermanentStatsManager.Instance != null)
        {
            var stats = PermanentStatsManager.Instance.stats;
            statusText.text =
                $"현재 업그레이드\n" +
                $"• 이동 속도: +{stats.speedBonus:F1}\n" +
                $"• 대쉬 속도: +{stats.dashSpeedBonus:F1}\n" +
                $"• 대쉬 쿨타임: -{stats.dashCooldownReduction:F2}초";
        }
    }

    // ==================== 업그레이드 ====================
    public void BuySpeedUpgrade()
    {
        if (TrophyManager.Instance == null) return;
        if (TrophyManager.Instance.currentRun.trophiesCollected >= speedCost)
        {
            TrophyManager.Instance.currentRun.trophiesCollected -= speedCost;
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
            PermanentStatsManager.Instance.UpgradeDash(3f, 0f);
            UpdateUI();
        }
    }

    public void BuyDashCooldownUpgrade()
    {
        if (TrophyManager.Instance == null) return;
        if (TrophyManager.Instance.currentRun.trophiesCollected >= dashCooldownCost)
        {
            TrophyManager.Instance.currentRun.trophiesCollected -= dashCooldownCost;
            PermanentStatsManager.Instance.UpgradeDash(0f, 0.2f);
            UpdateUI();
        }
    }

    // ==================== 새로 추가된 버튼 ====================
    public void ResetAllUpgrades()
    {
        if (PermanentStatsManager.Instance != null)
        {
            PermanentStatsManager.Instance.stats = new PermanentStats();
            PermanentStatsManager.Instance.SaveStats();
            UpdateUI();
            Debug.Log("모든 업그레이드가 초기화되었습니다.");
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

            Debug.Log("스테이지가 1로 초기화되었습니다.");
            CloseShop();
        }

    }
}
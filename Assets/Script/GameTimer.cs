using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI trophyCountText;
    public TextMeshProUGUI stageText;

    private float currentTime;
    private int goalTrophies = 10;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        currentTime = 60f;
        UpdateGoal();
    }

    private void Update()
    {
        // 상점이 열려있으면 시간 멈춤
        if (ShopManager.Instance != null && ShopManager.Instance.shopPanel.activeSelf)
        {
            // 시간 멈춤
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            SceneManager.LoadScene("MainMenu");
        }

        // 목표 트로피 달성 체크
        if (TrophyManager.Instance != null &&
            TrophyManager.Instance.currentRun.trophiesCollected >= goalTrophies)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.NextStage();

            currentTime = 60f;
            UpdateGoal();
        }

        UpdateUI();
    }

    private void UpdateGoal()
    {
        int stage = GameManager.Instance != null ? GameManager.Instance.currentStage : 1;
        goalTrophies = 10 + (stage - 1) * 5;
    }

    private void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";

        if (stageText != null && GameManager.Instance != null)
            stageText.text = $"Stage {GameManager.Instance.currentStage}";

        if (trophyCountText != null && TrophyManager.Instance != null)
        {
            int current = TrophyManager.Instance.currentRun.trophiesCollected;
            trophyCountText.text = $"트로피: {current} / {goalTrophies}";
        }
    }
}
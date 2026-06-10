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
    private int goalTrophies = 10;        // Stage 1 시작값

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        ResetStageTime();
        UpdateGoal();
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            SceneManager.LoadScene("MainMenu");
        }

        // 목표 트로피 달성 시 다음 스테이지
        if (TrophyManager.Instance != null &&
            TrophyManager.Instance.currentRun.trophiesCollected >= goalTrophies)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.NextStage();

            ResetStageTime();
            UpdateGoal();
        }

        UpdateUI();
    }

    private void ResetStageTime()
    {
        int stage = GameManager.Instance != null ? GameManager.Instance.currentStage : 1;
        currentTime = 30f + (stage - 1) * 5f;   // 30초 + 5초씩 증가
    }

    // 이전 목표에서 10씩 증가
    private void UpdateGoal()
    {
        int stage = GameManager.Instance != null ? GameManager.Instance.currentStage : 1;

        if (stage == 1)
            goalTrophies = 10;
        else
            goalTrophies = goalTrophies + 10;   // 이전 목표 + 10
    }

    private void UpdateUI()
    {
        // 타이머
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";

        if (stageText != null && GameManager.Instance != null)
            stageText.text = $"Stage {GameManager.Instance.currentStage}";

        // 트로피 진행도
        if (trophyCountText != null && TrophyManager.Instance != null)
        {
            int current = TrophyManager.Instance.currentRun.trophiesCollected;
            trophyCountText.text = $"트로피: {current} / {goalTrophies}";
        }
    }
}
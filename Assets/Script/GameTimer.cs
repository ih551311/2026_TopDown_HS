using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    public TextMeshProUGUI trophyCountText;
    public TextMeshProUGUI nextStageText;

    private float stageTimer = 0f;
    private float stageGoalTime = 30f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        stageTimer = 0f;
        stageGoalTime = 30f;
    }

    private void Update()
    {
        stageTimer += Time.deltaTime;

        if (stageTimer >= stageGoalTime)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.NextStage();

            stageTimer = 0f;
            stageGoalTime = 30f;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (TrophyManager.Instance != null && trophyCountText != null)
        {
            trophyCountText.text = "트로피: " + TrophyManager.Instance.currentRun.trophiesCollected;
        }

        if (nextStageText != null)
        {
            float timeLeft = stageGoalTime - stageTimer;
            if (timeLeft < 0) timeLeft = 0;
            nextStageText.text = $"다음 스테이지까지: {timeLeft:F0}초";
        }
    }

    public void AddStageGoalTime(float amount)
    {
        stageGoalTime -= amount;        // 트로피 먹으면 시간 감소
        if (stageGoalTime < 8f) stageGoalTime = 8f;
    }

    public void ReduceTime(float amount)
    {
        stageGoalTime += amount;        // 벽 충돌 시 시간 증가
        Debug.Log($"벽 충돌! +{amount}초 (현재 목표: {stageGoalTime}초)");
    }
}
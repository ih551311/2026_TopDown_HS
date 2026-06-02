using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [Header("레벨 데이터")]
    public LevelData levelData;           // ← 여기 연결 필수

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI trophyCountText;

    private float currentTime;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // LevelData의 timeLimit을 최우선으로 사용
        float startTime = 5f;   // 기본값

        if (levelData != null)
        {
            startTime = levelData.timeLimit;   // LevelData에서 시간 가져오기
            Debug.Log($"LevelData 적용됨 → 시작 시간 {startTime}초");
        }
        else
        {
            Debug.LogWarning("LevelData가 연결되지 않았습니다. 기본 5초 사용");
        }

        // TrophyManager의 시간 보너스 적용
        if (TrophyManager.Instance != null)
        {
            currentTime = TrophyManager.Instance.GetNextTimeLimit(startTime);
        }
        else
        {
            currentTime = startTime;
        }

        Debug.Log($"최종 시작 시간: {currentTime}초");
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            TimeUp();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (TrophyManager.Instance != null && trophyCountText != null)
        {
            trophyCountText.text = "트로피: " + TrophyManager.Instance.currentRun.trophiesCollected;
        }
    }

    private void TimeUp()
    {
        if (TrophyManager.Instance != null)
            TrophyManager.Instance.EndRun(5f);

        SceneManager.LoadScene("MainMenu");
    }

    public void ReduceTime(float amount)
    {
        currentTime -= amount;
        if (currentTime < 0) currentTime = 0;
    }
}   
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("설정")]
    public float totalTime = 120f;           // 2분 = 120초
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI trophyCountText;

    private float currentTime;

    private void Start()
    {
        currentTime = totalTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            TimeUp();                    // 시간 종료 시 실행
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        // 타이머 표시
        int min = Mathf.FloorToInt(currentTime / 60);
        int sec = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{min:00}:{sec:00}";

        // 현재 트로피 수 표시
        if (TrophyManager.Instance != null && trophyCountText != null)
        {
            trophyCountText.text = "트로피: " + TrophyManager.Instance.currentRun.trophiesCollected;
        }
    }

    private void TimeUp()
    {
        // 트로피 데이터 저장
        if (TrophyManager.Instance != null)
        {
            TrophyManager.Instance.EndRun(120f - currentTime);
        }

        // 메인 화면으로 자동 이동
        SceneManager.LoadScene("MainMenu");   // ← 메인 메뉴 씬 이름 확인!
    }
}
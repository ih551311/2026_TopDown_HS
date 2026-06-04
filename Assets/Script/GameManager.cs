using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Stage 설정")]
    public int currentStage = 1;
    public TextMeshProUGUI stageText;

    private bool isStageChanging = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateStageUI();
    }

    public void NextStage()
    {
        if (isStageChanging) return;
        isStageChanging = true;

        currentStage++;
        UpdateStageUI();

        Debug.Log($"→ Stage {currentStage} 시작!");

        isStageChanging = false;
    }

    private void UpdateStageUI()
    {
        if (stageText != null)
            stageText.text = $"Stage {currentStage}";
    }

    // 스테이지가 올라갈수록 벽 충돌 보너스 증가
    public float GetWallBonus()
    {
        return 5f + (currentStage - 1) * 3f;
        // Stage 1 = +5초
        // Stage 2 = +8초
        // Stage 3 = +11초
        // Stage 4 = +14초 ...
    }
}
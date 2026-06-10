using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Stage 설정")]
    public int currentStage = 1;
    public TextMeshProUGUI stageText;

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
        currentStage++;
        UpdateStageUI();
        Debug.Log($"Stage {currentStage} 시작!");
    }

    private void UpdateStageUI()
    {
        if (stageText != null)
            stageText.text = $"Stage {currentStage}";
    }
}
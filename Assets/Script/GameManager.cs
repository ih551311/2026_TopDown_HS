using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Stage 설정")]
    public int currentStage = 1;
    public TextMeshProUGUI stageText;

    [Header("스테이지 전환 이펙트")]
    public GameObject stageClearText;
    public GameObject nextStageText;
    public float effectDuration = 2.5f;

    private bool isTransitioning = false;   // 중복 호출 강력 방지

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
        if (isTransitioning) return;   // 이미 진행 중이면 무시
        StartCoroutine(StageTransition());
    }

    private IEnumerator StageTransition()
    {
        isTransitioning = true;

        // Stage Clear 표시
        if (stageClearText != null)
            stageClearText.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        currentStage++;
        UpdateStageUI();

        // Next Stage 표시
        if (stageClearText != null) stageClearText.SetActive(false);
        if (nextStageText != null)
        {
            nextStageText.GetComponent<TextMeshProUGUI>().text = $"Stage {currentStage}";
            nextStageText.SetActive(true);
        }

        yield return new WaitForSeconds(effectDuration);

        if (nextStageText != null)
            nextStageText.SetActive(false);

        isTransitioning = false;
    }

    private void UpdateStageUI()
    {
        if (stageText != null)
            stageText.text = $"Stage {currentStage}";
    }

    public float GetWallBonus()
    {
        return 5f + (currentStage - 1) * 3f;
    }
}
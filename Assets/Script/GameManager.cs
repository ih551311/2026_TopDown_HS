using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentStage = 1;
    public TextMeshProUGUI stageText;

    public GameObject stageClearText;
    public GameObject nextStageText;

    private bool isTransitioning = false;

    private void Awake()
    {
        Instance = this;
    }

    public void NextStage()
    {
        if (isTransitioning) return;
        StartCoroutine(StageTransition());
    }

    private IEnumerator StageTransition()
    {
        isTransitioning = true;

        // Stage Clear
        if (stageClearText != null)
        {
            stageClearText.SetActive(true);
            Debug.Log("StageClearText ON");
        }

        yield return new WaitForSeconds(1.5f);

        currentStage++;

        if (stageText != null)
            stageText.text = $"Stage {currentStage}";

        if (stageClearText != null) stageClearText.SetActive(false);

        // Next Stage
        if (nextStageText != null)
        {
            nextStageText.GetComponent<TextMeshProUGUI>().text = $"Stage {currentStage}";
            nextStageText.SetActive(true);
            Debug.Log("NextStageText ON");
        }

        yield return new WaitForSeconds(3f);

        if (nextStageText != null)
            nextStageText.SetActive(false);

        isTransitioning = false;
    }
}
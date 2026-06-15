using UnityEngine;

[System.Serializable]
public class RunData
{
    public int trophiesCollected = 0;     // 이번 스테이지에서 먹은 트로피
    public int totalTrophies = 0;         // 전체 누적
}

public class TrophyManager : MonoBehaviour
{
    public static TrophyManager Instance;

    public RunData currentRun = new RunData();

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

    public void CollectTrophy()
    {
        currentRun.trophiesCollected++;
        currentRun.totalTrophies++;
        Debug.Log($"트로피 획득! 현재: {currentRun.trophiesCollected}");
    }

    public void ResetForNewStage()
    {
        currentRun.trophiesCollected = 0;
        Debug.Log("새 스테이지 시작 - 트로피 초기화");
    }
}
using UnityEngine;

[System.Serializable]
public class RunData
{
    public int trophiesCollected = 0;   // 누적 (초기화 안 됨)
    public int totalTrophies = 0;
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
    }
}
using System.IO;
using UnityEngine;


public class RunData
{
    public int trophiesCollected = 0;
    public float bestTime = 0f;
    public int totalTrophies = 0;
    public int lastRunTrophies = 0;
    public float timeBonus = 0f;        // 누적 시간 보너스
}

public class TrophyManager : MonoBehaviour
{
    public static TrophyManager Instance;

    public RunData currentRun = new RunData();
    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/TrophyData.json";
            LoadData();
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
        SaveData();
    }

    public void EndRun(float playTime)
    {
        currentRun.lastRunTrophies = currentRun.trophiesCollected;

        if (playTime > currentRun.bestTime)
            currentRun.bestTime = playTime;

        // 트로피 1개당 0.5초 증가
        currentRun.timeBonus += currentRun.trophiesCollected * 0.5f;

        SaveData();

        Debug.Log($"[EndRun] 트로피 {currentRun.lastRunTrophies}개 → +{currentRun.lastRunTrophies * 0.5f}초 (총 보너스: {currentRun.timeBonus}초)");

        currentRun.trophiesCollected = 0;
    }

    public float GetNextTimeLimit(float baseTime = 5f)
    {
        float finalTime = baseTime + currentRun.timeBonus;
        Debug.Log($"[GetNextTimeLimit] 다음 판 시작: {finalTime}초");
        return finalTime;
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(currentRun, true);
        File.WriteAllText(savePath, json);
    }

    private void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            currentRun = JsonUtility.FromJson<RunData>(json);
        }
    }
}
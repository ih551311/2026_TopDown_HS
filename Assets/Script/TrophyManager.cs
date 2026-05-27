using System.IO;
using UnityEngine;

[System.Serializable]
public class RunData
{
    public int trophiesCollected = 0;
    public float bestTime = 0f;
    public int totalTrophies = 0;
    public int lastRunTrophies = 0;
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
            Debug.Log("TrophyManager Awake 완료 - 총 트로피: " + currentRun.totalTrophies);
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
        // 지난 런 트로피 저장
        currentRun.lastRunTrophies = currentRun.trophiesCollected;

        if (playTime > currentRun.bestTime)
            currentRun.bestTime = playTime;

        SaveData();

        // 새 런 시작을 위해 초기화
        currentRun.trophiesCollected = 0;
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(currentRun, true);
        File.WriteAllText(savePath, json);
        Debug.Log("JSON 저장 완료: " + savePath);
    }

    private void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            currentRun = JsonUtility.FromJson<RunData>(json);
            Debug.Log("JSON 로드 완료");
        }
        else
        {
            Debug.Log("저장된 파일이 없음. 새로 시작");
        }
    }
}
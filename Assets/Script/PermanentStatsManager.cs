using System.IO;
using UnityEngine;

[System.Serializable]
public class PermanentStats
{
    public float speedBonus = 0f;
    public float dashSpeedBonus = 0f;
    public float dashCooldownReduction = 0f;
    public int extraTrophySpawn = 0;     // ← 추가: 스폰 트로피 증가
}

public class PermanentStatsManager : MonoBehaviour
{
    public static PermanentStatsManager Instance;

    public PermanentStats stats = new PermanentStats();
    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/PermanentUpgrades.json";
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveStats()
    {
        string json = JsonUtility.ToJson(stats, true);
        File.WriteAllText(savePath, json);
    }

    private void LoadStats()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            stats = JsonUtility.FromJson<PermanentStats>(json);
        }
    }

    public void UpgradeSpeed(float amount)
    {
        stats.speedBonus += amount;
        SaveStats();
    }

    public void UpgradeDash(float speedAmount, float cooldownAmount)
    {
        stats.dashSpeedBonus += speedAmount;
        stats.dashCooldownReduction += cooldownAmount;
        SaveStats();
    }

    // 스폰 트로피 증가
    public void AddExtraTrophySpawn(int amount)
    {
        stats.extraTrophySpawn += amount;
        SaveStats();
    }
}
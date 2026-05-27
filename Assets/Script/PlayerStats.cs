using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("기본 스탯")]
    public float baseMoveSpeed = 5f;

    [Header("저장된 업그레이드")]
    private float speedBonus = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetMoveSpeed()
    {
        return baseMoveSpeed + speedBonus;
    }

    // 속도 업그레이드 (트로피 보상이나 상점에서 사용)
    public void UpgradeSpeed(float amount)
    {
        speedBonus += amount;
        PlayerPrefs.SetFloat("SpeedBonus", speedBonus);
        PlayerPrefs.Save();
        Debug.Log("속도 업그레이드: +" + amount + " (현재: " + GetMoveSpeed() + ")");
    }

    private void LoadStats()
    {
        speedBonus = PlayerPrefs.GetFloat("SpeedBonus", 0f);
    }

    // 모든 저장 데이터 초기화 (필요할 때 사용)
    public void ResetAllStats()
    {
        speedBonus = 0f;
        PlayerPrefs.DeleteKey("SpeedBonus");
        PlayerPrefs.Save();
    }
}
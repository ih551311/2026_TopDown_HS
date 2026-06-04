using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("레벨 설정")]
    public int trophyCount = 8;           // 트로피 개수
    public float timeLimit = 60f;         // 시작 시간 (초)

    [Header("플레이어 설정")]
    public float playerSpeed = 5f;        // ← 플레이어 속도 추가
}
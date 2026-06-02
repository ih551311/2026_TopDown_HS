using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("레벨 설정")]
    public int trophyCount = 8;           // 트로피 개수
    public float timeLimit = 10f;         // 시작 시간 (초) ← 여기서 관리
}
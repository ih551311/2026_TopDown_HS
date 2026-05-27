using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("기본 레벨 설정")]
    public int trophyCount = 8;        // 트로피 개수
    public float timeLimit = 120f;     // 제한 시간 (초)
}
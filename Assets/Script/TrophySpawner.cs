using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrophySpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject trophyPrefab;
    public LevelData levelData;

    [Header("트로피 생성 설정")]
    public int initialTrophyCount = 8;      // 처음 생성될 트로피 개수
    public float spawnInterval = 8f;        // 몇 초마다 새로운 트로피 생성
    public int maxTrophies = 12;            // 화면에 최대 존재할 트로피 수

    private List<GameObject> activeTrophies = new List<GameObject>();

    private void Start()
    {
        int count = levelData != null ? levelData.trophyCount : initialTrophyCount;
        SpawnInitialTrophies(count);

        // 게임 진행 중에도 주기적으로 트로피 생성
        InvokeRepeating("SpawnNewTrophy", spawnInterval, spawnInterval);
    }

    // 처음 시작할 때 여러 개 생성
    private void SpawnInitialTrophies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnTrophy();
        }
    }

    // 진행 중에 하나씩 생성
    private void SpawnNewTrophy()
    {
        if (activeTrophies.Count < maxTrophies)
        {
            SpawnTrophy();
        }
    }

    private void SpawnTrophy()
    {
        BoundsInt bounds = tilemap.cellBounds;
        List<Vector3> validPositions = new List<Vector3>();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(cell))
                {
                    validPositions.Add(tilemap.GetCellCenterWorld(cell));
                }
            }
        }

        if (validPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validPositions.Count);
            GameObject newTrophy = Instantiate(trophyPrefab, validPositions[randomIndex], Quaternion.identity);
            activeTrophies.Add(newTrophy);
        }
    }

    // 트로피가 먹혔을 때 리스트에서 제거
    public void RemoveTrophy(GameObject trophy)
    {
        if (activeTrophies.Contains(trophy))
            activeTrophies.Remove(trophy);
    }
}
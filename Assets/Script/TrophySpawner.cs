using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrophySpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject trophyPrefab;

    [Header("트로피 생성 설정")]
    public int baseTrophyCount = 8;        // 기본 스폰 수
    public float spawnInterval = 8f;       // 추가 생성 간격
    public int maxTrophies = 20;           // 최대 존재 수

    private List<GameObject> activeTrophies = new List<GameObject>();

    private void Start()
    {
        SpawnInitialTrophies();
        InvokeRepeating("SpawnNewTrophy", spawnInterval, spawnInterval);
    }

    private void SpawnInitialTrophies()
    {
        int extra = 0;
        if (PermanentStatsManager.Instance != null)
            extra = PermanentStatsManager.Instance.stats.extraTrophySpawn;

        int totalCount = baseTrophyCount + extra;

        for (int i = 0; i < totalCount; i++)
        {
            SpawnTrophy();
        }
    }

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

    public void RemoveTrophy(GameObject trophy)
    {
        if (activeTrophies.Contains(trophy))
            activeTrophies.Remove(trophy);
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrophySpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject trophyPrefab;
    public int trophyCount = 8;

    private void Start()
    {
        SpawnRandomTrophies();
    }

    private void SpawnRandomTrophies()
    {
        BoundsInt bounds = tilemap.cellBounds;
        List<Vector3> positions = new List<Vector3>();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(cell))
                {
                    positions.Add(tilemap.GetCellCenterWorld(cell));
                }
            }
        }

        for (int i = 0; i < trophyCount && positions.Count > 0; i++)
        {
            int index = Random.Range(0, positions.Count);
            Instantiate(trophyPrefab, positions[index], Quaternion.identity);
            positions.RemoveAt(index);
        }
    }
}
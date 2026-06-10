using UnityEngine;

public class Trophy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 트로피 수집
            if (TrophyManager.Instance != null)
            {
                TrophyManager.Instance.CollectTrophy();
            }

            // Spawner에서 제거 (리스트 정리)
            TrophySpawner spawner = FindAnyObjectByType<TrophySpawner>();
            if (spawner != null)
            {
                spawner.RemoveTrophy(gameObject);
            }

            // 트로피 파괴
            Destroy(gameObject);
        }
    }
}
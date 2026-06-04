using UnityEngine;

public class Trophy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (TrophyManager.Instance != null)
                TrophyManager.Instance.CollectTrophy();

            // 트로피 먹으면 다음 스테이지까지 필요한 시간 0.5초 감소
            if (GameTimer.Instance != null)
                GameTimer.Instance.AddStageGoalTime(0.5f);

            TrophySpawner spawner = FindAnyObjectByType<TrophySpawner>();
            spawner?.RemoveTrophy(gameObject);

            Destroy(gameObject);
        }
    }
}
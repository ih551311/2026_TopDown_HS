using UnityEngine;

public class Trophy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TrophyManager.Instance.CollectTrophy();

            // Spawner에게 알려서 리스트에서 제거
            TrophySpawner spawner = FindObjectOfType<TrophySpawner>();
            spawner?.RemoveTrophy(gameObject);

            Destroy(gameObject);
        }
    }
}
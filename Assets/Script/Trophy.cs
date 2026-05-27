using UnityEngine;

public class Trophy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TrophyManager.Instance.CollectTrophy();
            Destroy(gameObject);
        }
    }
}
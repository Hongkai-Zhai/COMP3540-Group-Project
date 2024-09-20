using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int healAmount = 1;
    public float lifetime = 10f; 

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            if (spawnManager != null)
            {
                spawnManager.UpdateHP(healAmount);
                Destroy(gameObject);
            }
        }
    }
}

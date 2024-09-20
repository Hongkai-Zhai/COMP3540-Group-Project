using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonArea : MonoBehaviour
{
    public GameObject prisonBuilding;
    public GameObject bossPrefab;
    public float bossSpawnDistance = 200f;

    private bool buildingDestroyed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (!buildingDestroyed && prisonBuilding != null)
                {
                    DestroyBuildingAndSpawnBoss(player.transform.position);
                }
            }
        }
    }

    private void DestroyBuildingAndSpawnBoss(Vector3 playerPosition)
    {
        
        buildingDestroyed = true;

        Vector3 bossSpawnPosition = new Vector3(-400,3,-400);

        // Éú³ÉBoss
        Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
        Destroy(prisonBuilding);
    }
}

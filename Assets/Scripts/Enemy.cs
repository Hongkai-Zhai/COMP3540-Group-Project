using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;
    public float stoppingDistance = 1f; 
    public float maxSpeed = 10f; 
    public GameObject healthDropPrefab;
    public GameObject powerupDropPrefab;
    public float dropChance = 0.1f; 
    public int maxHealth = 3;
    private int currentHealth;
    private int playerDamage ;


    private Rigidbody enemyRb;
    private GameObject player;
    private SpawnManager spawnManager;
    public bool isBoss;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player"); 
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        currentHealth = maxHealth;
        playerDamage = spawnManager.power;
    }

    void FixedUpdate() 
    {
        if (player == null) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.transform.position);

        
        if (distance > stoppingDistance)
        {
            
            Vector3 newPosition = enemyRb.position + direction * speed * Time.fixedDeltaTime;
            enemyRb.MovePosition(newPosition);

            
            if (enemyRb.velocity.magnitude > maxSpeed)
            {
                enemyRb.velocity = enemyRb.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            enemyRb.velocity = Vector3.zero;
        }

        
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            enemyRb.MoveRotation(Quaternion.Slerp(enemyRb.rotation, lookRotation, 0.2f));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spawnManager.UpdateHP(-currentHealth);
            
            if (spawnManager.HP > 0 && isBoss)
            {
                
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(playerDamage); 
            Destroy(collision.gameObject); 
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            spawnManager.UpdateScore(maxHealth);
            TryDropItem();
            if (isBoss)
            {
                spawnManager.bossKilled = true;
                spawnManager.GameOver();
            }
            Destroy(gameObject);
        }
    }


    void TryDropItem()
    {
        if (Random.value < dropChance && healthDropPrefab != null)
        {
            Instantiate(healthDropPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            if (Random.value < dropChance && powerupDropPrefab != null)
            {
                Instantiate(powerupDropPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    
}
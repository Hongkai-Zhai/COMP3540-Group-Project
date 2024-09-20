using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public float spawnInterval = 2f; 
    public int enemiesPerWave = 5; 
    public int waveNumber;
    public float timeBetweenWaves = 5f; 

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI damageText;
    public GameObject player;
    public GameObject startButton;
    public GameObject key; 
    public Button restartButton;
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI TitleText;
    public Vector2 mapSize = new Vector2(900f, 900f);
    private int enemiesSpawned = 0;
    private bool isSpawning = false;
    private int score;
    public int HP;
    public int power;
    private bool isGameActive;

    public float gameDuration = 600f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI failText;
    public TextMeshProUGUI successText;
    private float timeRemaining;

    public bool bossKilled = false;
    void Start()
    {
        
    }

    public void StartGame()
    {
        bossKilled = false;
        isGameActive = true;
        timeRemaining = gameDuration;
        waveNumber = 1;
        player.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        HPText.gameObject.SetActive(true);
        damageText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        StartCoroutine(SpawnEnemyWaves());
        StartCoroutine(CountdownTimer());
        UpdateScore(0);
        UpdateHP(3);
        UpdatePowerup(1);
        
        startButton.gameObject.SetActive(false);
        successText.gameObject.SetActive(false);
        failText.gameObject.SetActive(false);
        key.gameObject.SetActive(true);
        TitleText.gameObject.SetActive(false);

        
    }

    IEnumerator SpawnEnemyWaves()
    {
        while (isGameActive) 
        {
            yield return StartCoroutine(SpawnEnemyWave());

            while (FindObjectsOfType<Enemy>().Length > 0)
            {
                yield return null;
            }

            Debug.Log($"Wave {waveNumber} completed. Next wave in {timeBetweenWaves} seconds.");
            yield return new WaitForSeconds(timeBetweenWaves);

            waveNumber++;
            enemiesPerWave += 2; 
        }
    }

    IEnumerator SpawnEnemyWave()
    {
        isSpawning = true;
        enemiesSpawned = 0;

        while (enemiesSpawned < enemiesPerWave)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    GameObject ChooseEnemyType()
    {
        int enemyIndex = (waveNumber - 1) / 3; 
        enemyIndex = Mathf.Min(enemyIndex, enemyPrefabs.Count - 1); 
        return enemyPrefabs[enemyIndex];
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = GenerateSpawnPosition();
        GameObject enemyToSpawn = ChooseEnemyType();
        Instantiate(enemyToSpawn, spawnPos, enemyToSpawn.transform.rotation);
        Debug.Log($"Enemy spawned. Total spawned this wave: {enemiesSpawned + 1}/{enemiesPerWave}");
    }

    private Vector3 GenerateSpawnPosition()
    {
        float halfWidth = mapSize.x / 2f;
        float halfLength = mapSize.y / 2f;
        float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float randomDistance = Random.Range(170f, 190f);
        float spawnPosX = player.transform.position.x + Mathf.Cos(randomAngle) * randomDistance;
        float spawnPosZ = player.transform.position.z + Mathf.Sin(randomAngle) * randomDistance;
        float spawnPosY = 20f;
        spawnPosX = Mathf.Clamp(spawnPosX, -halfWidth, halfWidth);
        spawnPosZ = Mathf.Clamp(spawnPosZ, -halfLength, halfLength);
        return new Vector3(spawnPosX, spawnPosY, spawnPosZ);
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void UpdateHP(int HPToAdd)
    {
        HP += HPToAdd;
        HPText.text = "HP: " + HP;
        if (HP <= 0)
        {
            GameOver();
        }
    }

    public void UpdatePowerup(int PowerToAdd)
    {
        power += PowerToAdd;
        damageText.text = "Danage: " + power;
        
    }

    IEnumerator CountdownTimer()
    {
        while (timeRemaining > 0 && isGameActive)
        {
            UpdateTimerDisplay();
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        if (isGameActive)
        {
            GameOver();
        }
    }


    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver()
    {
        isGameActive = false;
        player.gameObject.SetActive(false);
        GameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        Enemy[] remainingEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in remainingEnemies)
        {
            Destroy(enemy.gameObject);
        }

        if (bossKilled)
        {
            successText.gameObject.SetActive(true);
        }
        else
        {
            failText.gameObject.SetActive(true);
        }

        StopAllCoroutines();
            

    }
}
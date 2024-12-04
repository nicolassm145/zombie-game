using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] AudioClip spawnClip;
    [SerializeField] AudioClip destroyedClip;
    AudioSource audioSource;

    public Player player; 
    [SerializeField] Enemy enemyPrefab;
    TextMeshProUGUI roundsText;

    [SerializeField] SpawnArea[] spawnAreas; // Áreas de spawn
    float time; 
    int baseZombies = 10;
    int round = 1;
    int zombiesToSpawn;
    int spawnedZombies;
    int maxEnemies;
    int currentEnemies = 0;

    void Start()
    {
        time = 0; 
        SetupRound();
        roundsText = GameObject.FindWithTag("RoundInfo").GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        
        UpdateGameInfo();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 1.5f && spawnedZombies < zombiesToSpawn && currentEnemies < maxEnemies)
        {
            SpawnEnemyInActiveArea();
            time = 0;
        }
        if (spawnedZombies == zombiesToSpawn && currentEnemies == 0)
        {
            NextRound();
        }
    }
   
    void NextRound()
    {
        round++; 
        SetupRound(); 
    }
    void SetupRound()
    {
        zombiesToSpawn = baseZombies + (round - 1) * 5; 
        maxEnemies = Mathf.RoundToInt(10 * Mathf.Log(round + 1, 2)); 
        spawnedZombies = 0;
        player.Round = round;
    }
    
    void SpawnEnemyInActiveArea()
    {
        SpawnArea activeArea = GetActiveArea();
        if (activeArea == null || activeArea.SpawnPositions.Length == 0) return;

        int spawnIndex = Random.Range(0, activeArea.SpawnPositions.Length);
        Vector3 spawnPosition = activeArea.SpawnPositions[spawnIndex].position;

        Enemy newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.SetLife(Mathf.RoundToInt(30 + 10 * Mathf.Log(round, 2)));
        currentEnemies++;
        spawnedZombies++;
        audioSource.PlayOneShot(spawnClip);
        UpdateGameInfo();
    }

    SpawnArea GetActiveArea()
    {
        float closestDistance = float.MaxValue;
        SpawnArea closestArea = null;

        foreach (SpawnArea area in spawnAreas)
        {
            float distance = Vector3.Distance(player.transform.position, area.transform.position);
            if (distance < area.ActivationRadius && distance < closestDistance)
            {
                closestDistance = distance;
                closestArea = area;
            }
        }
        return closestArea;
    }

    public void EnemyDestroyed()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
        audioSource.PlayOneShot(destroyedClip);
        UpdateGameInfo();
    }

    void UpdateGameInfo()
    {
        int zombiesRemaining = zombiesToSpawn - spawnedZombies;
        roundsText.text = $"{round}";
    }
}

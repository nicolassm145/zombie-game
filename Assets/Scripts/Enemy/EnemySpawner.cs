using System.Collections;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] AudioClip spawnClip;
    [SerializeField] AudioClip destroyedClip;
    [SerializeField] AudioClip roundChangeClip;
    AudioSource audioSource;

    public Player player; 
    [SerializeField] Enemy[] enemyPrefabs; // Array de prefabs de zumbis
    [SerializeField] float[] spawnChances; // Porcentagens para cada tipo de zumbi
    TextMeshProUGUI roundsText;

    [SerializeField] SpawnArea[] spawnAreas; // Áreas de spawn
    bool isTransitioning = false;
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
            StartCoroutine(NextRoundSequence());
            
        }
    }
   
    IEnumerator NextRoundSequence()
    {
        if (isTransitioning) yield break; // Não executar se já estiver em transição
        isTransitioning = true;
        
        // Tocar som de mudança de round
        audioSource.PlayOneShot(roundChangeClip);

        // Piscar o texto
        yield return StartCoroutine(FlashRoundText());
        
        round++;
        SetupRound();
        UpdateGameInfo();
        
        isTransitioning = false;
    }
   
    IEnumerator FlashRoundText()
    {
        Color originalColor = roundsText.color;
        for (int i = 0; i < 5; i++) 
        {
            roundsText.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            roundsText.color = Color.black;
            yield return new WaitForSeconds(0.2f);
        }
        roundsText.color = originalColor; 
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
        if (round>1)
            player.AddMoney(100*round);
    }
    
    void SpawnEnemyInActiveArea()
    {
        SpawnArea activeArea = GetActiveArea();
        if (activeArea == null || activeArea.SpawnPositions.Length == 0) return;

        int spawnIndex = Random.Range(0, activeArea.SpawnPositions.Length);
        Vector3 spawnPosition = activeArea.SpawnPositions[spawnIndex].position;
    
        Enemy enemyToSpawn = GetRandomEnemyPrefab();
        Enemy newEnemy = Instantiate(enemyToSpawn , spawnPosition, Quaternion.identity);
        newEnemy.SetLife(Mathf.RoundToInt(30 + 10 * Mathf.Log(round, 2)));
        
        currentEnemies++;
        spawnedZombies++;
        print($"Spawned Zombie: {spawnedZombies}/{zombiesToSpawn}, Current Enemies: {currentEnemies}/{maxEnemies}");
        audioSource.PlayOneShot(spawnClip);
        UpdateGameInfo();
    }
    
    Enemy GetRandomEnemyPrefab()
    {
        float randomValue = Random.Range(0f, 100f);
        float cumulativeChance = 0f;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            cumulativeChance += spawnChances[i];
            if (randomValue <= cumulativeChance)
            {
                return enemyPrefabs[i];
            }
        }

        return enemyPrefabs[0]; // Caso nenhuma condição seja atendida, retorna o zumbi padrão.
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
        print($"Enemy destroyed. Current enemies: {currentEnemies}");
        audioSource.PlayOneShot(destroyedClip);
        UpdateGameInfo();
    }

    void UpdateGameInfo()
    {
        int zombiesRemaining = zombiesToSpawn - spawnedZombies;
        roundsText.text = $"{round}";
    }
}

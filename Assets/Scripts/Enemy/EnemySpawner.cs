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
    
    public GameObject[] pos; 
    public GameObject enemy; 
    
    float time; 
    int baseZombies = 10;
    int round = 1;
    int zombiesToSpawn;     // Zumbis que vão spawnar
    int spawnedZombies;     // Limite do round
    int maxEnemies;         // Limite max. de inimigos ativos
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
            int spawn = Random.Range(0, pos.Length); 
            //GameObject newEnemy = Instantiate(enemy, pos[spawn].transform.position, Quaternion.identity);
            //Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            //enemyScript.SetLife(Mathf.RoundToInt(30 + 10 * Mathf.Log(round, 2))); 
            Enemy newEnemy = Instantiate(enemyPrefab, pos[spawn].transform.position, Quaternion.identity);
            newEnemy.SetLife(Mathf.RoundToInt(30 + 10 * Mathf.Log(round, 2)));
            currentEnemies++; 
            spawnedZombies++; 
            time = 0; 
            audioSource.PlayOneShot(spawnClip);
            UpdateGameInfo();
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
        //Player player = FindObjectOfType<Player>();
        player.Round = round;
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


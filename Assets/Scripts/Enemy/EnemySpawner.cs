using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] pos; // Posições para spawn
    public GameObject enemy; // Prefab do inimigo
    TextMeshProUGUI roundsText; 
    
    private float time; 
    private int baseZombies = 10;
    private int round = 1;
    private int zombiesToSpawn; // Zumbis que vão spawnar
    private int spawnedZombies; // Limite do round
    public int maxEnemies; // Limite máximo de inimigos ativos na cena
    private int currentEnemies = 0; 

    void Start()
    {
        time = 0; 
        SetupRound();
        roundsText = GameObject.FindWithTag("RoundInfo").GetComponent<TextMeshProUGUI>();
        UpdateGameInfo();
    }

   void Update()
    {
        time += Time.deltaTime;

      
        if (time >= 1.5f && spawnedZombies < zombiesToSpawn && currentEnemies < maxEnemies)
        {
            int x = Random.Range(0, pos.Length); 
            GameObject newEnemy = Instantiate(enemy, pos[x].transform.position, Quaternion.identity);
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            if (round == 1)
            {
                enemyScript.SetLife(30); 
            }
            else
            {
                enemyScript.SetLife(Mathf.RoundToInt(30 + 10 * Mathf.Log(round, 2)));  
            }
            

            currentEnemies++; 
            spawnedZombies++; 
            time = 0; 
            UpdateGameInfo();
        }
        if (spawnedZombies == zombiesToSpawn && currentEnemies == 0)
        {
            NextRound();
        }
    }
    void SetupRound()
    {
        zombiesToSpawn = baseZombies + (round - 1) * 5; // Número de zumbis cresce linearmente
        maxEnemies = Mathf.RoundToInt(10 * Mathf.Log(round + 1, 2)); // Limite de zumbis vivos cresce logaritmicamente
        spawnedZombies = 0; // Reseta o contador de spawnados
        print($"Round {round} iniciado! Zombies para spawnar: {zombiesToSpawn}, Máximo vivos: {maxEnemies}");
    }
    
    private void NextRound()
    {
        round++; // Incrementa o número do round
        SetupRound(); // Configura os parâmetros para o próximo round
    }
    
    public void EnemyDestroyed()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
        UpdateGameInfo();
    }
    private void UpdateGameInfo()
    {
        // Atualiza o texto com as informações do round, spawnados e restantes
        int zombiesRemaining = zombiesToSpawn - spawnedZombies;
        roundsText.text = $"Round: {round}\n";
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] pos; // Posições para spawn
    public GameObject enemy; // Prefab do inimigo
    private float tempo; // Controle do tempo entre spawns
    public int maxEnemies = 10; // Limite máximo de inimigos ativos na cena
    private int currentEnemies = 0; // Contador de inimigos ativos

    void Start()
    {
        tempo = 0; // Inicializa o tempo
    }

    void Update()
    {
        tempo += Time.deltaTime;

        // Verifica o tempo e o limite de inimigos ativos
        if (tempo >= 1.5f && currentEnemies < maxEnemies)
        {
            int x = Random.Range(0, pos.Length); // Seleciona uma posição aleatória
            Instantiate(enemy, pos[x].transform.position, Quaternion.identity);

            currentEnemies++; // Incrementa o número de inimigos ativos
            tempo = 0; // Reseta o tempo
        }
    }

    public void EnemyDestroyed()
    {
        // Diminui o número de inimigos ativos, garantindo que não fique negativo
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
        Debug.Log($"Inimigo destruído. Total de inimigos ativos: {currentEnemies}");
    }
}
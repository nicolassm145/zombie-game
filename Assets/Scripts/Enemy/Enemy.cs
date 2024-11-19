using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player; 
    
    [SerializeField] int life; 
    private NavMeshAgent agent; 

    void Start()
    { 
        player = GameObject.Find("Player"); 
        agent = GetComponent<NavMeshAgent>(); 
        
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
        
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            life -= 10; // Reduz a vida do inimigo
            Destroy(collision.gameObject); // Destroi o projétil
        }

        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1); // Causa 1 de dano ao jogador
            }
        }
    }

    
    void OnDestroy()
    {
        
        GameObject spawner = GameObject.Find("Spawner");
        if (spawner != null)
        {
            spawner.GetComponent<EnemySpawner>().EnemyDestroyed();
        }
    }
}
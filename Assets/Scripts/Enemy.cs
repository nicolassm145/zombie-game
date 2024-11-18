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
            Debug.Log("Inimigo destruído!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.CompareTag("bullet"))
        {
            life -= 10; // Reduz a vida
            Destroy(collision.gameObject); // Destroi o projétil
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
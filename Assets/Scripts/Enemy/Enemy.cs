using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Biblioteca necessária para o NavMesh

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent; // Componente NavMeshAgent
    private int life;
    
    void Start()
    {
        life = 30;
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();

        // Configurações para 2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
     
        agent.SetDestination(player.transform.position);
       

        
        if (life <= 0)
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            life = life - 10;
            Destroy(collision.gameObject);
        }
    }
}
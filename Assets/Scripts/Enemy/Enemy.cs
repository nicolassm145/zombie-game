using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    AudioSource _audioSource; 
    [SerializeField] AudioClip zombieHitSound;
    
    NavMeshAgent agent; 
    GameObject player; 
    [SerializeField] int life; 
    
    SpriteRenderer spriteRenderer;
    Color originalColor;
    
    [SerializeField] float damageCooldown = 0.5f;
    float lastDamageTime;
    
    [SerializeField] GameObject poofVFX;
        
    void Start()
    { 
        player = GameObject.Find("Player"); 
        agent = GetComponent<NavMeshAgent>(); 
        _audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
        if (life <= 0)
        {
            Destroy(gameObject);
            Die();
        }
    }
    
    public void SetLife(int newLife)
    {
        life = newLife;
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.deltaTime - lastDamageTime >= damageCooldown)
            {
                Player playerScript = collision.GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.TakeDamage(1); 
                    lastDamageTime = Time.deltaTime; 
                }
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            //_audioSource.PlayOneShot(zombieHitSound);
            life -= 10; 
            Destroy(collision.gameObject);
            StartCoroutine(DamageZombie(0.1f));
        }

        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1); 
            }
        }
    }
    IEnumerator DamageZombie(float duration)
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }
    
    void OnDestroy()
    {
        GameObject spawner = GameObject.Find("Spawner");
        if (spawner != null)
            spawner.GetComponent<EnemySpawner>().EnemyDestroyed();

        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.ZombieKilled();
                int moneyReward = Random.Range(5, 30); 
                playerScript.AddMoney(moneyReward);
            }
        }
    }
    
    void Die()
    {
        GameObject poofGO = Instantiate(poofVFX, transform.position, Quaternion.identity);
        Destroy(poofGO, 1.0f); 
        Destroy(gameObject, 0.1f);
    }
}
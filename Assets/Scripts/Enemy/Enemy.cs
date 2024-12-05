using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject prefab; // O prefab do item
        public float dropChance;  // Probabilidade de drop (em percentual, ex.: 5% = 0.05)
        public int minValue;      // Valor mínimo (ex.: dinheiro ou quantidade de munição)
        public int maxValue;      // Valor máximo
    }
    
    [SerializeField] List<DropItem> possibleDrops; // Lista de itens possíveis

    AudioSource _audioSource; 
    [SerializeField] AudioClip zombieHitSound;
    
    //[SerializeField] GameObject moneyDropPrefab;
    //[SerializeField] GameObject ammoDropPrefab;

    
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
               // int moneyReward = Random.Range(5, 30); 
                //playerScript.AddMoney(moneyReward);
            }
        }
    }
    
    void Die()
    {
        // Cria o efeito visual de morte
        GameObject poofGO = Instantiate(poofVFX, transform.position, Quaternion.identity);
        Destroy(poofGO, 1.0f);

        // Itera sobre os possíveis drops
        foreach (DropItem drop in possibleDrops)
        {
            if (Random.value <= drop.dropChance) // Verifica se o item deve ser droppado
            {
                GameObject itemDrop = Instantiate(drop.prefab, transform.position, Quaternion.identity);

                // Configura o valor do item, se aplicável
                ItemDrop pickup = itemDrop.GetComponent<ItemDrop>();
                if (pickup != null)
                {
                    pickup.value = Random.Range(drop.minValue, drop.maxValue + 1); // Define o valor/quantidade
                }
            }
        }

        // Remove o zumbi do jogo
        Destroy(gameObject);
    }



}
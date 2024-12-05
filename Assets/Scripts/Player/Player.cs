using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // UI
    GameManager gameManager;
    TextMeshProUGUI _moneyText;
    [SerializeField] GameObject heartPrefab; 
    [SerializeField] GameObject healthPanel;
    
    // Player
    Animator _playerAnimator;
    Rigidbody2D _playerRb;
    Vector2 _movement;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    
    // Caracteristicas do player
    [SerializeField] float movespeed;
    [SerializeField] float runSpeed;
    [SerializeField] int money = 500;
    [SerializeField] int health = 5; 
    [SerializeField] float invincibilityDuration = 0.5f;
    
    // Infos
    [SerializeField] LayerMask solidObjectsLayer;  // Colisão
    int zombiesKilled = 0;
    float timeAlive = 0f;
    public int Round { get; set; } = 1;
    private bool _isRunning;
    bool isInvincible = false;
    public event Action OnInteractAction; 
    float timeSinceLastDamage = 0f;
    Coroutine regenerationCoroutine = null;
    [SerializeField] float regenerationTimer = 10f;
    [SerializeField] float regenerationDuration = 2f;

    // Logica da arma
    public bool HasWeapon { get; set; } = false;
    public Weapon Weapon { get; set; } = null;
    public GameObject spawnerBulletPos;
    
    // Sons do Player
    AudioSource _audioSource; 
    [SerializeField] AudioClip footStepClip;
    [SerializeField] AudioClip damageClip;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        _audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _moneyText = GameObject.FindWithTag("MoneyUI").GetComponent<TextMeshProUGUI>();
        UpdateHealthUI();
    }

    void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }
    
    void MovePlayer()
    {
        float currentSpeed = _isRunning ? runSpeed : movespeed; // Aplica a velocidade correta dependendo se estiver correndo
        Vector3 targetPosition = _playerRb.position + _movement * (currentSpeed * Time.fixedDeltaTime);
        
        if (IsWalkable(targetPosition))
            _playerRb.MovePosition(targetPosition); 

        bool isWalking = _movement.magnitude > Mathf.Epsilon;
        float speed = _isRunning ? 1 : 0.5f;    // Velocidade da animação aumenta se estiver correndo
        
        _playerAnimator.SetBool("isWalking", isWalking);
        _playerAnimator.SetFloat("Horizontal", _movement.x);
        _playerAnimator.SetFloat("Vertical", _movement.y);
        _playerAnimator.SetFloat("Speed", speed);

        // Para de correr se o player parar de andar
        _isRunning = isWalking && _isRunning;
        
        // Toca o som de passo se estiver andando
        if (isWalking && !_audioSource.isPlaying)
        {
            _audioSource.pitch = _isRunning ? 1.2f : 1f; // Aumenta o pitch ao correr, diminui ao andar
            _audioSource.PlayOneShot(footStepClip);       // Reproduz o som do passo
        }
    }
    
    bool IsWalkable(Vector3 targetPosition)
    {
        return Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsLayer) is null;
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }
    
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;
        _audioSource.PlayOneShot(damageClip);
        
        StartCoroutine(DamagePlayer(0.125f));
        health -= amount; 
        UpdateHealthUI();
        
        if (regenerationCoroutine != null)
        {
            StopCoroutine(regenerationCoroutine);
            regenerationCoroutine = null; // Libera a referência ao Coroutine
        }
        
        timeSinceLastDamage = 0f;
        
        if (health <= 0)
        {
            PlayerPrefs.SetFloat("TimeAlive", timeAlive);
            PlayerPrefs.SetInt("ZombiesKilled", zombiesKilled);
            PlayerPrefs.SetInt("RoundReached", Round);
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }
    IEnumerator DamagePlayer(float duration)
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }
    public void ZombieKilled()
    {
        zombiesKilled++;
    }
    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true; 
        float elapsed = 0f;
       
        while (elapsed < invincibilityDuration)
        {
            yield return new WaitForSeconds(0.2f); 
            elapsed += 0.1f;
        }
        isInvincible = false; 
    }

    void UpdateHealthUI()
    {
        foreach (Transform child in healthPanel.transform)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < health; i++)
        {
            Instantiate(heartPrefab, healthPanel.transform);
        }
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount) return false;
        
        money -= amount;
        UpdateMoneyUI();
        return true;
    }

    void UpdateMoneyUI()
    {
        _moneyText.text = money.ToString();
    }

    void OnRun(InputValue value)
    {
        _isRunning = !_isRunning;

        if (HasWeapon) Weapon.IsReloading = !_isRunning && Weapon.IsReloading; // Cancela o reload
    }
    
    void OnFire(InputValue value)
    {
        if (!HasWeapon) return;
        if (Time.timeScale == 0) return;
        _isRunning = false;

        Weapon.Fire();
    }

    void OnInteract(InputValue value)
    {
        OnInteractAction?.Invoke();
    }

    void OnReload(InputValue value)
    {
        if (!HasWeapon) return;
        if (Time.timeScale == 0) return;
        _isRunning = false; // Cancela a corrida quando carregar

        StartCoroutine(Weapon.IEReload());
    }

    void OnPause()
    {
        gameManager.OnPause();
    }
    IEnumerator RegenerateHealth()
    {
        while (health < 5) // Regenera até o máximo de saúde
        {
            health++; // Incrementa 1 ponto de vida
            UpdateHealthUI();
            yield return new WaitForSeconds(regenerationDuration); 
        }

        regenerationCoroutine = null; // Libera a referência ao Coroutine ao terminar
    }


    void Update()
    {
        MovePlayer();
        timeAlive += Time.deltaTime;

        if (health < 5) // Só tenta regenerar se a saúde não estiver completa
        {
            timeSinceLastDamage += Time.deltaTime;

            if (timeSinceLastDamage >= regenerationTimer && regenerationCoroutine == null && !isInvincible)
            {
                regenerationCoroutine = StartCoroutine(RegenerateHealth());
            }
        }
    }

}
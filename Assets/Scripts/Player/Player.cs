using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    GameManager gameManager;
    Animator _playerAnimator;
    Rigidbody2D _playerRb;
    Vector2 _movement;
    TextMeshProUGUI _moneyText;
    TextMeshProUGUI healthText; 
    int zombiesKilled = 0;
     float timeAlive = 0f;
    
    [SerializeField] GameObject heartPrefab; 
    [SerializeField] GameObject healthPanel;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    public bool HasWeapon { get; set; } = false;
    public Pistol Weapon { get; set; } = null;
    public GameObject spawnerBulletPos;
    
    [SerializeField] LayerMask solidObjectsLayer;

    [SerializeField] float movespeed;

    [SerializeField] int money = 500;
    [SerializeField] int health = 5; 
    [SerializeField] float invincibilityDuration = 2f; 
    bool isInvincible = false;
    
    public event Action OnInteractAction; 
  
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        
        _moneyText = GameObject.FindWithTag("MoneyUI").GetComponent<TextMeshProUGUI>();
        healthText = GameObject.FindWithTag("LifeUI").GetComponent<TextMeshProUGUI>();
        UpdateHealthUI();
    }

    void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }
    
    void MovePlayer()
    {
        Vector3 targetPosition = _playerRb.position + _movement * (movespeed * Time.fixedDeltaTime);
        
        if (IsWalkable(targetPosition))
        {
            _playerRb.MovePosition(targetPosition); 
        }

        bool isWalking = _movement.magnitude > Mathf.Epsilon;
        _playerAnimator.SetBool("isWalking", isWalking);

        _playerAnimator.SetFloat("Horizontal", _movement.x);
        _playerAnimator.SetFloat("Vertical", _movement.y);
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }
    
    bool IsWalkable(Vector3 targetPosition)
    {
        return Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsLayer) is null;
    }
    
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        StartCoroutine(DamagePlayer(0.1f));
        health -= amount; 
        UpdateHealthUI();

        if (health <= 0)
        {
            PlayerPrefs.SetFloat("TimeAlive", timeAlive);
            PlayerPrefs.SetInt("ZombiesKilled", zombiesKilled);
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
    
    private void OnFire(InputValue value)
    {
        if (!HasWeapon) return;

        Weapon.Fire();
    }

    void OnInteract(InputValue value)
    {
        OnInteractAction?.Invoke();
    }

    void OnReload(InputValue value)
    {
        if (!HasWeapon) return;
        
        Weapon.Reload();
    }

    void OnPause()
    {
        gameManager.OnPause();
    }
    void Update()
    {
        MovePlayer();
        timeAlive += Time.deltaTime;
    }
}
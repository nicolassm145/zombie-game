using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Animator _playerAnimator;
    Rigidbody2D _playerRb;
    Vector2 _movement;
    TextMeshProUGUI _moneyText;
    TextMeshProUGUI healthText; 
    
    [SerializeField] GameObject heartPrefab; // Prefab do ícone de coração
    [SerializeField] GameObject healthPanel; // Painel onde os corações serão exibidos

    public bool HasWeapon { get; set; } = false;
    public Pistol Weapon { get; set; } = null;
    public GameObject spawnerBulletPos;
    
    [SerializeField] LayerMask solidObjectsLayer;

    [SerializeField] float movespeed;

    [SerializeField] int money = 500;
    [SerializeField] int health = 5; 
    [SerializeField] float invincibilityDuration = 2f; 
    private bool isInvincible = false;

    public event Action OnInteractAction;
    
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

    bool IsWalkable(Vector3 targetPosition)
    {
        return Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsLayer) is null;
    }
    
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;
        
        health -= amount; 
        UpdateHealthUI();

        if (health <= 0)
        {
            Debug.Log("Jogador morreu!");
            SceneManager.LoadScene("MenuPrincipal");
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }
    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true; 
        
        float elapsed = 0f;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; 
            yield return new WaitForSeconds(0.2f); 
            elapsed += 0.1f;
        }
        spriteRenderer.enabled = true; 

        isInvincible = false; 
    }

    void UpdateHealthUI()
    {
        // Primeiro, limpe o painel de corações
        foreach (Transform child in healthPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Adicione corações de acordo com a vida atual
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
    
    void Update()
    {
        MovePlayer();
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    Animator _playerAnimator;
    Rigidbody2D _playerRb;
    Vector2 _movement;
    TextMeshProUGUI _moneyText;
    
    public bool HasWeapon { get; set; } = false;
    
    [SerializeField]
    LayerMask solidObjectsLayer;

    [SerializeField]
    float movespeed;

    [SerializeField]
    int money = 500;

    void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        
        _moneyText = GameObject.FindWithTag("MoneyUI").GetComponent<TextMeshProUGUI>();
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

    public bool SpendMoney(int amount)
    {
        if (money < amount) return false;
        
        money -= amount;
        UpdateMoneyUI();
        return true;
    }

    void UpdateMoneyUI()
    {
        _moneyText.text = "Dinheiro: " + money.ToString();
    }

    public GameObject bullet, spawnerBulletPos;
    void Update()
    {
        
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));

      
        Vector2 direction = new Vector2(mousePos.x - spawnerBulletPos.transform.position.x, mousePos.y - spawnerBulletPos.transform.position.y);
        direction.Normalize();

        if (Input.GetMouseButtonDown(0))
        {
            
            GameObject newBullet = Instantiate(bullet, spawnerBulletPos.transform.position, Quaternion.identity);
            
            newBullet.transform.up = direction;
        }
        MovePlayer();
    }
}
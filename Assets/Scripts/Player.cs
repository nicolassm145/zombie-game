using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Animator _playerAnimator;
    Rigidbody2D _playerRb;
    Vector2 _movement;
    
    [SerializeField]
    LayerMask solidObjectsLayer;

    [SerializeField]
    float movespeed;

    void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        Vector3 targetPosition = _playerRb.position + _movement * (movespeed * Time.deltaTime);
        
        if (IsWalkable(targetPosition))
        {
            _playerRb.velocity = _movement * movespeed;
        }
        else
        {
            _playerRb.velocity = Vector2.zero;
        }

        bool isWalking = Mathf.Abs(_playerRb.velocity.x) > Mathf.Epsilon || Mathf.Abs(_playerRb.velocity.y) > Mathf.Epsilon;
        _playerAnimator.SetBool("isWalking", isWalking);
        
        _playerAnimator.SetFloat("Horizontal", _movement.x);
        _playerAnimator.SetFloat("Vertical", _movement.y);
    }

    bool IsWalkable(Vector3 targetPosition)
    {
        return Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsLayer) is null;
    }
    
    void Update()
    {
        MovePlayer();
    }
}
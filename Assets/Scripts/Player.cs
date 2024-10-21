using UnityEngine;
using UnityEngine.InputSystem; 

public class Player : MonoBehaviour
{
    Animator _playerAnimator;
    Rigidbody2D _playerRb;
    Vector2 _movement;

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
        _playerRb.velocity = _movement * movespeed;
        
        bool isRunning = Mathf.Abs(_playerRb.velocity.x) > Mathf.Epsilon;
        _playerAnimator.SetBool("isRunning", isRunning);
        if (isRunning)
        {
            Flip();
        }
    }

    void Flip()
    {
        transform.localScale = new Vector3(Mathf.Sign(_playerRb.velocity.x),1,1);
    }
    void Update()
    {
        MovePlayer();
    }
}
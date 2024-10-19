using UnityEngine;
using UnityEngine.InputSystem; 

public class Player : MonoBehaviour
{
    Rigidbody2D _playerRb;
    Vector2 _movement;

    [SerializeField]
    float movespeed;

    void Awake()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }


    public void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        _playerRb.velocity = _movement * movespeed;
    }

    void Update()
    {
        MovePlayer();
    }
}
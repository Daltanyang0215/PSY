using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private bool _spriteDefaultRight;

    

    private Rigidbody2D _rb;
    private SpriteRenderer _playerSprite;
    private Vector2 _moveVec;
    private float _inputX;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        if (_inputX == 0)
        {
            _moveVec = Vector2.zero;
        }
        else
        {
            if(_inputX > 0)
            {
                _playerSprite.flipX = !_spriteDefaultRight;
            }
            else
            {
                _playerSprite.flipX = _spriteDefaultRight;

            }
            _moveVec = Vector2.right * _inputX;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveVec * _moveSpeed + Vector2.up * _rb.velocity.y;
    }
}

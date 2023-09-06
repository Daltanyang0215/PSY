using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _playerSprite;
    private Vector2 _moveVec;
    private float _inputX;

    private bool _isGround;
    private RaycastHit2D _rayHit;
    private Vector2 _rayBoxSize;

    private Vector2 _externalVec;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _rayBoxSize = GetComponent<BoxCollider2D>().size * 0.95f;
        _rayBoxSize.y = 0.1f;
    }

    private void Update()
    {
        _isGround = CheckGround();

        if (PlayerState.Instance.IsNotMove) return;
        PlayerMove();
        PlayerJump();
    }

    private void FixedUpdate()
    {
        if (PlayerState.Instance.IsNotMove) _moveVec = Vector2.zero;
        _rb.velocity = _moveVec * PlayerState.Instance.MoveSpeed + Vector2.up * _rb.velocity.y + _externalVec;
        _externalVec = Vector2.Lerp(_externalVec, Vector2.zero, Time.fixedDeltaTime * 3);
    }

    private bool CheckGround()
    {
        return Physics2D.OverlapBox(transform.position, _rayBoxSize, 0, PlayerState.Instance.GroundLayer);
    }

    private void PlayerMove()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        if (_inputX == 0)
        {
            _moveVec = Vector2.zero;
        }
        else
        {
            if (_inputX > 0)
            {
                _playerSprite.flipX = !PlayerState.Instance.SpriteDefaultRight;
            }
            else
            {
                _playerSprite.flipX = PlayerState.Instance.SpriteDefaultRight;

            }
            _moveVec = Vector2.right * _inputX;
        }
    }

    private void PlayerJump()
    {
        if (!_isGround) return;
        if (Input.GetButtonDown("Jump"))
        {
            _rb.AddForce(Vector2.up * PlayerState.Instance.JumpPower, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _rayBoxSize);
    }

    public void PlayerAddVelocity(Vector2 vector)
    {
        _externalVec = vector;
    }
}

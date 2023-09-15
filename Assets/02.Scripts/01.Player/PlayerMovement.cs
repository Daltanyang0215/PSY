using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _playerSprite;
    private Vector2 _moveVec;
    private float _inputX;

    private bool _islongJump;
    private bool _canJump;

    private bool _isGround;
    private RaycastHit2D _rayHit;
    private Vector2 _rayBoxSize;

    private Vector2 _externalVec;

    private float _dashTimer;

    private Collider2D _platform;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _rayBoxSize = GetComponent<BoxCollider2D>().size * 0.95f;
        _rayBoxSize.y = 0.1f;
        _dashTimer = PlayerState.Instance.DashDelay;
        _canJump = true;
    }

    private void Update()
    {
        _isGround = CheckGround();

        if (PlayerState.Instance.IsNotMove) return;
        PlayerMove();
        PlayerDash();
        PlayerJump();
        PlayerDownJump();
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
    private void PlayerDash()
    {
        _dashTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Dash") && _dashTimer < 0)
        {
            _inputX = Input.GetAxisRaw("Horizontal");
            if (_inputX != 0)
            {
                //_rb.MovePosition(transform.position + Vector3.right * _inputX * PlayerState.Instance.DashRange);
                _externalVec += Vector2.right * _inputX * PlayerState.Instance.DashRange + Vector2.up*0.2f;
                _dashTimer = PlayerState.Instance.DashDelay;
            }
        }
    }
    private void PlayerJump()
    {
        // 점프력 조절을 중력으로 함 낙하하기 전에 점프를 떼면 중력을 높여 빨리 떨어지게 함
        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0f)
        {
            _rb.gravityScale = 6;
        }
        if(_rb.velocity.y < 0.1f)
        {
            _rb.gravityScale = 2;
        }

        if (_isGround) _rb.gravityScale = 2;
        else return;

        if (Input.GetButtonDown("Jump") && _canJump)
        {
            _islongJump = true;
            _rb.gravityScale = 2;
            _rb.AddForce(Vector2.up * PlayerState.Instance.JumpPower, ForceMode2D.Impulse);
        }
    }
    private void PlayerDownJump()
    {
        if (!_isGround) return;
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            _platform = Physics2D.OverlapBox(transform.position, _rayBoxSize, 0, 1 << LayerMask.NameToLayer("Platform"));
            if (_platform != null)
            {
                Physics2D.IgnoreCollision(PlayerState.Instance.GetComponent<Collider2D>(), _platform, true);
                _rb.AddForce(Vector2.down * PlayerState.Instance.JumpPower * 0.1f, ForceMode2D.Impulse);
                _canJump = false;
                Invoke("IgnoreCancle", 0.5f);
            }
        }
    }
    private void IgnoreCancle()
    {
        if (_platform == null) return;
        _canJump = true;
        Physics2D.IgnoreCollision(PlayerState.Instance.GetComponent<Collider2D>(), _platform, false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : KinesisObjectBase
{
    private Rigidbody2D _rb;
    [SerializeField] private Vector2 _moveVec;
    [SerializeField] private int damage;

    private void Awake()
    {
        if (!TryGetComponent(out _rb))
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
        }

        SetPSYForce(_moveVec);
    }

    private void Update()
    {
        if (PSYPranet != null)
        {
            _rb.MovePosition(Vector2.Lerp(transform.position, PSYPranet.position + Vector3.up, .5f));
        }
    }

    public override void SetPSYForce(Vector2 vector)
    {
        //MoveVec += vector;
        _rb.drag = 0;
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.zero;
        transform.right = vector;
        _rb.AddForce(vector, ForceMode2D.Impulse);
    }
    public override void AddPSYForce(Vector2 vector)
    {
        _rb.drag = 0;
        _rb.gravityScale = 0.5f;
        transform.right = transform.right + (Vector3)vector;
        _rb.AddForce(vector, ForceMode2D.Impulse);
    }
    public override void StopPSYForce(bool notGravite = false)
    {
        //MoveVec = Vector2.zero;
        if (PSYLevel == 0 || PSYLevel <= PlayerState.Instance.PsyLevel)
        {
            _rb.velocity = Vector2.zero;

        }
        else
        {
            _rb.drag = 2/(PSYLevel- PlayerState.Instance.PsyLevel);
            if (_rb.velocity == Vector2.zero)
            {
                _rb.drag = 0;
            }
        }

        //_rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.zero, PSYLevel == 0 ? 1 : 1 / Time.deltaTime);// (PlayerState.Instance.PsyLevel / (float)PSYLevel)

        if (notGravite)
        {
            _rb.gravityScale = 0;
        }
        else
        {
            _rb.drag = 0;
            _rb.gravityScale = 1;
        }
    }
    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
            if (PSYPranet == null
                && collision.TryGetComponent(out IHitAction hit)
                && hit.Order != Order)
            {
                hit.OnHit(damage);
                Destroy(gameObject);
            }
        }
    }

    
}

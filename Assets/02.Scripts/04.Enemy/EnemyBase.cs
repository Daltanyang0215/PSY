using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : KinesisObject, IHitAction
{
    [SerializeField] protected EnemyStateData state;

    [SerializeField] protected int _curHP;

    [SerializeField] protected ParticleSystem _destroyParticle;

    private Vector2 _initPosition;
    private Quaternion _initrotate;
    public bool IsLive { get; private set; }

    private GameObject _renderer;
    private Collider2D _collider;
    private float _initGravity;

    private void Awake()
    {
        _initPosition = transform.position;
        _initrotate = transform.rotation;

        IsLive = true;

        SetOrder(OrderType.Enemy);

        _curHP = state.HP;
        _renderer = transform.GetComponentInChildren<SpriteRenderer>().gameObject;
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _initGravity = _rb.gravityScale;
    }

    protected virtual void Start()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Update() { }

    protected virtual void OnEnable()
    {
        transform.SetPositionAndRotation(_initPosition, _initrotate);
        IsLive = true;
        SetOrder(OrderType.Enemy);
        _curHP = state.HP;
        _renderer.SetActive(true);
        _collider.enabled = true;
    }

    public virtual void OnHit(int damage)
    {
        if (damage > 0)
        {
            _curHP -= damage;

            if (_curHP <= 0)
            {
                OnDie();
            }
        }
    }

    protected virtual void OnDie()
    {
        Debug.Log(gameObject.name + " EnemyDie");
        _renderer.SetActive(false);
        _collider.enabled = false;
        IsLive = false;
        _rb.velocity = Vector3.zero;

        if (state.DoDieExplosion)
        {
            if (_destroyParticle != null)
                Instantiate(_destroyParticle, transform.position, transform.rotation);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, state.ExplodeRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out IHitAction hit))
                {
                    hit.OnHit(state.ExplodeDamage);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHitAction>().OnHit(state.CollisionDamage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<IHitAction>().OnHit(state.CollisionDamage);
        }
        else
        {
            Debug.Log(gameObject.name + " : Damage " + (int)(GetVelocity.magnitude));
            OnHit((int)(GetVelocity.magnitude));
        }
    }

    public override void AddPSYForce(Vector2 vector, ForceMode2D mode = ForceMode2D.Impulse)
    {
        _rb.drag = 0;
        _rb.gravityScale = _initGravity;
        //transform.right = transform.right + (Vector3)vector;
        _rb.AddForce(vector, mode);
    }
    public override void PSYCancle(bool notGravite = false)
    {
        if (notGravite)
        {
            _rb.gravityScale = 0;
        }
        else
        {
            _rb.drag = 0;
            _rb.gravityScale = _initGravity;
        }
    }
}

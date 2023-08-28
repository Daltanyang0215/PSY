using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour, Ikinesis
{
    private Rigidbody2D _rb;
    [SerializeField] private Vector2 _moveVec;

    [SerializeField] private int _psyLevel;
    public int PSYLevel => _psyLevel;

    [SerializeField] private Transform _psyPranet;
    public Transform PSYPranet
    {
        get { return _psyPranet; }
    }

    public OrderType Order { get; private set; }

    public Transform Transform => transform;

    [SerializeField] private int damage;


    private void Start()
    {
        if (!TryGetComponent(out _rb))
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
        }

        _rb.AddForce(_moveVec, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (PSYPranet != null)
        {
            _rb.MovePosition(Vector2.Lerp(transform.position, PSYPranet.position + Vector3.up, .5f));
        }
    }

    public void AddPSYForce(Vector2 vector)
    {
        //MoveVec += vector;

        _rb.AddForce(vector, ForceMode2D.Impulse);
        Order = OrderType.Player;
    }
    public void StopPSYForce(bool notGravite = false)
    {
        //MoveVec = Vector2.zero;
        _rb.velocity = Vector2.zero;
        if (!notGravite)
        {
            _rb.gravityScale = 1;
        }
    }
    public void SetPSYPranet(Transform pranet)
    {
        _psyPranet = pranet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
            if (_psyPranet == null
                && collision.TryGetComponent(out IHitAction hit)
                && hit.Order != Order)
            {
                hit.OnHit(damage);
                Destroy(gameObject);
            }
        }
    }


}

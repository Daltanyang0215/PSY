using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour, Ikinesis
{
    private Rigidbody2D _rb;
    [SerializeField] private Vector2 _moveVec;

    public Vector2 MoveVec
    {
        get { return _moveVec; }
        set { _moveVec = value; }
    }
    [SerializeField] private float _mass;
    public float Mass
    {
        get { return _mass; }
        set { _mass = value; }
    }

    [SerializeField] private Transform _psyPranet;
    public Transform PSYPranet
    {
        get { return _psyPranet; }
        set { _psyPranet = value; }
    }

    private void Start()
    {
        if (!TryGetComponent(out _rb))
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
        }

        _rb.AddForce(MoveVec, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if(PSYPranet != null)
        {
            _rb.MovePosition( Vector2.Lerp(transform.position ,PSYPranet.position+Vector3.up,.5f));
        }
    }

    public void AddPSYForce(Vector2 vector)
    {
        //MoveVec += vector;
        _rb.AddForce(vector, ForceMode2D.Impulse);
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
        PSYPranet = pranet;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }

    
}

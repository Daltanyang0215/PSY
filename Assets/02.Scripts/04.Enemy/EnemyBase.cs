using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IHitAction
{
    [SerializeField] protected EnemyStateData state;

    [SerializeField] protected int _curHP;

    public bool IsLive {  get; private set; }
    
    private GameObject _renderer;
    private CircleCollider2D _collider;
    public OrderType Order { get; protected set; }


    protected virtual void Start()
    {
        IsLive = true;
        Order = OrderType.Enemy;

        _curHP = state.HP;
        _renderer = transform.GetComponentInChildren<SpriteRenderer>().gameObject;
        _collider = GetComponent<CircleCollider2D>();
    }

    protected virtual void Update() { }

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
        Debug.Log("EnemyDie");
        _renderer.SetActive(false);
        _collider.enabled = false;
        IsLive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHitAction>().OnHit(1);
        }
    }
}

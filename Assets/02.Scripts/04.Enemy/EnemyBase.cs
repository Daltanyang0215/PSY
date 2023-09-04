using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IHitAction
{
    [SerializeField] protected EnemyStateData state;

    [SerializeField] protected int _curHP;


    public OrderType Order { get; protected set; }


    protected virtual void Start()
    {
        Order = OrderType.Enemy;

        _curHP = state.HP;
    }


    public virtual void OnHit(int damage)
    {
        if (damage > 0)
        {
            _curHP -= damage;

            if (_curHP < 0)
            {
                OnDie();
            }
        }
    }

    protected virtual void OnDie()
    {
        Debug.Log("EnemyDie");
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IHitAction>().OnHit(1);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour, IHitAction
{
    private static PlayerState instance;
    public static PlayerState Instance
    {
        get
        {
            if(instance == null)
                instance = GameObject.Find("Player").GetComponent<PlayerState>();

            return instance;
        }
    }


    [SerializeField] private int _psyLevel;
    public int PsyLevel => _psyLevel;


    [SerializeField] private int _hpMax;
    [SerializeField] private int _hp;
    public float PlayerHpUI => (float)_hp /_hpMax;
    [SerializeField] private int _mpMax;
    [SerializeField] private int _mp;
    public float PlayerMpUI => (float)_mp/_mpMax;

    public OrderType Order { get; private set; }

    private void Start()
    {
        Order = OrderType.Player;
        _hp = _hpMax;
        _mp = _mpMax;
    }

    public bool CheckMpPoint(int usingMP)
    {
        if(usingMP < _mp) {
            _mp -= usingMP;
            return true;
        }
        return false;
    }

    public void OnHit(int damage)
    {
        _hp -= damage;
        if(_hp <= 0)
        {
            OnPlayerDie();
        }
    }

    private void OnPlayerDie()
    {
        Debug.Log("Playerdie");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerState : MonoBehaviour, IHitAction
{
    private static PlayerState instance;
    public static PlayerState Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("Player").GetComponent<PlayerState>();

            return instance;
        }
    }

    [Header("PSY")]
    [SerializeField] private int _psyLevel;
    public int PsyLevel => _psyLevel;

    [Header("HP")]
    [SerializeField] private int _hpMax;
    [SerializeField] private int _hp;
    public float PlayerHpUI => (float)_hp / _hpMax;
    [Header("MP")]
    [SerializeField] private int _mpMax;
    [SerializeField] private int _mp;
    [SerializeField] private int _clipmp;
    [SerializeField] private int _mpRecoveryPerStartSec;
    [SerializeField] private int _mpRecoveryPerSec;
    [SerializeField] private float _mpRecoveryTime;
    private float _mpRecTimer;
    public float PlayerMpUI => (float)_mp / _mpMax;
    public float PlayerClipMpUI => (float)_clipmp / _mpMax;

    [Header("Move")]
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    [SerializeField] private float _jumpPower;
    public float JumpPower => _jumpPower;
    [SerializeField] private bool _spriteDefaultRight;
    public bool SpriteDefaultRight => _spriteDefaultRight;
    [SerializeField] private LayerMask _groundLayer;
    public LayerMask GroundLayer => _groundLayer;
    public bool IsNotMove { get; private set; }

    [Header("Attack")]
    [SerializeField] private LayerMask _attackTargetLayer;
    public LayerMask AttackTargetLayer => _attackTargetLayer;
    [SerializeField] private List<PSYSkillKeySet> _keyboardskills = new List<PSYSkillKeySet>();
    public List<PSYSkillKeySet> SkillKeySets => _keyboardskills;
    [SerializeField] private List<PSYSkillKeySet> _mouseskills = new List<PSYSkillKeySet>();
    public List<PSYSkillKeySet> SkillsMouseSets => _mouseskills;
    public OrderType Order { get; private set; }

    private Vector2 _playerSize;

    private void Start()
    {
        Order = OrderType.Player;
        _hp = _hpMax;
        _mp = _mpMax;
        _playerSize = GetComponent<BoxCollider2D>().size;
    }

    private void Update()
    {
        RecoveryMp();
        OnCheckInteraction();
    }
    public void RecoveryMp()
    {
        if (_mpRecTimer < 0)
        {
            CheckMpPoint(-_mpRecoveryPerSec);
            _mpRecTimer = _mpRecoveryTime;
        }
        _mpRecTimer -= Time.deltaTime;
    }

    public bool CheckMpPoint(int usingMP, bool clipMP = false)
    {
        if (usingMP <= _mp)
        {
            _mp -= usingMP;
            _mpRecTimer = _mpRecoveryPerStartSec;

            if (clipMP)
            {
                _clipmp += usingMP;
                if (_clipmp < 0) _clipmp = 0;
            }

            MpClamp();
            return true;
        }
        return false;
    }

    private void MpClamp()
    {
        if (_mp > _mpMax - _clipmp)
        {
            _mp = _mpMax - _clipmp;
        }
    }

    public void OnCheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Physics2D.OverlapBox(transform.position + (Vector3.up * _playerSize.y * 0.5f), _playerSize, 0,AttackTargetLayer).TryGetComponent(out IInteraction interaction))
            {
                interaction.OnInteraction();
            }
        }
    }

    public void OnPlayerStop(bool isStop)
    {
        IsNotMove = isStop;
    }

    public void OnHit(int damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            OnPlayerDie();
        }
    }

    private void OnPlayerDie()
    {
        Debug.Log("Playerdie");
    }
}

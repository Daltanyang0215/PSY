using Cinemachine;
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
    public Action PlayerRecoveryAction;

    [Header("Move")]
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    [SerializeField] private float _jumpPower;
    public float JumpPower => _jumpPower;
    [SerializeField] private float _dashRange;
    public float DashRange => _dashRange;
    [SerializeField] private float _dashDelay;
    public float DashDelay => _dashDelay;
    [SerializeField] private bool _spriteDefaultRight;
    public bool SpriteDefaultRight => _spriteDefaultRight;
    [SerializeField] private LayerMask _groundLayer;
    public LayerMask GroundLayer => _groundLayer;
    public bool IsNotMove { get; private set; }
    private Vector2 _playerSize;

    [Header("Attack")]
    [SerializeField] private LayerMask _attackTargetLayer;
    public LayerMask AttackTargetLayer => _attackTargetLayer;
    [SerializeField] private List<PSYSkillKeySet> _keyboardskills = new List<PSYSkillKeySet>();
    public List<PSYSkillKeySet> SkillKeySets => _keyboardskills;
    [SerializeField] private List<PSYSkillKeySet> _mouseskills = new List<PSYSkillKeySet>();
    public List<PSYSkillKeySet> SkillsMouseSets => _mouseskills;
    public OrderType Order { get; private set; }
    public List<List<KinesisObjectBase>> bulletObjects = new List<List<KinesisObjectBase>>();

    private CinemachineConfiner _playerCamera;
    private Collider2D _prevZone;

    private void Start()
    {
        Order = OrderType.Player;
        _hp = _hpMax;
        _mp = _mpMax;
        _playerSize = GetComponent<BoxCollider2D>().size;
        _playerCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineConfiner>();
    }

    private void Update()
    {
        RecoveryMp();
        OnCheckInteraction();
        OnCheckZoneCamera();

        // recovery test. will delete
        if(Input.GetKeyDown(KeyCode.C))
        {
            PlayerFullRecovery();
        }
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
            if (GameManager.Instance.CurTalkNPC != null)
            {
                GameManager.Instance.CurTalkNPC.OnInteraction();
            }
            else if (Physics2D.OverlapBox(transform.position + (Vector3.up * _playerSize.y * 0.5f), _playerSize, 0, AttackTargetLayer).TryGetComponent(out IInteraction interaction))
            {
                interaction.OnInteraction();
            }
        }
    }

    // 맵 이동에 따른 존 카메라 변경
    private void OnCheckZoneCamera()
    {
        // 현재 위치의 카메라가 있는지 확인
        RaycastHit2D[] zones = Physics2D.RaycastAll(transform.position + Vector3.up, Vector2.zero, 0, 1 << LayerMask.NameToLayer("ZoneCamera"));
        // 카메라가 2개 겹치는 경우
        if (zones.Length == 2)
        {
            // 이전 카메라와 다른 카메라를 변경함
            foreach (RaycastHit2D zone in zones)
            {
                if (zone.collider != _prevZone)
                {
                    if (_playerCamera.m_BoundingShape2D != zone.collider)
                    {
                        _playerCamera.m_Damping = 1;
                        _playerCamera.m_BoundingShape2D = zone.collider;
                    }
                }
            }
        }
        else if (zones.Length == 1)
        {
            if (_playerCamera.m_BoundingShape2D != zones[0].collider)
            {
                _playerCamera.m_Damping = 1;
                _playerCamera.m_BoundingShape2D = zones[0].collider;
            }
            _prevZone = zones[0].collider;
        }
        _playerCamera.m_Damping = _playerCamera.m_Damping <= 0 ? 0 : _playerCamera.m_Damping - (Time.deltaTime * 2f);
    }

    private void PlayerFullRecovery()
    {
        _hp = _hpMax;
        _mp = _mpMax;
        MpClamp();
        PlayerRecoveryAction?.Invoke();
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

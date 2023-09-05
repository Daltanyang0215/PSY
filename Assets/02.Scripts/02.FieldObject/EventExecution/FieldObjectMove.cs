using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FieldObjectMove : FieldEventExecution
{
    private Rigidbody2D _rb;

    [SerializeField] private FIeldObjectMovePoint[] _movePos;
    private int _movePointIndex = 0;

    private enum MoveType
    {
        None,
        Once,
        Pingpong,
        Loop
    }
    [Tooltip("None,       // 동작 안함\n"
             + "Once,       // 1회만\n"
             + "Pingpong,   // 반복함 (1-2-3-2...)\n"
             + "Loop        // 반복함 (1-2-3-1...)")]
    [SerializeField] private MoveType _moveType;

    [Tooltip("이벤트가 취소되면 처음으로 돌아오는지 여부")]
    [SerializeField] private bool _isCancleReturn;
    private bool _pingpongCheck;

    private Vector3 _startPos;
    private Vector3 _prevPos;
    private Vector3 _moveVec;

    private float _waitTimer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _prevPos = _startPos = transform.position;
        _waitTimer = _movePos[0].movedelay;
    }

    private void FixedUpdate()
    {
        if (_moveType == MoveType.None)
        {
            Debug.Log($"{gameObject.name} is movetype.None");
            return;
        }

        _moveVec = transform.position - _prevPos;
        _moveVec *= 1 / Time.deltaTime;
        _moveVec.y = 0;
        _prevPos = transform.position;


        if (IsExecution)
        {
            if (Vector2.Distance(transform.position, _startPos + _movePos[_movePointIndex].movePos) < 0.05f)
            {
                _rb.position = _startPos + _movePos[_movePointIndex].movePos;
                switch (_moveType)
                {
                    case MoveType.Once:
                        return;
                    case MoveType.Pingpong:
                        if (_pingpongCheck)
                        {
                            _movePointIndex--;
                            if (_movePointIndex < 0)
                            {
                                _movePointIndex = 0;
                                _pingpongCheck = false;
                            }
                        }
                        else
                        {
                            _movePointIndex++;
                            if (_movePointIndex >= _movePos.Length)
                            {
                                _movePointIndex = _movePos.Length - 1;
                                _pingpongCheck = true;
                            }
                        }
                        _waitTimer = _movePos[_movePointIndex].movedelay;
                        break;
                    case MoveType.Loop:
                        _movePointIndex++;
                        if (_movePointIndex > _movePos.Length)
                        {
                            _movePointIndex = 0;
                        }
                        _waitTimer = _movePos[_movePointIndex].movedelay;
                        break;
                    default:
                        break;
                }
            }
            if (_waitTimer <= 0)
                _rb.MovePosition(Vector2.MoveTowards(transform.position, _startPos + _movePos[_movePointIndex].movePos, _movePos[_movePointIndex].moveSpeed * Time.fixedDeltaTime));
            else
                _waitTimer -= Time.deltaTime;
        }
        else if (_isCancleReturn)
        {
            if (Vector2.Distance(transform.position, _startPos + _movePos[0].movePos) < 0.05f)
            {
                _rb.position = _startPos + _movePos[0].movePos;
                return;
            }

            switch (_moveType)
            {
                case MoveType.None:
                    break;
                case MoveType.Once:
                    _rb.MovePosition(Vector2.MoveTowards(transform.position, _startPos + _movePos[0].movePos, _movePos[0].moveSpeed * Time.fixedDeltaTime));
                    break;
                case MoveType.Pingpong:
                case MoveType.Loop:
                    _rb.MovePosition(Vector2.MoveTowards(transform.position, _startPos + _movePos[_movePointIndex].movePos, _movePos[_movePointIndex].moveSpeed * Time.fixedDeltaTime));
                    if (Vector2.Distance(transform.position, _startPos + _movePos[_movePointIndex].movePos) < 0.05f)
                        _movePointIndex--;
                    break;
                default:
                    break;
            }
        }
    }

    public override void OnEvnentExecution()
    {
        base.OnEvnentExecution();
        _movePointIndex++;
    }

    public override void OnEvnentCancle()
    {
        base.OnEvnentCancle();
        if (_isCancleReturn) // 기존 위치로 돌아오는 오브젝트의 설정 초기화
        {
            if (_pingpongCheck)
            {
                _pingpongCheck = false;
                _movePointIndex++;
            }
            if (_movePointIndex > 0)
                _movePointIndex--;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().PlayerAddVelocity(_moveVec);
        }
    }

    [ContextMenu("GizmoReset")]
    void StartPosReset()
    {
        _startPos = transform.position;
    }

    private void OnDrawGizmos()
    {
        if(_startPos == Vector3.zero) _startPos = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_startPos, transform.localScale);

        for (int i = 0; i < _movePos.Length - 1; i++)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(_startPos + _movePos[i].movePos, _startPos + _movePos[i + 1].movePos);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_startPos + _movePos[i + 1].movePos, transform.localScale);
        }
    }
}
[Serializable]
public class FIeldObjectMovePoint
{
    public Vector3 movePos;
    public float moveSpeed;
    public float movedelay;
}
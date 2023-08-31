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
    [Tooltip("None,       // ���� ����\n"
             + "Once,       // 1ȸ��\n"
             + "Pingpong,   // �ݺ��� (1-2-3-2...)\n"
             + "Loop        // �ݺ��� (1-2-3-1...)")]
    [SerializeField] private MoveType _moveType;

    [Tooltip("�̺�Ʈ�� ��ҵǸ� ó������ ���ƿ����� ����")]
    [SerializeField] private bool _isCancleReturn;
    private bool _pingpongCheck;

    private Vector3 _prevPos;
    private Vector3 _moveVec;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); 
        _prevPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (_moveType == MoveType.None)
        {
            Debug.Log($"{gameObject.name} is movetype.None");
            return;
        }

        _moveVec = transform.position - _prevPos ;
        _moveVec *= 1/Time.deltaTime;
        _moveVec.y = 0;
        _prevPos = transform.position;


        if (IsExecution)
        {
            if (Vector2.Distance(transform.position, _movePos[_movePointIndex].movePos) < 0.05f)
            {
                _rb.position = _movePos[_movePointIndex].movePos;
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
                        break;
                    case MoveType.Loop:
                        _movePointIndex++;
                        if (_movePointIndex > _movePos.Length)
                        {
                            _movePointIndex = 0;
                        }
                        break;
                    default:
                        break;
                }
            }
            _rb.MovePosition(Vector2.MoveTowards(transform.position, _movePos[_movePointIndex].movePos, _movePos[_movePointIndex].moveSpeed * Time.fixedDeltaTime));
        }
        else if (_isCancleReturn)
        {
            if (Vector2.Distance(transform.position, _movePos[0].movePos) < 0.05f)
            {
                _rb.position = _movePos[0].movePos;
                return;
            }

            switch (_moveType)
            {
                case MoveType.None:
                    break;
                case MoveType.Once:
                    _rb.MovePosition(Vector2.MoveTowards(transform.position, _movePos[0].movePos, _movePos[0].moveSpeed * Time.fixedDeltaTime));
                    break;
                case MoveType.Pingpong:
                case MoveType.Loop:
                    _rb.MovePosition(Vector2.MoveTowards(transform.position, _movePos[_movePointIndex].movePos, _movePos[_movePointIndex].moveSpeed * Time.fixedDeltaTime));
                    if (Vector2.Distance(transform.position, _movePos[_movePointIndex].movePos) < 0.05f)
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
        if (_isCancleReturn) // ���� ��ġ�� ���ƿ��� ������Ʈ�� ���� �ʱ�ȭ
        {
            _pingpongCheck = false;
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
}
[Serializable]
public class FIeldObjectMovePoint
{
    public Vector3 movePos;
    public float moveSpeed;
    public float movedelay;
}
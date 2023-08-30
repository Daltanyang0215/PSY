using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObjectMove : FieldEventExecution
{
    private Vector3 _startPos;
    [SerializeField] private Vector3[] _movePos;
    private int _movePointIndex;

    private enum MoveType
    {
        None,       
        Once,       
        Pingpong,   
        Loop        
    }
    [Tooltip(  "None,       // ���� ����\n"
             + "Once,       // 1ȸ��\n"
             + "Pingpong,   // �ݺ��� (1-2-3-2...)\n"
             + "Loop        // �ݺ��� (1-2-3-1...)")]
    [SerializeField] private MoveType _moveType;
    
    [Tooltip("�̺�Ʈ�� ��ҵǸ� ó������ ���ƿ����� ����")]
    [SerializeField] private bool _isCancleReturn;

}

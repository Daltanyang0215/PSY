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
    [Tooltip(  "None,       // 동작 안함\n"
             + "Once,       // 1회만\n"
             + "Pingpong,   // 반복함 (1-2-3-2...)\n"
             + "Loop        // 반복함 (1-2-3-1...)")]
    [SerializeField] private MoveType _moveType;
    
    [Tooltip("이벤트가 취소되면 처음으로 돌아오는지 여부")]
    [SerializeField] private bool _isCancleReturn;

}

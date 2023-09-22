using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldViewClamp : FieldEventExecution
{
    private CinemachineConfiner _playerCamera;
    private PolygonCollider2D _viewClamp;

    private void Start()
    {
        _playerCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineConfiner>();
        _viewClamp = GetComponent<PolygonCollider2D>();
    }

    public override void OnEvnentExecution()
    {
        base.OnEvnentExecution();
        _playerCamera.m_BoundingShape2D = _viewClamp;
    }

    public override void OnEvnentCancle()
    {
        base.OnEvnentCancle();
        //_playerCamera.m_BoundingShape2D = null;
    }
}

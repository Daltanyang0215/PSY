using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class FieldViewCamera : FieldEventExecution
{
    private CinemachineVirtualCamera _araeCamera;

    private void Start()
    {
        _araeCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public override void OnEvnentExecution()
    {
        base.OnEvnentExecution();
        _araeCamera.Priority = 100;
    }

    public override void OnEvnentCancle()
    {
        base.OnEvnentCancle();
        _araeCamera.Priority = -1;
    }
}

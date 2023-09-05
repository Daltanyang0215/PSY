using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEventTrigger : MonoBehaviour
{
    protected Animator _animator;
    public bool isPressed { get; protected set; }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }
}

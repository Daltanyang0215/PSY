using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PSYSkillBase : ScriptableObject, IPSYSkill
{
    [SerializeField] private byte _id;
    public byte PSYID => _id;
    
    [SerializeField] private int _psyLevel;
    public int PSYLevel => _psyLevel;
    [SerializeField] private int _psyMP;
    public int PSYMP => _psyMP;
    [SerializeField] private bool _isMPClip;
    public bool IsMPClip => _isMPClip;

    public bool IsActive { get; protected set; }

    public abstract void OnPSYInit();
    public abstract void OnPSYEnter(Vector3 point, LayerMask targetlayer);

    public abstract void OnPSYUpdate(Vector3 point, LayerMask targetlayer);

    public abstract void OnPSYExit(Vector3 point, LayerMask targetlayer);

    public abstract void OnPSYEngineUpdate(Vector3 point, LayerMask targetlayer);
}

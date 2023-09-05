using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrderType
{
    None,
    Player,
    Enemy
}
public abstract class KinesisObjectBase : MonoBehaviour
{
    public OrderType Order { get; private set; }
    public byte EffectSkill { get; private set; }
    [SerializeField] private int _psyLevel;
    public int PSYLevel => _psyLevel;
    public Transform PSYPranet { get; private set; }

    public abstract void SetPSYForce(Vector2 vector);
    public abstract void AddPSYForce(Vector2 vector);
    public abstract void StopPSYForce(bool notGravite = false);
    public void SetPSYPranet(Transform pranet)
    {
        PSYPranet = pranet;
    }

    public void SetOrder(OrderType newOrder , byte _skillid = 0)
    {
        Order = newOrder;
        EffectSkill = _skillid;
    }
}

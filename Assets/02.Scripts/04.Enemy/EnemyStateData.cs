using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyState", menuName = "Enemy/EnemyState")]
public class EnemyStateData : ScriptableObject
{
    [SerializeField] private int _hp;
    public int HP => _hp;

    [SerializeField] private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;

    [SerializeField] private float _attackCool;
    public float AttackCool => _attackCool;
    [SerializeField] private int _collisionDamage;
    public int CollisionDamage => _collisionDamage;

    [SerializeField] private float _bulletSpeed;
    public float BulletSpeed => _bulletSpeed;

    [SerializeField] private bool _doDieExplosion;
    public bool DoDieExplosion => _doDieExplosion;
    [SerializeField] private float _explodeRange;
    public float ExplodeRange => _explodeRange;
    [SerializeField] private int _explodeDamage;
    public int ExplodeDamage => _explodeDamage;
}

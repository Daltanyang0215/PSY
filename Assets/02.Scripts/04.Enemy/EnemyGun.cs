using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : EnemyBase
{

    private float _attackTimer;
    [SerializeField] private TestBullet _bulletPrefab;

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
        if (!IsLive) return;

        if (_attackTimer <= 0
            && Vector2.Distance(PlayerState.Instance.transform.position, transform.position) < 10)
        {
            DoAttack();
        }

        _attackTimer -= Time.deltaTime;
    }

    private void DoAttack()
    {
        TestBullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.SetOrder(OrderType.Enemy);
        bullet.SetPSYForce((PlayerState.Instance.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 1f)) - transform.position).normalized * 10);

        _attackTimer = state.AttackCool;
    }
}

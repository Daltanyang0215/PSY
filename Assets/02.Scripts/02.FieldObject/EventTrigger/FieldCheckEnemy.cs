using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCheckEnemy : FieldEventTrigger
{
    [SerializeField] private bool _isOnlyOne;
    [SerializeField] private EnemyBase _onlyOneEnemy;

    
    private Collider2D[] enemys;
    private void LateUpdate()
    {
        if (isPressed) return;

        enemys = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);

        isPressed = true;

        if (_isOnlyOne)
        {
            isPressed = !_onlyOneEnemy.IsLive;
        }
        else
        {
            foreach (Collider2D collider in enemys)
            {
                if (collider.transform.TryGetComponent(out EnemyBase enemy))
                {
                    if (enemy.IsLive)
                    {
                        isPressed = false;
                        break;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEnemySpawner : FieldEventTrigger
{

    [SerializeField] private List<EnemyBase> enemies = new List<EnemyBase>();

    [ContextMenu("EnemyListReset")]
    private void EnemyListReset()
    {
        enemies.Clear();

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        foreach(Collider2D col in colliders)
        {
            if(col.TryGetComponent(out EnemyBase enemy))
            {
                enemies.Add(enemy);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPressed = true;
            foreach (EnemyBase enemy in enemies)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPressed = false;
            foreach (EnemyBase enemy in enemies)
            {
                enemy.gameObject.SetActive(false);
            }
        }

    }
}

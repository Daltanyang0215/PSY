using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private KeyCode graviteKey;
    [SerializeField] private float graviteRange;
    [SerializeField] private LayerMask graviteLayer;

    private Ikinesis target;
    private Vector2 startvec;
    private List<Ikinesis> _kineses = new List<Ikinesis>();
    private void Update()
    {
        if (Input.GetKeyDown(graviteKey))
        {
            Collider2D[] enemybullets = Physics2D.OverlapBoxAll(transform.position, Vector2.one * graviteRange, 0, graviteLayer);

            Ikinesis testBullet = null;
            foreach (Collider2D bullet in enemybullets)
            {
                if (bullet.TryGetComponent(out testBullet))
                {
                    testBullet.AddPSYForce((bullet.transform.position -transform.position).normalized*5);
                }
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            Collider2D[] enemybullets = Physics2D.OverlapBoxAll(transform.position, Vector2.one * graviteRange, 0, graviteLayer);

            Ikinesis testBullet = null;
            foreach (Collider2D bullet in enemybullets)
            {
                if (bullet.TryGetComponent(out testBullet))
                {
                    testBullet.StopPSYForce(true);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Collider2D[] enemybullets = Physics2D.OverlapBoxAll(transform.position, Vector2.one * graviteRange, 0, graviteLayer);

            Ikinesis testBullet = null;
            foreach (Collider2D bullet in enemybullets)
            {
                if (bullet.TryGetComponent(out testBullet))
                {
                    testBullet.StopPSYForce();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            startvec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D enemybullet = Physics2D.OverlapBox(startvec, Vector2.one, 0, graviteLayer);

            if (enemybullet != null && enemybullet.TryGetComponent(out target))
            {
                startvec = enemybullet.transform.position;
                Time.timeScale = 0;
            }
        }
        if (Input.GetMouseButton(0))
        {
            Debug.DrawLine(startvec, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (target != null)
            {
                Time.timeScale = 1;
                target.AddPSYForce((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - startvec);
                target = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            startvec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D enemybullet = Physics2D.OverlapBox(startvec, Vector2.one, 0, graviteLayer);

            if (enemybullet != null && enemybullet.TryGetComponent(out target))
            {
                target.StopPSYForce(true);
                target.SetPSYPranet(transform);
                _kineses.Add(target);
            }
            else if(_kineses.Count > 0)
            {
                _kineses[0].SetPSYPranet(null);
                _kineses[0].AddPSYForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - (transform.position + Vector3.up)).normalized*10);
                _kineses.RemoveAt(0);
            }
        }
    }
}

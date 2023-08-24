using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private float initTimer;
    private float timer;

    private void Start()
    {
        timer = initTimer;
    }

    private void Update()
    {
        if (timer < 0)
        {
            timer = initTimer;
            Instantiate(bullet,transform.position,Quaternion.identity);
        }
        timer -= Time.deltaTime;
    }
}

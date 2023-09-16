using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinesisObject : KinesisObjectBase
{


    private void Awake()
    {
        if (!TryGetComponent(out _rb))
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    public override void SetPSYForce(Vector2 vector, ForceMode2D mode = ForceMode2D.Impulse)
    {
        _rb.drag = 0;
        _rb.gravityScale = 0;
        //_rb.velocity = Vector2.zero;
        //transform.right = vector;
        //_rb.AddForce(vector);
        vector = PSYLevel <= 0 ? vector : vector / PSYLevel;

        _rb.velocity = vector;
    }
    public override void AddPSYForce(Vector2 vector, ForceMode2D mode = ForceMode2D.Impulse)
    {
        _rb.drag = 0;
        _rb.gravityScale = 1f;
        //transform.right = transform.right + (Vector3)vector;
        _rb.AddForce(vector, mode);
    }

    public override void StopPSYForce(bool notGravite = false)
    {
        if (PSYLevel == 0 || PSYLevel <= PlayerState.Instance.PsyLevel)
        {
            _rb.velocity = Vector2.zero;

        }
        else
        {
            _rb.drag = 2 / (PSYLevel - PlayerState.Instance.PsyLevel);
            if (_rb.velocity == Vector2.zero)
            {
                _rb.drag = 0;
            }
        }

        //_rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.zero, PSYLevel == 0 ? 1 : 1 / Time.deltaTime);// (PlayerState.Instance.PsyLevel / (float)PSYLevel)

        if (notGravite)
        {
            _rb.gravityScale = 0;
        }
        else
        {
            _rb.drag = 0;
            _rb.gravityScale = 1;
        }
    }
    public override void PSYCancle(bool notGravite = false)
    {
        if (notGravite)
        {
            _rb.gravityScale = 0;
        }
        else
        {
            _rb.drag = 0;
            _rb.gravityScale = 1;
        }
    }

}

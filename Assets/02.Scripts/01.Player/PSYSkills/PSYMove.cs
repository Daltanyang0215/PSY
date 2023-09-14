using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PSYMove", menuName = "PSYSkill/PSYMove")]
public class PSYMove : PSYSkillBase
{
    [SerializeField] private float _skillMut;

    private Transform _rangeCircle;
    private Transform _MoveVecIgame;

    private KinesisObjectBase _target;
    private PhysicsMaterial2D _targetMaterial;
    private PhysicsMaterial2D _moveMaterial;
    public override void OnPSYInit()
    {
        _moveMaterial = new PhysicsMaterial2D();
        _rangeCircle = GameObject.Find("PSYMoveRange").transform;
        _MoveVecIgame = _rangeCircle.GetChild(0);

        _rangeCircle.gameObject.SetActive(false);
    }
    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        Collider2D kineseTargets = Physics2D.OverlapCircle(point, 0.1f, targetlayer);

        if (kineseTargets != null && kineseTargets.TryGetComponent(out KinesisObjectBase target))
        {
            IsActive = true;
            target.SetOrder(OrderType.Player, PSYID);
            _target = target;
            // 물체의 마찰력 변경. 박스가 무거워서 레벨 1에서 움직이지를 않음
            _targetMaterial = target.GetComponent<Rigidbody2D>().sharedMaterial;
            target.GetComponent<Rigidbody2D>().sharedMaterial = _moveMaterial;

            PlayerState.Instance.CheckMpPoint(PSYMP, IsMPClip);
            _rangeCircle.transform.position = target.transform.position;
            _rangeCircle.gameObject.SetActive(true);
        }
    }

    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer)
    {
        if (!IsActive) return;

        if (_target == null)
        {
            PSYCancle();
            return;
        }
        float pointLength = Mathf.Clamp((_target.transform.position - point).magnitude / 3, 0, 1);
        _rangeCircle.transform.position = _target.transform.position;
        _MoveVecIgame.up = (point - _target.transform.position).normalized;
        _MoveVecIgame.localScale = new Vector3(0.25f, pointLength);

        // 타겟이 총알이며, 거리가 좁고 속도가 작을때 강제 속도로 설정
        if (_target.ObjectType == KinesisObjectType.Bullet
            && pointLength < .1f
            && _target.GetVelocity.magnitude < 1f)
        {
            _target.SetPSYForce((point - _target.transform.position).normalized * PlayerState.Instance.PsyLevel * _skillMut * pointLength);
        }
        _target.AddPSYForce((point - _target.transform.position).normalized * PlayerState.Instance.PsyLevel * _skillMut * pointLength,
                           _target.ObjectType == KinesisObjectType.None ? ForceMode2D.Force : ForceMode2D.Impulse);
    }

    public override void OnPSYExit(Vector3 point, LayerMask targetlayer)
    {
        if (IsActive)
        {
            _target.GetComponent<Rigidbody2D>().sharedMaterial = _targetMaterial;
            _target.PSYCancle();
            PSYCancle();
        }
    }

    private void PSYCancle()
    {
        IsActive = false;
        _rangeCircle.gameObject.SetActive(false);
        PlayerState.Instance.CheckMpPoint(-PSYMP, IsMPClip);
    }


    public override void OnPSYEngineUpdate(Vector3 point, LayerMask targetlayer) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PSYMove", menuName = "PSYSkill/PSYMove")]
public class PSYMove : PSYSkillBase
{
    [SerializeField] private float _skillMut;

    private Transform _rangeCircle;
    private Transform _MoveVecIgame;

    private KinesisObjectBase target;
    public override void OnPSYInit()
    {
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
            this.target = target;

            PlayerState.Instance.CheckMpPoint(PSYMP, IsMPClip);
            _rangeCircle.transform.position = target.transform.position;
            _rangeCircle.gameObject.SetActive(true);
        }
    }

    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer)
    {
        if (!IsActive) return;

        if (target == null)
        {
            PSYCancle();
            return;
        }
        float pointLength = Mathf.Clamp((target.transform.position - point).magnitude / 3, 0, 1);
        _rangeCircle.transform.position = target.transform.position;
        _MoveVecIgame.up = (point - target.transform.position).normalized;
        _MoveVecIgame.localScale = new Vector3(0.25f, pointLength);

        // 타겟이 총알이며, 거리가 좁고 속도가 작을때 강제 속도로 설정
        if (target.ObjectType == KinesisObjectType.Bullet
            && pointLength < .1f
            && target.GetVelocity.magnitude < 1f)
        {
            target.SetPSYForce((point - target.transform.position).normalized * PlayerState.Instance.PsyLevel * _skillMut * pointLength);
        }
        target.AddPSYForce((point - target.transform.position).normalized * PlayerState.Instance.PsyLevel * _skillMut * pointLength);
    }

    public override void OnPSYExit(Vector3 point, LayerMask targetlayer)
    {
        if (IsActive)
        {
            target.PSYCancle();
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

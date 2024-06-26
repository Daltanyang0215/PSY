using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PSYPull", menuName = "PSYSkill/PSYPull")]

public class PSYPull : PSYSkillBase
{
    private KinesisObjectBase target;
    [SerializeField] private float _skillMut;

    public override void OnPSYInit() { }

    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        Collider2D kineseTargets = Physics2D.OverlapCircle(point, 0.1f, targetlayer);

        if (kineseTargets != null && kineseTargets.TryGetComponent(out KinesisObjectBase target))
        {
            IsActive = true;
            target.SetOrder(OrderType.Player, PSYID);
            this.target = target;

            PlayerState.Instance.CheckMpPoint(PSYMP, IsMPClip);
        }
    }
    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer)
    {
        if (!IsActive) return;

        if (target == null)
        {
            PlayerState.Instance.CheckMpPoint(-PSYMP, IsMPClip);
            IsActive = false;
            return;
        }

        target.SetPSYForce((PlayerState.Instance.transform.position + Vector3.up - target.transform.position).normalized * PlayerState.Instance.PsyLevel * _skillMut);
    }

    public override void OnPSYExit(Vector3 point, LayerMask targetlayer)
    {
        if (!IsActive) return;
        PlayerState.Instance.CheckMpPoint(-PSYMP, IsMPClip);
        target.PSYCancle();
        IsActive = false;
        target = null;
    }


    public override void OnPSYEngineUpdate(Vector3 point, LayerMask targetlayer) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PSYExplosion", menuName = "PSYSkill/PSYExplosion")]

public class PSYExplosion : PSYSkillBase
{
    [SerializeField] private float _skillRange;
    [SerializeField] private float _skillMut;

    public override void OnPSYInit() { }
    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        if (!PlayerState.Instance.CheckMpPoint(PSYMP)) return;

        Collider2D[] kineseTargets = Physics2D.OverlapCircleAll((point + Vector3.up), _skillRange, targetlayer);

        KinesisObjectBase target = null;
        foreach (Collider2D kineses in kineseTargets)
        {
            if (kineses.TryGetComponent(out target))
            {
                target.SetOrder(OrderType.Player, PSYID);
                target.AddPSYForce((kineses.transform.position - (point + Vector3.up)).normalized * PlayerState.Instance.PsyLevel * _skillMut,
                                   ForceMode2D.Impulse);
            }
            if (kineses.CompareTag("CanDestroyObject") && kineses.TryGetComponent(out IHitAction hit))
            {
                hit.OnHit(PSYLevel);
            }
        }
    }
   
    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer)
    {
        //if (!IsActive) return;

        //Collider2D[] kineseTargets = Physics2D.OverlapCircleAll(point, _skillRange, targetlayer);

        //KinesisObjectBase target = null;
        //foreach (Collider2D kineses in kineseTargets)
        //{
        //    if (kineses.TryGetComponent(out target))
        //    {
        //        target.SetOrder(OrderType.Player, PSYID);
        //        target.AddPSYForce((kineses.transform.position + Vector3.up - point).normalized * PlayerState.Instance.PsyLevel * _skillMut * _UpdataMut);
        //    }
        //}
    }

    public override void OnPSYExit(Vector3 point, LayerMask targetlayer)
    {
        //if (IsActive)
        //{
        //    IsActive = false;
        //    PlayerState.Instance.CheckMpPoint(-PSYMP, true);
        //}
    }

    public override void OnPSYEngineUpdate(Vector3 point, LayerMask targetlayer) { }

}

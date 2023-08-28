using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PSYStop", menuName = "PSYSkill/PSYStop")]
public class PSYStop : PSYSkillBase
{
    [SerializeField] private float _skillRange;
    private List<KinesisObjectBase> _prevKineses = new List<KinesisObjectBase>();
    private List<KinesisObjectBase> _curKineses = new List<KinesisObjectBase>();

    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        _curKineses.Clear();
        _prevKineses.Clear();


        Collider2D[] kineseTargets = Physics2D.OverlapCircleAll(point, _skillRange, targetlayer);

        KinesisObjectBase target = null;
        foreach (Collider2D kineses in kineseTargets)
        {
            if (kineses.TryGetComponent(out target))
            {
                target.StopPSYForce(true);
                target.SetOrder(OrderType.Player);
                _prevKineses.Add(target);
            }
        }
    }

    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer)
    {

        Collider2D[] kineseTargets = Physics2D.OverlapCircleAll(point, _skillRange, targetlayer);

        KinesisObjectBase target = null;
        // 신규 등록
        foreach (Collider2D kineses in kineseTargets)
        {
            if (kineses.TryGetComponent(out target))
            {
                if (!_curKineses.Contains(target))
                {
                    target.StopPSYForce(true);
                    target.SetOrder(OrderType.Player);
                    _curKineses.Add(target);
                }
            }
        }
        // 이전항목과 동일비교
        for (int i = _prevKineses.Count - 1; i >= 0; i--)
        {
            if (!_prevKineses[i])
            {
                _prevKineses.RemoveAt(i);
            }
            else if (!_curKineses.Contains(_prevKineses[i]))
            {
                _prevKineses[i].StopPSYForce();
            }
        }
        _prevKineses.Clear();
        _prevKineses.AddRange(_curKineses);

        _curKineses.Clear();
    }

    public override void OnPSYExit(Vector3 point, LayerMask targetlayer)
    {
        foreach (KinesisObjectBase target in _prevKineses)
        {
            if (target != null)
                target.StopPSYForce();
        }
        _prevKineses.Clear();
    }
}

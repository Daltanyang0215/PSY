using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PSYStop", menuName = "PSYSkill/PSYStop")]
public class PSYStop : PSYSkillBase
{
    [SerializeField] private float _skillRange;
    private List<Ikinesis> _kineses = new List<Ikinesis>();

    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        Collider2D[] kineseTargets = Physics2D.OverlapCircleAll(point, _skillRange, targetlayer);

        Ikinesis target = null;
        foreach (Collider2D kineses in kineseTargets)
        {
            if (kineses.TryGetComponent(out target))
            {
                target.StopPSYForce(true);
                _kineses.Add(target);
            }
        }
    }

    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer)
    {
        for (int i = _kineses.Count - 1; i >= 0; i--)
        {
            if (_kineses[i].Transform == null)
            {
                _kineses.RemoveAt(i);
            }
            else if (Vector3.Distance(point, _kineses[i].Transform.position) > _skillRange)
            {
                _kineses[i].StopPSYForce();
                _kineses.RemoveAt(i);
            }
        }

        Collider2D[] kineseTargets = Physics2D.OverlapCircleAll(point, _skillRange, targetlayer);

        Ikinesis target = null;
        foreach (Collider2D kineses in kineseTargets)
        {
            if (kineses.TryGetComponent(out target))
            {
                if (!_kineses.Contains(target))
                {
                    target.StopPSYForce(true);
                    _kineses.Add(target);
                }
            }
        }
    }

    public override void OnPSYExit(Vector3 point, LayerMask targetlayer)
    {
        foreach (Ikinesis kineses in _kineses)
        {
            kineses.StopPSYForce();
        }
        _kineses.Clear();
    }
}

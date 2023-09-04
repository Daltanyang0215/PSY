using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PSYStop", menuName = "PSYSkill/PSYStop")]
public class PSYStop : PSYSkillBase
{
    [SerializeField] private float _skillRange;
    private List<KinesisObjectBase> _prevKineses = new List<KinesisObjectBase>();
    private List<KinesisObjectBase> _curKineses = new List<KinesisObjectBase>();

    private float timer;

    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        timer = 1;
        if (!PlayerState.Instance.CheckMpPoint(PSYMP)) return;

        IsActive = true;

        _curKineses.Clear();
        _prevKineses.Clear();


        Collider2D[] kineseTargets = Physics2D.OverlapCircleAll(point, _skillRange, targetlayer);

        KinesisObjectBase target = null;
        foreach (Collider2D kineses in kineseTargets)
        {
            if (kineses.TryGetComponent(out target))
            {
                target.StopPSYForce(true);
                target.SetOrder(OrderType.Player, PSYID);
                _prevKineses.Add(target);
            }
        }
    }

    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer)
    {
        if (!IsActive) return;

        // 1�� ���� �����Ҹ�
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (!PlayerState.Instance.CheckMpPoint(PSYMP))
            {
                OnPSYExit(point, targetlayer);
                IsActive = false;
                return;
            }
            timer = 1;
        }

        Collider2D[] kineseTargets = Physics2D.OverlapCircleAll(point, _skillRange, targetlayer);

        KinesisObjectBase target = null;
        // �ű� ���
        foreach (Collider2D kineses in kineseTargets)
        {
            if (kineses.TryGetComponent(out target))
            {
                if (!_curKineses.Contains(target))
                {
                    target.StopPSYForce(true);
                    target.SetOrder(OrderType.Player, PSYID);
                    _curKineses.Add(target);
                }
            }
        }
        // �����׸�� ���Ϻ�
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
        if (!IsActive) return;

        foreach (KinesisObjectBase target in _prevKineses)
        {
            if (target != null)
                target.StopPSYForce();
        }
        _prevKineses.Clear();
    }

    public override void OnPSYEngineUpdate(Vector3 point, LayerMask targetlayer) { }

    public override void OnPSYInit() { }
}

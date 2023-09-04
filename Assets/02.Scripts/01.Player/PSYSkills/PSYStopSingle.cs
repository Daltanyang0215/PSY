using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PSYStopSingle", menuName = "PSYSkill/PSYStopSingle")]
public class PSYStopSingle : PSYSkillBase
{
    //[SerializeField] private float _skillRange;
    private List<KinesisObjectBase> _activeKineses = new List<KinesisObjectBase>();


    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        Collider2D kineseTargets = Physics2D.OverlapCircle(point, 0.1f, targetlayer);

        if (kineseTargets != null)
        {
            if(kineseTargets.TryGetComponent(out FieldPushButton button))
            {
                button.OnPSYClick();
            }
            else if (kineseTargets.TryGetComponent(out KinesisObjectBase target))
            {
                // 기존 리스트에 있다면
                if (_activeKineses.Contains(target))
                {
                    // 있다면 해제
                    PlayerState.Instance.CheckMpPoint(-PSYMP, IsMPClip);
                    target.StopPSYForce();
                    _activeKineses.Remove(target);
                }
                else
                {
                    // 없다면 지정
                    if (PlayerState.Instance.CheckMpPoint(PSYMP, IsMPClip))
                    {
                        target.StopPSYForce(true);
                        target.SetOrder(OrderType.Player, PSYID);
                        _activeKineses.Add(target);
                    }
                }
            }
        }
    }

    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer) { }

    public override void OnPSYExit(Vector3 point, LayerMask targetlayer) { }
    public override void OnPSYEngineUpdate(Vector3 point, LayerMask targetlayer)
    {
        for (int i = _activeKineses.Count - 1; i >= 0; i--)
        {
            if (_activeKineses[i] == null || _activeKineses[i].EffectSkill != PSYID)
            {
                PlayerState.Instance.CheckMpPoint(-PSYMP, IsMPClip);
                _activeKineses.RemoveAt(i);
            }
        }
    }
}

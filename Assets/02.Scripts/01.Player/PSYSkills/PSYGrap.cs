using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PSYGrap", menuName = "PSYSkill/PSYGrap")]

public class PSYGrap : PSYSkillBase
{
    [SerializeField] private float _psyPowerMut;
    private List<KinesisObjectBase> kinesisObjects = new List<KinesisObjectBase>();

    public override void OnPSYInit()
    {
        kinesisObjects.Clear();
    }

    public override void OnPSYEngineUpdate(Vector3 point, LayerMask targetlayer) { }

    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        Collider2D enemybullet = Physics2D.OverlapCircle(point, 0.1f, PlayerState.Instance.AttackTargetLayer);

        if (enemybullet != null && enemybullet.TryGetComponent(out KinesisObjectBase target))
        {
            PlayerState.Instance.CheckMpPoint(PSYMP, IsMPClip);

            target.StopPSYForce(true);
            target.SetOrder(OrderType.Player);
            target.SetPSYPranet(PlayerState.Instance.transform);
            kinesisObjects.Add(target);
        }
        else if (kinesisObjects.Count > 0)
        {
            PlayerState.Instance.CheckMpPoint(-PSYMP, IsMPClip);
            kinesisObjects[0].SetPSYPranet(null);
            kinesisObjects[0].AddPSYForce((point - (PlayerState.Instance.transform.position + Vector3.up)).normalized * PSYLevel * _psyPowerMut);
            kinesisObjects.RemoveAt(0);
        }
    }

    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer) { }
    public override void OnPSYExit(Vector3 point, LayerMask targetlayer) { }
}

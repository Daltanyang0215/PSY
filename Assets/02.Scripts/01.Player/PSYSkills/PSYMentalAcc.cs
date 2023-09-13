using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PSYMentalAcc", menuName = "PSYSkill/PSYMentalAcc")]
public class PSYMentalAcc : PSYSkillBase
{
    [SerializeField] private float _timeScale;

    private float timer;

    public override void OnPSYEnter(Vector3 point, LayerMask targetlayer)
    {
        timer = 1;
        if (!PlayerState.Instance.CheckMpPoint(PSYMP)) return;

        IsActive = true;
        ChangeTimeScale(true);
    }

    public override void OnPSYUpdate(Vector3 point, LayerMask targetlayer)
    {
        if (!IsActive) return;

        // 1초 마다 마나소모
        timer -= Time.unscaledDeltaTime;
        if (timer <= 0)
        {
            if (!PlayerState.Instance.CheckMpPoint(PSYMP))
            {
                OnPSYExit(point, targetlayer);
                IsActive = false;
                ChangeTimeScale(false);
                return;
            }
            timer = 1;
        }
    }

    public override void OnPSYExit(Vector3 point, LayerMask targetlayer)
    {
        if (!IsActive) return;
        ChangeTimeScale(false);
    }

    public override void OnPSYEngineUpdate(Vector3 point, LayerMask targetlayer) { }

    public override void OnPSYInit() { }

    /// <summary>
    /// 타임 조절 ( true = > 슬로우  /  false = > 리얼타임)
    /// </summary>
    /// <param name="isSlow"></param>
    private void ChangeTimeScale(bool isSlow)
    {
        if (isSlow)
        {
            Time.timeScale = _timeScale;
        }
        else
        {
            Time.timeScale = 1;
        }
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}

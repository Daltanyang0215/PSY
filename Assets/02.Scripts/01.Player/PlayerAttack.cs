using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerAttack : MonoBehaviour
{
    private List<PSYSkillKeySet> _skillKeyboardSets;
    private List<PSYSkillKeySet> _skillMouseSets;

    private Camera _camera;
    private Vector2 _mousePos;

    private KinesisObjectBase target;
    private Vector2 startvec;
    private List<KinesisObjectBase> _kineses = new List<KinesisObjectBase>();

    private void Start()
    {
        _camera = Camera.main;
        _skillKeyboardSets = PlayerState.Instance.SkillKeySets;
        _skillMouseSets = PlayerState.Instance.SkillsMouseSets;
    }

    private void Update()
    {
        KeyBoardSkill();
        MouseSkill();


        //if (Input.GetMouseButtonDown(0))
        //{
        //    startvec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Collider2D enemybullet = Physics2D.OverlapBox(startvec, Vector2.one, 0, PlayerState.Instance.AttackTargetLayer);

        //    if (enemybullet != null && enemybullet.TryGetComponent(out target))
        //    {
        //        startvec = enemybullet.transform.position;
        //        Time.timeScale = 0;
        //    }
        //}
        //if (Input.GetMouseButton(0))
        //{
        //    Debug.DrawLine(startvec, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.red);
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (target != null)
        //    {
        //        Time.timeScale = 1;
        //        target.AddPSYForce((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - startvec);
        //        target = null;
        //    }
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    startvec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Collider2D enemybullet = Physics2D.OverlapBox(startvec, Vector2.one, 0, PlayerState.Instance.AttackTargetLayer);

        //    if (enemybullet != null && enemybullet.TryGetComponent(out target))
        //    {
        //        target.StopPSYForce(true);
        //        target.SetPSYPranet(transform);
        //        _kineses.Add(target);
        //    }
        //    else if(_kineses.Count > 0)
        //    {
        //        _kineses[0].SetPSYPranet(null);
        //        _kineses[0].AddPSYForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - (transform.position + Vector3.up)).normalized*10);
        //        _kineses.RemoveAt(0);
        //    }
        //}
    }

    public void KeyBoardSkill()
    {
        foreach (PSYSkillKeySet skillSet in _skillKeyboardSets)
        {
            if (Input.GetKeyDown(skillSet.inputKey))
            {
                skillSet.skill.OnPSYEnter(transform.position, PlayerState.Instance.AttackTargetLayer);
            }
            else if (Input.GetKey(skillSet.inputKey))
            {
                skillSet.skill.OnPSYUpdate(transform.position, PlayerState.Instance.AttackTargetLayer);
            }
            else if (Input.GetKeyUp(skillSet.inputKey))
            {
                skillSet.skill.OnPSYExit(transform.position, PlayerState.Instance.AttackTargetLayer);
            }
            else
            {
                skillSet.skill.OnPSYEngineUpdate(transform.position, PlayerState.Instance.AttackTargetLayer);
            }
        }
    }
    public void MouseSkill()
    {
        _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

        foreach (PSYSkillKeySet skillSet in _skillMouseSets)
        {
            if (Input.GetKeyDown(skillSet.inputKey))
            {
                skillSet.skill.OnPSYEnter(_mousePos, PlayerState.Instance.AttackTargetLayer);
            }
            else if (Input.GetKey(skillSet.inputKey))
            {
                skillSet.skill.OnPSYUpdate(_mousePos, PlayerState.Instance.AttackTargetLayer);
            }
            else if (Input.GetKeyUp(skillSet.inputKey))
            {
                skillSet.skill.OnPSYExit(_mousePos, PlayerState.Instance.AttackTargetLayer);
            }
            else
            {
                skillSet.skill.OnPSYEngineUpdate(_mousePos, PlayerState.Instance.AttackTargetLayer);
            }
        }
    }
}
[Serializable]
public class PSYSkillKeySet
{
    public string name;
    public KeyCode inputKey;
    public PSYSkillBase skill;
}

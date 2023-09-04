using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    private List<PSYSkillKeySet> _skillKeyboardSets;
    private List<PSYSkillKeySet> _skillMouseSets;

    private Camera _camera;
    private Vector2 _mousePos;

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

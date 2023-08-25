using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private static PlayerState instance;
    public static PlayerState Instance
    {
        get
        {
            if(instance == null)
                instance = GameObject.Find("Player").GetComponent<PlayerState>();

            return instance;
        }
    }

    [SerializeField] private int _psyLevel;
    public int PsyLevel => _psyLevel;

    [SerializeField] private int _hpMax;
    [SerializeField] private int _hp;
    [SerializeField] private int _mpMax;
    [SerializeField] private int _mp;

    private void Start()
    {
        _hp = _hpMax;
        _mp = _mpMax;
    }

    public bool CheckMpPoint(int usingMP)
    {
        if(usingMP < _mp) {
            _mp -= usingMP;
            return true;
        }
        return false;
    }
}

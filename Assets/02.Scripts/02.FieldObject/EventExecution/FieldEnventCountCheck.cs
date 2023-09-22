using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEnventCountCheck : FieldEventExecution
{
    [SerializeField] private int _setEventIndex;

    public override void OnEvnentExecution()
    {
        if (_setEventIndex == 0)
            GameManager.Instance.AddEventID(1);
        else
        {
            GameManager.Instance.SetEventID(_setEventIndex);
        }
    }
}

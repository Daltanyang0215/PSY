using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEventConnet : MonoBehaviour
{
    [SerializeField] private FieldEvent[] _fieldEvents ;

    private void LateUpdate()
    {
        foreach (FieldEvent fieldEvent in _fieldEvents)
        {
            // ��� ��ư�� ������������ Ȯ��
            bool allButtonPress = true;
            foreach (FieldPushButton button in fieldEvent.buttons)
            {
                if(button.isPressed == false) // �ϳ��� �ȵǸ� ĵ��
                {
                    allButtonPress = false;
                    break;
                }
            }

            if (allButtonPress)
            {
                // �̺�Ʈ �Ϸ� ����
                foreach(FieldEventExecution execution in fieldEvent.executionEvents)
                {
                    if(execution.IsExecution == false)
                    {
                        execution.OnEvnentExecution();
                    }
                }
            }
            else
            {
                foreach (FieldEventExecution execution in fieldEvent.executionEvents)
                {
                    if (execution.IsExecution == true)
                    {
                        execution.OnEvnentCancle();
                    }
                }
            }
        }
    }
}
[Serializable]
public class FieldEvent
{
    [SerializeField]private string eventName;
    public FieldPushButton[] buttons;
    public FieldEventExecution[] executionEvents;
}


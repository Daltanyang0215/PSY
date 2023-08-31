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
    private void OnDrawGizmos()
    {

        foreach (FieldEvent fieldEvent in _fieldEvents)
        {
        Gizmos.color = Color.cyan;
            for (int i = 0; i < fieldEvent.buttons.Length-1; i++)
            {
                Gizmos.DrawLine(fieldEvent.buttons[i].transform.position, fieldEvent.buttons[i + 1].transform.position);
            }
            Gizmos.color = Color.blue;
            for (int i = 0; i < fieldEvent.executionEvents.Length; i++)
            {
                Gizmos.DrawLine(fieldEvent.buttons[0].transform.position, fieldEvent.executionEvents[i].transform.position);
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


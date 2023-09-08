using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEventConnet : MonoBehaviour
{
    [SerializeField] private FieldEvent[] _fieldEvents;

    private void LateUpdate()
    {
        foreach (FieldEvent fieldEvent in _fieldEvents)
        {
            // 모든 버튼을 눌린상태인지 확인
            bool allButtonPress = true;
            foreach (FieldEventTrigger trigger in fieldEvent.triggers)
            {
                if (trigger.isPressed == false) // 하나라도 안되면 캔슬
                {
                    allButtonPress = false;
                    break;
                }
            }

            if (allButtonPress)
            {
                // 이벤트 완료 실행
                foreach (FieldEventExecution execution in fieldEvent.executionEvents)
                {
                    if (execution.IsExecution == false)
                    {
                        if (fieldEvent.isOnce && fieldEvent.isAction == true)
                        {
                            return;
                        }

                        fieldEvent.isAction = true;
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
                        if (!fieldEvent.isOnce)
                            fieldEvent.isAction = false;


                        execution.OnEvnentCancle();
                    }
                }
            }
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        foreach (FieldEvent fieldEvent in _fieldEvents)
        {
            if (fieldEvent.triggers.Length == 0 || fieldEvent.executionEvents.Length == 0) return;

            Gizmos.color = Color.cyan;
            for (int i = 0; i < fieldEvent.triggers.Length - 1; i++)
            {
                if (fieldEvent.triggers[i] != null)
                    Gizmos.DrawLine(fieldEvent.triggers[i].transform.position, fieldEvent.triggers[i + 1].transform.position);
            }
            Gizmos.color = Color.blue;
            for (int i = 0; i < fieldEvent.executionEvents.Length; i++)
            {
                if (fieldEvent.executionEvents[i] != null)
                    Gizmos.DrawLine(fieldEvent.triggers[0].transform.position, fieldEvent.executionEvents[i].transform.position);
            }
        }
    }
}
[Serializable]
public class FieldEvent
{
    [SerializeField] private string eventName;
    public bool isOnce;
    [HideInInspector] public bool isAction;
    public FieldEventTrigger[] triggers;
    public FieldEventExecution[] executionEvents;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldEventExecution : MonoBehaviour
{
    public bool IsExecution { get; private set; }
    
    public virtual void OnEvnentExecution()
    {
        IsExecution = true;
        Debug.Log(gameObject.name + "EventExcution");
    }
    public virtual void OnEvnentCancle()
    {
        IsExecution = false;
        Debug.Log(gameObject.name + "EventCancle");
    }
}

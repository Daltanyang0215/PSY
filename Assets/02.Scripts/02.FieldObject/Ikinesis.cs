using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ikinesis
{
    public int PSYLevel { get;}
    public Transform PSYPranet { get; }
    public void AddPSYForce(Vector2 vector);
    public void StopPSYForce(bool notGravite = false);
    public void SetPSYPranet(Transform pranet);
}

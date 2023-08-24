using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ikinesis
{
    public Transform PSYPranet { get; set; }
    public Vector2 MoveVec {  get; set; }
    public float Mass { get; set; }
    public void AddPSYForce(Vector2 vector);
    public void StopPSYForce(bool notGravite = false);
    public void SetPSYPranet(Transform pranet);
}

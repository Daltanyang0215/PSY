using UnityEngine;

public interface IPSYSkill
{
    public int PSYLevel { get; }
    public int PSYMP { get; }

    public void OnPSYEnter(Vector3 point, LayerMask targetlayer);
    public void OnPSYUpdate(Vector3 point, LayerMask targetlayer);
    public void OnPSYExit(Vector3 point, LayerMask targetlayer);

}

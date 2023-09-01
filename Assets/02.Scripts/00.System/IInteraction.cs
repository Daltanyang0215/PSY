
public interface IInteraction
{
    public bool IsCanInteraction { get;}

    public void OnInteractionZoneEnter();
    public void OnInteraction();
    public void OnInteractionZoneExit();
}

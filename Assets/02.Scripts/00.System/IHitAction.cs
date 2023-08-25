public interface IHitAction
{
    public OrderType Order { get;}
    public void OnHit(int damage);
}

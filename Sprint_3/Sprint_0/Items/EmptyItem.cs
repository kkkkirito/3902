//Dillon Brigode AU 2025
public sealed class EmptyItem : IUsableItem
{
    public static EmptyItem Instance = new();
    public string Name => "Empty";
    public bool IsConsumable => false;
    public bool CanUse(IPlayer player) => false;
    public void Use(IPlayer player) { /* no-op */ }
    public EmptyItem() { }
}
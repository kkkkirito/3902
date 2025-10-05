//Dillon Brigode AU 2025

public interface IItem
    {
        string Name { get; }
        bool IsConsumable { get;}
    }

public interface IUsableItem : IItem
{
    void Use(IPlayer player);
    bool CanUse(IPlayer player);
}

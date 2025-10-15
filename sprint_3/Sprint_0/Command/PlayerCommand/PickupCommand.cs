using Sprint_0.Interfaces;
public class PickupCommand : ICommand
{
    private readonly IPlayer _player;
    public PickupCommand(IPlayer player)
    {
        _player = player;
    }

    public void Execute()
    {
        var item = _player.HeldItem;
        _player.Pickup(item);
    }
}
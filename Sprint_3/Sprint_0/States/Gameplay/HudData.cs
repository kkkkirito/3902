namespace Sprint_0.States.Gameplay{
    public sealed class HudData
{
    public int CurrentHP { get; init; }
    public int MaxHP { get; init; }
    public int CurrentMP { get; init; }
    public int MaxMP { get; init; }
    public int XP { get; init; }
    public int XPToNext { get; init; }
    public int Lives { get; init; }
    public string RoomName { get; init; } = "";
}
}
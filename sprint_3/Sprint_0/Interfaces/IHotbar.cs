

namespace Sprint_0.Interfaces
{
    public interface IHotbar
    {
        int SlotCount { get; }
        IUsableItem GetSlot(int index);
        void SetSlot(int index, IUsableItem item);
        void UseSlot(int index, IPlayer player);

    }
}

using Sprint_0.Interfaces;


namespace Sprint_0.Player
{
    public class Hotbar : IHotbar
    {
        private readonly IUsableItem[] _slots;
        public int SlotCount => _slots.Length;

        public Hotbar(int slotCount = 3)
        {
            _slots = new IUsableItem[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                _slots[i] = EmptyItem.Instance;
            }

        }
        public IUsableItem GetSlot(int index) => _slots[index];

        public void SetSlot(int index, IUsableItem item)
        {
            _slots[index] = item ?? EmptyItem.Instance;
        }

        public void UseSlot(int index,  IPlayer player)
        {
            if (index < 0 || index >= SlotCount) return;

            var item = _slots[index];

            if (!item.CanUse(player)) return;

            item.Use(player);
            if (item.IsConsumable)
            {
                //placeholder not gonna work for items that stack
                _slots[index] = EmptyItem.Instance;
            }
        }
    }
}

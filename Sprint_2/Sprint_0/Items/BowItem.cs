using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0.Items
{
    public class BowItem: IUsableItem
    {
        public string Name => "Bow";
        public bool IsConsumable => false;
        private int _count;
        private int _arrows;

        public BowItem(int count)
        {
            _count = count;
        }

        public bool CanUse(IPlayer player) => _arrows > 0;

        public void Use(IPlayer player)
        {
            _arrows--;
            //world.SpawnArrow(player.Position, player.FacingDirection, player.ArrowSpeed);
        }
    }
}

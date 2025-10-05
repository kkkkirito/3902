using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sprint_0.Items
{
    public class BombItem : IUsableItem
    {
        private int _count;
        public string Name => "Bomb";
        public bool IsConsumable => true;

        public BombItem(int count) => _count = count;

        public bool CanUse(IPlayer player) => _count > 0;

        public void Use(IPlayer player)
        {
            //world.SpawnBomb(player.Position, player.FacingDirection);
            _count--;
        }
    }
}

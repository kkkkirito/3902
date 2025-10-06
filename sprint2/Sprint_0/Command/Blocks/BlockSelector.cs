using Microsoft.Xna.Framework;

namespace Sprint_0.Blocks
{
    public class BlockSelector
    {
        private readonly BlockFactory factory;
        private int index;
        public int Index => index;
        public BlockSelector(BlockFactory factory)
        {
            this.factory = factory;
            index = 0;
        }
        public void Next()
        {
            if (factory.Count == 0) return;
            index = (index + 1) % factory.Count;
        }
        public void Prev()
        {
            if (factory.Count == 0) return;
            index = (index - 1 + factory.Count) % factory.Count;
        }
        public Interfaces.IBlock CreateCurrent(Vector2 position)
        {
            return factory.CreateByIndex(index, position, isSolid: true);
        }
    }
}

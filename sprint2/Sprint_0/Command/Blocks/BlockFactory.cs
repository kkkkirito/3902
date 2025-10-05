using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Interfaces;

namespace Sprint_0.Blocks
{
    public class BlockFactory
    {
        private readonly Texture2D blockText;
        private readonly List<Rectangle> blockRects = new List<Rectangle>();
        public Texture2D Texture => blockText;
        public int Count => blockRects.Count;
        public Rectangle GetSourceByIndex(int i)
        {
            if (blockRects.Count == 0) return Rectangle.Empty;
            i = Math.Max(0, Math.Min(i, blockRects.Count - 1));
            return blockRects[i];
        }
        public BlockFactory(ContentManager content)
        {
            blockText = content.Load<Texture2D>("Clear_Zelda_2_Palace_Blocks 2");
            blockRects.Add(new Rectangle(17, 20, 17, 17));
            blockRects.Add(new Rectangle(68, 20, 17, 17));
            blockRects.Add(new Rectangle(102, 20, 17, 17));
            blockRects.Add(new Rectangle(119, 20, 17, 17));
            blockRects.Add(new Rectangle(136, 20, 17, 17));
            blockRects.Add(new Rectangle(153, 20, 17, 17));
            blockRects.Add(new Rectangle(0, 20, 17, 17));
            blockRects.Add(new Rectangle(170, 20, 17, 17));
            blockRects.Add(new Rectangle(225, 20, 17, 17));
            blockRects.Add(new Rectangle(242, 20, 17, 17));
            blockRects.Add(new Rectangle(259, 20, 17, 17));
        }
        public IBlock CreateByIndex(int index, Vector2 position, bool isSolid)
        {
            if (blockRects.Count == 0) return null;
            if (index < 0) index = 0;
            if(index >= blockRects.Count) index = blockRects.Count - 1;
            var frame = blockRects[index];
            var sprite = new StaticStillSprite(blockText, frame);
            return new Block(sprite, position, isSolid);
        }
    }
}

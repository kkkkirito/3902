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
        public int Count => blockRects.Count;
        public BlockFactory(ContentManager content)
        {
            //blockText = content.Load<Texture2D>("Zelda_2_Palace_Blocks 2");
            blockRects.Add(new Rectangle(0, 0, 16, 16));
            blockRects.Add(new Rectangle(0, 0, 16, 16));
            blockRects.Add(new Rectangle(0, 0, 16, 16));
            blockRects.Add(new Rectangle(0, 0, 16, 16));
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

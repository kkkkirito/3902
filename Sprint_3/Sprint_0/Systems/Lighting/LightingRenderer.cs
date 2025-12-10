using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0.Rooms;
using System;
using System.Collections.Generic;

namespace Sprint_0.Systems.Lighting
{
    public class LightingRenderer
    {
        private readonly GraphicsDevice graphics;
        private readonly Texture2D radialTexture;
        private readonly RenderTarget2D lightMap;
        private const float DEFAULT_DIM = 0.5f;

        private static readonly BlendState MultiplyBlend = new()
        {
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero,
            ColorBlendFunction = BlendFunction.Add,
        };

        public LightingRenderer(GraphicsDevice graphicsDevice)
        {
            graphics = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            radialTexture = CreateRadialLightTexture(graphics, 256);
            var vp = graphics.Viewport;
            lightMap = new RenderTarget2D(graphics, vp.Width, vp.Height);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, Room room, IPlayer player)
        {
            if (!IsLightingEnabled(room)) return;
            GenerateLightMap(camera, room, player, null);
            RenderOverlay(spriteBatch);
        }

        public void GenerateLightMap(Camera camera, Room room, IPlayer player, IProjectileManager projectiles)
        {
            if (!IsLightingEnabled(room)) return;

            var lights = GatherLights(room, player);
            if (projectiles != null){
                foreach (var pl in projectiles.GetActiveLightSources())
                    lights.Add(new LightSourceData(pl.LightPosition, pl.LightRadius, pl.LightColor, pl.Intensity));
            }
            graphics.SetRenderTarget(lightMap);
            graphics.Clear(GetAmbient(room));

            using (var sb = new SpriteBatch(graphics))
            {
                sb.Begin(blendState: BlendState.Additive, samplerState: SamplerState.LinearClamp, transformMatrix: camera?.TransformMatrix);
                foreach (var l in lights) DrawLight(sb, l);
                sb.End();
            }
            graphics.SetRenderTarget(null);
        }

        // Backwards-compatible API used by GameplayState
        public void DrawShadows(SpriteBatch spriteBatch, Room room)
        {
            if (!IsLightingEnabled(room)) return;
            RenderOverlay(spriteBatch);
        }

        private void RenderOverlay(SpriteBatch spriteBatch)
        {
            var vp = graphics.Viewport;
            spriteBatch.Begin(blendState: MultiplyBlend, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(lightMap, new Rectangle(0, 0, vp.Width, vp.Height), Color.White);
            spriteBatch.End();
        }

        private static bool IsLightingEnabled(Room room)
        {
            if (room == null) return false;
            //we ignore lightingn for exterior rooms 
            if (room.Id == 0 || room.Id == 1) return false;
            var name = room.Name ?? string.Empty;
            var n = name.ToLowerInvariant();
            return !n.Contains("exterior");
        }

        private static Color GetAmbient(Room room)
        {
            float dim = room?.AmbientLightLevel is float v ? MathHelper.Clamp(v, 0f, 1f) : DEFAULT_DIM;
            return new Color(dim, dim, dim, 1f);
        }

        private static List<LightSourceData> GatherLights(Room room, IPlayer player)
        {
            var lights = new List<LightSourceData>();
            foreach (var ls in room.GetActiveLightSources())
                lights.Add(new LightSourceData(ls.LightPosition, ls.LightRadius, ls.LightColor, ls.Intensity));

            if (player != null && player.HasTorch && player.TorchLightRadius > 0f)
            {
                var center = player.Position + new Vector2(8f, 16f);
                lights.Add(new LightSourceData(center, player.TorchLightRadius, new Color(1f, 0.85f, 0.6f, 0.6f), 1f));
            }
            return lights;
        }

        private void DrawLight(SpriteBatch spriteBatch, LightSourceData light)
        {
            float diameter = light.Radius * 2f;
            float scale = diameter / radialTexture.Width;
            var origin = new Vector2(radialTexture.Width / 2f, radialTexture.Height / 2f);
            var tint = light.Color * light.Intensity;
            spriteBatch.Draw(radialTexture, light.Position, null, tint, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        private static Texture2D CreateRadialLightTexture(GraphicsDevice gd, int diameter)
        {
            var tx = new Texture2D(gd, diameter, diameter);
            var colors = new Color[diameter * diameter];
            float r = diameter / 2f; var c = new Vector2(r);
            for (int y = 0; y < diameter; y++)
                for (int x = 0; x < diameter; x++)
                {
                    int i = y * diameter + x;
                    var p = new Vector2(x + 0.5f, y + 0.5f);
                    float d = Vector2.Distance(p, c);
                    float n = MathHelper.Clamp(1f - d / r, 0f, 1f);
                    float t = n * n;
                    colors[i] = new Color(t, t, t, t);
                }
            tx.SetData(colors);
            return tx;
        }
    }
}

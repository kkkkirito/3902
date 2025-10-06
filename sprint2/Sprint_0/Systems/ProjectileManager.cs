// Systems/ProjectileManager.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0;
using System.Collections.Generic;

public sealed class ProjectileManager : IProjectileManager
{
    private readonly Texture2D _linkSheet;
    private readonly List<PlayerProjectile> _projectiles = new();

    private const int MaxBeams = 1;
    private const int MaxFireballs = 2;
    private const float BeamSpeed = 320f;
    private const float FireballSpeed = 220f;

    public ProjectileManager(Texture2D linkSpriteSheet)
    {
        _linkSheet = linkSpriteSheet;
    }

    public void Update(GameTime gameTime)
    {
        for (int i = _projectiles.Count - 1; i >= 0; i--)
        {
            _projectiles[i].Update(gameTime);
            if (!_projectiles[i].IsActive) _projectiles.RemoveAt(i);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var p in _projectiles) p.Draw(spriteBatch);
    }

    public bool TrySpawnSwordBeam(IPlayer player)
    {
        if (Count(ProjectileType.SwordBeam) >= MaxBeams) return false;
        if (player.CurrentHealth < player.MaxHealth) return false;

        var proj = CreateBeam(player);
        _projectiles.Add(proj);
        return true;
    }

    public bool TrySpawnFireball(IPlayer player)
    {
        if (Count(ProjectileType.Fireball) >= MaxFireballs) return false;

        var proj = CreateFireball(player);
        _projectiles.Add(proj);
        return true;
    }

    private int Count(ProjectileType type)
    {
        int c = 0; foreach (var p in _projectiles) if (p.Type == type && p.IsActive) c++;
        return c;
    }

    private PlayerProjectile CreateBeam(IPlayer player)
    {
        var frames = new List<Rectangle>
        {
            SpriteFactory.GetBeamSprite(player.FacingDirection, 0),
            SpriteFactory.GetBeamSprite(player.FacingDirection, 1),
        };
        var anim = new Animation(_linkSheet, frames, 0.07f, true);
        var (spawnPos, vel) = SpawnKinematics(player, BeamSpeed);
        return new PlayerProjectile(anim, spawnPos, vel, ProjectileType.SwordBeam);
    }

    private PlayerProjectile CreateFireball(IPlayer player)
    {
        var frames = new List<Rectangle>
        {
            SpriteFactory.GetFireballSprite(player.FacingDirection, 0),
            SpriteFactory.GetFireballSprite(player.FacingDirection, 1),
        };
        var anim = new Animation(_linkSheet, frames, 0.10f, true);
        var (spawnPos, vel) = SpawnKinematics(player, FireballSpeed);
        return new PlayerProjectile(anim, spawnPos, vel, ProjectileType.Fireball);
    }

    // Small helper to compute spawn offset + velocity by facing/mode (supports both modes)
    private static (Vector2 pos, Vector2 vel) SpawnKinematics(IPlayer player, float speed)
    {
        var pos = player.Position;
        Vector2 dir = Vector2.Zero;

        switch (player.FacingDirection)
        {
            case Direction.Left: dir = new Vector2(-1, 0); pos += new Vector2(-8, -8); break;
            case Direction.Right: dir = new Vector2(1, 0); pos += new Vector2(20, -8); break;
            case Direction.Up: dir = new Vector2(0, -1); pos += new Vector2(4, -24); break;
            case Direction.Down: dir = new Vector2(0, 1); pos += new Vector2(4, 8); break;
        }
        return (pos, dir * speed);
    }
}

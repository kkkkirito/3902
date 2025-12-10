// Systems/ProjectileManager.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_0;
using Sprint_0.Interfaces;
using System.Collections.Generic;

public sealed class ProjectileManager : IProjectileManager
{
    private readonly Texture2D _linkSheet;
    private readonly List<PlayerProjectile> _projectiles = new();

    private const int MaxBeams = 1;
    private const int MaxFireballs = 2;
    private const float BeamSpeed = 320f;
    private const float FireballSpeed = 220f;

    private readonly Game game;

    public ProjectileManager(Texture2D linkSpriteSheet, Game game)
    {
        _linkSheet = linkSpriteSheet;
        this.game = game;
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

    public IEnumerable<ICollidable> GetCollidables()
    {
        foreach (var p in _projectiles)
        {
            if (p.IsActive) yield return p;
        }
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
        return new PlayerProjectile(game, anim, spawnPos, vel, ProjectileType.SwordBeam);
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
        return new PlayerProjectile(game, anim, spawnPos, vel, ProjectileType.Fireball);
    }

    // Small helper to compute spawn offset + velocity by facing/mode (supports both modes)
    private static (Vector2 pos, Vector2 vel) SpawnKinematics(IPlayer player, float speed)
    {
        var body = player.BoundingBox; 
        Vector2 dir;
        Vector2 pos;

        const int sideGap = 2;     // distance from player’s edge horizontally
        const int upGap = 6;       // vertical center offset for side shots
        const int upDownXPad = 4;  // horizontal pad for up/down shots
        const int upPad = 10;      // distance above the head
        const int downPad = 2;     // distance below the feet

        switch (player.FacingDirection)
        {
            case Direction.Right:
                dir = new Vector2(1, 0);
                pos = new Vector2(body.Right + sideGap, body.Center.Y - upGap);
                break;

            case Direction.Left:
                dir = new Vector2(-1, 0);
                pos = new Vector2(body.Left - sideGap - 8, body.Center.Y - upGap);
                break;

            case Direction.Up:
                dir = new Vector2(0, -1);
                pos = new Vector2(body.Center.X - upDownXPad, body.Top - upPad);
                break;

            case Direction.Down:
            default:
                dir = new Vector2(0, 1);
                pos = new Vector2(body.Center.X - upDownXPad, body.Bottom + downPad);
                break;
        }
        return (pos, dir * speed);
    }

    public IEnumerable<ILightSource> GetActiveLightSources()
    {
        foreach (var p in _projectiles)
        {
            if (p.IsActive && p is ILightSource ls && ls.IsLightActive)
                yield return ls;
        }
    }
}

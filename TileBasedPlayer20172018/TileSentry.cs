using AnimatedSprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiling;

namespace Tiler
{

    public class TileSentry : RotatingSprite
    {
        //List<TileRef> images = new List<TileRef>() { new TileRef(15, 2, 0)};
        //TileRef currentFrame;
        public Vector2 previousPosition;
        public float chaseRadius = 200;
        bool following = false;
        Projectile sentryProjectile;
        Vector2 target;
        float previousAngleOfRotation = 0;

        public TileSentry(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 1;

        }

        public bool inChaseZone(TilePlayer p)
        {
            float distance = Math.Abs(Vector2.Distance(this.PixelPosition, p.PixelPosition));
            if (distance <= chaseRadius)
                return true;
            return false;
        }

        public void follow(TilePlayer p)
        {
            bool inchaseZone = inChaseZone(p);
            if (inchaseZone)
            {
                this.angleOfRotation = TurnToFace(PixelPosition, p.PixelPosition, angleOfRotation, .3f);
                this.following = true;
                target = p.PixelPosition;
            } else
            {
                this.following = false;
            }

        }

        public void LoadProjectile(Projectile r)
        {
            sentryProjectile = r;
            sentryProjectile.DrawOrder = 2;
        }

        public override void Update(GameTime gameTime)
        {

            if (sentryProjectile != null && sentryProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL)
            {
                sentryProjectile.PixelPosition = this.PixelPosition;
                sentryProjectile.hit = false;
                // fire the rocket and it looks for the target
                if (following && previousAngleOfRotation == angleOfRotation)
                    sentryProjectile.fire(target);
            }

            previousAngleOfRotation = angleOfRotation;

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (sentryProjectile != null && sentryProjectile.ProjectileState != Projectile.PROJECTILE_STATE.STILL)
                sentryProjectile.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}

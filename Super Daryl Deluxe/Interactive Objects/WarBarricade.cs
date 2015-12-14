using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    class WarBarricade : BreakableObject
    {
        new public enum State
        {
            normal,
            beingDestroyed,
            destroyed
        }
        public State state;

        int moveframe;
        int frameDelay = 5;
        MapClass thisMap;

        public Texture2D cannonballSprite;

        Platform nonPassableWall;

        public WarBarricade(Game1 g, int x, int y, Texture2D s, int hlthDrop, Texture2D cannonballSheet, MapClass thisMap)
            : base(g, x, y, s, true, 1, hlthDrop, 0, false)
        {
            rec = new Rectangle(x, y, 508, 720);
            cannonballSprite = cannonballSheet;
            this.thisMap = thisMap;

            nonPassableWall = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(x + 154, y, 200, 720), false, false, true);

            if (!thisMap.Platforms.Contains(nonPassableWall))
                thisMap.Platforms.Add(nonPassableWall);
        }

        public override Rectangle GetSourceRec()
        {
            return new Rectangle(0, 0, 508, 720);
        }

        public Rectangle GetCannonballSourceRec()
        {
            if(moveframe < 4)
                return new Rectangle(moveframe * 921, 0, 921, 720);
            else if (moveframe < 8)
                return new Rectangle((moveframe - 4) * 921, 720, 921, 720);
            else if (moveframe < 12)
                return new Rectangle((moveframe - 8) * 921, 1440, 921, 720);
            else
                return new Rectangle((moveframe - 12) * 921, 2160, 921, 720);
        }

        public override void Update()
        {
            if (state == State.beingDestroyed)
            {
                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveframe++;
                    frameDelay = 5;

                    if (moveframe == 8)
                    {
                        DropHealth();
                        DropMoney();
                        isHidden = true;
                        Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.X + 128, rec.Y + 380, 270, 270), 3);
                        game.Camera.ShakeCamera(10, 15);
                    }
                    else if (moveframe == 15)
                    {
                        state = State.destroyed;

                        if (thisMap.Platforms.Contains(nonPassableWall))
                            thisMap.Platforms.Remove(nonPassableWall);
                    }
                }
            }

            if (!finished)
            {
                if (state == State.destroyed && finished == false)
                {
                    finished = true;
                }
            }

            base.Update();
        }

        public void RemoveBarricadeSilently()
        {
            finished = true;

            state = State.destroyed;

            if (thisMap.Platforms.Contains(nonPassableWall))
                thisMap.Platforms.Remove(nonPassableWall);

            isHidden = true;

        }

        public void DestroyBarricade()
        {
            state = State.beingDestroyed;

        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (state == State.beingDestroyed)
            {
                s.Draw(cannonballSprite, new Rectangle(rec.X - 206, rec.Y, 921, 720), GetCannonballSourceRec(), Color.White);
            }
        }
    }
}

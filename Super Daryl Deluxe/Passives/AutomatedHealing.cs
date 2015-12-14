using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class AutomatedHealing : Passive
    {

        int frame, frameDelay = 5;
        Boolean active = true;

        double currentTime = 0;
        public AutomatedHealing(Game1 g)
            : base(g)
        {
            name = "Automated Healing";
        }

        public override void LoadPassive()
        {
            base.LoadPassive();

            spriteSheet = content.Load<Texture2D>(@"SpriteSheets\Passives\AutomatedHealing");
        }

        public override void UnloadPassive()
        {
            base.UnloadPassive();

            currentTime = 0;
            active = false;
            frame = 0;
            frameDelay = 0;
        }

        public override void Update()
        {
            base.Update();
            
            if (game.CurrentChapter.state == Chapter.GameState.Game)
            {
                currentTime += Game1.gameTimeData.ElapsedGameTime.TotalMinutes;

                if (currentTime >= .33)
                {
                    active = true;
                    currentTime = 0;
                }
            }

            if (active)
            {
                HealPlayer();
            }

        }

        public void HealPlayer()
        {
            frameDelay--;

            if (frameDelay <= 0)
            {
                frame++;
                frameDelay = 5;

                if (frame == 3)
                {
                    Game1.Player.Health += (int)(Game1.Player.realMaxHealth * .05f);
                }

                if (frame > 10)
                {
                    frame = 0;
                    active = false;
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (active)
            {
                if (Game1.Player.FacingRight)
                    s.Draw(spriteSheet, new Rectangle(Game1.Player.RecX + 64, Game1.Player.RecY + 145, 385, 232), GetSourceRec(), Color.White);
                else
                    s.Draw(spriteSheet, new Rectangle(Game1.Player.RecX + 64, Game1.Player.RecY + 145, 385, 232), GetSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }

        public override void DrawBehindPlayer(SpriteBatch s)
        {
            base.DrawBehindPlayer(s);

            if (active)
            {
                if (Game1.Player.FacingRight)
                    s.Draw(spriteSheet, new Rectangle(Game1.Player.RecX + 64, Game1.Player.RecY + 145, 385, 232), GetBackgroundSourceRec(), Color.White);
                else
                    s.Draw(spriteSheet, new Rectangle(Game1.Player.RecX + 64, Game1.Player.RecY + 145, 385, 232), GetBackgroundSourceRec(), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
            }
        }

        public Rectangle GetBackgroundSourceRec()
        {
            return new Rectangle(frame * 385, 0, 385, 232);
        }

        public Rectangle GetSourceRec()
        {
            if (frame != 10)
                return new Rectangle(frame * 385, 232, 385, 232);
            else
                return new Rectangle(0, 464, 385, 232);
        }
    }
}

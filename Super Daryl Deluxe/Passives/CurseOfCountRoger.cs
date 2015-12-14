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
    public class CurseOfCountRoger : Passive
    {
        int timeBeforeDamage = 60;

        public CurseOfCountRoger(Game1 g)
            : base(g)
        {
            name = "Curse of Count Roger";
            defenseModifier = 5;
        }

        public override void LoadPassive()
        {
            base.LoadPassive();
        }

        public override void Update()
        {
            base.Update();

            if (game.CurrentChapter.CurrentMap.MapName == "The Quad" || game.CurrentChapter.CurrentMap.MapName == "The Far Side" || (game.CurrentChapter.CurrentMap.MapName == "Main Lobby" && Game1.Player.VitalRec.Intersects(MainLobby.sunRec)) || (game.CurrentChapter.CurrentMap.MapName == "Gym Lobby" && Game1.Player.VitalRec.Intersects(GymLobby.sunRec)))
            {
                timeBeforeDamage--;

                if (timeBeforeDamage == 0)
                {
                    timeBeforeDamage = 60;

                    Player.damageAlpha = .6f;

                    Game1.Player.Health -= (int)(Game1.Player.realMaxHealth / 20f);

                    Game1.Player.AddDamageNum((int)(Game1.Player.realMaxHealth / 20f));
                }
            }
        }
    }
}

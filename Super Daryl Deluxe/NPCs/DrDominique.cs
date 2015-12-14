using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class DrDominique : NPC
    {
        //--Constructor for non-Quest NPC
        public DrDominique(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {

        }

        //--Constructor for quest NPC
        public DrDominique(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {

        }

        public Boolean goblinified = false;

        public override Rectangle GetSourceRectangle(int frame)
        {

            if (goblinified)
            {
                Game1.npcHeightFromRecTop["Dr. Dominique Jean Larrey"] = 210;
                currentDialogueFace = "Goblin";
                return new Rectangle(516, 0, 516, 388);
            }
            else
            {
                currentDialogueFace = "Normal";

                return new Rectangle(0, 0, 516, 388);
            }
        }

        public override void Update()
        {
            base.Update();

            if (!goblinified && game.ChapterTwo.ChapterTwoBooleans["larreyTransformedToGoblin"])
                goblinified = true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class PeltKid : NPC
    {

        Sparkles sparkles;
        Rectangle smokeRec;
        int smokeFrame;
        int smokeTimer = 3;

                //--Constructor for quest NPC
        public PeltKid(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {
            sparkles = new Sparkles(1000, 475);
        }

        //--Constructor for non-Quest NPC
        public PeltKid(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {

        }

        Boolean dead = false;

        public Boolean Dead { get { return dead; } set { dead = value; } }

        public override Rectangle GetSourceRectangle(int frame)
        {

            if (!game.MapBooleans.chapterTwoMapBooleans["FoundPeltHat"] && dead)
            {
                sparkles.Update();
            }

            if (dead)
            {
                //Can't talk to him if he's dead
                if (canTalk)
                    canTalk = false;


                return new Rectangle(518, 0, 518, 388);
            }

            else
                return new Rectangle(0, 0, 518, 388);
        }

        public override void Update()
        {
            base.Update();

            smokeTimer--;

            if (smokeTimer == 0)
            {
                smokeFrame++;
                smokeTimer = 9;

                if (smokeFrame > 5)
                {
                    smokeFrame = 0;
                    smokeTimer = 3;
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {

            if (dead && game.CurrentChapter.CurrentMap.MapName == mapName)
            {
                facingRight = true;
                s.Draw(spriteSheet, new Rectangle(rec.X + 70, rec.Y + 130, 290, 193), new Rectangle(516 * smokeFrame, 388, 516, 472), Color.White);
            }

            base.Draw(s);

            if (!game.MapBooleans.chapterTwoMapBooleans["FoundPeltHat"] && game.CurrentChapter.CurrentMap.MapName == mapName && dead)
            {
                s.Draw(spriteSheet, new Rectangle(rec.X + 200, rec.Y + 230, 60, 70), new Rectangle(1140, 0, 60, 70), Color.White);
            }



            if (!game.MapBooleans.chapterTwoMapBooleans["FoundPeltHat"] && dead && game.CurrentChapter.CurrentMap.MapName == mapName)
            {
                sparkles.Draw(s);
            }
        }
    }

    class Callyn : NPC
    {                //--Constructor for quest NPC
        public Callyn(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {

        }

        //--Constructor for non-Quest NPC
        public Callyn(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {

        }
        Boolean gaveGoggles = false;

        public Boolean GaveGoggles { get { return gaveGoggles; } set { gaveGoggles = value; } }

        public override Rectangle GetSourceRectangle(int frame)
        {

            if (gaveGoggles)
                return new Rectangle(0, 388, 518, 388);

            else
                return new Rectangle(0, 0, 518, 388);
        }

    }

    class Julius : NPC
    {
                        //--Constructor for quest NPC
        public Julius(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {

        }

        //--Constructor for non-Quest NPC
        public Julius(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {

        }
        Boolean gaveToga = false;

        public Boolean GaveToga { get { return gaveToga; } set { gaveToga = value; } }

        public override Rectangle GetSourceRectangle(int frame)
        {

            if (gaveToga)
            {
                //Make his dialogue face the naked one
                if (currentDialogueFace != "Naked")
                    currentDialogueFace = "Naked";

                return new Rectangle(518, 0, 518, 388);
            }

            else
                return new Rectangle(0, 0, 518, 388);
        }

    }

    class Chelsea : NPC
    {
        //--Constructor for quest NPC
        public Chelsea(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {

        }

        //--Constructor for non-Quest NPC
        public Chelsea(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {
        }


        public override Rectangle GetSourceRectangle(int frame)
        {
                return new Rectangle(0, 0, 518, 388);
        }

    }

    class Jesse : NPC
    {
        //--Constructor for quest NPC
        public Jesse(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {

        }

        //--Constructor for non-Quest NPC
        public Jesse(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {
        }


        public override Rectangle GetSourceRectangle(int frame)
        {
                return new Rectangle(0, 0, 518, 388);
        }

    }

    class BridgeKid : NPC
    {
        //--Constructor for quest NPC
        public BridgeKid(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {

        }

        //--Constructor for non-Quest NPC
        public BridgeKid(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {
        }


        public override Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(0, 0, 518, 388);
        }

    }

    class Mark : NPC
    {
        //--Constructor for quest NPC
        public Mark(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {

        }

        //--Constructor for non-Quest NPC
        public Mark(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {
        }


        public override Rectangle GetSourceRectangle(int frame)
        {
            return new Rectangle(0, 0, 518, 388);
        }


    }

    class Balto : NPC
    {
        public enum NPCState
        {
            falling,
            ground
        }
        public NPCState state;

        int onGroundDialogueState = 0;

        //--Constructor for quest NPC
        public Balto(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {

        }

        //--Constructor for non-Quest NPC
        public Balto(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {
        }

        //--Check if the player is talking to the NPC
        public override void CheckInteraction()
        {
            last = current;
            current = Keyboard.GetState();

            //--Get the distance from daryl to the NPC
            Point distanceFromNPC = new Point(Math.Abs(player.VitalRec.Center.X - rec.Center.X),
            Math.Abs(player.VitalRec.Center.Y - rec.Center.Y));

            if (distanceFromNPC.X < 70 && distanceFromNPC.Y < 130 && state != NPCState.falling)
            {
                if (last.IsKeyDown(Keys.F) && current.IsKeyUp(Keys.F) && Game1.spokeThisFrame == false && player.CurrentPlat != null)
                {
                    game.CurrentChapter.TalkingToNPC = true;
                    talking = true;
                    Game1.spokeThisFrame = true;

                    if (game.ChapterTwo.ChapterTwoBooleans["baltoOnGround"])
                    {
                        onGroundDialogueState++;

                        if (onGroundDialogueState == 1)
                        {
                            dialogue[0] = ". . .";
                        }
                        else if (onGroundDialogueState == 2)
                        {
                            dialogue[0] = ". . . . . .";
                        }
                        else if (onGroundDialogueState == 3)
                        {
                            dialogue[0] = ". . .Go get help";
                        }
                    }
                }

                drawFButton = true;
            }
            else
                drawFButton = false;
        }


        public override Rectangle GetSourceRectangle(int frame)
        {
            switch (state)
            {
                case NPCState.falling:
                    return new Rectangle(0, 0, 518, 388);

                case NPCState.ground:
                    return new Rectangle(518, 0, 518, 388);
            }

            return new Rectangle();

        }
    }
}

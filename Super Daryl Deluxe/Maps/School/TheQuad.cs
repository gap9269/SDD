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
    public class TheQuad : MapClass
    {
        static Portal toMainLobby;
        static Portal toFarSide;

        int blinkTimer;

        public static Portal ToMainLobby { get { return toMainLobby; } }
        public static Portal ToFarSide { get { return toFarSide; } }

        Texture2D middle, fore, cameraBlink;

        Dictionary<String, Texture2D> birdStealTextures;

        Boolean birdStealingPaper = false;
        int birdFrame;
        int birdDelay = 5;

        public TheQuad(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 2000;
            mapHeight = 720;
            mapName = "The Quad";
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            enemyAmount = 0;
            AddPlatforms();
            AddBounds();
            SetPortals();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            if(birdStealTextures != null)
                birdStealTextures.Clear();
        }

        public override void Update()
        {
            base.Update();

            blinkTimer++;

            if (blinkTimer > 60)
                blinkTimer = 0;

            if (game.Prologue.PrologueBooleans["secondSceneNotPlayed"] && player.VitalRecX > 270)
            {
                game.Prologue.PrologueBooleans["secondSceneNotPlayed"] = false;
                player.CanJump = true;
                birdStealingPaper = true;
            }

            if (birdStealingPaper)
            {
                birdDelay--;

                if (birdDelay <= 0)
                {
                    birdFrame++;
                    birdDelay = 4;

                    if (birdFrame > 26)
                    {
                        birdStealingPaper = false;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (birdStealingPaper)
            {
                s.Draw(birdStealTextures["bird stealing paper" + birdFrame.ToString()], new Vector2(328, 0), Color.White);
            }
            else if (game.Prologue.PrologueBooleans["secondSceneNotPlayed"])
            {
                s.Draw(birdStealTextures["bird stealing paper0"], new Vector2(328, 0), Color.White);
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\quad"));

            middle = content.Load<Texture2D>(@"Maps\School\quadOverlay");
            fore = content.Load<Texture2D>(@"Maps\School\quadFore");
            cameraBlink = content.Load<Texture2D>(@"Maps\School\quadCameraBlink");

            if (game.Prologue.PrologueBooleans["secondSceneNotPlayed"])
            {
                birdStealTextures = ContentLoader.LoadContent(content, "Cutscenes\\BirdSteal");
            }

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                game.NPCSprites["Weapons Master"] = content.Load<Texture2D>(@"NPC\DD\inventory");
                Game1.npcFaces["Weapons Master"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\D&D\Equipment");
            }

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(middle, new Vector2(0, 0), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1.05f, this, game));
            s.Draw(fore, new Vector2(37, 0), Color.White);

            if(blinkTimer < 30)
                s.Draw(cameraBlink, new Vector2(1783, 253), Color.White);
            s.End();
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            if (game.chapterState == Game1.ChapterState.prologue)
            {
                game.NPCSprites["Weapons Master"] = Game1.whiteFilter;
                Game1.npcFaces["Weapons Master"].faces["Normal"] = Game1.whiteFilter;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMainLobby = new Portal(10, platforms[0], "TheQuad");
            toMainLobby.PortalNameYOffset = -90;
            toMainLobby.FButtonYOffset = -95;
            toFarSide = new Portal(1750, platforms[0], "TheQuad");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMainLobby, MainLobby.ToTheQuad);
            portals.Add(toFarSide, TheFarSide.ToTheQuad);
        }
    }
}

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
using System.IO;

namespace ISurvived
{

    public class FlyingBoyd
    {
        Texture2D tex;
        int moveFrame, framedelay;
        public Vector2 pos;
        int speed;
        Boolean flyingLeft;
        
        public FlyingBoyd(Texture2D t, int posY, int posX, int flySpeed, Boolean flyingLeft)
        {
            pos = new Vector2(posX, posY);
            speed = flySpeed;
            tex = t;
            framedelay = 7;
            this.flyingLeft = flyingLeft;
        }

        public void Update()
        {
            framedelay--;

            if (flyingLeft)
                pos.X -= speed;
            else
                pos.X += speed;

            if (framedelay == 0)
            {
                framedelay = 7;
                moveFrame++;

                if (moveFrame == 5)
                    moveFrame = 0;
            }
        }

        public void Draw(SpriteBatch s)
        {
            if (flyingLeft)
                s.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, 37, 35), new Rectangle(150 * moveFrame, 0, 150, 140), Color.White);
            else
                s.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, 37, 35), new Rectangle(150 * moveFrame, 0, 150, 140), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
        }
    }

    public class TheFarSide : MapClass
    {
        static Portal toTheQuad;

        public static Portal ToTheQuad { get { return toTheQuad; } }

        Texture2D fore1, fore2, mid1, mid2, sky, clouds1, clouds2, farClouds1, farClouds2, sky1, sky2, boyd, grave, moon;

        float cloudPos = 93;
        float farCloudPos = 493;

        List<FlyingBoyd> boyds;

        static Random random;
        int timeUntilNextBoyd;
        int maxTimeUntilNextBoyd = 500;

        public TheFarSide(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;

            mapWidth = 7000;
            mapHeight = 4000;
            mapName = "The Far Side";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            zoomLevel = .87f;
            enemyAmount = 0;
            AddPlatforms();
            AddBounds();
            SetPortals();

            boyds = new List<FlyingBoyd>();

            LockerCombo lockerSheet = new LockerCombo(5520, -2350, "Tim", game);
            collectibles.Add(lockerSheet);

            Dandelion d1 = new Dandelion(2200, -220);
            storyItems.Add(d1);

            Dandelion d2 = new Dandelion(4000, -1290);
            storyItems.Add(d2);

            Dandelion d3 = new Dandelion(2950, -2350);
            storyItems.Add(d3);

            random = new Random();

            timeUntilNextBoyd = random.Next(100, maxTimeUntilNextBoyd);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\FarSide\FarSideBack1"));
            background.Add(content.Load<Texture2D>(@"Maps\School\FarSide\FarSideBack2"));

            fore1 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideFore1");
            fore2 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideFore2");

            mid1 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideMiddle1");
            mid2 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideMiddle2");

            clouds1 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideCloseClouds1");
            clouds2 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideCloseClouds2");

            farClouds1 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideFarClouds1");
            farClouds2 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideFarClouds2");

            sky1 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideSky1");
            sky2 = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideSky2");
            moon = content.Load<Texture2D>(@"Maps\School\FarSide\FarSideMoon");

            boyd = content.Load<Texture2D>(@"Maps\School\FarSide\BoydSheet");

            grave = content.Load<Texture2D>(@"Maps\School\FarSide\grave");

            game.NPCSprites["Death"] = content.Load<Texture2D>(@"NPC\Main\Death");

            Game1.npcFaces["Death"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\DeathNormal");
            Game1.npcFaces["Dandelion"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Dandelion");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Death"] = Game1.whiteFilter;

            Game1.npcFaces["Death"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Dandelion"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            switch (game.chapterState)
            {
                case Game1.ChapterState.prologue:
                    BennyBeaker en = new BennyBeaker(pos, "Test", game, ref player, this);
                    monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
                    en.Position = new Vector2(monsterX, monsterY);
                    enemiesInMap.Add(en);
                    break;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            boyds.Clear();
        }

        public override void Update()
        {
            base.Update();

            timeUntilNextBoyd--;

            if (Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(storyItems[2].Rec.Center.X, storyItems[2].Rec.Center.Y)) < 300 && storyItems[2].PickedUp == false && player.VitalRecY < -2300)
            {
                Chapter.effectsManager.AddInGameDialogue("Is...is that dirt under your fingernails?", "Dandelion", "Normal", 1);
            }

            if (Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(storyItems[1].Rec.Center.X, storyItems[1].Rec.Center.Y)) < 700 && player.VitalRec.X > storyItems[1].RecX + 250 && storyItems[1].PickedUp == false && player.VitalRecY < -1400)
            {
                Chapter.effectsManager.AddInGameDialogue("I heard Johnny screaming down there! I think someone is picking us. Please, jump over here and protect me! \n\nHold 'Shift' to sprint. You can jump farther that way.", "Dandelion", "Normal", 1);
            }

            if (Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(storyItems[0].Rec.Center.X, storyItems[0].Rec.Center.Y)) < 670 && player.VitalRec.X < storyItems[0].RecX - 250 && storyItems[0].PickedUp == false)
            {
                Chapter.effectsManager.AddInGameDialogue("Hi there, friend! If you promise not to tear me out of the ground by my roots, I'll teach you how to jump! \n\nJust press the 'Up Arrow'!", "Dandelion", "Normal", 1);
            }
            else if (Vector2.Distance(new Vector2(player.VitalRec.Center.X, player.VitalRec.Center.Y), new Vector2(storyItems[0].Rec.Center.X, storyItems[0].Rec.Center.Y)) < 250 && storyItems[0].PickedUp == false)
                Chapter.effectsManager.AddInGameDialogue("Ahh! Getting a little close, there! You promised!", "Dandelion", "Normal", 1);

            //Add boyds to the map at a random Y position
            if (timeUntilNextBoyd <= 0)
            {
                timeUntilNextBoyd = random.Next(100, maxTimeUntilNextBoyd);
                FlyingBoyd flyingBoyd = new FlyingBoyd(boyd, random.Next(mapRec.Y + 300, mapRec.Y + 2500), 7000, 2, true);
                boyds.Add(flyingBoyd);
            }

            //Update boyds
            for (int i = 0; i < boyds.Count; i++)
            {
                boyds[i].Update();

                if (boyds[i].pos.X + 150 < 0)
                {
                    boyds.RemoveAt(i);
                    i--;
                }
            }

            //Dan can't draw trees for shit so we have to hide his mistakes.
            if (game.Camera.center.Y < -2400)
            {
                game.Camera.center.Y = -2400;
            }

            //CLOUD STUFF
            cloudPos -= 1.2f;
            farCloudPos -= .6f;

            if (cloudPos + clouds1.Width + clouds2.Width < 0)
                cloudPos = mapWidth;

            if (farCloudPos + farClouds1.Width + farClouds2.Width < 0)
                farCloudPos = mapWidth;
            toTheQuad.PortalRecX = 75;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheQuad = new Portal(200, platforms[0], "TheFarSide");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheQuad, TheQuad.ToFarSide);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.SideQuestManager.ratQuest.CompletedQuest)
            {
                s.Draw(grave, new Vector2(362, mapRec.Y + 524), Color.White);
            }
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.Transform);
//            for (int i = 0; i < interactiveObjects.Count; i++)
//            {
//                if (interactiveObjects[i].Foreground)
//                {
//                    interactiveObjects[i].Draw(s);
//                }
//            }
//            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
    null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(mid1, new Rectangle(0, mapRec.Y + 553, mid1.Width, mid1.Height), Color.White);
            s.Draw(mid2, new Rectangle(mid1.Width, mapRec.Y + 553, mid2.Width, mid2.Height), Color.White);
            s.Draw(fore1, new Rectangle(0, mapRec.Y, fore1.Width, fore1.Height), Color.White);
            s.Draw(fore2, new Rectangle(fore1.Width, -mapRec.Y, fore2.Width, fore2.Height), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Game1.camera.GetTransform(1.05f, this, game));

            s.Draw(sky1, new Rectangle(0, mapRec.Y, sky1.Width, sky1.Height), Color.White);
            s.Draw(sky2, new Rectangle(sky2.Width, mapRec.Y, sky1.Width, sky1.Height), Color.White);
            s.Draw(moon, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(farClouds1, new Rectangle((int)farCloudPos, mapRec.Y + 1406, farClouds1.Width, farClouds1.Height), Color.White);
            s.Draw(farClouds2, new Rectangle((int)farCloudPos + farClouds1.Width, mapRec.Y + 1406, farClouds2.Width, farClouds2.Height), Color.White);

            s.Draw(clouds1, new Rectangle((int)cloudPos, mapRec.Y + 1492, clouds1.Width, clouds1.Height), Color.White);
            s.Draw(clouds2, new Rectangle((int)cloudPos + clouds1.Width, mapRec.Y + 1492, clouds2.Width, clouds2.Height), Color.White);

            for (int i = 0; i < boyds.Count; i++)
            {
                boyds[i].Draw(s);
            }


//            if (moonIsAngry)
//                s.Draw(moonAngry, new Rectangle(405, mapRec.Y + 590, moonAngry.Width, moonAngry.Height), Color.White * moonFaceAlpha);
//            else
//                s.Draw(moonHappy, new Rectangle(405, mapRec.Y + 590, moonHappy.Width, moonHappy.Height), Color.White * moonFaceAlpha);


            s.End();

//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.GetTransformWithVertParallax(.1f, .95f, this, game));
//            s.Draw(clouds, new Rectangle((int)cloudPos, -725, clouds.Width, clouds.Height), Color.White);
//            s.Draw(clouds2, new Rectangle((int)cloud2Pos, -725, clouds2.Width, clouds2.Height), Color.White);
//            s.Draw(fastClouds, new Rectangle((int)fastCloudPos, -725, fastClouds.Width, fastClouds.Height), Color.White);
//            s.End();

//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.GetTransformWithVertParallax(.1f, .85f, this, game));
//            s.Draw(barn, new Rectangle(-100, -140, barn.Width, barn.Height), Color.White);
//            s.End();

//            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
//null, null, null, null, Game1.camera.GetTransformWithVertParallax(.6f, .92f, this, game));
//            s.Draw(backField, new Rectangle(0, 100, backField.Width, backField.Height), Color.White);
//            s.End();
        }
    }
}

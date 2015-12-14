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
    class GrandCanal : MapClass
    {
        static Portal toBridge;
        static Portal toPlayground;

        public static Portal ToBridge { get { return toBridge; } }
        public static Portal ToPlayground { get { return toPlayground; } }

        Texture2D elevator, sky, foreground, parallax;
        MovingPlatform movingPlat2;
        List<Vector2> targets2;

        float foregroundAlpha = 1f;

        LivingLocker locker;

        public GrandCanal(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 3000;
            mapWidth = 4000;
            mapName = "The Grand Canal";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 10;

            yScroll = true;

            zoomLevel = .95f;

            locker = new LivingLocker(game, new Rectangle(1850, -650, 650, 400));
            interactiveObjects.Add(locker);

            //--Map Quest
            mapWithMapQuest = true;

            enemiesToKill.Add(20);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Slay Dough");

            MapQuestSign sign = new MapQuestSign(215, mapRec.Y + 996, "Kill 20 Slay Dough", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Textbook(), new Experience(350), new Money(3.75f) });
            mapQuestSigns.Add(sign);


            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            targets2 = new List<Vector2>();
            targets2.Add(new Vector2(3225, mapRec.Y + 1272));
            targets2.Add(new Vector2(2897, mapRec.Y + 1014));
            targets2.Add(new Vector2(2667, mapRec.Y + 656));
            targets2.Add(new Vector2(2846, mapRec.Y + 352));
            targets2.Add(new Vector2(2667, mapRec.Y + 656));
            targets2.Add(new Vector2(2897, mapRec.Y + 1014));
            targets2.Add(new Vector2(3225, mapRec.Y + 1272));
            targets2.Add(new Vector2(2846, mapRec.Y + 1550));

            movingPlat2 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2846, mapRec.Y + 1550, 250, 50),
                true, false, false, targets2, 4, 5, Platform.PlatformType.rock);

            platforms.Add(movingPlat2);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\The Grand Canal\background"));

            elevator = content.Load<Texture2D>(@"Maps\Music\The Grand Canal\elevator");
            sky = content.Load<Texture2D>(@"Maps\Music\The Grand Canal\sky");
            foreground = content.Load<Texture2D>(@"Maps\Music\The Grand Canal\foreground");
            parallax = content.Load<Texture2D>(@"Maps\Music\The Grand Canal\parallax");

            game.NPCSprites["Leonardo Da Vinci"] = content.Load<Texture2D>(@"NPC\Music\Da Vinci");
            Game1.npcFaces["Leonardo Da Vinci"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Music\Da Vinci Normal");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SlayDoughEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Leonardo Da Vinci"] = Game1.whiteFilter;
            Game1.npcFaces["Leonardo Da Vinci"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            SlayDough en = new SlayDough(pos, "Slay Dough", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.TimeBeforeSpawn = 60;

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                en.UpdateRectangles();
                AddEnemyToEnemyList(en);
            }

        }

        public override void Update()
        {
            base.Update();

            if (enemyAmount > enemiesInMap.Count)
                RespawnGroundEnemies();

            if (enemiesKilledForQuest[0] >= enemiesToKill[0])
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBridge = new Portal(50, platforms[0], "The Grand Canal");
            toPlayground = new Portal(3850, 269, "The Grand Canal");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBridge, BridgeOfArmanhand.ToRiver);
            portals.Add(toPlayground, ArtistsPlayground.ToGrandCanal);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(elevator, new Vector2(movingPlat2.RecX - 55, movingPlat2.RecY - 20), Color.White);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            s.Draw(foreground, new Vector2(584, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Vector2(0, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.6f, this, game));
            s.Draw(parallax, new Vector2(-200, mapRec.Y), Color.White);
            s.End();
        }
    }
}

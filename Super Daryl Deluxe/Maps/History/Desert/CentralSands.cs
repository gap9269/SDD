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
    class CentralSands : MapClass
    {
        static Portal toWindyDesert;
        static Portal toEgypt;
        public static Portal toRift;

        public static Portal ToEgypt { get { return toEgypt; } }
        public static Portal ToWindyDesert { get { return toWindyDesert; } }

        Texture2D parallax, farParallax, sky;
        Dictionary<String, Texture2D> rip;
        int ripFrame;
        int ripDelay = 5;
        public CentralSands(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4000;
            mapWidth = 9000;
            mapName = "Central Sands";

            zoomLevel = .85f;

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 18;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Sexy Saguaro", 0);
            enemyNamesAndNumberInMap.Add("Burnie Buzzard", 0);
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.SexySaguaroEnemy(content);
            EnemyContentLoader.BurnieBuzzardEnemy(content);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\CentralSands\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\CentralSands\background2"));
            background.Add(content.Load<Texture2D>(@"Maps\History\CentralSands\background3"));
            sky = content.Load<Texture2D>(@"Maps\History\CentralSands\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\CentralSands\parallax");
            farParallax = content.Load<Texture2D>(@"Maps\History\CentralSands\parallaxFar");
            rip = ContentLoader.LoadContent(content, @"Maps\Music\Bridge of Armanhand\rip");

            game.NPCSprites["Portal Repair Specialist"] = content.Load<Texture2D>(@"NPC\Main\Portal Repair Specialist");
            Game1.npcFaces["Portal Repair Specialist"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Portal Repair Specialist Normal");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Portal Repair Specialist"] = Game1.whiteFilter;
            Game1.npcFaces["Portal Repair Specialist"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Sexy Saguaro"] < 10)
            {
                SexySaguaro en = new SexySaguaro(pos, "Sexy Saguaro", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 30;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Sexy Saguaro"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);
            if (enemyNamesAndNumberInMap["Burnie Buzzard"] < 8)
            {
                BurnieBuzzard en = new BurnieBuzzard(pos, "Burnie Buzzard", game, ref player, this, mapRec);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Burnie Buzzard"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount)
            {
                RespawnFlyingEnemies(new Rectangle(6950, mapRec.Y + 350, 5000, mapHeight - 1500));
                RespawnGroundEnemies();
            }

            ripDelay--;

            if (ripDelay <= 0)
            {
                ripFrame++;
                ripDelay = 1;

                if (ripFrame > 31)
                    ripFrame = 0;
            }

            toRift.PortalRecX = 3040;
            if (game.CurrentQuests.Contains(game.SideQuestManager.desertDimensions) && game.ChapterTwo.ChapterTwoBooleans["centralSandsRiftCompleted"] == false)
                toRift.IsUseable = true;
            else
                toRift.IsUseable = false;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toWindyDesert = new Portal(50, platforms[0], "Central Sands");
            toEgypt = new Portal(6950, -2087, "Central Sands");
            toRift = new Portal(3088, -1721, "Central Sands", Portal.DoorType.movement_portal_enter);
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["centralSandsRiftCompleted"] == false)
                s.Draw(rip.ElementAt(ripFrame).Value, new Vector2(2725, mapRec.Y + 728), Color.White);
            else
                s.Draw(rip["dimensional rip healed"], new Vector2(2725, mapRec.Y + 728), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toWindyDesert, WindyDesert.ToCentralSands);
            portals.Add(toEgypt, Egypt.ToCentralSands);
            portals.Add(toRift, CentralSandsRift.ToCentralSands);

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
           // s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.2f, this, game));
            s.Draw(farParallax, new Vector2(0, mapRec.Y), Color.White);

            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.4f, this, game));
            s.Draw(parallax, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }
    }
}

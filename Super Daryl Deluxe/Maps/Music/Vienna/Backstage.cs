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
    class Backstage : MapClass
    {
        static Portal toTheStage;
        static Portal toManagersOffice;
        static Portal toRestrictedHallway;

        public static Portal ToRestrictedHallway { get { return toRestrictedHallway; } }
        public static Portal ToTheStage { get { return toTheStage; } }
        public static Portal ToManagersOffice { get { return toManagersOffice; } }

        Texture2D foreground;

        public Backstage(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Backstage";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 4;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Sergeant Cymbal", 0);
            enemyNamesAndNumberInMap.Add("Maracas Hermanos", 0);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Sergeant Cymbal"] < 1)
            {
                SergeantCymbal en = new SergeantCymbal(pos, "Sergeant Cymbal", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 5;
                en.Hostile = true;
                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    AddEnemyToEnemyList(en);
                    enemyNamesAndNumberInMap["Sergeant Cymbal"]++;
                }
            }

            if (enemyNamesAndNumberInMap["Maracas Hermanos"] < 3)
            {
                MaracasHermanos en = new MaracasHermanos(pos, "Maracas Hermanos", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 5;
                en.Hostile = true;
                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    AddEnemyToEnemyList(en);
                    enemyNamesAndNumberInMap["Maracas Hermanos"]++;
                }
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Backstage\background"));
            foreground = content.Load<Texture2D>(@"Maps\Music\Backstage\foreground");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.MaracasHermanosEnemy(content);
            EnemyContentLoader.SergeantCymbalEnemy(content);
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && spawnEnemies && game.ChapterOne.ChapterOneBooleans["chasingTheManager"] && !game.ChapterOne.ChapterOneBooleans["clearedBackstage"])
            {
                RespawnGroundEnemies();
                toTheStage.IsUseable = false;
                toManagersOffice.IsUseable = false;
            }

            if (enemyAmount == enemiesInMap.Count)
            {
                spawnEnemies = false;
            }

            if (spawnEnemies == false && enemiesInMap.Count == 0 && !game.ChapterOne.ChapterOneBooleans["clearedBackstage"])
            {
                toTheStage.IsUseable = true;
                toManagersOffice.IsUseable = true;
                game.ChapterOne.ChapterOneBooleans["clearedBackstage"] = true;
                player.AddStoryItem("Security Clearance I.D", "Security Clearance", 1);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheStage = new Portal(50, 630, "Backstage");
            toRestrictedHallway = new Portal(1400, 630, "Backstage", "Security Clearance I.D");
            toManagersOffice = new Portal(1800, 630, "Backstage");

            toRestrictedHallway.FButtonYOffset = -35;
            toRestrictedHallway.PortalNameYOffset = -35;

            toManagersOffice.FButtonYOffset = -40;
            toManagersOffice.PortalNameYOffset = -40;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheStage, TheStage.ToBackstage);
            portals.Add(toManagersOffice, ManagersOffice.ToBackstage);
            portals.Add(ToRestrictedHallway, RestrictedHallway.ToBackstage);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }
    }
}

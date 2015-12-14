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
    class TenantHallwayWest : MapClass
    {
        static Portal toSecondFloor;
        static Portal toMozartsRoom;
        static Portal toTchaikovskysRoom;
        static Portal toVacantRoom;
        static Portal toHallEast;
        static Portal toBeethovensRoom;

        public static Portal ToSecondFloor { get { return toSecondFloor; } }
        public static Portal ToMozartsRoom { get { return toMozartsRoom; } }
        public static Portal ToTchaikovskysRoom { get { return toTchaikovskysRoom; } }
        public static Portal ToVacantRoom { get { return toVacantRoom; } }
        public static Portal ToHallEast { get { return toHallEast; } }
        public static Portal ToBeethovensRoom { get { return toBeethovensRoom; } }

        Texture2D hunters, foregroundHunters;

        public GhostSucker ghostSucker;

        int ghostsKilled = 0;

        public TenantHallwayWest(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3500;
            mapName = "Tenant Hallway West";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            if (game.ChapterOne.ChapterOneBooleans["spawnedGhostHunters"] == true)
            {
                hunters = content.Load<Texture2D>(@"Maps\Music\Tenant Hall\hunters");
                foregroundHunters = content.Load<Texture2D>(@"Maps\Music\Tenant Hall\foregroundHunters");

                game.NPCSprites["Jason Mysterio"] = content.Load<Texture2D>(@"NPC\TITS\Jason Mysterio");
                Game1.npcFaces["Jason Mysterio"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\JasonMysterioNormal");

                game.NPCSprites["Claire Voyant"] = content.Load<Texture2D>(@"NPC\TITS\Claire Voyant");
                Game1.npcFaces["Claire Voyant"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\TITS\ClaireVoyantNormal");

                ghostSucker = new GhostSucker(1985, 480, player, this);
                ghostSucker.LoadContent(content);
                ghostSucker.faceTexture = content.Load<Texture2D>(@"Bosses\GhostSuckerFace");

            }

            background.Add(content.Load<Texture2D>(@"Maps\Music\Tenant Hall\Tenant Hallway"));

        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Jason Mysterio"] = Game1.whiteFilter;
            Game1.npcFaces["Jason Mysterio"].faces["Normal"] = Game1.whiteFilter;

            game.NPCSprites["Claire Voyant"] = Game1.whiteFilter;
            Game1.npcFaces["Claire Voyant"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void ResetEnemyNamesAndNumberInMap()
        {
            base.ResetEnemyNamesAndNumberInMap();

            ghostsKilled = 0;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.TubaGhostEnemy(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();


            TubaGhost en = new TubaGhost(pos, "Tuba Ghost", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.TimeBeforeSpawn = 120;
            en.objectToAttack = ghostSucker;

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                en.UpdateRectangles();
                AddEnemyToEnemyList(en);
                ghostsKilled++;

                if (ghostsKilled >= 15)
                {
                    spawnEnemies = false;
                }
            }

        }

        public override void Update()
        {
            base.Update();

            if (game.CurrentChapter.BossFight && enemiesInMap.Count < enemyAmount && spawnEnemies && game.ChapterOne.ChapterOneBooleans["savedGhostHunters"] == false)
            {
                RespawnGroundEnemies();
            }

            if (enemiesInMap.Count == 0 && ghostsKilled > 1 && game.ChapterOne.ChapterOneBooleans["savedGhostHunters"] == false)
            {
                game.ChapterOne.ChapterOneBooleans["savedGhostHunters"] = true;

                for (int i = 0; i < portals.Count; i++)
                {
                    portals.ElementAt(i).Key.IsUseable = true;
                }

                game.ChapterOne.BossFight = false;
                game.ChapterOne.CurrentBoss = null;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSecondFloor = new Portal(75, platforms[0], "Tenant Hallway West");
            toTchaikovskysRoom = new Portal(502, platforms[0], "Tenant Hallway West");
            toMozartsRoom = new Portal(1078, platforms[0], "Tenant Hallway West");
            toVacantRoom = new Portal(1655, platforms[0], "Tenant Hallway West");
            toHallEast = new Portal(2810, platforms[0], "Tenant Hallway West");
            toBeethovensRoom = new Portal(2233, platforms[0], "Tenant Hallway West", "Ghost Key");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSecondFloor, SecondFloor.ToTenantFloor);

            portals.Add(toMozartsRoom, TchaikovskysRoom.ToTenantHallway);
            portals.Add(toTchaikovskysRoom, MozartsRoom.ToTenantHallway);
            portals.Add(toVacantRoom, VacantRoom.ToTenantHallway);
            portals.Add(toHallEast, TenantHallwayEast.ToHallwayWest);
            portals.Add(toBeethovensRoom, BeethovensRoom.ToTenantHallway);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (hunters != null && !game.ChapterOne.ChapterOneBooleans["finishedManagerQuest"])
            {
                s.Draw(hunters, new Vector2(2096, 0), Color.White);


                if (ghostSucker != null)
                {
                    ghostSucker.Update(3500);
                    ghostSucker.DrawGhostSucker(s);

                }
            }
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            if (foregroundHunters != null && !game.ChapterOne.ChapterOneBooleans["finishedManagerQuest"])
                s.Draw(foregroundHunters, new Vector2(2096, 0), Color.White);
            s.End();
        }
    }
}

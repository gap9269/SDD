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
    class ScienceChallengeRoomI : MapClass
    {
        static Portal toScience102;

        public static Portal ToScience102 { get { return toScience102; } }

        MovingPlatform movingPlat;
        List<Vector2> targets;

        Textbook t1;
        Textbook t2;

        double timeToComplete = 0;
        int timer;

        public ScienceChallengeRoomI(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2400;
            mapWidth = 3000;
            mapName = "Science Challenge Room I";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 15;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
            targets = new List<Vector2>();
            movingPlat = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2680, 150, 400, 50),
                false, false, false, targets, 3, 100);

            platforms.Add(movingPlat);

            zoomLevel = .90f;

            t1 = new Textbook(1500, 430, 3);
            t2 = new Textbook(1700, 430, 2);

            t1.AbleToPickUp = false;
            t2.AbleToPickUp = false;

            collectibles.Add(t1);
            collectibles.Add(t2);

            MapFire fire = new MapFire(120, 120, new Rectangle(1180, -1000, 100, 1800), game, 5);

            mapHazards.Add(fire);

            enemyNamesAndNumberInMap.Add("Erl The Flask", 0);
            enemyNamesAndNumberInMap.Add("Benny Beaker", 0);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Erl The Flask"] < 8)
            {
                ErlTheFlask erl = new ErlTheFlask(pos, "Erl The Flask", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - erl.Rec.Height - 1;
                erl.Position = new Vector2(monsterX, monsterY);

                Rectangle erlRec = new Rectangle(erl.RecX, monsterY, erl.Rec.Width, erl.Rec.Height);
                if (erlRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemiesInMap.Add(erl);
                    enemyNamesAndNumberInMap["Erl The Flask"]++;
                }
            }
            if (enemyNamesAndNumberInMap["Benny Beaker"] < 7)
            {
                BennyBeaker ben = new BennyBeaker(pos, "Benny Beaker", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
                ben.Position = new Vector2(monsterX, monsterY);

                Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);
                if (benRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemiesInMap.Add(ben);
                    enemyNamesAndNumberInMap["Benny Beaker"]++;
                }

            }
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            platforms.Remove(movingPlat);
            movingPlat = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2680, 200, 400, 50),
                false, false, false, targets, 3, 100);
            platforms.Add(movingPlat);
            game.MapBooleans.chapterOneMapBooleans["scienceTargetsAdded"] = false;
            targets.Clear();


            enemyNamesAndNumberInMap["Benny Beaker"] = 0;
            enemyNamesAndNumberInMap["Erl The Flask"] = 0;

            enemiesInMap.Clear();

            if(game.MapBooleans.chapterOneMapBooleans["scienceChallengeCompleted"] == false)
                spawnEnemies = true;
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
                RespawnGroundEnemies();
            if (enemiesInMap.Count == enemyAmount && game.MapBooleans.chapterOneMapBooleans["scienceChallengeStarted"] == false)
            {
                spawnEnemies = false;
                game.MapBooleans.chapterOneMapBooleans["scienceChallengeStarted"] = true;
                toScience102.ItemNameToUnlock = "clear";
                toScience102.PortalTexture = Game1.lockedPortalTexture;
                game.Camera.ShakeCamera(5, 15);
                timeToComplete = 00.30;
                Chapter.effectsManager.AddTimer(00.30);
            }

            if (game.MapBooleans.chapterOneMapBooleans["scienceChallengeStarted"] == true && game.MapBooleans.chapterOneMapBooleans["scienceChallengeCompleted"] == false)
            {
                if (timer < 60)
                    timer++;

                if (timer >= 60)
                {
                    timeToComplete -= .01;
                    timer = 0;
                }

                if (timeToComplete <= 0)
                {
                    toScience102.ItemNameToUnlock = null;
                    toScience102.PortalTexture = Game1.portalTexture;
                    game.Camera.ShakeCamera(5, 15);
                    game.MapBooleans.chapterOneMapBooleans["scienceChallengeStarted"] = false;
                }

                if (enemiesInMap.Count == 0 && timeToComplete > 0)
                {
                    game.MapBooleans.chapterOneMapBooleans["scienceChallengeCompleted"] = true;
                    collectibles[0].AbleToPickUp = true;
                    collectibles[1].AbleToPickUp = true;
                    toScience102.ItemNameToUnlock = null;
                    toScience102.PortalTexture = Game1.portalTexture;
                    game.Camera.ShakeCamera(5, 15);
                    timeToComplete = 0;
                    Chapter.effectsManager.AddTimer(0);
                }
            }


            if (player.CurrentPlat == movingPlat && game.MapBooleans.chapterOneMapBooleans["scienceTargetsAdded"] == false)
            {
                game.MapBooleans.chapterOneMapBooleans["scienceTargetsAdded"] = true;
            }

            if (game.MapBooleans.chapterOneMapBooleans["scienceTargetsAdded"] && targets.Count == 0)
            {
                targets.Add(new Vector2(2680, -1000));
                targets.Add(new Vector2(2680, 400));
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toScience102 = new Portal(50, platforms[0], "ScienceChallengeRoomI");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toScience102, Science102.ToBathroom);
        }
    }
}
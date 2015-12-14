using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    class SideVentsII : MapClass
    {

        static Portal toSideVentsI;

        public static Portal ToSideVentsI { get { return toSideVentsI; } }


        WallSwitch firstSwitch;
        WallSwitch lastSwitch;
        MapSteam steam;

        Texture2D foreground, foreground2;

        public SideVentsII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            overlayType = 1;
            mapWidth = 4300;
            mapHeight = 1680;
            mapName = "Side Vents II";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);

            AddPlatforms();
            AddBounds();
            SetPortals();

            zoomLevel = .9f;

            enemyAmount = 6;

            firstSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(3600, 320, (int)(333 * .8f), (int)(335 * .8f)), 470);
            lastSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(330, -430, (int)(333 * .8f), (int)(335 * .8f)));

            switches.Add(firstSwitch);
            switches.Add(lastSwitch);

            steam = new MapSteam(120, 100, 370, -490, game, 1, true);

            mapHazards.Add(steam);

            Barrel bar = new Barrel(game, 100, -100, Game1.interactiveObjects["Barrel"], true, 2, 5, 1.38f, true, Barrel.BarrelType.MetalBlank);
            interactiveObjects.Add(bar);

            Barrel bar2 = new Barrel(game, 250, -90, Game1.interactiveObjects["Barrel"], true, 2, 0, .46f, true, Barrel.BarrelType.WoodenRight);
            interactiveObjects.Add(bar2);

            Barrel bar1 = new Barrel(game, 150, -90, Game1.interactiveObjects["Barrel"], true, 2, 0, .76f, true, Barrel.BarrelType.MetalLabel);
            interactiveObjects.Add(bar1);

            enemyNamesAndNumberInMap.Add("Fluffles the Rat", 0);
            enemyNamesAndNumberInMap.Add("Vent Bat", 0);
        }
        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_vents");
        }
        public override void LoadContent()
        {
            Sound.LoadVentZoneSounds();

            foreach (MapSteam s in mapHazards)
            {
                s.object_steam_vent_loop = Sound.mapZoneSoundEffects["object_steam_vent_loop"].CreateInstance();
            }

            background.Add(content.Load<Texture2D>(@"Maps\Vents\Side Vents 2\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Side Vents 2\background2"));
            foreground = content.Load<Texture2D>(@"Maps\Vents\Side Vents 2\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\Vents\Side Vents 2\foreground2");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.BatEnemy(content);
            EnemyContentLoader.FlufflesRat(content); EnemyContentLoader.SharedRatSounds(content);
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            spawnEnemies = true;
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Fluffles the Rat"] < 2)
            {
                FlufflesTheRat en = new FlufflesTheRat(pos, "Fluffles the Rat", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Fluffles the Rat"]++;
                    AddEnemyToEnemyList(en);
                }
            }


        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);
            if (enemyNamesAndNumberInMap["Vent Bat"] < 4)
            {
                Bat en = new Bat(pos, "Vent Bat", game, ref player, this, mapRec, true);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Vent Bat"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            PlayAmbience();

            if (!firstSwitch.Active && !steam.Active)
                steam.TurnOn();

            if (CheckSwitch(firstSwitch))
            {
                if (firstSwitch.Active)
                {
                    foreach (Enemy e in enemiesInMap)
                    {
                        if(e is FlufflesTheRat)
                            e.Hostile = true;
                    }
                    
                    steam.TurnOff();
                }
                else
                    steam.TurnOn();
            }
            if (!lastSwitch.Active && CheckSwitch(lastSwitch))
            {

                if (lastSwitch.Active)
                {
                    firstSwitch.Active = true;
                    steam.TurnOff();
                }
                else
                {
                    firstSwitch.Active = false;
                    steam.TurnOn();
                }
            }



            lastSwitch.Active = firstSwitch.Active;

            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
            {
                RespawnGroundEnemies();
                RespawnFlyingEnemies(new Rectangle(650, -500, 800, 400));
            }
            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSideVentsI = new Portal(4100, 630, "Side Vents II");

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();


            portals.Add(toSideVentsI, SideVentsI.ToSideVentsII);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.End();

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

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            s.End();
        }
    }
}

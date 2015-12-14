using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    class UpperVentsIII : MapClass
    {
        static Portal toUpperVents2;
        static Portal toUpperVents4Bot;
        static Portal toUpperVents4Top;
        static Portal toCoalShaft;
        static Portal toBathroom;

        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToCoalShaft { get { return toCoalShaft; } }
        public static Portal ToUpperVents2 { get { return toUpperVents2; } }
        public static Portal ToUpperVents4Bot { get { return toUpperVents4Bot; } }
        public static Portal ToUpperVents4Top { get { return toUpperVents4Top; } }

        MovingPlatform floor;
        MovingPlatform ceiling;

        List<Vector2> floorTargets;
        List<Vector2> ceilingTargets;

        Texture2D foreground, foreground2, foreground3, fallingVent, outhouse;

        SoundEffect object_vents_platform_fall;

        CoalDeposit coal1;
        Texture2D coalTexture;

        public UpperVentsIII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;
            zoomLevel = .85f;

             mapWidth = 9700;
            mapHeight = 2500;
            mapName = "Upper Vents III";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            overlayType = 1;
            AddPlatforms();
            AddBounds();
            SetPortals();

            enemyAmount = 10;

            floorTargets = new List<Vector2>();
            ceilingTargets = new List<Vector2>();

            floor = new MovingPlatform(Game1.platformTextures.ElementAt(1).Value, new Rectangle(7518, -380, 800, 50), false, false, false, floorTargets, 5, 10);
            ceiling = new MovingPlatform(Game1.platformTextures.ElementAt(1).Value, new Rectangle(7518, -800, 800, 50), false, false, false, ceilingTargets, 3, 50);
            floor.type = Platform.PlatformType.vents;
            ceiling.type = Platform.PlatformType.vents;
            platforms.Add(floor);
            platforms.Add(ceiling);

            TreasureChest chest = new TreasureChest(Game1.treasureChestSheet, 8500, -400, player, 0, new BronzeKey(), this);
            treasureChests.Add(chest);

            coal1 = new CoalDeposit(game, 7594, mapRec.Y + 395, Game1.whiteFilter, 3, new Coal(7594, mapRec.Y + 795), 2, true, true);
            interactiveObjects.Add(coal1);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Upper Vents 3\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Upper Vents 3\background2"));
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Upper Vents 3\background3"));
            //foreground = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 3\foreground");
            //foreground2 = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 3\foreground2");
            //foreground3 = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 3\foreground3");
            fallingVent = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 3\fallingVent");
            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");
            Sound.LoadVentZoneSounds();

            object_vents_platform_fall = content.Load<SoundEffect>("Sound\\Objects\\object_vents_platform_fall");

            coalTexture = content.Load<Texture2D>(@"InteractiveObjects\CoalSprite");
            coal1.Sprite = coalTexture;

            if (Chapter.lastMap != "Upper Vents IV" && Chapter.lastMap != "Coal Shaft" && Chapter.lastMap != "Upper Vents II")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_vents");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_vents", amb);
            }
        }
        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_vents");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            if (Chapter.theNextMap != "Upper Vents IV" && Chapter.theNextMap != "Coal Shaft" && Chapter.theNextMap != "Upper Vents II")
            {
                Sound.UnloadAmbience();
                Sound.UnloadBackgroundMusic();
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

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

            switch (game.chapterState)
            {
                case Game1.ChapterState.chapterOne:
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
                        AddEnemyToEnemyList(en);
                    }
                    break;
            }

        }

        public override void Update()
        {
            base.Update();
            PlayAmbience();
            coal1.RecY = ceiling.RecY - 210;

            if (enemiesInMap.Count < enemyAmount && spawnEnemies == true)
                RespawnGroundEnemies();
            if (enemiesInMap.Count == enemyAmount)
                spawnEnemies = false;

            if (player.CurrentPlat == floor && floor.Velocity.Y == 0 && game.MapBooleans.chapterOneMapBooleans["VentLowered"] == false)
            {
                floor.Velocity = new Vector2(0, GameConstants.GRAVITY);
                ceiling.Velocity = new Vector2(0, GameConstants.GRAVITY);

                game.Camera.ShakeCamera(5, 5);

                Sound.PlaySoundInstance(object_vents_platform_fall, Game1.GetFileName(() => object_vents_platform_fall));
            }

            if (floor.Velocity.Y != 0 && game.MapBooleans.chapterOneMapBooleans["VentLowered"] == false)
            {
                floor.Velocity += new Vector2(0, GameConstants.GRAVITY);
                ceiling.Velocity += new Vector2(0, GameConstants.GRAVITY);
            }

            if (floor.Position.Y >= 0 && game.MapBooleans.chapterOneMapBooleans["VentLowered"] == false)
            {
                floor.Velocity = new Vector2(0, 0);
                ceiling.Velocity = new Vector2(0, 0);
                game.MapBooleans.chapterOneMapBooleans["VentLowered"] = true;
                game.Camera.ShakeCamera(5, 10);
            }

            if (game.MapBooleans.chapterOneMapBooleans["VentLowered"] == true)
            {
                floor.Position = new Vector2(7518, 0);
                ceiling.Position = new Vector2(7518, -400);
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents2 = new Portal(610, -120, "Upper Vents III");
            toUpperVents4Top = new Portal(9500, -400, "Upper Vents III");
            toUpperVents4Bot = new Portal(9100, 0, "Upper Vents III");
            toCoalShaft = new Portal(7900, -653 + Game1.portalTexture.Height, "Upper Vents III", "Silver Key");
            toBathroom = new Portal(8700, 0, "Upper Vents III");

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVents2, UpperVentsII.ToUpperVents3);
            portals.Add(toUpperVents4Bot, UpperVentsIV.ToUpperVents3Bot);
            portals.Add(toUpperVents4Top, UpperVentsIV.ToUpperVents3Top);
            portals.Add(toCoalShaft, CoalShaft.ToUpperVents3);
            portals.Add(toBathroom, Bathroom.ToLastMap);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(fallingVent, new Vector2(ceiling.RecX - 15, ceiling.RecY - 20), Color.White);
            s.Draw(outhouse, new Rectangle(8595, -outhouse.Height + 27, outhouse.Width, outhouse.Height), Color.White);
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
            s.End();

            //s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            //s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            //s.Draw(foreground3, new Vector2(foreground.Width + foreground2.Width, mapRec.Y), Color.White);
        }
    }
}
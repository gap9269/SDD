using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    class UpperVentsVI : MapClass
    {
        static Portal toUpperVents5;
        static Portal toPrincess;
        static Portal toFurance;

        public static Portal ToFurnace { get { return toFurance; } }
        public static Portal ToUpperVents5 { get { return toUpperVents5; } }
        public static Portal ToPrincess { get { return toPrincess; } }

        MapSteam fire1, fire2, fire3, fire4, fire5, fire6, fire9, fire10;

        WallSwitch s1, s2, s3, s4, s5;
        Texture2D foreground, foreground2;

        CoalDeposit coal1, coal2, coal3;
        Texture2D coalTexture;

        public UpperVentsVI(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            yScroll = true;

            mapWidth = 4696;
            mapHeight = 1948;
            mapName = "Upper Vents VI";
            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            overlayType = 1;
            AddPlatforms();
            AddBounds();
            SetPortals();

            zoomLevel = .8f;

            fire1 = new MapSteam(100, 100, 2180, 290, game, 1, true);
            mapHazards.Add(fire1);

            fire2 = new MapSteam(100, 100, 2480, 290, game, 1, true);
            mapHazards.Add(fire2);

            fire3 = new MapSteam(100, 100, 2180, -40, game, 1, true);
            mapHazards.Add(fire3);

            fire4 = new MapSteam(100, 100, 2480, -40, game, 1, true);
            mapHazards.Add(fire4);

            fire5 = new MapSteam(100, 100, 2180, -820 + 395, game, 1, true);
            mapHazards.Add(fire5);

            fire6 = new MapSteam(100, 100, 2480, -820 + 395, game, 1, true);
            mapHazards.Add(fire6);

            fire9 = new MapSteam(100, 100, 800, 290, game, 1, true);
            mapHazards.Add(fire9);

            fire10 = new MapSteam(100, 100, 800, -800 + 395, game, 1, true);
            mapHazards.Add(fire10);

            //fire7.Active = false;
           // fire8.Active = false;
            fire9.Active = false;
            fire6.Active = false;

            s1 = new WallSwitch(Game1.switchTexture, new Rectangle(1310, -165, (int)(333 *1f), (int)(335 * 1f)));
            switches.Add(s1);

            s2 = new WallSwitch(Game1.switchTexture, new Rectangle(1510, -165, (int)(333 * 1f), (int)(335 * 1f)));
            switches.Add(s2);

            s1.Active = true;
            //s2.Active = true;

            s3 = new WallSwitch(Game1.switchTexture, new Rectangle(1710, -165, (int)(333 * 1f), (int)(335 * 1f)));
            switches.Add(s3);

            s4 = new WallSwitch(Game1.switchTexture, new Rectangle(340, 335, (int)(333 * 1f), (int)(335 * 1f)));
            switches.Add(s4);

            s5 = new WallSwitch(Game1.switchTexture, new Rectangle(340, -400, (int)(333 * 1f), (int)(335 * 1f)));
            switches.Add(s5);

            coal1 = new CoalDeposit(game, 3754, mapRec.Y + 982, Game1.whiteFilter, 3, new Coal(3754, mapRec.Y + 982), 3, false, false);
            interactiveObjects.Add(coal1);

            coal2 = new CoalDeposit(game, 2732, mapRec.Y + 1295, Game1.whiteFilter, 2, new Coal(2632, mapRec.Y + 1295), 3, false, true);
            interactiveObjects.Add(coal2);

            coal3 = new CoalDeposit(game, 4366, mapRec.Y + 1295, Game1.whiteFilter, 2, new Coal(4366, mapRec.Y + 1295), 2, false, true);
            interactiveObjects.Add(coal3);

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

            background.Add(content.Load<Texture2D>(@"Maps\Vents\Upper Vents 6\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Upper Vents 6\background2"));
            foreground = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 6\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\Vents\Upper Vents 6\foreground2");

            coalTexture = content.Load<Texture2D>(@"InteractiveObjects\CoalSprite");
            coal1.Sprite = coalTexture;
            coal2.Sprite = coalTexture;
            coal3.Sprite = coalTexture;

            game.NPCSprites["Count Roger"] = content.Load<Texture2D>(@"NPC\Kickstarter\Count Roger");
            Game1.npcFaces["Count Roger"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Kickstarter\RogerNormal");

            if (Chapter.lastMap != "Upper Vents V" && Chapter.lastMap != "Princess' Room")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_vents");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_vents", amb);

                Sound.LoadVentZoneSounds();
            }
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Count Roger"] = Game1.whiteFilter;
            Game1.npcFaces["Count Roger"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap != "Upper Vents V" && Chapter.theNextMap != "Princess' Room")
            {
                Sound.UnloadBackgroundMusic();
                Sound.UnloadAmbience();
                Sound.UnloadMapZoneSounds();
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.BatEnemy(content);
            EnemyContentLoader.FlufflesRat(content); EnemyContentLoader.SharedRatSounds(content);
        }

        public override void Update()
        {
            base.Update();

            PlayAmbience();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVents5 = new Portal(145, 320, "Upper Vents VI");
            toFurance = new Portal(3570, 353, "Upper Vents VI");
            toPrincess = new Portal(2960, -47, "Upper Vents VI");

            toFurance.FButtonYOffset = -20;
            toFurance.PortalNameYOffset = -20;

            toUpperVents5.FButtonYOffset = 20;
            toUpperVents5.PortalNameYOffset = 20;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVents5, UpperVentsV.ToUpperVents6);
            portals.Add(toFurance, Furnace.ToUpperVentsVI);
            portals.Add(toPrincess, PrincessLockerRoom.ToUpperVentsVI);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if(CheckSwitch(s1))
            {
                if (s1.Active)
                {
                    fire2.TurnOn();
                    fire9.TurnOff();
                }
                else
                {
                    fire2.TurnOff();
                    fire9.TurnOn();
                }
            }
            if (CheckSwitch(s2))
            {
                if (s2.Active)
                {
                    fire6.TurnOn();
                    fire10.TurnOff();
                }
                else
                {
                    fire10.TurnOn();
                    fire6.TurnOff();
                }
            }

            if (CheckSwitch(s3))
            {
                if (s3.Active)
                {
                   // fire7.TurnOn();
                    fire3.TurnOff();
                }
                else
                {
                    fire3.TurnOn();
                    //fire7.TurnOff();
                }
            }

            if (CheckSwitch(s4))
            {
                if (s4.Active)
                {
                    fire4.TurnOn();
                    fire1.TurnOff();
                }
                else
                {
                    fire1.TurnOn();
                    fire4.TurnOff();
                }
            }

            if (CheckSwitch(s5))
            {
                if (s5.Active)
                {
                    //fire8.TurnOn();
                    fire5.TurnOff();
                }
                else
                {
                    fire5.TurnOn();
                   // fire8.TurnOff();
                }
            }
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
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            s.End();

            //s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            //s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            //s.Draw(foreground3, new Vector2(foreground.Width + foreground2.Width, mapRec.Y), Color.White);
        }
    }
}

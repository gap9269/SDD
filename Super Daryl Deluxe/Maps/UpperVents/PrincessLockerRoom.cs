using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    class PrincessLockerRoom : MapClass
    {
        static Portal toUpperVentsVI;
        static Portal toUpperVentsI;
        static Portal toJanitor;

        public static Portal ToJanitor { get { return toJanitor; } }
        public static Portal ToUpperVentsVI { get { return toUpperVentsVI; } }
        public static Portal ToUpperVentsI { get { return toUpperVentsI; } }

        Texture2D foreground, chandelier, vent;
        public static Texture2D platform, healthyVent;

        Platform chandelierBase, chandelierTop, plank;

        Dictionary<String, Texture2D> fire;
        public static Dictionary<String, Texture2D> collapseAnimation;

        int fireFrame;
        int fireDelay = 5;

        public PrincessLockerRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1860;
            mapWidth = 1400;
            mapName = "Princess' Room";

            mapRec = new Rectangle(0, -mapHeight + 720, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            chandelierBase = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(267, -74, 200, 50), true, false, false);
            chandelierTop = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(341, -233, 50, 50), true, false, false);
            plank = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(344, -462, 400, 50), true, false, false);
        }
        public override void UnloadContent()
        {
            base.UnloadContent();

            if (Chapter.theNextMap != "Upper Vents VI" && Chapter.theNextMap != "Upper Vents I")
            {
                Sound.UnloadBackgroundMusic();
                Sound.UnloadAmbience();
                Sound.UnloadMapZoneSounds();
            }
        
            game.NPCSprites["The Princess"] = Game1.whiteFilter;
            Game1.npcFaces["The Princess"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_vents");
        }

        public override void Update()
        {
            base.Update();

            PlayAmbience();

            if (!platforms.Contains(chandelierBase) && game.ChapterOne.ChapterOneBooleans["chandelierAdded"])
            {
                platforms.Add(chandelierBase);
                platforms.Add(chandelierTop);
                platforms.Add(plank);
            }

            if (player.VitalRecY < -640 && !game.ChapterOne.ChapterOneBooleans["playedPrincessScene"])
                game.Camera.center.Y = -862;

            fireDelay--;

            if (fireDelay <= 0)
            {
                fireDelay = 5;
                fireFrame++;

                if (fireFrame > 4)
                    fireFrame = 0;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            background.Add(content.Load<Texture2D>(@"Maps\Vents\Princess\background"));
            background.Add(content.Load<Texture2D>(@"Maps\Vents\Princess\background2"));
            foreground = content.Load<Texture2D>(@"Maps\Vents\Princess\foreground");
            chandelier = content.Load<Texture2D>(@"Maps\Vents\Princess\chandelier");
            fire = ContentLoader.LoadContent(content, @"Maps\Vents\Princess\fire");
            Sound.LoadVentZoneSounds();

            if (Chapter.lastMap != "Upper Vents VI" && Chapter.lastMap != "Upper Vents I")
            {
                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_vents");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_vents", amb);
            }

            game.NPCSprites["The Princess"] = content.Load<Texture2D>(@"NPC\Main\princess");
            Game1.npcFaces["The Princess"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\PrincessNormal");

            if (!game.ChapterOne.ChapterOneBooleans["playedPrincessScene"])
            {
                collapseAnimation = ContentLoader.LoadContent(content, @"Maps\Vents\Princess\collapse");
                healthyVent = content.Load<Texture2D>(@"Maps\Vents\Princess\healthyVent");
                platform = content.Load<Texture2D>(@"Maps\Vents\Princess\platform");
            }
            if (!game.ChapterOne.ChapterOneBooleans["protectCampQuestComplete"])
                vent = content.Load<Texture2D>(@"Maps\Vents\Princess\brokenVent");
            else
            {
                vent = content.Load<Texture2D>(@"Maps\Vents\Princess\fixedVent");
                toJanitor.IsUseable = true;
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toUpperVentsVI = new Portal(0, -450, "Princess' Room");
            toJanitor = new Portal(1150, -450, "Princess' Room");
            toJanitor.IsUseable = false;
            toUpperVentsI = new Portal(340, 50, "Princess' Room");
            toUpperVentsI.PortalRecY = 50;

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toUpperVentsVI, UpperVentsVI.ToPrincess);
            portals.Add(toJanitor, JanitorCloset.ToPrincess);
            portals.Add(toUpperVentsI, UpperVents1.ToPrincess);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(vent, new Vector2(0, mapRec.Y), Color.White);

            if(game.ChapterOne.ChapterOneBooleans["chandelierAdded"])
                s.Draw(chandelier, new Vector2(0, mapRec.Y), Color.White);

            s.Draw(fire.ElementAt(fireFrame).Value, new Vector2(0, mapRec.Y + 1200), Color.White);

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
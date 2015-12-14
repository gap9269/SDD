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
    class Science105 : MapClass
    {
        static Portal toScience104;

        public static Portal ToScience104 { get { return toScience104; } }

        Texture2D foreground;

        public Science105(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2500;
            mapName = "Science 105";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Science\105\background"));
            foreground = content.Load<Texture2D>(@"Maps\Science\105\fore");

            Sound.LoadScienceZoneSounds();

            game.NPCSprites["Trenchcoat Employee"] = content.Load<Texture2D>(@"NPC\Main\trenchcoat");
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Trenchcoat");

            SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_ethereal");
            SoundEffectInstance amb = am.CreateInstance();
            amb.IsLooped = true;
            Sound.ambience.Add("ambience_ethereal", amb);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Sound.UnloadAmbience();

            game.NPCSprites["Trenchcoat Employee"] = Game1.whiteFilter;
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void PlayAmbience()
        {
            //Sound.ambience["ambience_wasteland"].Stop();
            Sound.PlayAmbience("ambience_ethereal");
        }

        public override void PlayBackgroundMusic()
        {
            Sound.PauseBackgroundMusic();
        }

        public override void Update()
        {
            base.Update();
            PlayBackgroundMusic();
            PlayAmbience();
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            mapWidth = 2500;
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White);
            s.End();
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toScience104 = new Portal(45, platforms[0], "Science 105", Portal.DoorType.movement_door_open);
            toScience104.FButtonYOffset = -40;
            toScience104.PortalNameYOffset = -40;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toScience104, Science104.ToScience105);
        }
    }
}

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
    class MusicIntroRoom : MapClass
    {
        static Portal toSouthHall;
        static Portal toVienna;

        public static Portal ToSouthHall { get { return toSouthHall; } }
        public static Portal ToVienna { get { return toVienna; } }

        public MusicIntroRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2100;
            mapName = "Intro To Music";

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
            background.Add(content.Load<Texture2D>(@"Maps\hall"));

        }


        public override void Update()
        {
            base.Update();

            if (player.StoryItems.ContainsKey("Vienna Access Card"))
            {
                toSouthHall.ItemNameToUnlock = null;
                toSouthHall.PortalTexture = Game1.portalTexture;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSouthHall = new Portal(200, platforms[0], "IntroToMusic");
            toVienna = new Portal(600, platforms[0], "IntroToMusic", "Vienna Access Card");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSouthHall, SouthHall.ToMusic);
            portals.Add(toVienna, TheaterAnDerWien.ToMusicRoom);
        }
    }
}

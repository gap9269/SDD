using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class Kitchen : MapClass
    {
        static Portal toEastHall;

        public static Portal ToEastHall { get { return toEastHall; } }

        Texture2D table;

        public Kitchen(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1400;
            mapName = "Kitchen";

            currentBackgroundMusic = Sound.MusicNames.NoirHalls;


            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void PlayBackgroundMusic()
        {
            Sound.PlayBackGroundMusic(currentBackgroundMusic.ToString());
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_school_empty");
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\School\Kitchen\background"));

            table = content.Load<Texture2D>(@"Maps\School\Kitchen\table");
            game.NPCSprites["Chef Flex"] = content.Load<Texture2D>(@"NPC\Kickstarter\colin");
            Game1.npcFaces["Chef Flex"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Kickstarter\ColinNormal");
        }

        public override void UnloadNPCContent()
        {
            if (game.chapterState == Game1.ChapterState.prologue)
            {
                game.NPCSprites["Chef Flex"] = Game1.whiteFilter;
                Game1.npcFaces["Chef Flex"].faces["Normal"] = Game1.whiteFilter;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toEastHall = new Portal(45, platforms[0], "Kitchen", Portal.DoorType.movement_door_open);
            ToEastHall.FButtonYOffset = -50;
            ToEastHall.PortalNameYOffset = -50;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEastHall, EastHall.ToKitchen);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(table, new Vector2(1089, 556), Color.White);
            s.End();
        }

    }
}

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
    class MongolCamp : MapClass
    {
        static Portal toIntroRoom;
        static Portal toTent;
        static Portal toGreatWall;
        static Portal toDesert;
        public static Portal toBathroom;

        public static Portal ToDesert { get { return toDesert; } }
        public static Portal ToGreatWall { get { return toGreatWall; } }
        public static Portal ToIntroRoom { get { return toIntroRoom; } }
        public static Portal ToTent { get { return toTent; } }

        Texture2D parallax, clouds, sky, trojanHorse, soldiers, foregroundSoldier, outhouseTexture;

        float cloudPosX = 0;

        public MongolCamp(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Mongolian Camp";

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
            background.Add(content.Load<Texture2D>(@"Maps\History\MongolCamp\background"));
            parallax = content.Load<Texture2D>(@"Maps\History\MongolCamp\parallax");
            clouds = content.Load<Texture2D>(@"Maps\History\MongolCamp\clouds");
            sky = content.Load<Texture2D>(@"Maps\History\NapoleonsCamp\sky");
            trojanHorse = content.Load<Texture2D>(@"Maps\History\TrojanHorseNormal\horse00");
            soldiers = content.Load<Texture2D>(@"Maps\History\MongolCamp\soldiers");
            foregroundSoldier = content.Load<Texture2D>(@"Maps\History\OutsideFort\genghisSoldier");
            outhouseTexture = content.Load<Texture2D>(@"Maps\Outhouse");

            game.NPCSprites["Genghis"] = content.Load<Texture2D>(@"NPC\History\Genghis");
            Game1.npcFaces["Genghis"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\GenghisNormal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Genghis"] = Game1.whiteFilter;

            Game1.npcFaces["Genghis"].faces["Normal"] = Game1.whiteFilter;

        }

        public override void Update()
        {
            base.Update();

            cloudPosX += .5f;

            if (cloudPosX > mapWidth)
                cloudPosX = mapWidth - clouds.Width - 100;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toIntroRoom = new Portal(3800, platforms[0], "Mongolian Camp");
            toDesert = new Portal(3370, platforms[0], "Mongolian Camp");
            toTent = new Portal(1889, platforms[0], "Mongolian Camp");
            toGreatWall = new Portal(50, platforms[0], "Mongolian Camp");
            toBathroom = new Portal(2942, platforms[0], "Mongolian Camp");

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["lumberQuestScenePlayed"] && !game.ChapterTwo.ChapterTwoBooleans["invadeChinaPartThreePlayed"])
                s.Draw(trojanHorse, new Rectangle(301, -65, trojanHorse.Width, trojanHorse.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);

            if (game.ChapterTwo.ChapterTwoBooleans["mongolsRetreated"] && (!game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"] || game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"]))
                s.Draw(soldiers, new Vector2(957, 0), Color.White);

            s.Draw(outhouseTexture, new Vector2(2832, platforms[0].RecY - outhouseTexture.Height), null, Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toIntroRoom, HistoryEntrance.ToMongolCamp);
            portals.Add(toTent, GenghisYurt.ToCamp);
            portals.Add(toDesert, DryDesert.ToMongols);
            portals.Add(toGreatWall, TheGreatWall.ToMongolCamp);
            portals.Add(toBathroom, Bathroom.ToLastMap);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            if (game.ChapterTwo.ChapterTwoBooleans["mongolsRetreated"] && (!game.ChapterTwo.ChapterTwoBooleans["finishedCaesarArc"] || game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"]))
                s.Draw(foregroundSoldier, new Vector2(2664, -640), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.15f, this, game));
            s.Draw(sky, new Rectangle(0, 0, mapWidth, mapHeight), Color.White);
            s.Draw(clouds, new Vector2(cloudPosX, 0), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(parallax, new Rectangle(0, 0, parallax.Width, parallax.Height), Color.White);

            s.End();
        }
    }
}

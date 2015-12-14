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
    class EbenezersMansion : MapClass
    {
        static Portal toSnowyStreets;
        static Portal toWesternCorridor;
        static Portal toTheFoyer;
        static Portal toLivingArea;

        public static Portal toBathroom;

        public static Portal ToTheFoyer { get { return toTheFoyer; } }
        public static Portal ToLivingArea { get { return toLivingArea; } }
        public static Portal ToSnowyStreets { get { return toSnowyStreets; } }
        public static Portal ToWesternCorridor { get { return toWesternCorridor; } }

        Texture2D foreground, door, parallax, sign;

        LivingLocker locker;
        float doorAlpha;

        public List<InteractiveObject> firePlaces;
        public EbenezersMansion(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Ebenezer's Mansion";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            locker = new LivingLocker(game, new Rectangle(2230, 100, 850, 500));
            interactiveObjects.Add(locker);
            firePlaces = new List<InteractiveObject>();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\EbenezersMansion\background"));
            foreground = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\EbenezersMansion\foreground");
            parallax = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\EbenezersMansion\parallax");
            sign = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\EbenezersMansion\sign");
            door = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\EbenezersMansion\foregroundDoor");
            game.NPCSprites["Poole"] = content.Load<Texture2D>(@"NPC\Literature\Poole");
            Game1.npcFaces["Poole"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Literature\Poole Normal");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Poole"] = Game1.whiteFilter;
            Game1.npcFaces["Poole"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if(!game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"] && toTheFoyer.IsUseable)
                toTheFoyer.IsUseable = false;
            else if(game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"] && !toTheFoyer.IsUseable)
                toTheFoyer.IsUseable = true;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSnowyStreets = new Portal(1000, platforms[0], "Ebenezer's Mansion");
            toWesternCorridor = new Portal(50, platforms[0], "Ebenezer's Mansion");
            toLivingArea = new Portal(1900, platforms[0], "Ebenezer's Mansion");
            toTheFoyer = new Portal(3750, platforms[0], "Ebenezer's Mansion");
            toBathroom = new Portal(3375, platforms[0], "Ebenezer's Mansion");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            if(!game.ChapterTwo.ChapterTwoBooleans["lightsTurnedOn"])
                s.Draw(sign, new Vector2(3800, 447), Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSnowyStreets, SnowyStreets.ToEbenezersMansion);
            portals.Add(toWesternCorridor, WesternCorridor.ToEbenezersMansion);
            portals.Add(toLivingArea, LivingArea.ToEbenezersMansion);
            portals.Add(toTheFoyer, TheGrandCorridor.toEbenezersMansion);
            portals.Add(toBathroom, Bathroom.ToLastMap);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (player.VitalRec.X < 1550 && player.VitalRecX > 700)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            s.Draw(door, new Vector2(300, 211), Color.White * doorAlpha);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.93f, this, game));
            s.Draw(parallax, new Vector2(1222, 66), Color.White);
            s.End();
        }
    }
}

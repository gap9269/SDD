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
    class SquigglesTheClown : MapClass
    {
        static Portal toMapFive;
        static Portal toMapSix;

        public static Portal ToMapFive { get { return toMapFive; } }
        public static Portal ToMapSix { get { return toMapSix; } }

        List<Texture2D> lowBack;
        Texture2D foreground, lockerTex, clown, balloon;

        StudentLocker locker;

        Rectangle clownRec;
        float ballonPos;

        Sparkles sparkles;

        public SquigglesTheClown(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Squiggles The Clown";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            clownRec = new Rectangle(700, 383, 75, 256);
            sparkles = new Sparkles(675, 400);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map5"));
            clown = content.Load<Texture2D>(@"Maps\Tutorial\Clown");
            balloon = content.Load<Texture2D>(@"Maps\Tutorial\Balloon");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();

            if (game.MapBooleans.tutorialMapBooleans["TakenClown"] == false)
                sparkles.Update();

            if (game.MapBooleans.tutorialMapBooleans["TakenClown"] == false)
            {
                Chapter.effectsManager.AddToolTipWithImage("What a surprise! It's Squiggles! And look! See \nthose sparkles on the balloon? You can interact \nwith objects that have sparkles. Press 'F' to \ntake the balloon away from Squiggles!", 100, 150, game.ChapterTwo.associateOneTex);
            }
            else if (game.MapBooleans.tutorialMapBooleans["BalloonFloated"] == false)
            {
                Chapter.effectsManager.AddToolTipWithImage("Good job!\n\nLet's get out of here.", 100, 150, game.ChapterTwo.associateOneTex);
                game.MapBooleans.tutorialMapBooleans["BalloonFloated"] = true;
                toMapSix.IsUseable = true;
                toMapFive.IsUseable = true;
            }
            /*
            if (game.MapBooleans.tutorialMapBooleans["BalloonFloated"] == true && game.MapBooleans.tutorialMapBooleans["BalloonGone"] == false)
            {
                ballonPos -= 8;
            }

            if (ballonPos <= -720 && game.MapBooleans.tutorialMapBooleans["BalloonGone"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["BalloonGone"] = true;
            }*/

            if (player.VitalRec.Intersects(clownRec) && last.IsKeyDown(Keys.Space) && current.IsKeyUp(Keys.Space) && game.MapBooleans.tutorialMapBooleans["TakenClown"] == false)
            {
                player.AddStoryItem("Squiggles the Hostage", "Squiggles the Clown", 1);
                game.MapBooleans.tutorialMapBooleans["TakenClown"] = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if(game.MapBooleans.tutorialMapBooleans["TakenClown"] == false)
                s.Draw(clown, mapRec, Color.White);

            s.Draw(balloon, new Vector2(0, ballonPos), Color.White);

            if (game.MapBooleans.tutorialMapBooleans["TakenClown"] == false)
                sparkles.Draw(s);
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapFive = new Portal(160, platforms[0], "SquigglesTheClown");
            toMapSix = new Portal(1080, platforms[0], "SquigglesTheClown");

            toMapFive.IsUseable = false;
            toMapSix.IsUseable = false;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapFive, TutorialMapFive.ToSquiggles);
            portals.Add(toMapSix, TutorialMapSix.ToSquiggles);
        }
    }
}
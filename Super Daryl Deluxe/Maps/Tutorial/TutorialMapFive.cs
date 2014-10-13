using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class TutorialMapFive : MapClass
    {
        static Portal toMapFour;
        static Portal toSquiggles;

        public static Portal ToMapFour { get { return toMapFour; } }
        public static Portal ToSquiggles { get { return toSquiggles; } }

        List<Texture2D> lowBack;
        Texture2D foreground, lockerTex;

        StudentLocker locker;

        public TutorialMapFive(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Tutorial Map Five";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            //ADD A STUDENT LOCKER
            locker = new StudentLocker(player, game, new Rectangle(580, 630 - 258, 165, 258), new List<object> { new BronzeKey() }, "Someone's Locker", Game1.whiteFilter);
            lockers.Add(locker);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map5"));
            lockerTex = content.Load<Texture2D>(@"Tutorial\Locker");

            locker.Tex = lockerTex;
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            //Not really an NPC but it works
            locker.Tex = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if (player.PositionX > 100 && game.MapBooleans.tutorialMapBooleans["TutorialTipElevenUsed"] == false)
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][22], 100, 150, game.ChapterTwo.associateOneTex);
            }

            if (player.StoryItems.ContainsKey("Bronze Key"))
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][23], 550, 170, game.ChapterTwo.associateOneTex);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapFour = new Portal(160, platforms[0], "TutorialMapFive");
            toSquiggles = new Portal(1080, platforms[0], "TutorialMapFive", "Bronze Key");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapFour, TutorialMapFour.ToMapFive);
            portals.Add(toSquiggles, SquigglesTheClown.ToMapFive);
        }
    }
}
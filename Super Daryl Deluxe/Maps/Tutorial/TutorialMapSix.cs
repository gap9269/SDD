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
    class TutorialMapSix : MapClass
    {
        static Portal toSquiggles;
        static Portal toMapSeven;

        public static Portal ToSquiggles { get { return toSquiggles; } }
        public static Portal ToMapSeven { get { return toMapSeven; } }

        Texture2D  chest;
        TreasureChest chester;
        public TutorialMapSix(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {

            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Tutorial Map Six";
            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            //ADD A CHEST
            chester = new TreasureChest(chest, 580, 630, player, 0, new Textbook(), this);
            chester.Rec = new Rectangle(580, 600 - 180, 220, 199);
            chester.OpenBar = new Rectangle(chester.Rec.X + chester.Rec.Width / 2 - 50, chester.Rec.Y, 0, 20);
            treasureChests.Add(chester);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map5"));
            chest = content.Load<Texture2D>(@"Tutorial\TutChest");
            chester.Spritesheet = chest;
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            //Not really an NPC but it works
            chester.Spritesheet = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if (game.MapBooleans.tutorialMapBooleans["TutorialTipTwelveUsed"] == false)
            {
                if(game.MapBooleans.tutorialMapBooleans["LeftWithoutChest"] == false)
                    Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][26], 100, 150, game.ChapterTwo.associateOneTex);
                else
                    Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][27], 100, 150, game.ChapterTwo.associateOneTex);

                if (player.Textbooks > 0 && player.PositionX < 900)
                {
                    Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][28], 400, 100, game.ChapterTwo.associateOneTex);
                    game.MapBooleans.tutorialMapBooleans["FoundTextbook"] = true;
                }

                if (player.PositionX > 800 && player.Textbooks > 0)
                {
                    game.MapBooleans.tutorialMapBooleans["TutorialTipTwelveUsed"] = true;
                    Chapter.effectsManager.RemoveToolTip();
                }
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toSquiggles = new Portal(160, platforms[0], "TutorialMapSix");
            toMapSeven = new Portal(1080, platforms[0], "TutorialMapSix");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toSquiggles, SquigglesTheClown.ToMapSix);
            portals.Add(toMapSeven, TutorialMapSeven.ToMapSix);
        }
    }
}
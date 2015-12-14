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
    class TutorialMapOne : MapClass
    {
        static Portal toMapTwo;

        public static Portal ToMapTwo { get { return toMapTwo; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        public TutorialMapOne(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Tutorial Map One";

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
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map1"));
            foreground = content.Load<Texture2D>(@"Maps\Tutorial\Map1Fore");
            foregroundLow = content.Load<Texture2D>(@"Maps\Tutorial\Map1ForeLow");
            lowBack = new List<Texture2D>();
            lowBack.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map1Low"));

            Game1.npcFaces["Demo Danny"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Tutorial\DemoDanny");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Game1.npcFaces["Demo Danny"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if (game.ChapterTwo.ChapterTwoBooleans["lowResTutorial"] == true && background != lowBack)
            {
                background = lowBack;
                foreground = foregroundLow;
            }

            //TOOLTIPS
            if (player.PositionX == 200 && game.MapBooleans.tutorialMapBooleans["TutorialTipOneUsed"] == false)
            {
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][0], 400, 200, game.ChapterTwo.associateOneTex);

            }

            if (player.PositionX > 500 && game.MapBooleans.tutorialMapBooleans["TutorialTipOneUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipOneUsed"] = true;
                Chapter.effectsManager.RemoveToolTip();
            }

            if (player.PositionX > 1200 && game.MapBooleans.tutorialMapBooleans["TutorialTipTwoUsed"] == false)
            {
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][1], 700, 180, game.ChapterTwo.associateOneTex);
            }

            if (player.PositionX > 1700 && game.MapBooleans.tutorialMapBooleans["TutorialTipTwoUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipTwoUsed"] = true;
                Chapter.effectsManager.RemoveToolTip();
            }

            if (player.PositionX > 2000 && game.MapBooleans.tutorialMapBooleans["TutorialTipThreeUsed"] == false)
            {
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][2], 720, 30, game.ChapterTwo.associateOneTex);
            }

            if (player.PositionX < 2000 && game.MapBooleans.tutorialMapBooleans["TutorialTipThreeUsed"] == false && game.MapBooleans.tutorialMapBooleans["TutorialTipTwoUsed"] == true)
            {
                Chapter.effectsManager.RemoveToolTip();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapTwo = new Portal(2730, 460, "TutorialMapOne");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapTwo, TutorialMapTwo.ToMapOne);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, mapRec, Color.White);
            s.End();
        }
    }
}
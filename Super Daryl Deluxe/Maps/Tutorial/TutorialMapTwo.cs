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
    class TutorialMapTwo : MapClass
    {
        static Portal toMapOne;
        static Portal toMapThree;

        public static Portal ToMapOne { get { return toMapOne; } }
        public static Portal ToMapThree { get { return toMapThree; } }

        List<Texture2D> foreground;
        List<Texture2D> treeTextures;

        Platform treePlat;

        int timesFailedGapJump = 0;
        Rectangle colliderInGap;
        Rectangle colliderOutOfGap;

        bool climbedOut = false;

        public TutorialMapTwo(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2000;
            mapWidth = 5200;
            mapName = "Tutorial Map Two";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            zoomLevel = .85f;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            treePlat = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2340, -360, 480, 50), true, false, false);

            colliderInGap = new Rectangle(3950, 350, 450, 100);
            colliderOutOfGap = new Rectangle(3480, -100, 400, 100);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map2Left"));
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map2Right"));
            foreground = new List<Texture2D>();

            foreground.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map2ForeLeft"));
            foreground.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map2ForeRight"));

            treeTextures = new List<Texture2D>();
            treeTextures.Add(content.Load<Texture2D>(@"Maps\Tutorial\TreeUp"));
            treeTextures.Add(content.Load<Texture2D>(@"Maps\Tutorial\TreeDown"));
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

            //Counts the number of times the player has missed the sprint jump
            if(game.MapBooleans.tutorialMapBooleans["JumpedGap"] == false)
            {
                if (player.VitalRec.Intersects(colliderOutOfGap))
                    climbedOut = true;
                if (player.VitalRec.Intersects(colliderInGap) && climbedOut == true)
                {
                    timesFailedGapJump++;
                    climbedOut = false;
                }
            }


            //TOOLTIPS
            if (game.MapBooleans.tutorialMapBooleans["TutorialTipThreeUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipThreeUsed"] = true;
                Chapter.effectsManager.RemoveToolTip();
            }

            //One
            if (player.PositionX > 650 && game.MapBooleans.tutorialMapBooleans["TutorialTipFourUsed"] == false)
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][3], 300, 100, game.ChapterTwo.associateOneTex);

            }
            if (player.PositionX > 1400 && game.MapBooleans.tutorialMapBooleans["TutorialTipFourUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipFourUsed"] = true;
                Chapter.effectsManager.RemoveToolTip();
            }

            if (game.MapBooleans.tutorialMapBooleans["TreeFell"] == true && !platforms.Contains(treePlat))
            {
                platforms.Add(treePlat);
            }

            if (player.PositionX > 2250 && game.MapBooleans.tutorialMapBooleans["TreeFell"] == true && game.MapBooleans.tutorialMapBooleans["TutorialTipFiveUsed"] == false)
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][4], 400, (int)(Game1.aspectRatio * 1280 * .75f) + 10, game.ChapterTwo.associateOneTex);
            }

            if (player.PositionY > -280 && game.MapBooleans.tutorialMapBooleans["TutorialTipFiveUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipFiveUsed"] = true;
                Chapter.effectsManager.RemoveToolTip();
            }

            if (game.MapBooleans.tutorialMapBooleans["TutorialTipFiveUsed"] == true && game.MapBooleans.tutorialMapBooleans["TutorialTipSixUsed"] == false)
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][5], 730, 130, game.ChapterTwo.associateOneTex);
            }

            if (player.PositionX > 2850 && player.Position.Y > -280 && game.MapBooleans.tutorialMapBooleans["TutorialTipSixUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipSixUsed"] = true;
                Chapter.effectsManager.RemoveToolTip();
            }

            if (game.MapBooleans.tutorialMapBooleans["JumpedGap"] == false)
            {
                switch (timesFailedGapJump)
                {
                    case 1:
                        Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][6], 450, -5, game.ChapterTwo.associateOneTex);
                        break;
                    case 2:
                        Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][7], 450, -5, game.ChapterTwo.associateOneTex);
                        break;
                    case 3:
                        Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][8], 450, -5, game.ChapterTwo.associateOneTex);
                        break;
                    case 4:
                        Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][9], 450, -5, game.ChapterTwo.associateOneTex);
                        break;
                    case 5:
                        Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][10], 450, -5, game.ChapterTwo.associateOneTex);
                        break;
                    case 6:
                        Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][11], 450, -5, game.ChapterTwo.associateOneTex);
                        break;
                    case 7:
                        Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][12], 450, -5, game.ChapterTwo.associateOneTex);
                        break;
                    case 8:
                        Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][13], 450, -5, game.ChapterTwo.associateOneTex);
                        break;
                    case 9:
                        Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][14], 450, -5, game.ChapterTwo.associateOneTex);
                        break;
                }
            }

            if (climbedOut && timesFailedGapJump == 9)
            {
                Chapter.effectsManager.RemoveToolTip();

                //Increment this so this if statement isn't entered again
                timesFailedGapJump = 10;
            }

            if (player.PositionX >= 4220 && game.MapBooleans.tutorialMapBooleans["TutorialTipSevenUsed"] == false && game.MapBooleans.tutorialMapBooleans["JumpedGap"] == false)
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][15], 200, 80, game.ChapterTwo.associateOneTex);
                game.MapBooleans.tutorialMapBooleans["JumpedGap"] = true;
            }

            if (player.PositionX > 4550 && game.MapBooleans.tutorialMapBooleans["TutorialTipSevenUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipSevenUsed"] = true;
                Chapter.effectsManager.RemoveToolTip();
            }

            if (player.VitalRec.Intersects(colliderInGap) && game.MapBooleans.tutorialMapBooleans["JumpedGap"] == true && game.MapBooleans.tutorialMapBooleans["FellBackInGap"] == false)
            {
                if(timesFailedGapJump > 7)
                    Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][16], 450, -5, game.ChapterTwo.associateOneTex);
                else
                    Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][17], 450, -5, game.ChapterTwo.associateOneTex);

                game.MapBooleans.tutorialMapBooleans["FellBackInGap"] = true;
            }

            if (game.MapBooleans.tutorialMapBooleans["FellBackInGap"] == true && player.VitalRec.Intersects(colliderOutOfGap))
            {
                Chapter.effectsManager.RemoveToolTip();
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapOne = new Portal(100, platforms[0], "TutorialMapTwo");
            toMapThree = new Portal(4870, 95, "TutorialMapTwo");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

             portals.Add(toMapOne, TutorialMapOne.ToMapTwo);
             portals.Add(ToMapThree, TutorialMapThree.ToMapTwo);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground[0], new Rectangle(0, mapRec.Y, foreground[0].Width, 2000), Color.White);
            s.Draw(foreground[1], new Rectangle(foreground[0].Width, mapRec.Y, foreground[1].Width, 2000), Color.White);

            if(game.MapBooleans.tutorialMapBooleans["TreeFell"] == false)
                s.Draw(treeTextures[0], new Rectangle(0, mapRec.Y, treeTextures[0].Width, 2000), Color.White);
            else
                s.Draw(treeTextures[1], new Rectangle(0, mapRec.Y, treeTextures[1].Width, 2000), Color.White);
            s.End();
        }
    }
}
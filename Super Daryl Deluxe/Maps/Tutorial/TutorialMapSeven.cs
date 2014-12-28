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
    class TutorialMapSeven : MapClass
    {
        static Portal toMapsix;
        static Portal toMapEight;

        public static Portal ToMapSix { get { return toMapsix; } }
        public static Portal ToMapEight { get { return toMapEight; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        static Button toYourLockerButton;
        public static Button ToYourLockerButton { get { return toYourLockerButton; } }

        public TutorialMapSeven(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {

            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Tutorial Map Seven";

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
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map7"));
            game.NPCSprites["Paul"] = content.Load<Texture2D>(@"Tutorial\paul");
            game.NPCSprites["Alan"] = content.Load<Texture2D>(@"Tutorial\alan");
            Game1.npcFaces["Paul"].faces["Tutorial"] = content.Load<Texture2D>(@"NPCFaces\Paul\PaulTutorial");
            Game1.npcFaces["Alan"].faces["Tutorial"] = content.Load<Texture2D>(@"NPCFaces\Alan\AlanTutorial");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Alan"] = Game1.whiteFilter;
            game.NPCSprites["Paul"] = Game1.whiteFilter;
            Game1.npcFaces["Paul"].faces["Tutorial"] = Game1.whiteFilter;
            Game1.npcFaces["Alan"].faces["Tutorial"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            //ADD DISCUSS DIFFERENCE TO THE SHOP
            if (!game.MapBooleans.tutorialMapBooleans["AddedDiscussDifferencesToShop"])
            {
                game.MapBooleans.tutorialMapBooleans["AddedDiscussDifferencesToShop"] = true;
                game.YourLocker.SkillsOnSale.Add(SkillManager.AllSkills["Discuss Differences"]);
            }

            //MAKE THE TREASURE CHEST DIALOGUE CHANGE IF YOU HADN'T OPENED IT YET, AND ENTERED THIS ROOM
            if (game.MapBooleans.tutorialMapBooleans["LeftWithoutChest"] == false)
                game.MapBooleans.tutorialMapBooleans["LeftWithoutChest"] = true;

            //Tooltip for friends
            if (game.Prologue.PrologueBooleans["firstSkillLocker"] == true)
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][29], 400, 100, game.ChapterTwo.associateOneTex);
            }

            //Tooltip for skill HUD
            if (player.EquippedSkills.Count > 0 && !game.MapBooleans.tutorialMapBooleans["TutorialTipThirteenUsed"])
            {
                game.CurrentChapter.HUD.SkillsHidden = false;
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][30], 0, 440, game.ChapterTwo.associateOneTex);
                game.MapBooleans.tutorialMapBooleans["TutorialTipThirteenUsed"] = true;

                game.AllQuests["A Convenient Sequence of Tasks"].CompletedQuest = true;
                game.CurrentChapter.NPCs["YourFriend"].RemoveQuest(game.AllQuests["A Convenient Sequence of Tasks"]);
            }

            if (player.VitalRec.Intersects(toYourLockerButton.ButtonRec) && ((current.IsKeyUp(Keys.F) && last.IsKeyDown(Keys.F)) || MyGamePad.RightBumperPressed()))
            {
                game.YourLocker.LoadContent();
                Chapter.effectsManager.RemoveToolTip();
                game.CurrentChapter.state = Chapter.GameState.YourLocker;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapsix = new Portal(160, platforms[0], "TutorialMapSeven");
            toMapEight = new Portal(1080, platforms[0], "TutorialMapSeven");

            toYourLockerButton = new Button(Game1.portalLocker, new Rectangle(665, 630 - Game1.portalLocker.Width,
Game1.portalLocker.Width - 150, Game1.portalLocker.Height));
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapsix, TutorialMapSix.ToMapSeven);
            portals.Add(toMapEight, TutorialMapEight.ToMapSeven);
        }
    }
}
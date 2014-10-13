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
    class TutorialMapThree : MapClass
    {
        static Portal toMapTwo;
        static Portal toMapFour;

        public static Portal ToMapTwo { get { return toMapTwo; } }
        public static Portal ToMapFour { get { return toMapFour; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        public TutorialMapThree(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {

            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Tutorial Map Three";

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
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map3"));
            game.NPCSprites["Your Friend"] = content.Load<Texture2D>(@"Tutorial\FriendOne");
            Game1.npcFaces["Your Friend"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Tutorial\YourFriend");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Your Friend"] = Game1.whiteFilter;
            Game1.npcFaces["Your Friend"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if (game.CurrentQuests.Count > 0)
            {
                toMapFour.IsUseable = true;
            }

            if (game.MapBooleans.tutorialMapBooleans["TutorialTipEightUsed"] == false)
            {
                if(game.CurrentQuests.Count == 0)
                    Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][18], 100, 170, game.ChapterTwo.associateOneTex);
                else
                    Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][19], 360, 0, game.ChapterTwo.associateOneTex);
            }

            if (player.PositionX > 1400 && game.MapBooleans.tutorialMapBooleans["TutorialTipEightUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipEightUsed"] = true;
                Chapter.effectsManager.RemoveToolTip();
            }
            
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapTwo = new Portal(160, 630, "TutorialMapThree");
            toMapFour = new Portal(1675, 630, "TutorialMapThree");
            toMapFour.IsUseable = false;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapTwo, TutorialMapTwo.ToMapThree);
            portals.Add(toMapFour, TutorialMapFour.ToMapThree);
        }
    }
}
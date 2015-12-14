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
    class TutorialMapTwelve : MapClass
    {
        static Portal toMapEleven;
        static Portal toMapThirteen;

        public static Portal ToMapEleven { get { return toMapEleven; } }
        public static Portal ToMapThirteen { get { return toMapThirteen; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        public TutorialMapTwelve(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Tutorial Map Twelve";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            Barrel b1 = new Barrel(game, 400, 690, Game1.interactiveObjects["BadBarrel"], true, 3, 5, 0, false, 0);
            Barrel b2 = new Barrel(game, 450, 720, Game1.interactiveObjects["BadBarrel"], true, 3, new JarOfDirt(), 0, true, 0);
            Barrel b3 = new Barrel(game, 600, 680, Game1.interactiveObjects["BadBarrel"], true, 3, 5, 0, false, 0);

            interactiveObjects.Add(b1);
            interactiveObjects.Add(b2);
            interactiveObjects.Add(b3);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map5"));
            game.NPCSprites["Trenchcoat Employee"] = content.Load<Texture2D>(@"NPC\Main\trenchcoat");
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Trenchcoat");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Trenchcoat Employee"] = Game1.whiteFilter;
            Game1.npcFaces["Trenchcoat Employee"].faces["Normal"] = Game1.whiteFilter;
        }


        public override void Update()
        {
            base.Update();

            if (drops.Count > 0)
            {
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][40], 400, 100, game.ChapterTwo.associateOneTex);
            }
            else if (player.OwnedAccessories.Count > 0 && player.EquippedAccessory == null)
            {
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][41], 400, 100, game.ChapterTwo.associateOneTex);
            }

            //Move the shop keeper onto screen
            if (player.EquippedAccessory is JarOfDirt && game.CurrentChapter.NPCs["TutorialCrony"].PositionX != 549 && game.Prologue.PrologueBooleans["firstTrench"] == true)
            {

                game.MapBooleans.tutorialMapBooleans["AddedShop"] = true;
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][42], 400, 100, game.ChapterTwo.associateOneTex);

            }

            if (game.MapBooleans.tutorialMapBooleans["AddedShop"] && game.CurrentChapter.NPCs["TutorialCrony"].PositionX != 550)
            {
                game.CurrentChapter.NPCs["TutorialCrony"].PositionX = 550;
                game.CurrentChapter.NPCs["TutorialCrony"].PositionX = 660 - 388;
                game.CurrentChapter.NPCs["TutorialCrony"].RecX = 550;
                game.CurrentChapter.NPCs["TutorialCrony"].RecY = 660 - 388;

                toMapEleven.IsUseable = true;
                toMapThirteen.IsUseable = true;
            }

            if (game.Prologue.PrologueBooleans["firstTrench"] == false && game.MapBooleans.tutorialMapBooleans["EquipRemind"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["EquipRemind"] = true;
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][43], 400, 100, game.ChapterTwo.associateOneTex);
            }

            if (game.CurrentChapter.TalkingToNPC)
            {
                Chapter.effectsManager.RemoveToolTip();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapEleven = new Portal(160, platforms[0], "TutorialMapTwelve");
            toMapThirteen = new Portal(1080, platforms[0], "TutorialMapTwelve");

            toMapEleven.IsUseable = false;
            toMapThirteen.IsUseable = false;
        }


        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }
            s.End();
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapEleven, TutorialMapEleven.ToMapTwelve);
            portals.Add(toMapThirteen, TutorialMapThirteen.ToMapTwelve);
        }
    }
}
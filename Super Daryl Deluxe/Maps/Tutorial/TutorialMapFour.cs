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
    class TutorialMapFour : MapClass
    {
        static Portal toMapThree;
        static Portal toMapFive;

        public static Portal ToMapThree { get { return toMapThree; } }
        public static Portal ToMapFive { get { return toMapFive; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        MovingPlatform movingPlat;
        List<Vector2> targets;
        WallSwitch doorSwitch;
        LockerCombo combo;

        public TutorialMapFour(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1900;
            mapWidth = 3200;
            mapName = "Tutorial Map Four";
            zoomLevel = .9f;

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            //ADD A LOCKER COMBO SHEET TO THE MAP
            combo = new LockerCombo(1325, -190, "Someone", game);
            collectibles.Add(combo);

            //CREATE THE SWITCH, BUT DON'T ADD IT YET
            doorSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(16600, -10, 42, 83));
            switches.Add(doorSwitch);

            //CREATE THE MOVING PLATFORM AND THE TARGETS LIST FOR IT
            targets = new List<Vector2>();
            movingPlat = new MovingPlatform(Game1.platformTextures["TutorialMoving"], new Rectangle(2100, 220, 273, 85),
                true, false, false, targets, 3, 100);

            platforms.Add(movingPlat);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map4"));
            foreground = content.Load<Texture2D>(@"Maps\Tutorial\Map4Fore");
        }

        public override void Update()
        {
            base.Update();


            //IF IT IS IN THE ACTIVE POSITION, ADD THE TARGETS FOR THE MOVING PLATFORM
            if (doorSwitch.Active)
            {
                if (!game.MapBooleans.tutorialMapBooleans["targetsAdded"])
                    game.MapBooleans.tutorialMapBooleans["targetsAdded"] = true;
            }
            else //OTHERWISE, REMOVE THEM IF THEY ARE THERE AND STOP THE PLATFORM
            {
                if (game.MapBooleans.tutorialMapBooleans["targetsAdded"] == true)
                {
                    game.MapBooleans.tutorialMapBooleans["targetsAdded"] = false;
                    targets.Clear();
                    movingPlat.Velocity = Vector2.Zero;
                }
            }

            //ADD THE TARGETS
            if (game.MapBooleans.tutorialMapBooleans["targetsAdded"] && targets.Count == 0)
            {
                targets.Add(new Vector2(2100, -228));
                targets.Add(new Vector2(2100, 220));
            }

            //TIP ONCE THE PLAYED HAS THE COMBO
            if (game.MapBooleans.tutorialMapBooleans["TutorialTipNineUsed"] == false && combo.PickedUp)
            {
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][20], 700, 150, game.ChapterTwo.associateOneTex);
            }

            //CHECK TO SEE IF THE SWITCH IS BEING USED AND DRAW A TOOLTIP WHEN IT IS
            if (switches.Count > 0 && CheckSwitch(doorSwitch) && game.MapBooleans.tutorialMapBooleans["TutorialTipTenUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipNineUsed"] = true;
                //Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][21], 360, 0, game.ChapterTwo.associateOneTex);
            }

            //CLEAR FINAL TOOLTIP AND MOVE THE NPC TO MAP 8
            if (player.PositionX > 2330 && game.MapBooleans.tutorialMapBooleans["TutorialTipTenUsed"] == false)
            {
                game.MapBooleans.tutorialMapBooleans["TutorialTipTenUsed"] = true;
                game.CurrentChapter.NPCs["YourFriend"].MapName = "Tutorial Map Eight";
                Chapter.effectsManager.RemoveToolTip();
            }

            //IF THE PLAYER HAS GOTTEN THE COMBO, ADD THE WALL SWITCH
            if (combo.PickedUp && doorSwitch.Rec.X != 1660)
            {
                doorSwitch.Rec = new Rectangle(1660, -10, 42, 83);
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapThree = new Portal(100, platforms[0], "TutorialMapFour");
            toMapFive = new Portal(2950, -230, "TutorialMapFour");
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, mapRec, Color.White);
            s.End();
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapThree, TutorialMapThree.ToMapFour);
            portals.Add(toMapFive, TutorialMapFive.ToMapFour);
        }
    }
}
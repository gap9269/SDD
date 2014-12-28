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
    class TutorialMapThirteen : MapClass
    {
        static Portal toMapTwelve;
        static Portal toBathroom;
        static Portal toMapFourteen;

        public static Portal ToMapTwelve { get { return toMapTwelve; } }
        public static Portal ToMapFourteen { get { return toMapFourteen; } }
        public static Portal ToBathroom { get { return toBathroom; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        public TutorialMapThirteen(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {

            mapHeight = 720;
            mapWidth = 1280;
            mapName = "Tutorial Map Thirteen";

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
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map13"));
        }

        public override void Update()
        {
            base.Update();

            if (game.MapBooleans.tutorialMapBooleans["YouShouldSave"] == false)
            {
                //game.MapBooleans.tutorialMapBooleans["YouShouldSave"] = true;
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][44], 400, 100, game.ChapterTwo.associateOneTex);
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapTwelve = new Portal(160, 630, "TutorialMapThirteen");
            toBathroom = new Portal(600, 630, "TutorialMapThirteen");
            toMapFourteen = new Portal(1080, 630, "TutorialMapThirteen");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapTwelve, TutorialMapTwelve.ToMapThirteen);
            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toMapFourteen, TutorialMapFourteen.ToMapThirteen);
        }
    }
}
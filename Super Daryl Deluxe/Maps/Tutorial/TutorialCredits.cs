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
    class TutorialCredits : MapClass
    {
        static Portal toMapFourteen;

        public static Portal ToMapFourteen { get { return toMapFourteen; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow, creditsText;

        int creditsPos = 800;

        public TutorialCredits(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1280;
            mapName = "The Credits";

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
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\credits"));
            creditsText = content.Load<Texture2D>(@"Maps\Tutorial\creditsText");
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

            creditsPos -= 2;

            if (creditsPos == -creditsText.Height - 500 && game.ChapterTwo.ChapterTwoBooleans["tutorialEndScenePlayed"] == false)
            {
                game.ChapterTwo.ChapterTwoBooleans["tutorialEndScenePlayed"] = true;
                game.CurrentChapter.state = Chapter.GameState.Cutscene;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapFourteen = new Portal(30, platforms[0], "TheCredits");
            toMapFourteen.IsUseable = false;
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(creditsText, new Rectangle(0, creditsPos, creditsText.Width, creditsText.Height), Color.White);

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapFourteen, TutorialMapFourteen.ToCredits);
        }
    }
}
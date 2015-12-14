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
    class Crossroads : MapClass
    {
        static Portal toClearing;
        static Portal toBridge;
        static Portal toPathTwo;
        static Portal toPathThree;
        static Portal toPathFour;

        public static Portal ToPathFour { get { return toPathFour; } }
        public static Portal ToPathThree { get { return toPathThree; } }
        public static Portal ToPathTwo { get { return toPathTwo; } }
        public static Portal ToClearing { get { return toClearing; } }
        public static Portal ToBridge { get { return toBridge; } }

        Texture2D blocks;

        KidCage kidCage;

        public Crossroads(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2500;
            mapName = "Crossroads";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
            kidCage = new KidCage(Game1.whiteFilter, 1670, 436, player);
        }

        public override void Update()
        {
            base.Update();

            if (!kidCage.Finished)
            {
                //Only allow it to be opened if the deer are all dead
                if(enemiesInMap.Count == 0)
                    kidCage.Update();
            }
                //If the cage is gone but the boolean hasn't be activated, activate it
            else if (!game.ChapterTwo.ChapterTwoBooleans["kidOneSaved"])
            {
                game.ChapterTwo.ChapterTwoBooleans["kidOneSaved"] = true;

                game.ChapterTwo.state = Chapter.GameState.Cutscene;
            }

            //If the game has loaded and the boolean is activated, but the cage isn't finished, set it to finished
            if (game.ChapterTwo.ChapterTwoBooleans["kidOneSaved"] && !kidCage.Finished)
                kidCage.Finished = true;

            if (game.ChapterTwo.ChapterTwoBooleans["kidOneSaved"])
            {
                ToBridge.IsUseable = true;
                ToClearing.IsUseable = true;
            }

            if (game.ChapterTwo.ChapterTwoBooleans["kidTwoSaved"])
            {
                toPathTwo.IsUseable = true;
                game.ChapterTwo.NPCs["Tim"].RecX = 346;
                game.ChapterTwo.NPCs["Tim"].PositionX = 346;
                game.ChapterTwo.NPCs["Tim"].MapName = "Totally Safe Room";


            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\Crossroads"));
            blocks = content.Load<Texture2D>(@"Maps\Chelseas\CrossroadsBushes");
            game.NPCSprites["Paul"] = content.Load<Texture2D>(@"NPC\Main\paul");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Benny Beaker", content.Load<Texture2D>(@"EnemySprites\BennySprite"));
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toClearing = new Portal(50, platforms[0], "Crossroads");
            toBridge = new Portal(1400, platforms[0].Rec.Y, "Crossroads");
            ToBridge.IsUseable = false;
            toClearing.IsUseable = false;

            toPathTwo = new Portal(376, platforms[0].Rec.Y, "Crossroads");
            toPathTwo.IsUseable = false;

            toPathThree = new Portal(1030, platforms[0].Rec.Y, "Crossroads");
            toPathThree.IsUseable = false;

            toPathFour = new Portal(2300, platforms[0].Rec.Y, "Crossroads");
            //toPathFour.IsUseable = false;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toClearing, MysteriouslyPeacefulClearing.ToCrossroads);
            portals.Add(toBridge, WoodsyRiver.ToCrossroads);
            portals.Add(toPathTwo, DirtyPath.ToCrossroads);
            portals.Add(toPathThree, EmptyField.ToCrossroads);
            portals.Add(toPathFour, HiddenPath.ToCrossroads);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!game.ChapterTwo.ChapterTwoBooleans["kidTwoSaved"])
                s.Draw(blocks, new Rectangle(306, 358, 410, 295), new Rectangle(0, 0, 410, 295), Color.White);
            if (!game.ChapterTwo.ChapterTwoBooleans["kidThreeSaved"])
                s.Draw(blocks, new Rectangle(901, 300, 330, 338), new Rectangle(410, 0, 330, 338), Color.White);
            if (!game.ChapterTwo.ChapterTwoBooleans["kidFourSaved"])
                s.Draw(blocks, new Rectangle(2124, 183, 376, 472), new Rectangle(740, 0, 376, 472), Color.White);

            //Only draw the cage if it hasn't been opened
            if (!kidCage.Finished)
            {
                kidCage.Draw(s);
            }
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, Game1.camera.GetTransform(1.25f, this, game));

            s.End();
        }
    }
}

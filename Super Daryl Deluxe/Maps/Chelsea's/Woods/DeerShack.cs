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
    class DeerShack : MapClass
    {
        static Portal toHiddenPath;

        public static Portal ToHiddenPath { get { return toHiddenPath; } }

        KidCage kidCage;

        WallSwitch trapSwitch;

        public DeerShack(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1300;
            mapWidth = 1600;
            mapName = "Deer Shack";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            zoomLevel = .8f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            kidCage = new KidCage(Game1.interactiveObjects["KidCage"], 1300, 630 - 237, player);
            kidCage.showFButton = false;
            trapSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(1000, 500, 42, 83), 470);

            switches.Add(trapSwitch);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\Dirty Path"));
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Field Goblin", content.Load<Texture2D>(@"EnemySprites\FieldGoblinSheet"));
        }

        public override void Update()
        {
            base.Update();

            if (player.VitalRecX > 700 && game.ChapterTwo.ChapterTwoBooleans["ApproachedTimAgain"] == false)
            {
                game.ChapterTwo.ChapterTwoBooleans["ApproachedTimAgain"] = true;
                Chapter.effectsManager.AddInGameDialogue("This is fucking bullshit. Get me out of here!", "Tim", "Normal", 120);
            }

            CheckSwitch(trapSwitch);

            //Activate the Saved Tim boolean, which launches a cutscene in the ChapterTwo class
            if (trapSwitch.Active && !game.ChapterTwo.ChapterTwoBooleans["SavedTim"])
            {
                game.ChapterTwo.ChapterTwoBooleans["SavedTim"] = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toHiddenPath = new Portal(100, platforms[0], "DeerShack");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            kidCage.Draw(s);
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toHiddenPath, HiddenPath.ToDeerShack);
        }
    }
}

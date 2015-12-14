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
    class OrgranStorageRoomThree : MapClass
    {
        static Portal toButterflyChamber;
        public static Portal ToButterflyChamber { get { return toButterflyChamber; } }

        Texture2D foreground, hole, jar;
        Rectangle crackRec;

        WallSwitch button;
        Sparkles sparkles;
        public OrgranStorageRoomThree(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1300;
            mapWidth = 2600;
            mapName = "Organ Storage Room Three";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 5;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            button = new WallSwitch(Game1.switchTexture, new Rectangle(1800, 320, 333, 335));
            switches.Add(button);

            sparkles = new Sparkles(1380, mapRec.Y + 624);
            crackRec = new Rectangle(1192, mapRec.Y + 447, 485, 430);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomThree\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomThree\foreground");
            hole = content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomThree\hole");
            jar = content.Load<Texture2D>(@"Maps\History\Pyramid\OrganRoomThree\jar");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.VileMummyEnemy(content);
            EnemyContentLoader.MummyEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            Enemy en;
            if(Game1.randomNumberGen.Next(0, 4) == 1)
                en = new VileMummy(pos, "Vile Mummy", game, ref player, this);
            else
                en = new Mummy(pos, "Mummy", game, ref player, this);

            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(2210, mapRec.Y - 400);

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            en.UpdateRectangles();
            AddEnemyToEnemyList(en);

        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count > 0)
            {
                button.Active = true;
            }
            else
            {
                button.Active = false;

                if (CheckSwitch(button))
                {
                    RespawnGroundEnemies();
                }
            }

            if (game.ChapterTwo.ChapterTwoBooleans["organChamberThreeWallDestroyed"])
            {
                if (player.VitalRec.Intersects(sparkles.rec) && ((last.IsKeyDown(Keys.Space) && current.IsKeyUp(Keys.Space)) || MyGamePad.RightTriggerPressed()) && game.ChapterTwo.ChapterTwoBooleans["organChamberThreeJarObtained"] == false)
                {
                    player.AddStoryItem("Jarred Brain", "a Jarred Brain", 1);
                    game.ChapterTwo.ChapterTwoBooleans["organChamberThreeJarObtained"] = true;
                }
                if (!game.ChapterTwo.ChapterTwoBooleans["organChamberThreeJarObtained"])
                {
                    sparkles.Update();
                }
            }
            if (enemiesInMap.Count > 0 && (enemiesInMap[0] is VileMummy) && (enemiesInMap[0] as VileMummy).exploding && (enemiesInMap[0] as VileMummy).explosionRec.Intersects(crackRec) && !game.ChapterTwo.ChapterTwoBooleans["organChamberThreeWallDestroyed"])
            {
                game.ChapterTwo.ChapterTwoBooleans["organChamberThreeWallDestroyed"] = true;
                game.Camera.ShakeCamera(15, 25);
                Chapter.effectsManager.AddSmokePoof(crackRec, 3);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toButterflyChamber = new Portal(50, platforms[0], "Organ Storage Room Three");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["organChamberThreeWallDestroyed"])
            {
                s.Draw(hole, new Vector2(1192, mapRec.Y + 447), Color.White);

                if (!game.ChapterTwo.ChapterTwoBooleans["organChamberThreeJarObtained"])
                {
                    s.Draw(jar, new Vector2(1350, mapRec.Y + 594), Color.White);

                    sparkles.Draw(s);
                }
            }
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toButterflyChamber, ButterflyChamber.ToOrganStorageRoomThree);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}

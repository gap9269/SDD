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
    class FruitGarden : MapClass
    {
        static Portal toMapTen;

        public static Portal ToMapTen { get { return toMapTen; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow, lockerTex;

        Boolean hitMelon = false;

        public FruitGarden(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1500;
            mapName = "Fruit Garden";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyAmount = 15;
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\FruitGarden"));
            foreground = content.Load<Texture2D>(@"Maps\Tutorial\FruitGardenFore");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Garden Beast", content.Load<Texture2D>(@"Tutorial\EnemieSheet"));
        }

        public override void RespawnGroundEnemies()
        {

            base.RespawnGroundEnemies();

            MelonEnemy en = new MelonEnemy(pos, "Garden Beast", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
            en.Position = new Vector2(monsterX, monsterY);

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                enemiesInMap.Add(en);
            }

        }

        public override void Update()
        {
            base.Update();

            if (spawnEnemies && enemiesInMap.Count < enemyAmount)
            {
                RespawnGroundEnemies();

                if (enemyAmount == enemiesInMap.Count)
                    spawnEnemies = false;
            }

            if (hitMelon == false && enemyAmount == enemiesInMap.Count)
            {
                Chapter.effectsManager.AddToolTipWithImage("Cool! Flying melons!", 400, 100, game.ChapterTwo.associateOneTex);

                for (int i = 0; i < 15; i++)
                {
                    if (enemiesInMap[i].Health < enemiesInMap[i].MaxHealth)
                    {
                        hitMelon = true;
                        break;
                    }
                }
            }
            else
                Chapter.effectsManager.RemoveToolTip();

            if (spawnEnemies == false)
            {
                if (enemiesInMap.Count < 10)
                    game.MapBooleans.tutorialMapBooleans["DestroyedSomeFruit"] = true;
                if (enemiesInMap.Count == 0)
                    game.MapBooleans.tutorialMapBooleans["DestroyedAllFruit"] = true;
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapTen = new Portal(70, platforms[0], "FruitGarden");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapTen, TutorialMapTen.ToFruitGarden);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, mapRec, Color.White);
            s.End();
        }
    }
}
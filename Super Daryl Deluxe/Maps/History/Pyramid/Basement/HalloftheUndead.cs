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
    class HalloftheUndead : MapClass
    {
        static Portal toHallofMoaning;
        public static Portal ToHallofMoaning { get { return toHallofMoaning; } }

        Texture2D foreground, wall, coffinSprite;

        Coffin coffin1, coffin2, coffin3, coffin4, coffin5, coffin6;

        PyramidKey pyramidKey;

        public HalloftheUndead(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2000;
            mapWidth = 4000;
            mapName = "Hall of the Undead";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 5;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            collectibles.Add(new Textbook(112, mapRec.Y + 87, 3));

            //--Map Quest
            mapWithMapQuest = true;

            MapQuestSign sign = new MapQuestSign(3305, mapRec.Y + 1129, "Destroy all coffins", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, null);
            mapQuestSigns.Add(sign);

            coffin1 = new Coffin(game, 1972, mapRec.Y + 1247, Game1.whiteFilter, false, 2, 600);
            interactiveObjects.Add(coffin1);

            coffin2 = new Coffin(game, 2982, mapRec.Y + 369, Game1.whiteFilter, true, 1, 1500);
            interactiveObjects.Add(coffin2);

            coffin3 = new Coffin(game, 2156, mapRec.Y + 803, Game1.whiteFilter, true, 3, 1000);
            interactiveObjects.Add(coffin3);

            coffin4 = new Coffin(game, 764, mapRec.Y + 369, Game1.whiteFilter, false, 2, 900);
            interactiveObjects.Add(coffin4);

            coffin5 = new Coffin(game, 2642, mapRec.Y - 45, Game1.whiteFilter, false, 3, 1000);
            interactiveObjects.Add(coffin5);

            coffin6 = new Coffin(game, 482, mapRec.Y + 1253, Game1.whiteFilter, true, 2, 600);
            interactiveObjects.Add(coffin6);

            pyramidKey = new PyramidKey(967, mapRec.Y + 917);
            pyramidKey.AbleToPickUp = false;
            collectibles.Add(pyramidKey);

            interactiveObjects.Add(new Barrel(game, 2393, mapRec.Y + 1383 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 2483, mapRec.Y + 1383 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 13, mapRec.Y + 1381 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 111, mapRec.Y + 1381 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 1247, mapRec.Y + 939 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 677, mapRec.Y + 939 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 3793, mapRec.Y + 934 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 3705, mapRec.Y + 934 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 3617, mapRec.Y + 934 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 1502, mapRec.Y + 505 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 788, mapRec.Y + 105 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 909, mapRec.Y + 105 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));

            interactiveObjects.Add(new Barrel(game, 2349, mapRec.Y + 81 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 2243, mapRec.Y + 81 + 155, Game1.interactiveObjects["Barrel"], true, 2, 6, .16f, false, Barrel.BarrelType.pyramidPitcher));

            interactiveObjects.Add(new Barrel(game, 3238, mapRec.Y + 500 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .36f, false, Barrel.BarrelType.pyramidUrn));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\HallOfUndead\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\HallOfUndead\foreground");
            wall = content.Load<Texture2D>(@"Maps\History\Pyramid\HallOfUndead\wall");
            coffinSprite = content.Load<Texture2D>(@"InteractiveObjects\CorruptedCoffinSprite");

            coffin1.Sprite = coffinSprite;
            coffin2.Sprite = coffinSprite;
            coffin3.Sprite = coffinSprite;
            coffin4.Sprite = coffinSprite;
            coffin5.Sprite = coffinSprite;
            coffin6.Sprite = coffinSprite;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.MummyEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();

            if (coffin1.Finished && coffin2.Finished && coffin3.Finished && coffin4.Finished && coffin5.Finished && coffin6.Finished && !completedMapQuest)
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
                pyramidKey.AbleToPickUp = true;

            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toHallofMoaning = new Portal(3750, platforms[0], "Hall of the Undead");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toHallofMoaning, TheMoaningHallway.ToTheHallofMummies);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White * .994f);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}

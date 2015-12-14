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
    class PrisonChamber : MapClass
    {
        static Portal toBurialChamber;
        public static Portal ToBurialChamber { get { return toBurialChamber; } }
        static Portal toTortureChamber;
        public static Portal ToTortureChamber { get { return toTortureChamber; } }
        static Portal toChamber44;
        public static Portal ToChamber44 { get { return toChamber44; } }
        static Portal toTunnelOfCertainDeath;
        public static Portal ToTunnelOfCertainDeath { get { return toTunnelOfCertainDeath; } }

        Texture2D foreground, coffinSprite;

        public PrisonChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 4000;
            mapName = "Prison Chamber";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            interactiveObjects.Add(new ExplodingFlower(game, 2690, 467, false, 200));
            interactiveObjects.Add(new ExplodingFlower(game, 1484, 466, false, 200));

            interactiveObjects.Add(new Coffin(game, 2528, 324, Game1.whiteFilter, false, 2, 600, true, false));
            interactiveObjects.Add(new Coffin(game, 2300, 324, Game1.whiteFilter, false, 1, 600, true, false));

            interactiveObjects.Add(new Coffin(game, 556, 329, Game1.whiteFilter, false, 1, 600, true, false));
            interactiveObjects.Add(new Coffin(game, 1565, 324, Game1.whiteFilter, false, 2, 600, true, false));
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\PrisonChamber\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\PrisonChamber\foreground");
            coffinSprite = content.Load<Texture2D>(@"InteractiveObjects\CorruptedCoffinSprite");

            foreach (InteractiveObject i in interactiveObjects)
            {
                if (i is Coffin)
                {
                    i.Sprite = coffinSprite;
                }
            }
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.MummyEnemy(content);
            EnemyContentLoader.VileMummyEnemy(content);
            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.SpawnWithPoof = true;

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }

            else
            {
                en.UpdateRectangles();
                AddEnemyToEnemyList(en);
            }

        }

        public override void Update()
        {
            base.Update();

            if (spawnEnemies && enemiesInMap.Count < enemyAmount)
            {
                RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount)
                    spawnEnemies = false;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toBurialChamber = new Portal(1200, platforms[0], "Prison Chamber");
            toTortureChamber = new Portal(2200, platforms[0], "Prison Chamber");
            toChamber44 = new Portal(100, platforms[0], "Prison Chamber");
            toTunnelOfCertainDeath = new Portal(3600, platforms[0], "Prison Chamber");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBurialChamber, BurialChamber.ToPrisonChamber);
            portals.Add(ToTortureChamber, TortureChamber.ToPrisonChamber);
            portals.Add(toChamber44, Chamber44.ToPrisonChamber);
            portals.Add(toTunnelOfCertainDeath, TunnelOfCertainDeath.ToPrisonChamber);
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

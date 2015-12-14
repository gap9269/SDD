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
    class MainChamber : MapClass
    {
        static Portal toBasementStairs;
        static Portal toPharaohsKeep;
        static Portal toOuterChamber;
        static Portal toCentralHallI;

        public static Portal ToCentralHallI { get { return toCentralHallI; } }
        public static Portal ToOuterChamber { get { return toOuterChamber; } }
        public static Portal ToPharaohsKeep { get { return toPharaohsKeep; } }
        public static Portal ToBasementStairs { get { return toBasementStairs; } }

        Texture2D foreground, door;
        float doorAlpha;

        public MainChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1800;
            mapWidth = 3000;
            mapName = "Main Chamber";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 4;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);

            Barrel bar = new Barrel(game, 486, mapRec.Y + 1245 + 155, Game1.interactiveObjects["Barrel"], true, 1, 5, .08f, true, Barrel.BarrelType.pyramidUrn);
            interactiveObjects.Add(bar);

            Barrel bar2 = new Barrel(game, 374, mapRec.Y + 1242 + 155, Game1.interactiveObjects["Barrel"], true, 2, 4, .16f, true, Barrel.BarrelType.pyramidPitcher);
            interactiveObjects.Add(bar2);

            Barrel bar1 = new Barrel(game, 1094, mapRec.Y + 683 + 155, Game1.interactiveObjects["Barrel"], true, 2, 1, .26f, false, Barrel.BarrelType.pyramidBirdJar);
            interactiveObjects.Add(bar1);

            interactiveObjects.Add(new Barrel(game, 28, mapRec.Y + 683 + 155, Game1.interactiveObjects["Barrel"], true, 3, 2, .26f, false, Barrel.BarrelType.pyramidBirdJar));

            interactiveObjects.Add(new Barrel(game, 148, mapRec.Y + 683 + 155, Game1.interactiveObjects["Barrel"], true, 1, 2, .06f, false, Barrel.BarrelType.pyramidPitcher));
            interactiveObjects.Add(new Barrel(game, 261, mapRec.Y + 683 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .13f, false, Barrel.BarrelType.pyramidPitcher));

            Barrel bar4 = new Barrel(game, 2471, mapRec.Y + 1197 + 155, Game1.interactiveObjects["Barrel"], true, 3, 4, .0f, false, Barrel.BarrelType.pyramidUrn);
            bar4.facingRight = false;
            interactiveObjects.Add(bar4);
            interactiveObjects.Add(new Barrel(game, 2216, mapRec.Y + 1187 + 155, Game1.interactiveObjects["Barrel"], true, 2, 2, .13f, false, Barrel.BarrelType.pyramidUrn));


        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\MainChamber\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\MainChamber\foreground");
            door = content.Load<Texture2D>(@"Maps\History\Pyramid\MainChamber\door");
            game.NPCSprites["Chained Pharaoh Guard"] = content.Load<Texture2D>(@"NPC\History\Chained Pharaoh Guard");
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Pharaoh Guard Normal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Chained Pharaoh Guard"] = Game1.whiteFilter;
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < enemyAmount)
            {
                AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);
                en.SpawnWithPoof = false;

                if (Game1.randomNumberGen.Next(0, 2) == 0) 
                    en.FacingRight = true;
                else
                    en.FacingRight = false;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Anubis Warrior"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            spawnEnemies = true;
        }

        public override void Update()
        {
            base.Update();

            if (enemyAmount > enemiesInMap.Count && spawnEnemies)
                RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBasementStairs = new Portal(115, platforms[0], "Main Chamber");
            toPharaohsKeep = new Portal(1420, platforms[1], "Main Chamber", "Pyramid Key");
            toOuterChamber = new Portal(895, platforms[0], "Main Chamber");
            toCentralHallI = new Portal(2685, platforms[0], "Main Chamber");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toOuterChamber, OuterChamber.ToMainChamber);
            portals.Add(toPharaohsKeep, PharaohsKeep.ToMainChamber);
            portals.Add(toBasementStairs, BasementStairs.ToMainChamber);
            portals.Add(toCentralHallI, CentralHallI.ToMainChamber);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            if (player.VitalRec.X < 1400 && player.VitalRecX > 500)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            s.Draw(door, new Vector2(357, mapRec.Y + 784), Color.White * doorAlpha);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}

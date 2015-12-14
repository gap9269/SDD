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
    class NoMansValley : MapClass
    {
        static Portal toTrenchfootField;
        static Portal toOutskirts;

        public static Portal ToOutskirts { get { return toOutskirts; } }
        public static Portal ToTrenchfootField { get { return toTrenchfootField; } }

        Texture2D foreground, foreground2, foreground3, sky, parallax, injuredSoldierSprite, parallax2, barricadeTexture, cannonballSprite, crater, soldier;

        GoblinMortar mortar, mortar2, mortar3, mortar4;

        Cannonball[] cannonballs;
        int[] cannonballTimers;
        int cannonballAmount = 10;

        InjuredSoldier inj1;

        public NoMansValley(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2000;
            mapWidth = 10500;
            mapName = "No Man's Valley";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 16;
            zoomLevel = .9f;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
            enemyNamesAndNumberInMap.Add("Goblin", 0);
            enemyNamesAndNumberInMap.Add("Bomblin", 0);

            cannonballs = new Cannonball[cannonballAmount];
            cannonballTimers = new int[cannonballAmount];

            inj1 = new InjuredSoldier(2988 + 516, mapRec.Y + 1171, player, true, InjuredSoldier.HealedState.normal, false);
            interactiveObjects.Add(inj1);
        }

        public override void RespawnGroundEnemies()
        {

            while (platforms[platformNum = rand.Next(0, platforms.Count)].SpawnOnTop == false)
            {
                platformNum = rand.Next(0, platforms.Count);
            }

            monsterX = rand.Next(platforms[platformNum].Rec.X + 100, platforms[platformNum].Rec.X + platforms[platformNum].Rec.Width - 300);
            monsterY = 0;
            pos = new Vector2(monsterX, monsterY);


            if (enemyNamesAndNumberInMap["Bomblin"] < 5)
            {
                Bomblin erl = new Bomblin(pos, "Bomblin", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - erl.Rec.Height - 1;
                erl.Position = new Vector2(monsterX, monsterY);

                Rectangle erlRec = new Rectangle(erl.RecX, monsterY, erl.Rec.Width, erl.Rec.Height);
                if (erlRec.Intersects(player.Rec))
                {
                }
                else
                {
                    AddEnemyToEnemyList(erl);
                    enemyNamesAndNumberInMap["Bomblin"]++;
                }
            }

            if (enemyNamesAndNumberInMap["Goblin"] < 7)
            {
                Goblin erl = new Goblin(pos, "Goblin", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - erl.Rec.Height - 1;
                erl.Position = new Vector2(monsterX, monsterY);

                Rectangle erlRec = new Rectangle(erl.RecX, monsterY, erl.Rec.Width, erl.Rec.Height);
                if (erlRec.Intersects(player.Rec))
                {
                }
                else
                {
                    AddEnemyToEnemyList(erl);
                    enemyNamesAndNumberInMap["Goblin"]++;
                }
            }
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            spawnEnemies = true;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.GoblinMortarEnemy(content);
            EnemyContentLoader.GoblinEnemy(content);
            EnemyContentLoader.BomblinEnemy(content);
            EnemyContentLoader.SharedGoblinSounds(content);

            if (game.ChapterTwo.ChapterTwoBooleans["behindGoblinyLinesTwoCompleted"] == false && game.ChapterTwo.behindGoblinyLinesPartTwo.CompletedQuest == false)
            {
                mortar = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                mortar.Position = new Vector2(0, -553);
                mortar.FacingRight = true;
                mortar.TimeBeforeSpawn = 0;
                mortar.SpawnWithPoof = false;

                AddEnemyToEnemyList(mortar);

                mortar2 = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                mortar2.Position = new Vector2(400, -553);
                mortar2.FacingRight = true;

                mortar2.TimeBeforeSpawn = 0;
                mortar2.SpawnWithPoof = false;

                AddEnemyToEnemyList(mortar2);

                mortar3 = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                mortar3.Position = new Vector2(10000, -547);
                mortar3.FacingRight = false;

                mortar3.TimeBeforeSpawn = 0;
                mortar3.SpawnWithPoof = false;

                AddEnemyToEnemyList(mortar3);

                mortar4 = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                mortar4.Position = new Vector2(9700, -547);
                mortar4.FacingRight = false;

                mortar4.TimeBeforeSpawn = 0;
                mortar4.SpawnWithPoof = false;

                AddEnemyToEnemyList(mortar4);
            }
        }

        public override void LoadContent()
        {

            background.Add(content.Load<Texture2D>(@"Maps\History\NoMansValley\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\NoMansValley\background2"));
            background.Add(content.Load<Texture2D>(@"Maps\History\NoMansValley\background3"));
            sky = content.Load<Texture2D>(@"Maps\History\NoMansValley\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\NoMansValley\parallax");
            parallax2 = content.Load<Texture2D>(@"Maps\History\NoMansValley\parallax2");
            foreground = content.Load<Texture2D>(@"Maps\History\NoMansValley\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\NoMansValley\foreground2");
            foreground3 = content.Load<Texture2D>(@"Maps\History\NoMansValley\foreground3");
            barricadeTexture = content.Load<Texture2D>(@"Maps\History\Battlefield\barricade");
            cannonballSprite = content.Load<Texture2D>(@"Maps\History\Battlefield\CannonballSheet");
            crater = content.Load<Texture2D>(@"Maps\History\Battlefield\crater");
            soldier = content.Load<Texture2D>(@"NPC\History\French Soldier");

            game.NPCSprites["Private Brian"] = content.Load<Texture2D>(@"NPC\History\French Soldier");
            Game1.npcFaces["French Soldier"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\French Soldier Normal");
            game.NPCSprites["French Soldier"] = game.NPCSprites["Private Brian"];

            injuredSoldierSprite = content.Load<Texture2D>(@"InteractiveObjects\injuredFrenchSoldier");

            inj1.Sprite = injuredSoldierSprite;
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Private Brian"] = Game1.whiteFilter;
            Game1.npcFaces["Private Brian"].faces["Normal"] = Game1.whiteFilter;
            game.NPCSprites["French Soldier"] = Game1.whiteFilter;
            Game1.npcFaces["French Soldier"].faces["Normal"] = Game1.whiteFilter;
        }


        public override void Update()
        {
            base.Update();

            //TODO make tooutskirts useable when quest is done
            if (game.ChapterTwo.ChapterTwoBooleans["behindGoblinyLinesTwoCompleted"])
            {
                toOutskirts.IsUseable = true;
            }

            for (int i = 0; i < cannonballAmount; i++)
            {
                if (cannonballs[i] != null)
                {
                    cannonballs[i].Update();

                    if (cannonballs[i].finished)
                    {
                        cannonballs[i] = null;
                        cannonballTimers[i] = Game1.randomNumberGen.Next(180, 360);
                        i--;
                    }
                }
            }
            if (enemiesInMap.OfType<GoblinMortar>().Any())
            {
                for (int i = 0; i < cannonballAmount; i++)
                {
                    cannonballTimers[i]--;
                    if (cannonballTimers[i] <= 0)
                    {

                        switch (i)
                        {
                            case 0:
                                cannonballs[i] = (new Cannonball(-180, mapRec.Y + 510, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 1:
                                cannonballs[i] = (new Cannonball(1233, mapRec.Y + 650, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 2:
                                cannonballs[i] = (new Cannonball(2391, mapRec.Y + 662, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 3:
                                cannonballs[i] = (new Cannonball(3651, mapRec.Y + 847, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 4:
                                cannonballs[i] = (new Cannonball(248, mapRec.Y + 486, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 5:
                                cannonballs[i] = (new Cannonball(6252, mapRec.Y + 857, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 6:
                                cannonballs[i] = (new Cannonball(7634, mapRec.Y + 662, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 7:
                                cannonballs[i] = (new Cannonball(9369, mapRec.Y + 487, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 8:
                                cannonballs[i] = (new Cannonball(9797, mapRec.Y + 511, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 9:
                                cannonballs[i] = (new Cannonball(5846, mapRec.Y + 847, cannonballSprite, 50, true, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;

                        }
                    }
                }
            }
            else
            {
                if (game.ChapterTwo.ChapterTwoBooleans["valleyMortarsDestroyed"] == false)
                {
                    game.ChapterTwo.ChapterTwoBooleans["valleyMortarsDestroyed"] = true;
                }
            }

            //--If there aren't max enemies on the screen, spawn more
            if (enemiesInMap.Count < enemyAmount)
            {
                if (spawnEnemies)
                    RespawnGroundEnemies();
            }
            else
                spawnEnemies = false;
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toTrenchfootField = new Portal(2090, platforms[0], "No Man's Valley");
            toOutskirts = new Portal(10300, platforms[0], "No Man's Valley");
            ToOutskirts.IsUseable = false;
            toTrenchfootField.PortalRecY = 115;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTrenchfootField, TrenchfootField.ToNoMansValley);
            portals.Add(toOutskirts, BattlefieldOutskirts.ToNoMansValley);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(barricadeTexture, new Vector2(4250, -50), Color.White);
            s.Draw(barricadeTexture, new Vector2(5750, -50), Color.White);

            s.Draw(crater, new Vector2(-180, mapRec.Y + 500), Color.White);
            s.Draw(crater, new Vector2(248, mapRec.Y + 476), Color.White);
            s.Draw(crater, new Vector2(1233, mapRec.Y + 640), Color.White);
            s.Draw(crater, new Vector2(2391, mapRec.Y + 652), Color.White);
            s.Draw(crater, new Vector2(3651, mapRec.Y + 837), Color.White);

            s.Draw(crater, new Vector2(5846, mapRec.Y + 837), Color.White);
            s.Draw(crater, new Vector2(6252, mapRec.Y + 847), Color.White);
            s.Draw(crater, new Vector2(7634, mapRec.Y + 652), Color.White);
            s.Draw(crater, new Vector2(9369, mapRec.Y + 477), Color.White);
            s.Draw(crater, new Vector2(9797, mapRec.Y + 501), Color.White);

            for (int i = 0; i < cannonballAmount; i++)
            {
                if (cannonballs[i] != null && cannonballs[i].foreground == false)
                    cannonballs[i].Draw(s);
            }
        }
        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White);
            s.Draw(foreground3, new Vector2(foreground.Width + foreground2.Width, mapRec.Y), Color.White);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            for (int i = 0; i < cannonballAmount; i++)
            {
                if (cannonballs[i] != null && cannonballs[i].foreground)
                    cannonballs[i].Draw(s);
            }

            if (game.CurrentChapter.NPCs["Private Brian"].MapName == mapName)
            {
                s.Draw(soldier, new Vector2(4699, mapRec.Y + 1374), Color.White);
                s.Draw(soldier, new Rectangle(4861, mapRec.Y + 1376, soldier.Width, soldier.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, sky.Height), Color.White);

            //s.Draw(clouds, new Vector2(cloudPosX, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.2f, this, game));
            s.Draw(parallax, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(parallax2, new Vector2(parallax.Width, mapRec.Y), Color.White);
            s.End();
        }
    }
}

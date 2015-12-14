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
    class Battlefield : MapClass
    {
        static Portal toNapoleonsCamp;
        static Portal toEnemyMedicalCamp;

        public static Portal ToEnemyMedicalCamp { get { return toEnemyMedicalCamp; } }
        public static Portal ToNapoleonsCamp { get { return toNapoleonsCamp; } }

        Texture2D fog,fog2, barricade, crater, parallax, parallax2, sky, cannonballSprite, injuredSoldierSprite;

        WarBarricade barricade1, barricade2, barricade3, barricade4;

        Cannonball[] cannonballs;
        int[] cannonballTimers;

        int cannonballAmount = 7;
        int intitalBomblinAmount = 4;
        int intitalGoblinAmount = 13;

        GoblinMortar mortar;

        InjuredSoldier inj1, inj2, inj3, inj4;

        public Battlefield(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 8000;
            mapName = "Battlefield";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            cannonballs = new Cannonball[cannonballAmount];
            cannonballTimers = new int[cannonballAmount];

            barricade1 = new WarBarricade(game, 5941, 0, barricade, 5, cannonballSprite, this);
            barricade2 = new WarBarricade(game, 4144, 0, barricade, 5, cannonballSprite, this);
            barricade3 = new WarBarricade(game, 3222, 0, barricade, 5, cannonballSprite, this);
            barricade4 = new WarBarricade(game, 1477, 0, barricade, 5, cannonballSprite, this);

            interactiveObjects.Add(barricade1);
            interactiveObjects.Add(barricade2);
            interactiveObjects.Add(barricade3);
            interactiveObjects.Add(barricade4);

            interactiveObjects.Add(new MapAnimation(game, 2818, 361, true, MapAnimation.AnimationType.shoot, false));
            interactiveObjects.Add(new MapAnimation(game, 7254, 353, false, MapAnimation.AnimationType.shoot, false));
            interactiveObjects.Add(new MapAnimation(game, 2576, 313, true, MapAnimation.AnimationType.FrenchCorpse, false));
            interactiveObjects.Add(new MapAnimation(game, 5121, 402, false, MapAnimation.AnimationType.FrenchCorpse, true));

            inj2 = new InjuredSoldier(6500, 280, player, false, InjuredSoldier.HealedState.normal);
            interactiveObjects.Add(inj2);

            inj3 = new InjuredSoldier(4000, 260, player, false, InjuredSoldier.HealedState.normal);
            interactiveObjects.Add(inj3);

            inj1 = new InjuredSoldier(2100, 280, player, false, InjuredSoldier.HealedState.goblin);
            interactiveObjects.Add(inj1);

            inj4 = new InjuredSoldier(1600, 330, player, true, InjuredSoldier.HealedState.normal, false);
            interactiveObjects.Add(inj4);

            enemyNamesAndNumberInMap.Add("Nurse Goblin", 0);
            enemyNamesAndNumberInMap.Add("Goblin", 0);
            enemyNamesAndNumberInMap.Add("Bomblin", 0);
            enemyNamesAndNumberInMap.Add("Goblin Mortar", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Battlefield\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\Battlefield\background2"));
            fog = content.Load<Texture2D>(@"Maps\History\Battlefield\fog");
            fog2 = content.Load<Texture2D>(@"Maps\History\Battlefield\fog2");

            barricade = content.Load<Texture2D>(@"Maps\History\Battlefield\barricade");
            cannonballSprite = content.Load<Texture2D>(@"Maps\History\Battlefield\CannonballSheet");
            StoneFortCentral.soldierAnimations = ContentLoader.LoadContent(content, "Maps\\History\\SoldierAnimations");
            for (int i = 0; i < cannonballAmount; i++)
            {
                cannonballTimers[i] = Game1.randomNumberGen.Next(180, 360);
            }

            barricade1.Sprite = barricade;
            barricade2.Sprite = barricade;
            barricade3.Sprite = barricade;
            barricade4.Sprite = barricade;

            barricade1.cannonballSprite = cannonballSprite;
            barricade2.cannonballSprite = cannonballSprite;
            barricade3.cannonballSprite = cannonballSprite;
            barricade4.cannonballSprite = cannonballSprite;

            injuredSoldierSprite = content.Load<Texture2D>(@"InteractiveObjects\injuredFrenchSoldier");

            inj1.Sprite = injuredSoldierSprite;
            inj2.Sprite = injuredSoldierSprite;
            inj3.Sprite = injuredSoldierSprite;
            inj4.Sprite = injuredSoldierSprite;

            crater = content.Load<Texture2D>(@"Maps\History\Battlefield\crater");
            sky = content.Load<Texture2D>(@"Maps\History\Battlefield\sky");

            parallax = content.Load<Texture2D>(@"Maps\History\Battlefield\parallax");
            parallax2 = content.Load<Texture2D>(@"Maps\History\Battlefield\parallax2");

            Game1.npcFaces["French Soldier"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\French Soldier Normal");

        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Game1.npcFaces["French Soldier"].faces["Normal"] = Game1.whiteFilter;

        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.GoblinEnemy(content);
            EnemyContentLoader.NurseGoblinEnemy(content);
            EnemyContentLoader.BomblinEnemy(content);
            EnemyContentLoader.GoblinMortarEnemy(content);
            EnemyContentLoader.SharedGoblinSounds(content);
        }

        public void RespawnInitialGroundEnemies()
        {
            base.RespawnGroundEnemies();

            //Mortar
            if (enemyNamesAndNumberInMap["Goblin Mortar"] < 1)
            {
                mortar = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                mortar.Position = new Vector2(100, platforms[platformNum].Rec.Y - mortar.Rec.Height - 30);

                mortar.TimeBeforeSpawn = 0;
                mortar.SpawnWithPoof = false;

                AddEnemyToEnemyList(mortar);
                enemyNamesAndNumberInMap["Goblin Mortar"]++;
            }

            //The rest
            Goblin ben = new Goblin(pos, "Goblin", game, ref player, this);
            ben.Hostile = false;

            monsterY = platforms[platformNum].Rec.Y - ben.Rec.Height - 1;
            ben.Position = new Vector2(monsterX, monsterY);

            if (enemyNamesAndNumberInMap["Goblin"] < 2)
            {
                ben.PositionX = 6300 + (200 * enemyNamesAndNumberInMap["Goblin"]);
                Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                ben.TimeBeforeSpawn = 0;
                ben.SpawnWithPoof = false;

                if (!(benRec.Intersects(player.Rec)))
                {
                    AddEnemyToEnemyList(ben);
                    enemyNamesAndNumberInMap["Goblin"]++;
                }
            }
            else if (enemyNamesAndNumberInMap["Goblin"] < 7)
            {
                ben.PositionX = 4700 + (150 * (enemyNamesAndNumberInMap["Goblin"] - 2));
                Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                ben.TimeBeforeSpawn = 0;
                ben.SpawnWithPoof = false;

                if (!(benRec.Intersects(player.Rec)))
                {
                    AddEnemyToEnemyList(ben);
                    enemyNamesAndNumberInMap["Goblin"]++;
                }
            }
            else if (enemyNamesAndNumberInMap["Goblin"] == 7)
            {
                ben.PositionX = 3800;
                Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                ben.TimeBeforeSpawn = 0;
                ben.SpawnWithPoof = false;

                if (!(benRec.Intersects(player.Rec)))
                {
                    AddEnemyToEnemyList(ben);
                    enemyNamesAndNumberInMap["Goblin"]++;
                }
            }
            else if (enemyNamesAndNumberInMap["Goblin"] < 11)
            {
                ben.PositionX = 1900 + (150 * (enemyNamesAndNumberInMap["Goblin"] - 7));
                Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                ben.TimeBeforeSpawn = 0;
                ben.SpawnWithPoof = false;

                if (!(benRec.Intersects(player.Rec)))
                {
                    AddEnemyToEnemyList(ben);
                    enemyNamesAndNumberInMap["Goblin"]++;
                }
            }
            else if (enemyNamesAndNumberInMap["Goblin"] < 14)
            {
                ben.PositionX = 400 + (200 * (enemyNamesAndNumberInMap["Goblin"] - 10));
                Rectangle benRec = new Rectangle(ben.RecX, monsterY, ben.Rec.Width, ben.Rec.Height);

                ben.TimeBeforeSpawn = 0;
                ben.SpawnWithPoof = false;

                if (!(benRec.Intersects(player.Rec)))
                {
                    AddEnemyToEnemyList(ben);
                    enemyNamesAndNumberInMap["Goblin"]++;
                }
            }

            Bomblin ben2 = new Bomblin(pos, "Bomblin", game, ref player, this);

            monsterY = platforms[platformNum].Rec.Y - ben2.Rec.Height - 1;

            if (enemyNamesAndNumberInMap["Bomblin"] == 1)
            {
                ben2.PositionX = 3700;
                Rectangle benRec = new Rectangle(ben2.RecX, monsterY, ben2.Rec.Width, ben2.Rec.Height);

                ben2.TimeBeforeSpawn = 0;
                ben2.SpawnWithPoof = false;

                if (!(benRec.Intersects(player.Rec)))
                {
                    AddEnemyToEnemyList(ben2);
                    enemyNamesAndNumberInMap["Bomblin"]++;
                }
            }
            else if (enemyNamesAndNumberInMap["Bomblin"] < 3)
            {
                ben2.PositionX = 2100 + (enemyNamesAndNumberInMap["Bomblin"] - 1);
                Rectangle benRec = new Rectangle(ben2.RecX, monsterY, ben2.Rec.Width, ben2.Rec.Height);

                ben2.TimeBeforeSpawn = 0;
                ben2.SpawnWithPoof = false;

                if (!(benRec.Intersects(player.Rec)))
                {
                    AddEnemyToEnemyList(ben2);
                    enemyNamesAndNumberInMap["Bomblin"]++;
                }
            }
            else if (enemyNamesAndNumberInMap["Bomblin"] < 4)
            {
                ben2.PositionX = 400 + (enemyNamesAndNumberInMap["Bomblin"] - 3);
                Rectangle benRec = new Rectangle(ben2.RecX, monsterY, ben2.Rec.Width, ben2.Rec.Height);

                ben2.TimeBeforeSpawn = 0;
                ben2.SpawnWithPoof = false;

                if (!(benRec.Intersects(player.Rec)))
                {
                    AddEnemyToEnemyList(ben2);
                    enemyNamesAndNumberInMap["Bomblin"]++;
                }
            }
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Goblin"] < 6 && enemiesInMap.Count < enemyAmount)
            {
                Goblin en = new Goblin(pos, "Goblin", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Goblin"]++;
                    AddEnemyToEnemyList(en);
                }
            }

            if (enemyNamesAndNumberInMap["Nurse Goblin"] < 3 && enemiesInMap.Count < enemyAmount)
            {
                NurseGoblin en = new NurseGoblin(pos, "Nurse Goblin", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Nurse Goblin"]++;
                    AddEnemyToEnemyList(en);
                }
            }



            if (enemyNamesAndNumberInMap["Bomblin"] < 3 && enemiesInMap.Count < enemyAmount)
            {
                Bomblin ben2 = new Bomblin(pos, "Bomblin", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - ben2.Rec.Height - 1;
                ben2.Position = new Vector2(monsterX, monsterY);

                Rectangle benRec = new Rectangle(ben2.RecX, monsterY, ben2.Rec.Width, ben2.Rec.Height);


                if (!(benRec.Intersects(player.Rec)))
                {
                    ben2.UpdateRectangles();

                    AddEnemyToEnemyList(ben2);
                    enemyNamesAndNumberInMap["Bomblin"]++;
                }
            }
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();


            if (game.chapterState == Game1.ChapterState.chapterOne && enemiesInMap.Count == 0 && game.ChapterOne.ChapterOneBooleans["battlefieldCleared"] == false)
            {
                spawnEnemies = true;
                ResetEnemyNamesAndNumberInMap();
            }
            else if (game.chapterState == Game1.ChapterState.chapterTwo)
                spawnEnemies = true;
        }

        public override void Update()
        {
            base.Update();

            if (game.chapterState > Game1.ChapterState.chapterOne)
            {
                enemyAmount = 12;

                if (enemiesInMap.Count < enemyAmount)
                {
                    RespawnGroundEnemies();
                }
            }

            if (game.chapterState <= Game1.ChapterState.chapterOne)
                toEnemyMedicalCamp.IsUseable = false;
            else
            {
                toEnemyMedicalCamp.IsUseable = true;

                foreach (InteractiveObject o in interactiveObjects)
                {
                    if (o is WarBarricade)
                        (o as WarBarricade).RemoveBarricadeSilently();
                }
            }

            foreach(InteractiveObject o in interactiveObjects)
            {
                if (o is MapAnimation && (!game.ChapterOne.ChapterOneBooleans["meetingNapleonSceneStarted"] || game.chapterState != Game1.ChapterState.chapterOne))
                    o.IsHidden = true;
                else if (o is MapAnimation && game.ChapterOne.ChapterOneBooleans["meetingNapleonSceneStarted"] && o.IsHidden)
                    o.IsHidden = false;

                if (o is InjuredSoldier && game.chapterState <= Game1.ChapterState.chapterOne)
                    o.IsHidden = true;
                else if (o is InjuredSoldier && o.IsHidden)
                    o.IsHidden = false;
            }

            #region Chapter One Attack
            if (game.chapterState == Game1.ChapterState.chapterOne && game.ChapterOne.ChapterOneBooleans["meetingNapleonSceneStarted"])
            {
                if (game.ChapterOne.ChapterOneBooleans["battlefieldCleared"] == false && spawnEnemies)
                {
                    RespawnInitialGroundEnemies();

                    if (enemyNamesAndNumberInMap["Goblin"] == intitalGoblinAmount && enemyNamesAndNumberInMap["Bomblin"] == intitalBomblinAmount)
                    {
                        spawnEnemies = false;
                    }
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

                if (game.ChapterOne.ChapterOneBooleans["battlefieldCleared"] == false && spawnEnemies == false)
                {
                    if (enemyNamesAndNumberInMap["Goblin"] == intitalGoblinAmount - 2 && barricade1.Finished == false)
                        barricade1.DestroyBarricade();
                    else if (enemyNamesAndNumberInMap["Goblin"] == intitalGoblinAmount - 7 && barricade2.Finished == false)
                        barricade2.DestroyBarricade();
                    else if (enemyNamesAndNumberInMap["Goblin"] == intitalGoblinAmount - 8 && enemyNamesAndNumberInMap["Bomblin"] == intitalBomblinAmount - 1 && barricade3.Finished == false)
                        barricade3.DestroyBarricade();
                    else if (enemyNamesAndNumberInMap["Goblin"] == intitalGoblinAmount - 11 && enemyNamesAndNumberInMap["Bomblin"] == intitalBomblinAmount - 3 && barricade4.Finished == false)
                        barricade4.DestroyBarricade();

                    if (enemiesInMap.Count == 0)
                    {
                        game.ChapterOne.ChapterOneBooleans["battlefieldCleared"] = true;
                    }

                    for (int i = 0; i < cannonballAmount; i++)
                    {
                        cannonballTimers[i]--;
                        if (cannonballTimers[i] <= 0)
                        {
                            switch (i)
                            {
                                case 0:
                                    cannonballs[i] = (new Cannonball(6446, 0, cannonballSprite, 50, true));
                                    cannonballTimers[i] = int.MaxValue;
                                    break;
                                case 1:
                                    cannonballs[i] = (new Cannonball(5213, 0, cannonballSprite, 50, true));
                                    cannonballTimers[i] = int.MaxValue;
                                    break;
                                case 2:
                                    cannonballs[i] = (new Cannonball(4641, -15, cannonballSprite, 50, true));
                                    cannonballTimers[i] = int.MaxValue;
                                    break;
                                case 3:
                                    cannonballs[i] = (new Cannonball(3476, 0, cannonballSprite, 50, true));
                                    cannonballTimers[i] = int.MaxValue;
                                    break;
                                case 4:
                                    cannonballs[i] = (new Cannonball(2003, 0, cannonballSprite, 50, true));
                                    cannonballTimers[i] = int.MaxValue;
                                    break;
                                case 5:
                                    cannonballs[i] = (new Cannonball(703, 0, cannonballSprite, 50, true));
                                    cannonballTimers[i] = int.MaxValue;
                                    break;
                                case 6:
                                    cannonballs[i] = (new Cannonball(275, -8, cannonballSprite, 50, true));
                                    cannonballTimers[i] = int.MaxValue;
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toNapoleonsCamp = new Portal(7800, platforms[0], "Battlefield");
            toEnemyMedicalCamp = new Portal(50, platforms[0], "Battlefield");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            if (game.ChapterOne.ChapterOneBooleans["meetingNapleonSceneStarted"])
            {
                s.Draw(crater, new Vector2(6446, -10), Color.White);
                s.Draw(crater, new Vector2(5213, -10), Color.White);
                s.Draw(crater, new Vector2(4641, -25), Color.White);
                s.Draw(crater, new Vector2(3476, -10), Color.White);
                s.Draw(crater, new Vector2(2003, -10), Color.White);
                s.Draw(crater, new Vector2(703, -10), Color.White);
                s.Draw(crater, new Vector2(275, -18), Color.White);
            }

            for (int i = 0; i < cannonballAmount; i++)
            {
                if(cannonballs[i] != null)
                    cannonballs[i].Draw(s);
            }

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEnemyMedicalCamp, GoblinMedicalCamp.ToBattlefield);
            portals.Add(toNapoleonsCamp, NapoleonsCamp.ToBattlefield);
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

            s.Draw(fog, new Rectangle(0, 0, fog.Width, fog.Height), Color.White);
            s.Draw(fog2, new Rectangle(fog.Width, 0, fog2.Width, fog2.Height), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, 0, 8000, sky.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.8f, this, game));
            s.Draw(parallax, new Rectangle(0, 0, parallax.Width, parallax.Height), Color.White);
            s.Draw(parallax2, new Rectangle(parallax.Width, 0, parallax2.Width, parallax2.Height), Color.White);
            s.End();
        }
    }
}

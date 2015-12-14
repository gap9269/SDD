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
    class TrenchfootField : MapClass
    {
        static Portal toNapoleonsCamp;
        static Portal toNoMansValley;
        static Portal toMikrovsHill;

        public static Portal ToMikrovsHill { get { return toMikrovsHill; } }
        public static Portal ToNoMansValley { get { return toNoMansValley; } }
        public static Portal ToNapoleonsCamp { get { return toNapoleonsCamp; } }

        Texture2D foreground, foreground2, fog1, fog2, sky, parallax, parallax2, barricadeTexture, cannonballSprite, crater, injuredSoldierSprite;

        WarBarricade barricade;
        Cannonball[] cannonballs;
        int[] cannonballTimers;
        int cannonballAmount = 5;

        float foreAlpha = 1f;


        GoblinMortar mortar, mortar2, mortar3;

        Boolean spawnMortars = true;

        InjuredSoldier inj1, inj2, inj3;


        public TrenchfootField(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1400;
            mapWidth = 8000;
            mapName = "Trenchfoot Field";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 15;
            zoomLevel = .9f;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            cannonballs = new Cannonball[cannonballAmount];
            cannonballTimers = new int[cannonballAmount];

            barricade = new WarBarricade(game, 4627, -320, barricadeTexture, 5, cannonballSprite, this);

            interactiveObjects.Add(barricade);

            enemyNamesAndNumberInMap.Add("Goblin", 0);
            enemyNamesAndNumberInMap.Add("Bomblin", 0);

            collectibles.Add(new Textbook(0, 630, 1));

            inj2 = new InjuredSoldier(1521, mapRec.Y + 274, player, false, InjuredSoldier.HealedState.skeleton);
            interactiveObjects.Add(inj2);

            inj3 = new InjuredSoldier(2323 + 516, mapRec.Y + 680, player, false, InjuredSoldier.HealedState.goblin, false);
            interactiveObjects.Add(inj3);

            inj1 = new InjuredSoldier(5338 + 516, mapRec.Y + 279, player, false, InjuredSoldier.HealedState.normal);
            interactiveObjects.Add(inj1);
        }

        public override void RespawnGroundEnemies()
        {

            base.RespawnGroundEnemies();


            if (enemyNamesAndNumberInMap["Bomblin"] < 4)
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

            if (enemyNamesAndNumberInMap["Goblin"] < 11)
            {
                if (enemyNamesAndNumberInMap["Goblin"] < 6)
                    platformNum = 0;

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

            if (spawnMortars)
            {
                mortar = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                mortar.Position = new Vector2(5006, mapRec.Y - 93);
                mortar.FacingRight = false;
                mortar.TimeBeforeSpawn = 0;
                mortar.SpawnWithPoof = false;

                AddEnemyToEnemyList(mortar);

                mortar2 = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                mortar2.Position = new Vector2(5260, mapRec.Y - 93);
                mortar2.FacingRight = false;

                mortar2.TimeBeforeSpawn = 0;
                mortar2.SpawnWithPoof = false;

                AddEnemyToEnemyList(mortar2);

                mortar3 = new GoblinMortar(pos, "Goblin Mortar", game, ref player, this);
                mortar3.Position = new Vector2(5514, mapRec.Y - 105);
                mortar3.FacingRight = false;

                mortar3.TimeBeforeSpawn = 0;
                mortar3.SpawnWithPoof = false;

                AddEnemyToEnemyList(mortar3);

                spawnMortars = false;
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\TrenchfootField\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\TrenchfootField\background2"));
            sky = content.Load<Texture2D>(@"Maps\History\TrenchfootField\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\TrenchfootField\parallax");
            parallax2 = content.Load<Texture2D>(@"Maps\History\TrenchfootField\parallax2");
            foreground = content.Load<Texture2D>(@"Maps\History\TrenchfootField\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\TrenchfootField\foreground2");
            fog1 = content.Load<Texture2D>(@"Maps\History\TrenchfootField\foregroundFog");
            fog2 = content.Load<Texture2D>(@"Maps\History\TrenchfootField\foregroundFog2");
            barricadeTexture = content.Load<Texture2D>(@"Maps\History\Battlefield\barricade");
            crater = content.Load<Texture2D>(@"Maps\History\Battlefield\crater");
            cannonballSprite = content.Load<Texture2D>(@"Maps\History\Battlefield\CannonballSheet");

            barricade.Sprite = barricadeTexture;

            injuredSoldierSprite = content.Load<Texture2D>(@"InteractiveObjects\injuredFrenchSoldier");

            inj1.Sprite = injuredSoldierSprite;
            inj2.Sprite = injuredSoldierSprite;
            inj3.Sprite = injuredSoldierSprite;
            Game1.npcFaces["French Soldier"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\French Soldier Normal");

        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            Game1.npcFaces["French Soldier"].faces["Normal"] = Game1.whiteFilter;

        }

        public override void Update()
        {
            base.Update();

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
                                cannonballs[i] = (new Cannonball(93, mapRec.Y + 3, cannonballSprite, 50, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 1:
                                cannonballs[i] = (new Cannonball(1302, mapRec.Y -13, cannonballSprite, 50, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 2:
                                cannonballs[i] = (new Cannonball(2198, mapRec.Y + 3, cannonballSprite, 50, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 3:
                                cannonballs[i] = (new Cannonball(2626, mapRec.Y - 13, cannonballSprite, 50, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;
                            case 4:
                                cannonballs[i] = (new Cannonball(3567, mapRec.Y - 13, cannonballSprite, 50, true));
                                cannonballTimers[i] = int.MaxValue;
                                break;

                        }
                    }
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
            toNapoleonsCamp = new Portal(50, platforms[0], "Trenchfoot Field");
            toNoMansValley = new Portal(7800, platforms[0], "Trenchfoot Field");
            toMikrovsHill = new Portal(4400, platforms[0], "Trenchfoot Field");
            toNapoleonsCamp.PortalRecY = 100;
            toMikrovsHill.PortalRecY = 100;
            toNoMansValley.PortalRecY = 100;

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toNapoleonsCamp, NapoleonsCamp.ToTrenchfootField);
            portals.Add(toNoMansValley, NoMansValley.ToTrenchfootField);
            portals.Add(toMikrovsHill, MikrovsHill.ToTrenchfootField);

        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(crater, new Vector2(93, mapRec.Y -7), Color.White);
            s.Draw(crater, new Vector2(1302, mapRec.Y - 23), Color.White);
            s.Draw(crater, new Vector2(2198, mapRec.Y - 7), Color.White);
            s.Draw(crater, new Vector2(2626, mapRec.Y - 23), Color.White);
            s.Draw(crater, new Vector2(3567, mapRec.Y - 23), Color.White);

            for (int i = 0; i < cannonballAmount; i++)
            {
                if (cannonballs[i] != null)
                    cannonballs[i].Draw(s);
            }
        }
        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (player.PositionY < 50)
            {
                if (foreAlpha < 1f)
                    foreAlpha += .05f;
            }
            else
            {
                if (foreAlpha > 0)
                    foreAlpha -= .05f;
            }

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White * foreAlpha);
            s.Draw(foreground2, new Vector2(foreground.Width, mapRec.Y), Color.White * foreAlpha);

            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            s.Draw(fog1, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(fog2, new Vector2(fog1.Width, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, sky.Height), Color.White);

            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1, this, game));
            s.Draw(parallax, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(parallax2, new Vector2(parallax.Width, mapRec.Y), Color.White);
            s.End();
        }
    }
}

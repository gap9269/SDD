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
    class PharaohsTrap : MapClass
    {
        static Portal toPharaohsRoad;
        static Portal toHiddenPassage;

        public static Portal ToHiddenPassage { get { return toHiddenPassage; } }
        public static Portal ToPharaohsRoad { get { return toPharaohsRoad; } }

        Texture2D foreground, crackedTile, floor, farBackground, levelTwoFOreground, transition, bars;

        int locustAmount;
        int mummyAmount;
        int vileMummyAmount;
        int anubisAmount;
        int scorpionAmount;

        int enemyState;
        Boolean spawningEnemies = false;

        Platform upperFloor;

        int brokenFloorTransitionTime = 250;
        float foregroundTwoAlpha = 0f;

        public PharaohsTrap(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1500;
            mapWidth = 2300;
            mapName = "Pharaoh's Trap";

            mapRec = new Rectangle(0, -410, mapWidth, mapHeight);
            enemyAmount = 5;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            upperFloor = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(-19, 187, 2350, 50), false, true, false);
            platforms.Add(upperFloor);

            enemyNamesAndNumberInMap.Add("Locust", 0);
            enemyNamesAndNumberInMap.Add("Mummy", 0);
            enemyNamesAndNumberInMap.Add("Vile Mummy", 0);
            enemyNamesAndNumberInMap.Add("Scorpadillo", 0);
            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);
        }

        public override void LoadContent()
        {
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsTrap\foreground");
            crackedTile = content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsTrap\crackedTile");
            floor = content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsTrap\floor");
            levelTwoFOreground = content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsTrap\levelTwoForeground");
            transition = content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsTrap\transition");
            farBackground = content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsTrap\background");
            bars = content.Load<Texture2D>(@"Maps\History\Pyramid\PharaohsTrap\bars");
            background.Add(floor);
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.LocustEnemy(content);
            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.MummyEnemy(content);
            EnemyContentLoader.ScorpadilloEnemy(content);
            EnemyContentLoader.VileMummyEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Anubis Warrior"] < anubisAmount)
            {
                AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = 187 - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 10;
                en.Hostile = true;
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

            if (enemyNamesAndNumberInMap["Mummy"] < mummyAmount)
            {
                Mummy en = new Mummy(pos, "Mummy", game, ref player, this);
                monsterY = 187 - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 10;
                en.Hostile = true;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Mummy"]++;
                    AddEnemyToEnemyList(en);
                }
            }

            if (enemyNamesAndNumberInMap["Vile Mummy"] < vileMummyAmount)
            {
                VileMummy en = new VileMummy(pos, "Vile Mummy", game, ref player, this);
                monsterY = 187 - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 10;
                en.Hostile = true;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Vile Mummy"]++;
                    AddEnemyToEnemyList(en);
                }
            }

            if (enemyNamesAndNumberInMap["Scorpadillo"] < scorpionAmount)
            {
                Scorpadillo en = new Scorpadillo(pos, "Scorpadillo", game, ref player, this);
                monsterY = platforms[0].RecY - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 10;
                en.Hostile = true;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }

                else
                {
                    en.UpdateRectangles();
                    enemyNamesAndNumberInMap["Scorpadillo"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);

            if (enemyNamesAndNumberInMap["Locust"] < locustAmount)
            {
                Locust en = new Locust(pos, "Locust", game, ref player, this, mapRec);

                en.TimeBeforeSpawn = 10;

                Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
                if (testRec.Intersects(player.Rec))
                {
                }
                else
                {
                    enemyNamesAndNumberInMap["Locust"]++;
                    AddEnemyToEnemyList(en);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (!game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapStarted"] && !game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapCleared"])
            {
                AnubisWarrior en = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                monsterY = 187 - en.Rec.Height - 10;
                en.Position = new Vector2(1100, monsterY);
                en.FacingRight = false;

                en.UpdateRectangles();
                enemyNamesAndNumberInMap["Anubis Warrior"]++;
                AddEnemyToEnemyList(en);

                AnubisWarrior en2 = new AnubisWarrior(pos, "Anubis Warrior", game, ref player, this);
                en2.Position = new Vector2(1500, 187 - en2.Rec.Height - 10);
                en2.FacingRight = false;

                en.UpdateRectangles();
                enemyNamesAndNumberInMap["Anubis Warrior"]++;
                AddEnemyToEnemyList(en2);
                
                game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapStarted"] = true;
                game.Camera.ShakeCamera(20, 10);

                toHiddenPassage.IsUseable = false;
                toPharaohsRoad.IsUseable = false;

                enemyState = 1;
            }
            if (enemiesInMap.Count == 0 && !spawningEnemies && !game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapCleared"])
            {
                if (enemyState < 10)
                {
                    enemyState++;

                    spawningEnemies = true;
                    switch (enemyState)
                    {
                        case 2:
                            anubisAmount = 3;
                            break;
                        case 3:
                            anubisAmount = 3;
                            locustAmount = 4;
                            break;
                        case 4:
                            anubisAmount = 3;
                            locustAmount = 2;
                            mummyAmount = 2;
                            vileMummyAmount = 2;
                            break;
                        case 5:
                            anubisAmount = 2;
                            locustAmount = 2;
                            mummyAmount = 3;
                            vileMummyAmount = 1;
                            break;
                        case 6:
                            anubisAmount = 2;
                            locustAmount = 7;
                            mummyAmount = 0;
                            vileMummyAmount = 4;
                            break;
                        case 7:
                            anubisAmount = 5;
                            locustAmount = 0;
                            mummyAmount = 0;
                            vileMummyAmount = 2;
                            break;
                        case 8:
                            anubisAmount = 0;
                            locustAmount = 4;
                            mummyAmount = 0;
                            vileMummyAmount = 6;
                            break;
                        case 9:
                            anubisAmount = 3;
                            locustAmount = 3;
                            mummyAmount = 4;
                            vileMummyAmount = 2;
                            break;
                        case 10:
                            anubisAmount = 0;
                            locustAmount = 0;
                            mummyAmount = 0;
                            vileMummyAmount = 0;
                            spawningEnemies = false;
                            game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapFloorBroken"] = true;
                            break;
                    }
                }

                if (enemyState == 11)
                {
                    game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapCleared"] = true;
                    game.Camera.ShakeCamera(20, 10);
                }
            }

            if (game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapCleared"] && toHiddenPassage.IsUseable == false)
            {
                toHiddenPassage.IsUseable = true;
                toPharaohsRoad.IsUseable = true;
            }

            if (foregroundTwoAlpha >= 1 && enemyState == 10)
            {
                enemyState = 11;
                spawningEnemies = true;
                scorpionAmount = 6;
            }

            if (!game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapCleared"])
            {
                if (game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapFloorBroken"] && platforms.Contains(upperFloor))
                {
                    platforms.Remove(upperFloor);

                    player.CurrentPlat = null;
                    player.PositionY += 20;

                }
                if (spawningEnemies)
                {
                    RespawnFlyingEnemies(new Rectangle(100, mapRec.Y + 100, mapWidth - 200, 400));
                    RespawnGroundEnemies();

                    if (enemiesInMap.Count == mummyAmount + vileMummyAmount + anubisAmount + locustAmount + scorpionAmount)
                        spawningEnemies = false;
                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toPharaohsRoad = new Portal(50, 187, "Pharaoh's Trap");
            toHiddenPassage = new Portal(2030, platforms[0], "Pharaoh's Trap");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (background[0] == floor && enemyState >= 6)
            {
                background[0] = crackedTile;
            }

            if (background[0] != Game1.whiteFilter && game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapFloorBroken"])
            {
                background[0] = Game1.whiteFilter;
                background.Add( Game1.whiteFilter);
            }

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPharaohsRoad, PharaohsRoad.ToPharaohsTrap);
            portals.Add(toHiddenPassage, HiddenPassage.ToPharaohsTrap);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapFloorBroken"] && brokenFloorTransitionTime > 0)
            {
                brokenFloorTransitionTime--;
                s.Draw(transition, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), Color.White);
            }
            else if (game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapFloorBroken"] && brokenFloorTransitionTime <= 0)
            {
                if (foregroundTwoAlpha < 1f)
                    foregroundTwoAlpha += .01f;

                s.Draw(levelTwoFOreground, new Vector2(0, mapRec.Y), Color.White);
                s.Draw(transition, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), Color.White * (1 - foregroundTwoAlpha));
            }
            else if (!game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapFloorBroken"])
                s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(farBackground, new Vector2(0, mapRec.Y), Color.White);

            if (game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapStarted"] && !game.ChapterTwo.ChapterTwoBooleans["pharaohsTrapCleared"])
                s.Draw(bars, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }
    }
}

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
    class HiddenPassage : MapClass
    {
        static Portal toPharaohsTrap;
        static Portal toRoomOfRedundancy;
        public static Portal ToRoomOfRedundancy { get { return toRoomOfRedundancy; } }
        public static Portal ToPharaohsTrap { get { return toPharaohsTrap; } }

        Texture2D foreground, bars;

        int mummyAmount;
        int vileMummyAmount;
        int anubisAmount;
        int scorpionAmount;

        int enemyState;
        Boolean spawningEnemies = false;

        public HiddenPassage(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2300;
            mapName = "Hidden Passage";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Mummy", 0);
            enemyNamesAndNumberInMap.Add("Vile Mummy", 0);
            enemyNamesAndNumberInMap.Add("Scorpadillo", 0);
            enemyNamesAndNumberInMap.Add("Anubis Warrior", 0);

            SpikeTrap spikeTrap1 = new SpikeTrap(180, 1713, 504, game, 80);
            SpikeTrap spikeTrap2 = new SpikeTrap(180, 1013, 504, game, 80);
            SpikeTrap spikeTrap3 = new SpikeTrap(180, 289, 504, game, 80);

            spikeTrap1.Timer = 35;
            spikeTrap2.Timer = 35;
            spikeTrap3.Timer = 35;

            mapHazards.Add(spikeTrap1);
            mapHazards.Add(spikeTrap2);
            mapHazards.Add(spikeTrap3);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\HiddenPassage\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\HiddenPassage\foreground");
            bars = content.Load<Texture2D>(@"Maps\History\Pyramid\HiddenPassage\bars");

        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

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
                monsterY = platforms[platformNum].RecY - en.Rec.Height - 10;
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
                monsterY = platforms[platformNum].RecY - en.Rec.Height - 10;
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
                monsterY = platforms[platformNum].RecY - en.Rec.Height - 10;
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
                monsterY = platforms[platformNum].RecY - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 10;

                if(enemyState != 1)
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

        public override void Update()
        {
            base.Update();

            if (!game.ChapterTwo.ChapterTwoBooleans["hiddenPassageLocked"] && !game.ChapterTwo.ChapterTwoBooleans["hiddenPassageCleared"])
            {

                game.ChapterTwo.ChapterTwoBooleans["hiddenPassageLocked"] = true;
                game.Camera.ShakeCamera(20, 10);

                toPharaohsTrap.IsUseable = false;
                toRoomOfRedundancy.IsUseable = false;

                enemyState = 1;
            }

            if (enemiesInMap.Count == 0 && !spawningEnemies && !game.ChapterTwo.ChapterTwoBooleans["hiddenPassageCleared"])
            {

                enemyState++;

                spawningEnemies = true;
                switch (enemyState)
                {
                    case 1:
                        scorpionAmount = 4;
                        break;
                    case 2:
                        anubisAmount = 3;
                        scorpionAmount = 2;
                        break;
                    case 3:
                        anubisAmount = 2;
                        scorpionAmount = 2;
                        break;
                    case 4:
                        anubisAmount = 3;
                        mummyAmount = 2;
                        vileMummyAmount = 2;
                        scorpionAmount = 2;
                        break;
                    case 5:
                        game.ChapterTwo.ChapterTwoBooleans["hiddenPassageCleared"] = true;
                        game.Camera.ShakeCamera(20, 10);
                        spawningEnemies = false;
                        anubisAmount = 0;
                        mummyAmount = 0;
                        vileMummyAmount = 0;
                        scorpionAmount = 0;
                        break;
                }

            }

            if (game.ChapterTwo.ChapterTwoBooleans["hiddenPassageCleared"] && toPharaohsTrap.IsUseable == false)
            {
                toPharaohsTrap.IsUseable = true;
                toRoomOfRedundancy.IsUseable = true;
            }

            if (!game.ChapterTwo.ChapterTwoBooleans["hiddenPassageCleared"])
            {
                if (spawningEnemies)
                {
                    RespawnFlyingEnemies(new Rectangle(100, 100, mapWidth - 200, 400));
                    RespawnGroundEnemies();

                    if (enemiesInMap.Count == mummyAmount + vileMummyAmount + anubisAmount + scorpionAmount)
                        spawningEnemies = false;
                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toPharaohsTrap = new Portal(2100, platforms[0], "Hidden Passage");
            toRoomOfRedundancy = new Portal(50, platforms[0], "Hidden Passage");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);


            if (game.ChapterTwo.ChapterTwoBooleans["hiddenPassageLocked"] && !game.ChapterTwo.ChapterTwoBooleans["hiddenPassageCleared"])
                s.Draw(bars, new Vector2(0, mapRec.Y), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPharaohsTrap, PharaohsTrap.ToHiddenPassage);
            portals.Add(toRoomOfRedundancy, PharaohsGate.ToHiddenPassage);
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

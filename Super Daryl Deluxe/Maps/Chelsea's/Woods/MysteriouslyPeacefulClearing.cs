using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class MysteriouslyPeacefulClearing : MapClass
    {
        static Portal toCrossroads;
        static Portal toDeepWoods;

        public static Portal ToDeepWoods { get { return toDeepWoods; } }
        public static Portal ToCrossroads { get { return toCrossroads; } }

        float pitCoverAlpha2 = 0f;
        float pitCoverAlpha = 0f;

        Texture2D front, coverPit, coverUnderground;


        Boolean clearedPit = false;
        Boolean clearedPit2 = false;
        Boolean clearedPit3 = false;

        Platform step, step1, step2, step3, step4; //First pitfall steps
        Platform step21, step22, step23; //Second pitfall steps
        Platform step31, step32, step33; //Third pitfall steps
        Platform step41, step42; //Fourth pitfall steps

        List<Enemy> pitfallEnemies;
        Boolean pitfallEnemiesSpawned = false;
        Boolean pitfallEnemiesSpawned2 = false;
        Boolean pitfallEnemiesSpawned3 = false;

        PitfallCover pitfall, pitfall2, pitfall3, pitfall4;

        public MysteriouslyPeacefulClearing(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4000;
            mapWidth = 5000;
            mapName = "Mysteriously Peaceful Clearing";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;
            zoomLevel = .8f;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            step = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3920, -1030, 250, 50), true, false, false);
            step1 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3920, -750, 200, 50), true, false, false);
            step2 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3920, -465, 200, 50), true, false, false);
            step3 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3920, -200, 200, 50), true, false, false);
            step4 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3920, 80, 200, 50), true, false, false);

            step21 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2240, -1040, 200, 50), true, false, false);
            step22 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2240, -740, 200, 50), true, false, false);
            step23 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2240, -450, 200, 50), true, false, false);

            step31 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(440, -1050, 200, 50), true, false, false);
            step32 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(440, -760, 200, 50), true, false, false);
            step33 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(440, -490, 200, 50), true, false, false);

            step41 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1540, 80, 200, 50), true, false, false);
            step42 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1540, 380, 200, 50), true, false, false);

            pitfall = new PitfallCover(game, 3700, -1170, true, false, PitfallCover.PitfallType.leaves);
            interactiveObjects.Add(pitfall);

            pitfall4 = new PitfallCover(game, 1660, -50, true, false, PitfallCover.PitfallType.leaves);
            interactiveObjects.Add(pitfall4);

            pitfall2 = new PitfallCover(game, 2400, -1170, true, false, PitfallCover.PitfallType.leaves);
            interactiveObjects.Add(pitfall2);

            pitfall3 = new PitfallCover(game, 580, -1170, true, false, PitfallCover.PitfallType.leaves);
            interactiveObjects.Add(pitfall3);

            pitfallEnemies = new List<Enemy>();
        }

        public override void LoadContent()
        {
            background.Add(Game1.whiteFilter);
            //background.Add(content.Load<Texture2D>(@"Maps\Chelseas\DeepWoods2"));
            front = content.Load<Texture2D>(@"Maps\Chelseas\DeepWoodsFrontRight");
            coverPit = content.Load<Texture2D>(@"Maps\Chelseas\DeepWoodsCoverPit");
            coverUnderground = content.Load<Texture2D>(@"Maps\Chelseas\DeepWoodsCoverUnder");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Field Goblin", content.Load<Texture2D>(@"EnemySprites\FieldGoblinSheet"));
        }

        public void SpawnPitfallEnemies(int pitfallNum)
        {
            Goblin en;
            if (pitfallNum == 1)
            {
                monsterX = rand.Next(3280, 3580);
                pos = new Vector2(monsterX, monsterY);
                en = new Goblin(pos, "Field Goblin", game, ref player, this);
                monsterY = 224 - en.Rec.Height - 1;
            }
            else if (pitfallNum == 12)
            {
                monsterX = rand.Next(3300, 4651);
                pos = new Vector2(monsterX, monsterY);
                en = new Goblin(pos, "Field Goblin", game, ref player, this);
                monsterY = 630 - en.Rec.Height - 1;
            }

            else if (pitfallNum == 2)
            {
                monsterX = rand.Next(2380, 3380);
                pos = new Vector2(monsterX, monsterY);
                en = new Goblin(pos, "Field Goblin", game, ref player, this);
                monsterY = -251 - en.Rec.Height - 1;
            }
            else
            {
                monsterX = rand.Next(559, 1500);
                pos = new Vector2(monsterX, monsterY);
                en = new Goblin(pos, "Field Goblin", game, ref player, this);
                monsterY = -211 - en.Rec.Height - 1;
            }



            en.Position = new Vector2(monsterX, monsterY);
            en.TimeBeforeSpawn = 20;
            en.Hostile = true;
            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                enemiesInMap.Add(en);
                pitfallEnemies.Add(en);
            }

        }

        public override void Update()
        {
            base.Update();
            if (pitfall.Finished && player.CurrentPlat != null && !clearedPit && !pitfallEnemiesSpawned)
            {

                if (pitfallEnemies.Count < 3)
                {
                    SpawnPitfallEnemies(1);
                }
                else if (pitfallEnemies.Count < 10)
                {
                    SpawnPitfallEnemies(12);
                }
                else if (pitfallEnemies.Count == 10)
                    pitfallEnemiesSpawned = true;
            }

            if (pitfall2.Finished && player.CurrentPlat != null && !clearedPit2 && !pitfallEnemiesSpawned2)
            {
                SpawnPitfallEnemies(2);

                if (pitfallEnemies.Count == 6)
                    pitfallEnemiesSpawned2 = true;
            }

            if (pitfall3.Finished && player.CurrentPlat != null && !clearedPit3 && !pitfallEnemiesSpawned3)
            {
                SpawnPitfallEnemies(3);

                if (pitfallEnemies.Count == 6)
                    pitfallEnemiesSpawned3 = true;
            }

            if (pitfall4.Finished && player.CurrentPlat != null && !platforms.Contains(step41))
            {
                platforms.Add(step41);
                platforms.Add(step42);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(step41.Rec.X + step41.Rec.Width / 2 - 75, step41.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step42.Rec.X + step42.Rec.Width / 2 - 75, step42.Rec.Y - 50, 150, 150), 2);
            }

            //Clear the enemies from the list if they are dead
            for (int i = 0; i < pitfallEnemies.Count; i++)
            {
                if (!enemiesInMap.Contains(pitfallEnemies[i]))
                {
                    pitfallEnemies.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            if (pitfallEnemies.Count == 0)
            {
                if(pitfallEnemiesSpawned && !clearedPit)
                    clearedPit = true;
                else if (pitfallEnemiesSpawned2 && !clearedPit2)
                    clearedPit2 = true;
                else if (pitfallEnemiesSpawned3 && !clearedPit3)
                    clearedPit3 = true;
            }

            if (!platforms.Contains(step) && clearedPit)
            {
                platforms.Add(step);
                platforms.Add(step1);
                platforms.Add(step2);
                platforms.Add(step3);
                platforms.Add(step4);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(step.Rec.X + step.Rec.Width / 2 - 75, step.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step1.Rec.X + step1.Rec.Width / 2 - 75, step1.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step2.Rec.X + step2.Rec.Width / 2 - 75, step2.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step3.Rec.X + step3.Rec.Width / 2 - 75, step3.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step4.Rec.X + step4.Rec.Width / 2 - 75, step4.Rec.Y - 50, 150, 150), 2);
            }

            if (!platforms.Contains(step21) && clearedPit2)
            {
                platforms.Add(step21);
                platforms.Add(step22);
                platforms.Add(step23);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(step21.Rec.X + step21.Rec.Width / 2 - 75, step21.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step22.Rec.X + step22.Rec.Width / 2 - 75, step22.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step23.Rec.X + step23.Rec.Width / 2 - 75, step23.Rec.Y - 50, 150, 150), 2);

            }

            if (!platforms.Contains(step31) && clearedPit3)
            {
                platforms.Add(step31);
                platforms.Add(step32);
                platforms.Add(step33);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(step31.Rec.X + step31.Rec.Width / 2 - 75, step31.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step32.Rec.X + step32.Rec.Width / 2 - 75, step32.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step33.Rec.X + step33.Rec.Width / 2 - 75, step33.Rec.Y - 50, 150, 150), 2);

            }

            if (enemiesInMap.Count < enemyAmount)
            {
                RespawnGroundEnemies();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCrossroads = new Portal(0, -1320, "MysteriouslyPeacefulClearing");
            toDeepWoods = new Portal(4800, -1320, "MysteriouslyPeacefulClearing");
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(front, new Rectangle(background[0].Width, mapRec.Y, front.Width, front.Height), Color.White);
            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            if (player.VitalRec.X < 3100 && player.VitalRecX > 2000 && player.VitalRecY > -1100)
            {
                if (pitCoverAlpha > 0)
                    pitCoverAlpha -= .05f;
            }
            else
            {
                if (pitCoverAlpha < 1f)
                    pitCoverAlpha += .05f;
            }
          //  s.Draw(coverPit, new Rectangle(0, mapRec.Y, coverPit.Width, coverPit.Height), Color.White * pitCoverAlpha);

            s.End();
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCrossroads, Crossroads.ToClearing);
            portals.Add(toDeepWoods, DeepWoods.ToClearing);
        }
    }
}

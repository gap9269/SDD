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
    class DeepWoods : MapClass
    {
        static Portal toTheWoods;
        static Portal toClearing;

        public static Portal ToClearing { get { return toClearing; } }
        public static Portal ToTheWoods { get { return toTheWoods; } }

        float undergroundCoverAlpha = 0f;
        float pitCoverAlpha = 0f;

        Texture2D front, coverPit, coverUnderground;


        Boolean clearedPit = false;
        Platform step, step1, step2, step3;

        List<Enemy> pitfallEnemies;
        Boolean pitfallEnemiesSpawned = false;

        PitfallCover pitfall;

        public DeepWoods(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4000;
            mapWidth = 6860;
            mapName = "Deep Woods";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 10;
            zoomLevel = .8f;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            step = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2440, -100, 250, 50), true, false, false);
            step1 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2440, -350, 200, 50), true, false, false);
            step2 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2440, -600, 200, 50), true, false, false);
            step3 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2440, -850, 200, 50), true, false, false);


            pitfall = new PitfallCover(game, 2535, -1000, true, false, PitfallCover.PitfallType.leaves);
            interactiveObjects.Add(pitfall);

            pitfallEnemies = new List<Enemy>();
        }
        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\DeepWoods1"));
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\DeepWoods2"));
            front = content.Load<Texture2D>(@"Maps\Chelseas\DeepWoodsFrontRight");
            coverPit = content.Load<Texture2D>(@"Maps\Chelseas\DeepWoodsCoverPit");
            coverUnderground = content.Load<Texture2D>(@"Maps\Chelseas\DeepWoodsCoverUnder");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Field Goblin", content.Load<Texture2D>(@"EnemySprites\FieldGoblinSheet"));
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            Goblin en = new Goblin(pos, "Field Goblin", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
            en.Position = new Vector2(monsterX, monsterY);
            en.TimeBeforeSpawn = 120;


            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                enemiesInMap.Add(en);
            }

        }

        public void SpawnPitfallEnemies()
        {
            monsterX = rand.Next(2230, 3130);
            pos = new Vector2(monsterX, monsterY);

            Goblin en = new Goblin(pos, "Field Goblin", game, ref player, this);
            monsterY = -2 - en.Rec.Height - 1;
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
                SpawnPitfallEnemies();

                if (pitfallEnemies.Count == 4)
                {
                    pitfallEnemiesSpawned = true;
                }
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

            if (pitfallEnemies.Count == 0 && pitfallEnemiesSpawned)
                clearedPit = true;
            
            if(!platforms.Contains(step) && clearedPit)
            {
                platforms.Add(step);
                platforms.Add(step1);
                platforms.Add(step2);
                platforms.Add(step3);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(step.Rec.X + step.Rec.Width / 2 - 75, step.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step1.Rec.X + step1.Rec.Width / 2 - 75, step1.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step2.Rec.X + step2.Rec.Width / 2 - 75, step2.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step3.Rec.X + step3.Rec.Width / 2 - 75, step3.Rec.Y - 50, 150, 150), 2);
            }

            if (enemiesInMap.Count < enemyAmount)
            {
                RespawnGroundEnemies();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheWoods = new Portal(4400, 660, "DeepWoods");
            toClearing = new Portal(100, -1717, "DeepWoods");
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
            s.Draw(coverPit, new Rectangle(0, mapRec.Y, coverPit.Width, coverPit.Height), Color.White * pitCoverAlpha);

            //some magic math shit right here
            undergroundCoverAlpha = (((4500f) - (float)(player.VitalRecX))) / 1000f;

            s.Draw(coverUnderground, new Rectangle(background[0].Width, mapRec.Y, coverUnderground.Width, coverUnderground.Height), Color.White * undergroundCoverAlpha);
            s.End();
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheWoods, TheWoods.ToDeepWoods);
            portals.Add(toClearing, MysteriouslyPeacefulClearing.ToDeepWoods);
        }
    }
}

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
    class TheWoods : MapClass
    {
        static Portal toTheField;
        static Portal toDeepWoods;

        public static Portal ToDeepWoods { get { return toDeepWoods; } }
        public static Portal ToTheField { get { return toTheField; } }

        TreasureChest trollChest;

        FieldTroll troll;

        public TheWoods(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3100;
            mapName = "The Woods";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            //enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();



            //--Map Quest
            enemiesToKill.Add(30);
            enemiesKilledForQuest.Add(0);
            enemyNames.Add("Field Goblin");
            mapWithMapQuest = true;

            MapQuestSign sign = new MapQuestSign(2300, platforms[0].Rec.Y - Game1.mapSign.Height + 20, "Slay Thirty Mushrooms", enemiesToKill,
enemiesKilledForQuest, enemyNames, player, new List<Object>() { new Experience(50), new Karma(2) });
            mapQuestSigns.Add(sign);

            trollChest = new TreasureChest(Game1.treasureChestSheet, -2000, 0, player, 0, new GoldKey(), this);
            treasureChests.Add(trollChest);


        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();

            troll.ClearGroundHoles();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\SpookyField"));
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Goblin", content.Load<Texture2D>(@"EnemySprites\GoblinSheet"));

            game.EnemySpriteSheets.Add("Field Troll", content.Load<Texture2D>(@"EnemySprites\TrollSprite"));
            game.EnemySpriteSheets.Add("TrollFall", content.Load<Texture2D>(@"EnemySprites\TrollFallSprite"));
            game.EnemySpriteSheets.Add("TrollAttack", content.Load<Texture2D>(@"EnemySprites\TrollAttackSprite"));
            game.EnemySpriteSheets.Add("TrollClubGone", content.Load<Texture2D>(@"EnemySprites\TrollClubDisappearSprite"));
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            Goblin en = new Goblin(pos, "Goblin", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
            en.Position = new Vector2(monsterX, monsterY);
            en.TimeBeforeSpawn = 120;

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                AddEnemyToEnemyList(en);
            }

        }

        public override void Update()
        {
            base.Update();

            //Map quest
            if (enemiesKilledForQuest[0] >= enemiesToKill[0])
            {
                completedMapQuest = true;
                mapQuestSigns[0].CompletedQuest = true;
            }

            
            if (enemiesInMap.Count < 1)
            {
                troll = new FieldTroll(new Vector2(3060, 50), "Field Troll", game, ref player, this, trollChest);
                troll.Hostile = true;
                AddEnemyToEnemyList(troll);
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheField = new Portal(2900, platforms[0], "The Woods");
            toDeepWoods = new Portal(800, platforms[0], "The Woods");
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }
            s.End();
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheField, ChelseasField.ToTheWoods);
            portals.Add(toDeepWoods, DeepWoods.ToTheWoods);
        }
    }
}

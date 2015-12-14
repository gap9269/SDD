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
    class GoblinMedicalCamp : MapClass
    {
        static Portal toBattlefield;
        static Portal toMikrovsHill;

        public static Portal ToMikrovsHill { get { return toMikrovsHill; } }
        public static Portal ToBattlefield { get { return toBattlefield; } }

        Texture2D parallax, foreground, sky, vendingMachine, injuredSoldierSprite;

        FirstAidVendingMachine machine1, machine2;

        InjuredSoldier inj1;

        public GoblinMedicalCamp(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Enemy Medical Camp";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            machine1 = new FirstAidVendingMachine(game, 257, 375, null, 5, 1.37f, 2);
            machine2 = new FirstAidVendingMachine(game, 2305, 375, null, 5, .89f, 1);

            interactiveObjects.Add(machine1);
            interactiveObjects.Add(machine2);

            enemyNamesAndNumberInMap.Add("Nurse Goblin", 0);

            inj1 = new InjuredSoldier(257 + 516, 360, player, true, InjuredSoldier.HealedState.goblin);
            interactiveObjects.Add(inj1);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Nurse Goblin"] < 5)
            {
                NurseGoblin en = new NurseGoblin(pos, "Nurse Goblin", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

                en.TimeBeforeSpawn = 230;

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
        }

        public override void LoadContent()
        {
            vendingMachine = content.Load<Texture2D>(@"Maps\History\GoblinMedicalCamp\vendingMachine");
            machine1.Sprite = vendingMachine;
            machine2.Sprite = vendingMachine;
            background.Add(content.Load<Texture2D>(@"Maps\History\GoblinMedicalCamp\background"));
            parallax = content.Load<Texture2D>(@"Maps\History\GoblinMedicalCamp\parallax");
            foreground = content.Load<Texture2D>(@"Maps\History\GoblinMedicalCamp\foreground");
            sky = content.Load<Texture2D>(@"Maps\History\GoblinMedicalCamp\sky");

            injuredSoldierSprite = content.Load<Texture2D>(@"InteractiveObjects\injuredFrenchSoldier");
            inj1.Sprite = injuredSoldierSprite;

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
            EnemyContentLoader.SharedGoblinSounds(content);
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount)
                RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBattlefield = new Portal(2800, platforms[0], "Enemy Medical Camp");
            toMikrovsHill = new Portal(50, platforms[0], "Enemy Medical Camp");

        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBattlefield, Battlefield.ToEnemyMedicalCamp);
            portals.Add(toMikrovsHill, MikrovsHill.ToGoblinMedicalCamp);
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

            //s.Draw(foreground, new Rectangle(0, 0, mapWidth, mapHeight), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.15f, this, game));
            s.Draw(sky, new Rectangle(0, 0, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(parallax, new Rectangle(0, 0, parallax.Width, parallax.Height), Color.White);

            s.End();
        }
    }
}

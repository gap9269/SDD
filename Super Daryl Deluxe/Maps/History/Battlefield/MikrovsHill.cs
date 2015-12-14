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
    class MikrovsHill : MapClass
    {
        static Portal toGoblinMedicalCamp;
        static Portal toTrenchfootField;

        public static Portal ToTrenchfootField { get { return toTrenchfootField; } }
        public static Portal ToGoblinMedicalCamp { get { return toGoblinMedicalCamp; } }

        Texture2D foreground, foregroundFog, sky, parallax, elevator, injuredSoldierSprite;

        float foreAlpha = 1f;

        MovingPlatform movingPlat2;
        List<Vector2> targets2;

        InjuredSoldier inj1, inj2, inj3;

        public MikrovsHill(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 4090;
            mapWidth = 3000;
            mapName = "Mikrov's Hill";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 16;
            zoomLevel = .85f;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyNamesAndNumberInMap.Add("Goblin", 0);
            enemyNamesAndNumberInMap.Add("Bomblin", 0);

            targets2 = new List<Vector2>();

            movingPlat2 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(337, mapRec.Y + 3097, 250, 50),
                true, false, false, targets2, 4, 5, Platform.PlatformType.rock);

            platforms.Add(movingPlat2);

            treasureChests.Add(new TreasureChest(Game1.treasureChestSheet, 1000, -1350, player, 0, new Textbook(), this));
            treasureChests.Add(new TreasureChest(Game1.treasureChestSheet, 1300, -1350, player, 0, new EnemyDrop("Topaz", new Rectangle()), this));

            inj2 = new InjuredSoldier(2600, mapRec.Y + 3390, player, false, InjuredSoldier.HealedState.goblin);
            interactiveObjects.Add(inj2);

            inj3 = new InjuredSoldier(587 + 516, mapRec.Y + 3388, player, false, InjuredSoldier.HealedState.skeleton, false);
            interactiveObjects.Add(inj3);

            inj1 = new InjuredSoldier(1892 + 516, mapRec.Y + 1044, player, false, InjuredSoldier.HealedState.normal);
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

            if (enemyNamesAndNumberInMap["Goblin"] < 4)
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
            EnemyContentLoader.GoblinEnemy(content);
            EnemyContentLoader.BomblinEnemy(content);
            EnemyContentLoader.SharedGoblinSounds(content);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\MikrovsHill\background"));
            sky = content.Load<Texture2D>(@"Maps\History\MikrovsHill\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\MikrovsHill\parallax");
            foreground = content.Load<Texture2D>(@"Maps\History\MikrovsHill\foreground");
            foregroundFog = content.Load<Texture2D>(@"Maps\History\MikrovsHill\foregroundFog");
            elevator = content.Load<Texture2D>(@"Maps\Music\The Grand Canal\elevator");

            Game1.npcFaces["French Soldier"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\French Soldier Normal");
            game.NPCSprites["Mikrov"] = content.Load<Texture2D>(@"NPC\History\Mikrov");
            Game1.npcFaces["Mikrov"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Mikrov Normal");
            injuredSoldierSprite = content.Load<Texture2D>(@"InteractiveObjects\injuredFrenchSoldier");

            inj1.Sprite = injuredSoldierSprite;
            inj2.Sprite = injuredSoldierSprite;
            inj3.Sprite = injuredSoldierSprite;

        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
            game.NPCSprites["Mikrov"] = Game1.whiteFilter;
            Game1.npcFaces["Mikrov"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["French Soldier"].faces["Normal"] = Game1.whiteFilter;

        }

        public override void Update()
        {
            base.Update();

            //--If there aren't max enemies on the screen, spawn more
            if (enemiesInMap.Count < enemyAmount)
            {
                if (spawnEnemies)
                    RespawnGroundEnemies();
            }
            else
                spawnEnemies = false;

            if (game.ChapterTwo.ChapterTwoBooleans["bridgeUsable"] && targets2.Count == 0)
            {
                targets2.Add(new Vector2(337, mapRec.Y + 1230));
                targets2.Add(new Vector2(337, mapRec.Y + 3097));
            }

        }

        public override void SetPortals()
        {
            base.SetPortals();
            toGoblinMedicalCamp = new Portal(2330, platforms[0], "Mikrov's Hill");
            toTrenchfootField = new Portal(2300, platforms[0], "Mikrov's Hill");
            toGoblinMedicalCamp.PortalRecY = -2900;
            toTrenchfootField.PortalRecY = 510;

        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toGoblinMedicalCamp, GoblinMedicalCamp.ToMikrovsHill);
            portals.Add(toTrenchfootField, TrenchfootField.ToMikrovsHill);

        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(elevator, new Vector2(movingPlat2.RecX - 55, movingPlat2.RecY - 20), Color.White);

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

            if (player.PositionY < -780 && player.PositionX > 800 && player.PositionX < 1600 && player.PositionY > -2100)
            {

                if (foreAlpha > 0)
                    foreAlpha -= .05f;
            }
            else
            {
                if (foreAlpha < 1f)
                    foreAlpha += .05f;
            }

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White * foreAlpha);
            s.Draw(foregroundFog, new Vector2(0, mapRec.Y - foregroundFog.Height), Color.White);

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
null, null, null, null, Game1.camera.GetTransform(.65f, this, game));
            s.Draw(parallax, new Vector2(0, mapRec.Y - parallax.Height), Color.White);
            s.End();
        }
    }
}

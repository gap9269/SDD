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
    class CollapsingRoom : MapClass
    {
        static Portal toCentralHallIII;
        static Portal toForgottenChamberI;
        static Portal toFalseRoom;

        public static Portal ToFalseRoom { get { return toFalseRoom; } }
        public static Portal ToForgottenChamberI { get { return toForgottenChamberI; } }
        public static Portal ToCentralHallIII { get { return toCentralHallIII; } }

        Texture2D foreground, topPlat, crackedTile, tile, farBackground;

        DisappearingPlatform collapsingPlat;

        public CollapsingRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1500;
            mapWidth = 1800;
            mapName = "Collapsing Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 4;
            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            collapsingPlat = new DisappearingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(892,570, 300, 50), false, false, false, 20, 0);
            platforms.Add(collapsingPlat);

            enemyNamesAndNumberInMap.Add("Scorpadillo", 0);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\CollapsingRoom\middle"));
            background.Add(Game1.whiteFilter);
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\CollapsingRoom\foreground");
            topPlat = content.Load<Texture2D>(@"Maps\History\Pyramid\CollapsingRoom\topPlatBroken");
            crackedTile = content.Load<Texture2D>(@"Maps\History\Pyramid\CollapsingRoom\crackedTile");
            tile = content.Load<Texture2D>(@"Maps\History\Pyramid\CollapsingRoom\tile");
            farBackground = content.Load<Texture2D>(@"Maps\History\Pyramid\CollapsingRoom\background");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.ScorpadilloEnemy(content);
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            if (enemyNamesAndNumberInMap["Scorpadillo"] < enemyAmount)
            {
                Scorpadillo en = new Scorpadillo(pos, "Scorpadillo", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
                en.Position = new Vector2(monsterX, monsterY);

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

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
            spawnEnemies = true;
        }

        public override void Update()
        {
            base.Update();

            if (enemiesInMap.Count < enemyAmount && spawnEnemies)
            {
                RespawnGroundEnemies();

                if (enemiesInMap.Count == enemyAmount)
                    spawnEnemies = false;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCentralHallIII = new Portal(50, platforms[1], "Collapsing Room");
            toFalseRoom = new Portal(1500, platforms[1], "Collapsing Room");
            toForgottenChamberI = new Portal(1650, platforms[0], "Collapsing Room");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(topPlat, new Vector2(0, 557), Color.White);

            if(collapsingPlat.GetPercentHealth() > .5f)
                s.Draw(tile, new Vector2(0, 557), Color.White);
            else if (collapsingPlat.Exists)
                s.Draw(crackedTile, new Vector2(0, 557), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentralHallIII, CentralHallIII.ToCollapsingRoom);
            portals.Add(toForgottenChamberI, ForgottenChamberI.ToCollapsingRoom);
            portals.Add(toFalseRoom, FalseRoom.ToCollapsingRoom);
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
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(farBackground, new Vector2(0, 0), Color.White);
            s.End();
        }
    }
}

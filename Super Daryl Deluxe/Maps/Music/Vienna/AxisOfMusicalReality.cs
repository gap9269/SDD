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
    class AxisOfMusicalReality : MapClass
    {
        static Portal toRestrictedHallway;
        static Portal toAxisOfArt;
        static Portal toManagersOffice;

        public static Portal ToManagersOffice { get { return toManagersOffice; } }
        public static Portal ToRestrictedHallway { get { return toRestrictedHallway; } }
        public static Portal ToAxisOfArt { get { return toAxisOfArt; } }

        int bookshelfPos;
        Texture2D bookshelf, foreground;

        Textbook text;

        public AxisOfMusicalReality(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Axis of Musical Reality";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 1;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            text = new Textbook(1005, 571, 0);
            text.AbleToPickUp = false;
            collectibles.Add(text);


            //TODO make these musical notes or something floating in the air
            Barrel bar = new Barrel(game, 286, 710, Game1.interactiveObjects["Barrel"], true, 3, 10, 0, true, Barrel.BarrelType.WoodenLeft);
            interactiveObjects.Add(bar);

            Barrel bar2 = new Barrel(game, 1788, 710, Game1.interactiveObjects["Barrel"], true, 3, 6, 0, true, Barrel.BarrelType.MetalRadioactive);
            interactiveObjects.Add(bar2);

            Barrel bar1 = new Barrel(game, 1538, 700, Game1.interactiveObjects["Barrel"], true, 3, 4, 0, true, Barrel.BarrelType.WoodenRight);
            interactiveObjects.Add(bar1);

            Barrel bar3 = new Barrel(game, 697, 695, Game1.interactiveObjects["Barrel"], true, 3, 5, 0, true, Barrel.BarrelType.MetalLabel);
            interactiveObjects.Add(bar3);
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            LordXylophone en = new LordXylophone(pos, "Lord Glockenspiel", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            en.TimeBeforeSpawn = 5;
            en.Hostile = true;
            en.UpdateRectangles();

            Rectangle testRec = new Rectangle(en.VitalRecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);
            if (testRec.Intersects(player.VitalRec))
            {
            }
            else
            {
                en.UpdateRectangles();
                AddEnemyToEnemyList(en);
            }
        }


        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.XylophoneEnemy(content);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Axis of Musical Reality\background"));
            bookshelf = content.Load<Texture2D>(@"Maps\Music\Axis of Musical Reality\bookshelf");
            foreground = content.Load<Texture2D>(@"Maps\Music\Axis of Musical Reality\foreground");

            game.NPCSprites["Theater Manager"] = content.Load<Texture2D>(@"NPC\Music\Theater Manager");
            Game1.npcFaces["Theater Manager"].faces["Sneer"] = content.Load<Texture2D>(@"NPCFaces\Music\TheaterManagerSneer");
            Game1.npcFaces["Theater Manager"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Music\TheaterManagerNormal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Theater Manager"] = Game1.whiteFilter;
            Game1.npcFaces["Theater Manager"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Theater Manager"].faces["Sneer"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            if (game.ChapterOne.ChapterOneBooleans["xylophoneDestroyed"] && toManagersOffice.IsUseable == false)
            {
                toManagersOffice.IsUseable = true;
                text.AbleToPickUp = true;
            }
            //if (enemiesInMap.Count < enemyAmount)
            //    RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toRestrictedHallway = new Portal(50, platforms[0], "Axis of Musical Reality");
            toAxisOfArt = new Portal(2200, platforms[0], "Axis of Musical Reality");
            toManagersOffice = new Portal(960, platforms[0], "Axis of Musical Reality");
            toManagersOffice.FButtonYOffset = -30;
            toManagersOffice.PortalNameYOffset = -30;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toRestrictedHallway, RestrictedHallway.ToAxisOfMusic);
            portals.Add(toAxisOfArt, AxisOfArtisticReality.ToAxisOfMusic);
            portals.Add(toManagersOffice, ManagersOffice.ToAxisOfMusic);

            toManagersOffice.IsUseable = false;
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!game.ChapterOne.ChapterOneBooleans["xylophoneDestroyed"])
                s.Draw(bookshelf, new Vector2(0, bookshelfPos), Color.White);
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

            s.Draw(foreground, new Vector2(0, 0), Color.White);

            s.End();
        }


        public bool RaiseBookshelf()
        {
            if (bookshelfPos > -176)
                bookshelfPos -= 2;
            else
            {
                bookshelfPos = -176;
                return true;
            }

            game.Camera.ShakeCamera(3, 1);

            return false;
        }

        public bool LowerBookshelf()
        {
            if (bookshelfPos < 0)
                bookshelfPos += 2;
            else
            {
                bookshelfPos = 0;
                return true;
            }

            game.Camera.ShakeCamera(3, 1);

            return false;
        }
    }
}

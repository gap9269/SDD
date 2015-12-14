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
    class ManagersOffice : MapClass
    {
        static Portal toBackstage;
        static Portal toAxisOfMusic;

        public static Portal ToAxisOfMusic { get { return toAxisOfMusic; } }
        public static Portal ToBackstage { get { return toBackstage; } }

        public Texture2D bookshelf;
        int bookshelfPos;

        public ManagersOffice(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Manager's Office";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\Manager's Office\Manager's Office"));
            bookshelf = content.Load<Texture2D>(@"Maps\Music\Manager's Office\bookshelf");

            game.NPCSprites["Theater Manager"] = content.Load<Texture2D>(@"NPC\Music\Theater Manager");
            Game1.npcFaces["Theater Manager"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Music\TheaterManagerNormal");
            Game1.npcFaces["Theater Manager"].faces["Sneer"] = content.Load<Texture2D>(@"NPCFaces\Music\TheaterManagerSneer");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.MaracasHermanosEnemy(content);
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

            if (enemiesInMap.Count == 0 && toBackstage.IsUseable == false && game.ChapterOne.ChapterOneBooleans["chasingTheManager"])
                toBackstage.IsUseable = true;

            if (game.ChapterOne.ChapterOneBooleans["finishedManagerQuest"] && toAxisOfMusic.IsUseable == false)
                toAxisOfMusic.IsUseable = true;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBackstage = new Portal(50, platforms[0], "Manager's Office");
            toAxisOfMusic = new Portal(805, platforms[0], "Manager's Office");
            toAxisOfMusic.IsUseable = false;
            toAxisOfMusic.FButtonYOffset = -55;
            toAxisOfMusic.PortalNameYOffset = -55;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBackstage, Backstage.ToManagersOffice);
            portals.Add(toAxisOfMusic, AxisOfMusicalReality.ToManagersOffice);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (!game.ChapterOne.ChapterOneBooleans["xylophoneDestroyed"])
                s.Draw(bookshelf, new Vector2(0, bookshelfPos), Color.White);
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

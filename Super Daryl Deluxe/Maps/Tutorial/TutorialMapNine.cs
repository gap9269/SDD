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
    class TutorialMapNine : MapClass
    {
        static Portal toMapEight;
        static Portal toBathroom;
        static Portal toMapTen;

        public static Portal ToMapEight { get { return toMapEight; } }
        public static Portal ToMapTen { get { return toMapTen; } }
        public static Portal ToBathroom { get { return toBathroom; } }

        List<Texture2D> lowBack;
        Texture2D foreground, foregroundLow;

        int toolTipTimer;

        public TutorialMapNine(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2200;
            mapName = "Tutorial Map Nine";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            enemyAmount = 2;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Garden Beast", content.Load<Texture2D>(@"Tutorial\EnemieSheet"));
        }


        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Tutorial\Map9"));
            foreground = content.Load<Texture2D>(@"Maps\Tutorial\Map9Fore");
        }

        public override void RespawnFlyingEnemies(Rectangle mapRec)
        {
            base.RespawnFlyingEnemies(mapRec);

            ForkEnemy en = new ForkEnemy(pos, "Garden Beast", game, ref player, this, mapRec);

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                enemiesInMap.Add(en);
            }

        }
        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            GnomeEnemy en = new GnomeEnemy(pos, "Garden Beast", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 10;
            en.Position = new Vector2(monsterX, monsterY);

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                en.UpdateRectangles();
                enemiesInMap.Add(en);
                Chapter.effectsManager.AddSmokePoof(en.VitalRec, 2);
            }

        }

        public override void Update()
        {
            base.Update();

            //TOOLTIPS FOR SAVING
            if (player.VitalRec.Intersects(toBathroom.PortalRec) && !game.MapBooleans.tutorialMapBooleans["TutorialSaved"])
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][31], 400, 100, game.ChapterTwo.associateOneTex);
            }
            else if (player.PositionX > 1250 && !game.MapBooleans.tutorialMapBooleans["TutorialSaved"])
            {
                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][32], 400, 100, game.ChapterTwo.associateOneTex);
            }


            if (player.PositionX >= 600 && enemiesInMap.Count == 0 && !game.MapBooleans.tutorialMapBooleans["MonsterOneKilled"] && game.MapBooleans.tutorialMapBooleans["TutorialSaved"])
            {
                GnomeEnemy en = new GnomeEnemy(pos, "TutorialEnemy", game, ref player, this);
                monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
                en.Position = new Vector2(1700, monsterY);

                enemiesInMap.Add(en);

                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][33], 400, 100, game.ChapterTwo.associateOneTex);
            }

            if (!game.MapBooleans.tutorialMapBooleans["MonsterOneKilled"] && player.Experience > 0)
            {
                game.MapBooleans.tutorialMapBooleans["MonsterOneKilled"] = true;
            }

            if (!game.MapBooleans.tutorialMapBooleans["MonsterTwoKilled"] && enemiesInMap.Count < 2 && game.MapBooleans.tutorialMapBooleans["MonsterOneKilled"] == true && game.MapBooleans.tutorialMapBooleans["TutorialMonstersSpawned"] == false)
            {
                if(enemiesInMap.Count == 0 || (enemiesInMap.Count == 1 && enemiesInMap[0] is ForkEnemy))
                    RespawnGroundEnemies();

                if (enemiesInMap.Count == 0 || (enemiesInMap.Count == 1 && enemiesInMap[0] is GnomeEnemy))
                    RespawnFlyingEnemies(new Rectangle(1000, 100, 1000, 500));

                if (enemiesInMap.Count == 2)
                {
                    game.MapBooleans.tutorialMapBooleans["TutorialMonstersSpawned"] = true;
                }
            }

            if (!game.MapBooleans.tutorialMapBooleans["MonsterTwoKilled"] && game.MapBooleans.tutorialMapBooleans["TutorialMonstersSpawned"] && enemiesInMap.Count == 0)
            {
                game.MapBooleans.tutorialMapBooleans["MonsterTwoKilled"] = true;

                Chapter.effectsManager.AddToolTipWithImage(game.ChapterTwo.AssociateDialogue[game.ChapterTwo.SelectedAssociate][36], 0, 400, game.ChapterTwo.associateOneTex);

                toMapTen.IsUseable = true;
            }

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, mapRec, Color.White);
            s.End();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toMapEight = new Portal(-10, 630, "TutorialMapNine");
            toMapEight.IsUseable = false;
            toBathroom = new Portal(1100, 620, "TutorialMapNine");
            toMapTen = new Portal(2030, 630, "TutorialMapNine");
            toMapTen.IsUseable = false;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toMapEight, TutorialMapEight.ToMapNine);
            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toMapTen, TutorialMapTen.ToMapNine);
        }
    }
}
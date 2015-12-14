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
    class Egypt : MapClass
    {
        static Portal toCentralSands;
        static Portal toCursedSands;

        public static Portal ToCursedSands { get { return toCursedSands; } }
        public static Portal ToCentralSands { get { return toCentralSands; } }

        Texture2D foreground, foreground2, sky, parallax, sun, parallaxFar;

        public Egypt(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 5000;
            mapName = "Egypt";

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
            background.Add(content.Load<Texture2D>(@"Maps\History\Egypt\background"));
            background.Add(content.Load<Texture2D>(@"Maps\History\Egypt\background2"));
            foreground = content.Load<Texture2D>(@"Maps\History\Egypt\foreground");
            foreground2 = content.Load<Texture2D>(@"Maps\History\Egypt\foreground2");
            sky = content.Load<Texture2D>(@"Maps\History\DryDesert\sky");
            parallax = content.Load<Texture2D>(@"Maps\History\Egypt\parallax");
            parallaxFar = content.Load<Texture2D>(@"Maps\History\Egypt\parallaxFar");
            sun = content.Load<Texture2D>(@"Maps\History\Oasis\sun");
            game.NPCSprites["Chained Pharaoh Guard"] = content.Load<Texture2D>(@"NPC\History\Chained Pharaoh Guard");
            game.NPCSprites["Pharaoh Guard"] = content.Load<Texture2D>(@"NPC\History\Pharaoh Guard");
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\Pharaoh Guard Normal");
            Game1.npcFaces["Pharaoh Guard"].faces["Normal"] = Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"];
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Chained Pharaoh Guard"] = Game1.whiteFilter;
            Game1.npcFaces["Chained Pharaoh Guard"].faces["Normal"] = Game1.whiteFilter;
            game.NPCSprites["Pharaoh Guard"] = Game1.whiteFilter;
            Game1.npcFaces["Pharaoh Guard"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.AnubisWarriorEnemy(content);
            EnemyContentLoader.SharedAnubisSounds(content);

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["pharaohGuardsChained"] && !Game1.g.ChapterTwo.ChapterTwoBooleans["savedPharaohGuards"])
            {
                enemiesInMap.Clear();

                //Spawn initial enemies
                AnubisWarrior ben = new AnubisWarrior(new Vector2(3181, platforms[0].RecY - 396 * 1.1f), "Anubis Warrior", game, ref player, this, new Rectangle(500, 300, mapRec.Width - 600, 500));
                ben.Hostile = false;
                ben.FacingRight = true;
                ben.SpawnWithPoof = false;
                ben.priority = AnubisWarrior.Priority.melee;
                AddEnemyToEnemyList(ben);

                ben.distanceFromFeetToBottomOfRectangleRandomOffset = 10;

                //Spawn initial enemies
                AnubisWarrior ben2 = new AnubisWarrior(new Vector2(2837, platforms[0].RecY - 396 * 1.1f), "Anubis Warrior", game, ref player, this, new Rectangle(500, 300, mapRec.Width - 600, 500));
                ben2.Hostile = false;
                ben2.FacingRight = true;
                ben2.SpawnWithPoof = false;
                ben.priority = AnubisWarrior.Priority.range;
                AddEnemyToEnemyList(ben2);
                ben2.distanceFromFeetToBottomOfRectangleRandomOffset = 0;

            }
        }

        public override void ResetMapAssetsOnEnter()
        {
            base.ResetMapAssetsOnEnter();
        }

        public override void Update()
        {
            base.Update();

            player.Health = 1000;

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["pharaohGuardsChained"] && !Game1.g.ChapterTwo.ChapterTwoBooleans["savedPharaohGuards"])
            {
                if (enemiesInMap.Count == 0)
                {
                    game.ChapterTwo.ChapterTwoBooleans["savedPharaohGuards"] = true;
                }
                else if (player.PositionX < 3675 && enemiesInMap[0].Hostile == false)
                {
                    enemiesInMap[0].Hostile = true;
                    (enemiesInMap[0] as AnubisWarrior).priority = AnubisWarrior.Priority.melee;

                    enemiesInMap[1].Hostile = true;
                    (enemiesInMap[1] as AnubisWarrior).priority = AnubisWarrior.Priority.range;
                }


            }

            if (Game1.g.ChapterTwo.ChapterTwoBooleans["pharaohGuardsChained"] && !Game1.g.ChapterTwo.ChapterTwoBooleans["enteredEgyptDialoguePlayed"])
            {
                Game1.g.ChapterTwo.ChapterTwoBooleans["enteredEgyptDialoguePlayed"] = true;
                Chapter.effectsManager.AddInGameDialogue("Yeah, real tough, bringing the underworld to a fist fight. Stupid cheating dogs.", "Pharaoh Guard", "Normal", 180);
            }

            if (game.ChapterTwo.ChapterTwoBooleans["savedPharaohGuards"])
                toCursedSands.IsUseable = true;
            else
                toCursedSands.IsUseable = false;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCentralSands = new Portal(4800, platforms[0], "Egypt");
            toCursedSands = new Portal(3455, platforms[0], "Egypt");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentralSands, CentralSands.ToEgypt);
            portals.Add(toCursedSands, CursedSands.ToEgypt);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, 0), Color.White);
            s.Draw(foreground2, new Vector2(mapWidth - foreground2.Width, 0), Color.White);

            if(Game1.g.ChapterTwo.ChapterTwoBooleans["pharaohGuardsChained"])
                Game1.DrawFlipHorizontal(s, game.NPCSprites["Chained Pharaoh Guard"], new Vector2(2608, 358));
            else
                Game1.DrawFlipHorizontal(s, game.NPCSprites["Pharaoh Guard"], new Vector2(2608, 358));

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, 0, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(sun, new Vector2(-200, 0), Color.White);
            s.End();


            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.30f, this, game));
            s.Draw(parallaxFar, new Vector2(-860, 0), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.7f, this, game));
            s.Draw(parallax, new Vector2(-600, 0), Color.White);
            s.End();
        }
    }
}

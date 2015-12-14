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
    class BattlefieldOutskirts : MapClass
    {
        static Portal toNoMansValley;
        static Portal toOutsideFort;
        public static Portal toBathroom;

        public static Portal ToOutsideFort { get { return toOutsideFort; } }
        public static Portal ToNoMansValley { get { return toNoMansValley; } }

        Texture2D parallax, sky, soldier, army, trojanHorse, outhouse;

        Barrel bar, bar1, bar2, bar3, bar4, bar5;
        public BattlefieldOutskirts(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1660;
            mapWidth = 3000;
            mapName = "Battlefield Outskirts";

            mapRec = new Rectangle(0, 360 + 720 - mapHeight, mapWidth, 1500);
            enemyAmount = 0;

            yScroll = true;

            zoomLevel = 1f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            bar = new Barrel(game, 559, 459 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, 0f, false, Barrel.BarrelType.WoodenRight);
            bar.IsHidden = true;
            interactiveObjects.Add(bar);

            bar1 = new Barrel(game, 685, 467 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, 0f, false, Barrel.BarrelType.WoodenLeft);
            bar1.IsHidden = true;
            interactiveObjects.Add(bar1);

            bar2 = new Barrel(game, 434, 459 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, 0f, false, Barrel.BarrelType.WoodenRight);
            bar2.IsHidden = true;
            interactiveObjects.Add(bar2);

            bar3 = new Barrel(game, 905, 565 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, 0f, true, Barrel.BarrelType.WoodenLeft);
            bar3.IsHidden = true;
            interactiveObjects.Add(bar3);

            bar4 = new Barrel(game, 784, 565 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, 0f, true, Barrel.BarrelType.MetalLabel);
            bar4.IsHidden = true;
            interactiveObjects.Add(bar4);

            bar5 = new Barrel(game, 1366, 554 + 155, Game1.interactiveObjects["Barrel"], true, 3, 5, 0f, true, Barrel.BarrelType.WoodenLeft);
            bar5.IsHidden = true;
            interactiveObjects.Add(bar5);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Outskirts\background"));
            parallax = content.Load<Texture2D>(@"Maps\History\Outskirts\parallax");
            sky = content.Load<Texture2D>(@"Maps\History\Outskirts\sky");
            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");

            soldier = content.Load<Texture2D>(@"NPC\History\French Soldier");

            if (game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"] && game.chapterState == Game1.ChapterState.chapterTwo)
            {
                trojanHorse = content.Load<Texture2D>(@"Maps\History\TrojanHorseNormal\horse00");
                army = content.Load<Texture2D>(@"Maps\History\OutsideFort\soldiers");


                game.NPCSprites["Napoleon"] = content.Load<Texture2D>(@"NPC\History\Napoleon");
                game.NPCSprites["Cleopatra"] = content.Load<Texture2D>(@"NPC\History\Cleopatra");
                game.NPCSprites["Julius Caesar"] = content.Load<Texture2D>(@"NPC\Party\Julius");
                game.NPCSprites["Genghis"] = content.Load<Texture2D>(@"NPC\History\Genghis");
                Game1.npcFaces["Genghis"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\GenghisNormal");
                Game1.npcFaces["Julius Caesar"].faces["Helmet"] = content.Load<Texture2D>(@"NPCFaces\Party\JuliusHelmet");

                Game1.npcFaces["Napoleon"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\NapoleonNormal");
                Game1.npcFaces["Cleopatra"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\CleopatraNormal");
            }

            game.NPCSprites["Private Brian"] = content.Load<Texture2D>(@"NPC\History\French Soldier");
            Game1.npcFaces["French Soldier"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\French Soldier Normal");
            game.NPCSprites["French Soldier"] = game.NPCSprites["Private Brian"];
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Private Brian"] = Game1.whiteFilter;
            Game1.npcFaces["Private Brian"].faces["Normal"] = Game1.whiteFilter;
            game.NPCSprites["French Soldier"] = Game1.whiteFilter;
            Game1.npcFaces["French Soldier"].faces["Normal"] = Game1.whiteFilter;

            if (game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"] && game.chapterState == Game1.ChapterState.chapterTwo)
            {
                game.NPCSprites["Napoleon"] = Game1.whiteFilter;
                game.NPCSprites["Cleopatra"] = Game1.whiteFilter;
                game.NPCSprites["Julius Caesar"] = Game1.whiteFilter;
                game.NPCSprites["Genghis"] = Game1.whiteFilter;

                Game1.npcFaces["Genghis"].faces["Normal"] = Game1.whiteFilter;
                Game1.npcFaces["Julius Caesar"].faces["Helmet"] = Game1.whiteFilter;
                Game1.npcFaces["Napoleon"].faces["Normal"] = Game1.whiteFilter;
                Game1.npcFaces["Cleopatra"].faces["Normal"] = Game1.whiteFilter;
            }
        }

        public override void Update()
        {
            base.Update();

            if (game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"] && toBathroom.IsUseable == false)
                toBathroom.IsUseable = true;

            else if(!game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"] && toBathroom.IsUseable)
                toBathroom.IsUseable = false;

            if (bar.IsHidden && game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"])
            {
                bar.IsHidden = false;
                bar1.IsHidden = false;
                bar2.IsHidden = false;
                bar3.IsHidden = false;
                bar4.IsHidden = false;
                bar5.IsHidden = false;
            }
            else if (game.ChapterTwo.ChapterTwoBooleans["suppliesDelivered"])
            {
                if(bar.Finished && bar1.Finished && bar2.Finished && bar3.Finished && bar4.Finished && bar5.Finished)
                {
                    game.ChapterTwo.NPCs["Private Brian"].ClearDialogue();
                    game.ChapterTwo.NPCs["Private Brian"].Dialogue.Add("Wh-why did you do that? You just delivered those supplies! Now we're doomed!");
                    game.ChapterTwo.NPCs["Private Brian"].Dialogue.Add("We're doomed!");
                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toNoMansValley = new Portal(0, platforms[0], "Battlefield Outskirts");
            toOutsideFort = new Portal(2800, platforms[0], "Battlefield Outskirts");
            toBathroom = new Portal(250, platforms[0], "Battlefield Outskirts");
            toBathroom.IsUseable = false;
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"] && game.chapterState == Game1.ChapterState.chapterTwo)
            {
                s.Draw(army, new Vector2(1411, mapRec.Y - 221), Color.White);
                s.Draw(army, new Vector2(1294, mapRec.Y - 203), Color.White);

                if(!game.ChapterTwo.ChapterTwoBooleans["bombArriveScenePlayed"])
                    s.Draw(trojanHorse, new Vector2(769, mapRec.Y + 441), Color.White);
                else
                    s.Draw(trojanHorse, new Vector2(2013, mapRec.Y + 447), Color.White);

            }
            if (game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"])
                s.Draw(outhouse, new Rectangle(toBathroom.PortalRecX - 110, 350, outhouse.Width, outhouse.Height), Color.White);

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toNoMansValley, NoMansValley.ToOutskirts);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (game.ChapterTwo.ChapterTwoBooleans["soldiersLeavingValleyPlayed"] && !game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"])
            {
                s.Draw(soldier, new Vector2(859, 332), Color.White);
                s.Draw(soldier, new Rectangle(1035, 347, soldier.Width, soldier.Height), Color.White);
            }
            for (int i = 0; i < interactiveObjects.Count; i++)
            {
                if (interactiveObjects[i].Foreground)
                {
                    interactiveObjects[i].Draw(s);
                }
            }

            if (game.ChapterTwo.ChapterTwoBooleans["movedToOutskirts"] && game.chapterState == Game1.ChapterState.chapterTwo)
            {
                s.Draw(army, new Vector2(1101, mapRec.Y + 6), Color.White);
                s.Draw(army, new Vector2(1660, mapRec.Y + 70), Color.White);

            }

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(1f, this, game));
            s.Draw(sky, new Rectangle(0, mapRec.Y, mapWidth, mapHeight), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.14f, this, game));
            s.Draw(parallax, new Rectangle(-20, mapRec.Y + 400, parallax.Width, parallax.Height), Color.White);
            s.End();
        }
    }
}

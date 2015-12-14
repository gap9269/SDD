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
    class BurialChamber : MapClass
    {
        static Portal toHallOfTrials;
        public static Portal ToHallOfTrials { get { return toHallOfTrials; } }

        static Portal toPrisonChamber;
        public static Portal ToPrisonChamber { get { return toPrisonChamber; } }
        Texture2D foreground, jar1, jar2, jar3, openCoffin;

        Rectangle heartJarSpot, brainJarSpot, liverJarSpot;

        PyramidKey key;

        public BurialChamber(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2300;
            mapName = "Burial Chamber";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            heartJarSpot = new Rectangle(770, 350, 110, 135);
            brainJarSpot = new Rectangle(1240, 350, 110, 135);
            liverJarSpot = new Rectangle(1410, 350, 110, 135);

            key = new PyramidKey(1100, 200);
            key.AbleToPickUp = false;
            collectibles.Add(key);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\BurialChamber\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\BurialChamber\foreground");
            jar1 = content.Load<Texture2D>(@"Maps\History\Pyramid\BurialChamber\jar1");
            jar2 = content.Load<Texture2D>(@"Maps\History\Pyramid\BurialChamber\jar2");
            jar3 = content.Load<Texture2D>(@"Maps\History\Pyramid\BurialChamber\jar3");
            openCoffin = content.Load<Texture2D>(@"Maps\History\Pyramid\BurialChamber\backgroundOpenCoffin");

            game.NPCSprites["King Hasbended"] = content.Load<Texture2D>(@"NPC\History\King Hasbended");
            Game1.npcFaces["King Hasbended"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\History\King Hasbended Normal");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["King Hasbended"] = Game1.whiteFilter;
            Game1.npcFaces["King Hasbended"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            #region Heart Jar
            Rectangle heartFRec = new Rectangle(heartJarSpot.Center.X - 24, heartJarSpot.Center.Y - 80, 43, 65);

            if (Game1.Player.StoryItems.ContainsKey("Jarred Heart"))
            {
                if (Math.Abs(Game1.Player.VitalRec.Center.X - heartJarSpot.Center.X) < 50)
                {
                    if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(heartFRec))
                        Chapter.effectsManager.AddForeroundFButton(heartFRec);

                    if ((game.current.IsKeyUp(Keys.F) && game.last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                    {
                        if (Chapter.effectsManager.foregroundFButtonRecs.Contains(heartFRec))
                            Chapter.effectsManager.foregroundFButtonRecs.Remove(heartFRec);

                        player.RemoveStoryItem("Jarred Heart", 1);
                        game.ChapterTwo.ChapterTwoBooleans["jarOnePlaced"] = true;
                        game.Camera.ShakeCamera(10, 10);
                    }

                }
                else
                {
                    if (Chapter.effectsManager.foregroundFButtonRecs.Contains(heartFRec))
                        Chapter.effectsManager.foregroundFButtonRecs.Remove(heartFRec);
                }
            }
            #endregion

            #region Brain Jar
            Rectangle brainFRec = new Rectangle(brainJarSpot.Center.X - 24, brainJarSpot.Center.Y - 80, 43, 65);

            if (Game1.Player.StoryItems.ContainsKey("Jarred Brain"))
            {
                if (Math.Abs(Game1.Player.VitalRec.Center.X - brainJarSpot.Center.X) < 50)
                {
                    if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(brainFRec))
                        Chapter.effectsManager.AddForeroundFButton(brainFRec);

                    if ((game.current.IsKeyUp(Keys.F) && game.last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                    {
                        if (Chapter.effectsManager.foregroundFButtonRecs.Contains(brainFRec))
                            Chapter.effectsManager.foregroundFButtonRecs.Remove(brainFRec);

                        player.RemoveStoryItem("Jarred Brain", 1);
                        game.ChapterTwo.ChapterTwoBooleans["jarTwoPlaced"] = true;
                        game.Camera.ShakeCamera(10, 10);

                    }

                }
                else
                {
                    if (Chapter.effectsManager.foregroundFButtonRecs.Contains(brainFRec))
                        Chapter.effectsManager.foregroundFButtonRecs.Remove(brainFRec);
                }
            }
            #endregion

            #region Liver Jar
            Rectangle liverFRec = new Rectangle(liverJarSpot.Center.X - 24, liverJarSpot.Center.Y - 80, 43, 65);

            if (Game1.Player.StoryItems.ContainsKey("Jarred Liver"))
            {
                if (Math.Abs(Game1.Player.VitalRec.Center.X - liverJarSpot.Center.X) < 50)
                {
                    if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(liverFRec))
                        Chapter.effectsManager.AddForeroundFButton(liverFRec);

                    if ((game.current.IsKeyUp(Keys.F) && game.last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                    {
                        if (Chapter.effectsManager.foregroundFButtonRecs.Contains(liverFRec))
                            Chapter.effectsManager.foregroundFButtonRecs.Remove(liverFRec);

                        player.RemoveStoryItem("Jarred Liver", 1);
                        game.ChapterTwo.ChapterTwoBooleans["jarThreePlaced"] = true;
                        game.Camera.ShakeCamera(10, 10);

                    }

                }
                else
                {
                    if (Chapter.effectsManager.foregroundFButtonRecs.Contains(liverFRec))
                        Chapter.effectsManager.foregroundFButtonRecs.Remove(liverFRec);
                }
            }
            #endregion

            if (game.ChapterTwo.ChapterTwoBooleans["jarOnePlaced"] && game.ChapterTwo.ChapterTwoBooleans["jarTwoPlaced"] && game.ChapterTwo.ChapterTwoBooleans["jarThreePlaced"] && !game.ChapterTwo.ChapterTwoBooleans["mummySummoned"])
            {
                game.ChapterTwo.ChapterTwoBooleans["mummySummoned"] = true;
            }

            if (!key.AbleToPickUp && game.ChapterTwo.ChapterTwoBooleans["mummySummoned"])
                key.AbleToPickUp = true;

        }

        public override void SetPortals()
        {
            base.SetPortals();
            toHallOfTrials = new Portal(2100, platforms[0], "Burial Chamber");
            toPrisonChamber = new Portal(50, platforms[0], "Burial Chamber");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.ChapterTwo.ChapterTwoBooleans["mummySummoned"] && background[0] != openCoffin)
                background[0] = openCoffin;

            if (game.ChapterTwo.ChapterTwoBooleans["jarOnePlaced"])
                s.Draw(jar1, new Vector2(748, 51), Color.White);
            if (game.ChapterTwo.ChapterTwoBooleans["jarTwoPlaced"])
                s.Draw(jar2, new Vector2(1227, 51), Color.White);
            if (game.ChapterTwo.ChapterTwoBooleans["jarThreePlaced"])
                s.Draw(jar3, new Vector2(1385, 51), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toHallOfTrials, HallofTrials.ToBurialChamber);
            portals.Add(toPrisonChamber, PrisonChamber.ToBurialChamber);
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
        }
    }
}

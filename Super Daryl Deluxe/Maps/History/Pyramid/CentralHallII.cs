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
    class CentralHallII : MapClass
    {
        static Portal toCentralHallI;
        static Portal toCentralHallIII;
        public static Portal ToCentralHallIII { get { return toCentralHallIII; } }
        public static Portal ToCentralHallI { get { return toCentralHallI; } }

        Texture2D foreground;

        public CentralHallII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Central Hall II";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\CentralHallII\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\CentralHallII\foreground");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();

            Rectangle fRec = new Rectangle(1000, 500, 43, 65);

            if (Game1.Player.StoryItems.ContainsKey("Empty Bottle"))
            {
                if (Math.Abs(Game1.Player.VitalRec.Center.X - 1000) < 200)
                {

                    if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                        Chapter.effectsManager.AddForeroundFButton(fRec);

                    if ((game.current.IsKeyUp(Keys.F) && game.last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                    {
                        if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                            Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);

                        player.RemoveStoryItem("Empty Bottle", 1);
                        player.AddStoryItem("Pyramid Water", "some Pyramid Water", 1);
                    }

                }
                else
                {
                    if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                        Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);
                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toCentralHallI = new Portal(200, platforms[0], "Central Hall II");
            toCentralHallIII = new Portal(1620, platforms[0], "Central Hall II");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCentralHallI, CentralHallI.ToCentralHallII);
            portals.Add(toCentralHallIII, CentralHallIII.ToCentralHallII);
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

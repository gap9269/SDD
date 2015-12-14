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
    class UnderTheMansion : MapClass
    {
        public static Portal toTheFoyer;
        public static Portal toAbandonedSafeRoom;

        Texture2D battery, emptyBattery, bulb;

        int lightTime;
        int flickAmount;
        int maxFlick;
        static Random lightRandom;
        Boolean lightOn;

        public UnderTheMansion(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Under the Mansion";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            lightRandom = new Random();
            maxFlick = lightRandom.Next(2, 8);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\UnderTheMansion\background"));
            emptyBattery = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\UnderTheMansion\empty battery");
            battery = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\UnderTheMansion\battery");
            bulb = content.Load<Texture2D>(@"Maps\Literature\ChristmasCarol\UnderTheMansion\bulb");
        }

        public override void Update()
        {
            base.Update();

            Rectangle fRec = new Rectangle(1184, 406, 43, 65);

            if (Game1.Player.StoryItems.ContainsKey("Old Battery"))
            {
                if (Math.Abs(Game1.Player.VitalRec.Center.X - 1184) < 200)
                {

                    if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                        Chapter.effectsManager.AddForeroundFButton(fRec);

                    if ((game.current.IsKeyUp(Keys.F) && game.last.IsKeyDown(Keys.F)) || MyGamePad.LeftBumperPressed())
                    {
                        if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                            Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);

                        player.RemoveStoryItem("Old Battery", 1);
                        game.ChapterTwo.ChapterTwoBooleans["batteryPlaced"] = true;
                    }

                }
                else
                {
                    if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                        Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);
                }
            }

            lightTime--;

            if (lightTime <= 0)
            {
                lightOn = !lightOn;
                lightTime = lightRandom.Next(2, 5);
                flickAmount++;

                if (flickAmount == maxFlick)
                {
                    int onOff = lightRandom.Next(2);

                    if (onOff == 0)
                        lightOn = true;
                    else
                        lightOn = false;

                    flickAmount = 0;
                    maxFlick = lightRandom.Next(2, 8);
                    lightTime = lightRandom.Next(60, 300);
                }
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toTheFoyer = new Portal(50, platforms[0], "Under the Mansion");
            toAbandonedSafeRoom = new Portal(1650, platforms[0], "Under the Mansion");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);


            if (game.ChapterTwo.ChapterTwoBooleans["batteryPlaced"])
            {
                s.Draw(battery, new Vector2(1009, 62), Color.White);
                s.Draw(bulb, new Vector2(211, -5), Color.White);
            }
            else
                s.Draw(emptyBattery, new Vector2(1009, 62), Color.White);

            if(lightOn)
                s.Draw(bulb, new Vector2(0,0), Color.White);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toTheFoyer, TheGrandCorridor.toUnderTheFoyer);
            portals.Add(toAbandonedSafeRoom, AbandonedSafeRoom.toUnderTheFoyer);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
        }
    }
}

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
    class OldShed : MapClass
    {
        static Portal toChelseasPool;
        static Portal toChelseasPoolTop;

        public static Portal ToChelseasPoolTop { get { return toChelseasPoolTop; } }

        public static Portal ToChelseasPool { get { return toChelseasPool; } }

        Texture2D doorFront, windowFront, deadGuySaw, deadGuyNoSaw, fore, shadow, light;

        float doorAlpha = 0f;
        float windowAlpha = 0f;

        Rectangle skeletonRec;

        Platform leftBound, rightBound;

        int lightTime;
        int flickAmount;
        int maxFlick;
        Boolean lightOn = false;
        static Random lightRandom;

        Sparkles sparkles;

        public OldShed(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1300;
            mapName = "Old Shed";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            /*
            Beer beer = new Beer(544, 537);
            beer.ShowFButton = false;
            storyItems.Add(beer);*/
            Textbook text = new Textbook(320, 220, 3);

            collectibles.Add(text);

            Barrel bar1 = new Barrel(game, 105, 646 + 55, Game1.interactiveObjects["Barrel"], true, 1, 0, .09f, true, Barrel.BarrelType.MetalRadioactive);
            interactiveObjects.Add(bar1);

            Barrel bar2 = new Barrel(game, 1041, 237 + 55, Game1.interactiveObjects["Barrel"], true, 1, 0, 2.34f, true, Barrel.BarrelType.MetalLabel);
            interactiveObjects.Add(bar2);

            Barrel bar3 = new Barrel(game, 551, 197 + 55, Game1.interactiveObjects["Barrel"], true, 1, 0, 1.47f, false, Barrel.BarrelType.WoodenRight);
            interactiveObjects.Add(bar3);

            Barrel bar4 = new Barrel(game, 402, 547 + 55, Game1.interactiveObjects["Barrel"], true, 1, 0, .36f, false, Barrel.BarrelType.WoodenLeft);
            interactiveObjects.Add(bar4);

            Barrel bar5 = new Barrel(game, 978, 662 + 55, Game1.interactiveObjects["Barrel"], true, 1, 0, .58f, true, Barrel.BarrelType.WoodenRight);
            interactiveObjects.Add(bar5);

            skeletonRec = new Rectangle(954, 455, 188, 178);

            lightRandom = new Random();
            maxFlick = lightRandom.Next(2, 8);

            sparkles = new Sparkles(1080, skeletonRec.Y);
        }

        public override void PlayBackgroundMusic()
        {
            //Sound.PlayBackGroundMusic("Exploring");
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\OldShed"));
            doorFront = content.Load<Texture2D>(@"Maps\Chelseas\OldShedDoor");
            windowFront = content.Load<Texture2D>(@"Maps\Chelseas\OldShedWindow");
            deadGuyNoSaw = content.Load<Texture2D>(@"Maps\Chelseas\SkeletonNoSaw");
            deadGuySaw = content.Load<Texture2D>(@"Maps\Chelseas\SkeletonSaw");
            fore = content.Load<Texture2D>(@"Maps\Chelseas\ShedFore");
            shadow = content.Load<Texture2D>(@"Maps\Chelseas\ShedShadow");
            light = content.Load<Texture2D>(@"Maps\Chelseas\OldShedLight");


            ////If the last map does not have the same music
            //if (Chapter.lastMap != "Chelsea's Pool")
            //{
            //    SoundEffect bg1 = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Hidden Agenda");
            //    SoundEffectInstance backgroundMusic1 = bg1.CreateInstance();
            //    backgroundMusic1.IsLooped = true;
            //    Sound.music.Add("Exploring", backgroundMusic1);
            //}

            //Sound.backgroundVolume = 1f;
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            //if (Chapter.theNextMap != "Chelsea's Pool")
            //{
            //    Sound.UnloadBackgroundMusic();
            //}
        }

        public override void Update()
        {
            base.Update();

            PlayBackgroundMusic();

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


                if (game.MapBooleans.chapterTwoMapBooleans["FoundHandsaw"] == false)
                    sparkles.Update();

            if (player.VitalRec.Intersects(skeletonRec) && last.IsKeyDown(Keys.Space) && current.IsKeyUp(Keys.Space) && game.MapBooleans.chapterTwoMapBooleans["FoundHandsaw"] == false)
            {
                player.AddWeaponToInventory(new HandSaw());
                game.MapBooleans.chapterTwoMapBooleans["FoundHandsaw"] = true;

                Chapter.effectsManager.AddFoundItem("a Hand Saw", Game1.equipmentTextures["Hand Saw"]);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toChelseasPool = new Portal(230, platforms[0].Rec.Y + 40, "Old Shed");
            toChelseasPoolTop = new Portal(900, 223, "Old Shed");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toChelseasPool, ChelseasPool.ToOldShed);
            portals.Add(toChelseasPoolTop, ChelseasPool.ToOldShedTop);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if (game.MapBooleans.chapterTwoMapBooleans["FoundHandsaw"])
            {
                s.Draw(deadGuyNoSaw, new Rectangle(954, 455, 188, 178), Color.White);
            }
            else
            {
                s.Draw(deadGuySaw, new Rectangle(954, 455, 188, 178), Color.White);
                sparkles.Draw(s);
            }

            if (lightOn)
                s.Draw(light, new Rectangle(451, 271, 407, 223), Color.White);
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
            s.End();
        

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
        null, null, null, null, Game1.camera.GetTransform(1f, this, game));

            s.Draw(fore, new Rectangle(1072, 548, 161, 175), Color.White * .9f);

            if (player.Rec.Y > 50)
            {
                s.Draw(shadow, mapRec, Color.White);
            }

            //Front door
            if (player.VitalRec.X < 500 && player.VitalRecY > 250)
            {
                if (doorAlpha < .7f)
                    doorAlpha += .05f;
            }
            else
            {
                if (doorAlpha > 0)
                    doorAlpha -= .05f;
            }

            //window
            if (player.VitalRec.X > 600 && player.VitalRecY < 100)
            {
                if (windowAlpha < .7f)
                    windowAlpha += .05f;
            }
            else
            {
                if (windowAlpha > 0)
                    windowAlpha -= .05f;
            }

            s.Draw(doorFront, new Rectangle(243, 431, 160, 289), Color.White * doorAlpha);
            s.Draw(windowFront, new Rectangle(730, 59, 353, 287), Color.White * windowAlpha);

            s.End();


        }
    }
}

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
using System.IO;

namespace ISurvived
{
    class EmptyField : MapClass
    {
        static Portal toCrossroads;

        public static Portal ToCrossroads { get { return toCrossroads; } }

        Texture2D blocks;

        ButtonState previous;
        ButtonState buttonCurrent;

        KeyboardState keyLast, keyCurrent;

        List<TripLaser> newLasers;
        List<Vector2> laserPoints;
        List<TripLaser> tripLasers, movingPlatLasers, firstLasers, firstWallLasers, secondLasers, secondWallLasers, thirdLasers, thirdWallLasers;

        int tempTime = 0;

        KidCage kidCage;

        MovingPlatform move1, move2, move3, move4, move5, move6;

        Platform firstStep, secondStep; //First plats
        Platform wallOne, wallTwo; //First two laser-made walls
        Platform wallThree, wallFour; //Walls on the right side, low ceiling area
        Platform wallFive, wallSix; //Walls on the right side, low ceiling area
        Platform wallSeven, wallEight; //Walls on the right side, low ceiling area
        Platform thirdStep, fourthStep, fifthStep, sixthStep; //Second series of steps
        Platform step7, step8, step9, step10; //Third series of steps
        Platform holeCover;

        Boolean movingPlatLaserTripped, lavaActive, firstLasersTripped, firstWallLasersTripped, secondLasersTripped, secondWallLasersTripped, thirdLasersTripped, thirdWallLasersTripped, fourthWallLasersTripped;

        Boolean spawnedFirstHorizontalMonsters = false;
        Boolean spawnedSecondHorizontalMonsters = false;
        Boolean spawnedThirdHorizontalMonsters = false;
        Boolean spawnedFourthHorizontalMonsters = false;

        Rectangle lava;

        Platform secondCover;

        public EmptyField(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 6800;
            mapWidth = 6500;
            mapName = "Empty Field";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            lavaActive = false;
            firstLasersTripped = false;
            firstWallLasersTripped = false;
            secondLasersTripped = false;
            secondWallLasersTripped = false;
            thirdLasersTripped = false;
            fourthWallLasersTripped = false;

            tripLasers = new List<TripLaser>();
            newLasers = new List<TripLaser>();
            laserPoints = new List<Vector2>();

            holeCover = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1191, -5117, 700, 50), true, false, false);

            move1 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2446, -3155, 150, 50), true, false, false, new List<Vector2>() { new Vector2(2666, -3155), new Vector2(2446, -3155) }, 3, 100);
            move2 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2344, -3850, 100, 50), true, false, false, new List<Vector2>() { new Vector2(2666, -3850), new Vector2(2344, -3850) }, 3, 50);
            move3 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2704,-4123, 100, 50), true, false, false, new List<Vector2>(){new Vector2(2350,-4123), new Vector2(2704,-4123)}, 4, 100);
            move4 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2488,-3529, 150, 50), true, false, false, new List<Vector2>(){new Vector2(2700,-3529), new Vector2(2488,-3529)}, 3, 50);
            move5 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2340,-2588, 150, 50), true, false, false, new List<Vector2>(){new Vector2(2700,-2588), new Vector2(2340,-2588)}, 4, 100);
            move6 = new MovingPlatform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2696,-2860, 150, 50), true, false, false, new List<Vector2>() { new Vector2(2340,-2860), new Vector2(2696,-2860) }, 3, 50);

            platforms.Add(move1);
            platforms.Add(move2);
            platforms.Add(move3);
            platforms.Add(move4);
            platforms.Add(move5);
            platforms.Add(move6);

            firstStep = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1885, -605, 300, 50), true, false, false);
            secondStep = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2283,-879, 200, 50), true, false, false);

            platforms.Add(firstStep);
            platforms.Add(secondStep);

            firstLasers = new List<TripLaser>();
            firstLasers.Add(new TripLaser(2198, -603));
            firstLasers.Add(new TripLaser(2277, -662));
            firstLasers.Add(new TripLaser(2371, -694));
            firstLasers.Add(new TripLaser(2490, -699));
            firstLasers.Add(new TripLaser(1878, -544));
            firstLasers.Add(new TripLaser(1982, -590));
            firstLasers.Add(new TripLaser(2210, -967, new List<Vector2>() { new Vector2(2209, -1047), new Vector2(2213, -776) }, 3, 3));
            firstLasers.Add(new TripLaser(2307, -971, 50, 50, true, 0));

            firstWallLasers = new List<TripLaser>();
            firstWallLasers.Add(new TripLaser(2832, -1266));
            firstWallLasers.Add(new TripLaser(2972, -1410));
            firstWallLasers.Add(new TripLaser(3332, -1475, new List<Vector2>() { new Vector2(3180, -1247), new Vector2(3364, -1523) }, 3, 3));
            firstWallLasers.Add(new TripLaser(3375, -1300, new List<Vector2>() { new Vector2(3389, -1287), new Vector2(3183, -1498) }, 3, 3));
            firstWallLasers.Add(new TripLaser(3617, -1522, 40, 40, true, 20));
            firstWallLasers.Add(new TripLaser(3612, -1257, 40, 40, true, 0));
            firstWallLasers.Add(new TripLaser(3711, -1382));
            firstWallLasers.Add(new TripLaser(3909, -1296));
            firstWallLasers.Add(new TripLaser(4106, -1332));

            wallOne = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2573, -1689, 70, 580), false, false, false);
            wallTwo = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4223, -1689, 70, 580), false, false, false);


            secondLasers = new List<TripLaser>();
            secondLasers.Add(new TripLaser(4317, -1765));
            secondLasers.Add(new TripLaser(4508, -1743, 40, 40, true, 0));
            secondLasers.Add(new TripLaser(4464, -2133, 40, 40, true, 0));
            secondLasers.Add(new TripLaser(4582, -2166, 40, 40, true, 0));
            secondLasers.Add(new TripLaser(4685, -2164, 40, 40, true, 0));

            thirdStep = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4560,-1432, 150, 50), true, false, false);
            fourthStep = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4353, -1684, 150, 50), true, false, false);
            fifthStep = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4212, -1868, 150, 50), true, false, false);
            sixthStep = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4542, -2103, 200, 50), true, false, false);

            platforms.Add(thirdStep);
            platforms.Add(fourthStep);
            platforms.Add(fifthStep);
            platforms.Add(sixthStep);

            secondWallLasers = new List<TripLaser>();
            secondWallLasers.Add(new TripLaser(4875, -2378));
            secondWallLasers.Add(new TripLaser(5091, -2397));
            secondWallLasers.Add(new TripLaser(5290, -2368));
            secondWallLasers.Add(new TripLaser(5318, -2467, new List<Vector2>() { new Vector2(4838, -2463), new Vector2(5393, -2468) }, 3, 3));
            secondWallLasers.Add(new TripLaser(5513, -2424));
            secondWallLasers.Add(new TripLaser(5682, -2363));
            secondWallLasers.Add(new TripLaser(5800, -2466));
            secondWallLasers.Add(new TripLaser(5686, -2447, new List<Vector2>() { new Vector2(5392, -2443), new Vector2(5749, -2448)}, 3, 3));

            wallThree = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5949,-2754, 70, 500), false, false, false);
            wallFour = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4744,-2763, 70, 500), false, false, false);


            thirdLasers = new List<TripLaser>();

            thirdLasers.Add(new TripLaser(6290, -2839, 40, 40, true, 0));
            thirdLasers.Add(new TripLaser(6198, -2928, 40, 40, true, 0));
            thirdLasers.Add(new TripLaser(6071, -3247, 30, 30, true, 0));
            thirdLasers.Add(new TripLaser(6274, -3291, 30, 30, true, 0));
            thirdLasers.Add(new TripLaser(6202, -3366, 30, 30, true, 0));
            thirdLasers.Add(new TripLaser(6116, -3410));
            thirdLasers.Add(new TripLaser(6157, -3411));


            step10 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(6062,-3460, 100, 50), true, false, false);
            step9 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(6001,-3158, 500, 50), true, false, false);
            step8 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(6075,-2880, 100, 50), true, false, false);
            step7 = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(6380,-2597, 100, 50), true, false, false);

            platforms.Add(step7);
            platforms.Add(step8);
            platforms.Add(step9);
            platforms.Add(step10);


            thirdWallLasers = new List<TripLaser>();
            thirdWallLasers.Add(new TripLaser(5766, -3828));
            thirdWallLasers.Add(new TripLaser(5588, -3908));
            thirdWallLasers.Add(new TripLaser(5401, -3844));
            thirdWallLasers.Add(new TripLaser(5137, -3924, new List<Vector2>() { new Vector2(4936, -3922), new Vector2(5326, -3926) }, 3, 3));
            thirdWallLasers.Add(new TripLaser(5234, -3827, 40, 40, true, 0));
            thirdWallLasers.Add(new TripLaser(5016, -3832, 40, 40, true, 0));
            thirdWallLasers.Add(new TripLaser(4716, -3833));
            thirdWallLasers.Add(new TripLaser(4574, -3951));
            thirdWallLasers.Add(new TripLaser(4403, -3839));
            thirdWallLasers.Add(new TripLaser(4254, -3839));

            wallFive = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5954, -4193, 70, 500), false, false, false);
            wallSix = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4044, -4239, 70, 500), false, false, false);

            tripLasers.Add(new TripLaser(4166, -2355, 40, 40, true, 0));
            tripLasers.Add(new TripLaser(4169, -2439, 40, 40, true, 1));
            tripLasers.Add(new TripLaser(4168, -2511, 40, 40, true, 2));
            tripLasers.Add(new TripLaser(4170, -2585, 40, 40, true, 3));
            tripLasers.Add(new TripLaser(3993, -2370));
            tripLasers.Add(new TripLaser(3961, -2428));
            tripLasers.Add(new TripLaser(3926, -2476));
            tripLasers.Add(new TripLaser(3872, -2480, 50, 50, true, 0));
            tripLasers.Add(new TripLaser(3803, -2476, 50, 50, true, 1));
            tripLasers.Add(new TripLaser(3626, -2329, 40, 40, true, 2));
            tripLasers.Add(new TripLaser(3626, -2413, 40, 40, true, 3));
            tripLasers.Add(new TripLaser(3624, -2508, 40, 40, true, 4));
            tripLasers.Add(new TripLaser(3623, -2591, 40, 40, true, 5));
            tripLasers.Add(new TripLaser(3475, -2468));
            tripLasers.Add(new TripLaser(3404, -2414));
            tripLasers.Add(new TripLaser(3335, -2355));
            tripLasers.Add(new TripLaser(3173, -2413, 30, 30, true, 0));
            tripLasers.Add(new TripLaser(3175, -2520, 30, 30, true, 1));
            tripLasers.Add(new TripLaser(3177, -2620, 30, 30, true, 2));
            tripLasers.Add(new TripLaser(3172, -2333, 30, 30, true, 3));
            tripLasers.Add(new TripLaser(2968, -2331));
            tripLasers.Add(new TripLaser(2969, -2668));

            wallSeven = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(2897, -2799, 70, 500), false, false, false);
            wallEight = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4186, -2799, 70, 500), false, false, false);

            movingPlatLasers = new List<TripLaser>();
            movingPlatLasers.Add(new TripLaser(2791, -2881));
            movingPlatLasers.Add(new TripLaser(2348, -2894));
            movingPlatLasers.Add(new TripLaser(2682, -2969));
            movingPlatLasers.Add(new TripLaser(2446, -2986));
            movingPlatLasers.Add(new TripLaser(2332, -3349));
            movingPlatLasers.Add(new TripLaser(2514, -3454));
            movingPlatLasers.Add(new TripLaser(2725, -3461));
            movingPlatLasers.Add(new TripLaser(2328, -3835, 50, 50, true, 0));
            movingPlatLasers.Add(new TripLaser(2470, -3844, 50, 50, true, 0));
            movingPlatLasers.Add(new TripLaser(2655, -3833, 50, 50, true, 0));
            movingPlatLasers.Add(new TripLaser(2789, -3821, 50, 50, true, 0));
            movingPlatLasers.Add(new TripLaser(2407, -3841));
            movingPlatLasers.Add(new TripLaser(2735, -3820));
            movingPlatLasers.Add(new TripLaser(2408, -4009, new List<Vector2>() { new Vector2(2880, -4015), new Vector2(2349, -4009), }, 3, 3));
            movingPlatLasers.Add(new TripLaser(2428, -4110));
            movingPlatLasers.Add(new TripLaser(2488, -4110));
            movingPlatLasers.Add(new TripLaser(2554, -4110));
            movingPlatLasers.Add(new TripLaser(2620, -4110));
            movingPlatLasers.Add(new TripLaser(2673, -4105));
            movingPlatLasers.Add(new TripLaser(2735, -4105));
            movingPlatLasers.Add(new TripLaser(2798, -4105));

            secondCover = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1852, -5133, 3800, 50), true, false, false);
            platforms.Add(secondCover);

            lava = new Rectangle(0, 720, mapWidth, 50);

            kidCage = new KidCage(Game1.interactiveObjects["KidCage"], 2300, -5133 - 237, player);

        }

        public void SpawnEnemies(int horizontalAreaNum)
        {
            Goblin en;
            if (horizontalAreaNum == 1)
            {
                monsterX = rand.Next(2800, 3800);
                pos = new Vector2(monsterX, monsterY);
                en = new Goblin(pos, "Field Goblin", game, ref player, this);
                monsterY = -1200 - en.Rec.Height - 1;
            }
            else if (horizontalAreaNum == 2)
            {
                monsterX = rand.Next(4900, 5600);
                pos = new Vector2(monsterX, monsterY);
                en = new Goblin(pos, "Field Goblin", game, ref player, this);
                monsterY = -2303 - en.Rec.Height - 1;
            }
            else if (horizontalAreaNum == 3)
            {
                monsterX = rand.Next(4300, 5600);
                pos = new Vector2(monsterX, monsterY);
                en = new Goblin(pos, "Field Goblin", game, ref player, this);
                monsterY = -3786 - en.Rec.Height - 1;
            }
            else
            {
                monsterX = rand.Next(3100, 3800);
                pos = new Vector2(monsterX, monsterY);
                en = new Goblin(pos, "Field Goblin", game, ref player, this);
                monsterY = -2293 - en.Rec.Height - 1;
            }

            en.Position = new Vector2(monsterX, monsterY);
            en.TimeBeforeSpawn = 20;
            en.Hostile = true;
            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                enemiesInMap.Add(en);
            }

        }

        public override void Update()
        {
            base.Update();


            if (game.CurrentChapter.state != Chapter.GameState.mapEdit)
            {
                #region MAP EDITOR FOR LASERS
                /*
                MouseState mouse = Mouse.GetState();
                keyLast = keyCurrent;

                keyCurrent = Keyboard.GetState();

                previous = buttonCurrent;

                if (mouse.LeftButton == ButtonState.Pressed)
                    buttonCurrent = ButtonState.Pressed;
                else
                    buttonCurrent = ButtonState.Released;

                if (keyCurrent.IsKeyUp(Keys.Z) && keyLast.IsKeyDown(Keys.Z))
                {
                    laserPoints.Add(new Vector2(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y - 360));
                }

                if (keyCurrent.IsKeyUp(Keys.D1) && keyLast.IsKeyDown(Keys.D1))
                {
                    tempTime = 10;
                }
                if (keyCurrent.IsKeyUp(Keys.D2) && keyLast.IsKeyDown(Keys.D2))
                {
                    tempTime = 20;
                }
                if (keyCurrent.IsKeyUp(Keys.D3) && keyLast.IsKeyDown(Keys.D3))
                {
                    tempTime = 30;
                }
                if (keyCurrent.IsKeyUp(Keys.D4) && keyLast.IsKeyDown(Keys.D4))
                {
                    tempTime = 40;
                }
                if (keyCurrent.IsKeyUp(Keys.D5) && keyLast.IsKeyDown(Keys.D5))
                {
                    tempTime = 50;
                }
                if (keyCurrent.IsKeyUp(Keys.D6) && keyLast.IsKeyDown(Keys.D6))
                {
                    tempTime = 60;
                }
                if (keyCurrent.IsKeyUp(Keys.D7) && keyLast.IsKeyDown(Keys.D7))
                {
                    tempTime = 70;
                }
                if (keyCurrent.IsKeyUp(Keys.D8) && keyLast.IsKeyDown(Keys.D8))
                {
                    tempTime = 80;
                }
                if (keyCurrent.IsKeyUp(Keys.D9) && keyLast.IsKeyDown(Keys.D9))
                {
                    tempTime = 90;
                }

                if (keyCurrent.IsKeyUp(Keys.Back) && keyLast.IsKeyDown(Keys.Back))
                {
                    if (tripLasers.Count > 0)
                    {
                        tripLasers.RemoveAt(tripLasers.Count - 1);
                        newLasers.RemoveAt(newLasers.Count - 1);
                    }
                }

                if (buttonCurrent == ButtonState.Released && previous == ButtonState.Pressed)
                {
                    if (laserPoints.Count == 0 && tempTime == 0)
                    {
                        tripLasers.Add(new TripLaser(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y - 360));
                        newLasers.Add(new TripLaser(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y - 360));

                        player.CanJump = true;
                    }
                    else if (tempTime != 0)
                    {
                        tripLasers.Add(new TripLaser(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y - 360, tempTime, tempTime, true, 0));
                        newLasers.Add(new TripLaser(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y - 360, tempTime, tempTime, true, 0));

                        tempTime = 0;
                    }
                    else
                    {
                        List<Vector2> temp = new List<Vector2>();

                        for (int i = 0; i < laserPoints.Count; i++)
                        {
                            temp.Add(laserPoints[i]);
                        }

                        temp.Add(new Vector2(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y - 360));
                        tripLasers.Add(new TripLaser(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y - 360, temp, 3, 3));
                        newLasers.Add(new TripLaser(Cursor.last.CursorRec.X + (int)game.Camera.Center.X - 640, Cursor.last.CursorRec.Y + (int)game.Camera.Center.Y - 360, temp, 3, 3));
                        laserPoints.Clear();
                    }
                }


                if (last.IsKeyDown(Keys.LeftControl) && current.IsKeyUp(Keys.LeftControl))
                {
                    String file = "Maps\\lasers.txt";
                    File.Delete(file);
                    StreamWriter sw = File.AppendText(file);


                    for (int i = 0; i < tripLasers.Count; i++)
                    {
                        if (tripLasers[i].maxTimeActive > 0)
                        {
                            sw.WriteLine("tripLasers.Add(new TripLaser(" + tripLasers[i].rec.X + "," +
                                tripLasers[i].rec.Y + "," + tripLasers[i].maxTimeActive + "," + tripLasers[i].maxTimeInactive + ", true, 0));");
                        }
                        else if (tripLasers[i].speed > 0)
                        {

                            String pathString = "";

                            for (int j = 0; j < tripLasers[i].path.Count; j++)
                            {
                                pathString += "new Vector2(" + tripLasers[i].path[j].X + ", " + tripLasers[i].path[j].Y + "),";
                            }

                            sw.WriteLine("tripLasers.Add(new TripLaser(" + tripLasers[i].rec.X + "," +
    tripLasers[i].rec.Y + ", new List<Vector2>(){" + pathString + "}," + "3,3));");
                        }
                        else
                        {
                            sw.WriteLine("tripLasers.Add(new TripLaser(" + tripLasers[i].rec.X + "," +
tripLasers[i].rec.Y + "));");
                        }
                    }

                    sw.Close();
                }*/
                #endregion
            }


            if (lavaActive == false && player.CurrentPlat != null && player.RecY > -150)
            {
                lavaActive = true;
                game.MapBooleans.chapterTwoMapBooleans["HoleCoverAdded"] = true;

            }

            if (game.MapBooleans.chapterTwoMapBooleans["HoleCovered"] == true && !platforms.Contains(holeCover))
            {
                platforms.Add(holeCover);
            }

            if (lavaActive == true)
            {
                lava.Y -= 1;

                if(lava.Intersects(player.VitalRec))
                {
                    player.TakeDamage(100000);
                }
            }

            if (player.CurrentPlat == secondCover && secondCover.Passable == true)
            {
                secondCover.Passable = false;
                lavaActive = false;
            }

            kidCage.Update();


            //If the cage is gone but the boolean hasn't be activated, activate it
            if (!game.ChapterTwo.ChapterTwoBooleans["kidFourSaved"] && kidCage.Finished)
            {
                game.ChapterTwo.ChapterTwoBooleans["kidFourSaved"] = true;

                Goblin en;
                monsterX = rand.Next(2400, 2402);
                pos = new Vector2(monsterX, monsterY);
                en = new Goblin(pos, "Field Goblin", game, ref player, this);
                monsterY = -5133 - en.Rec.Height - 1;
                en.TimeBeforeSpawn = 0;
                en.Position = new Vector2(monsterX, monsterY);
                en.Hostile = true;
                en.MaxHealth = 1;
                en.Health = 1;
                enemiesInMap.Add(en);

                game.ChapterTwo.NPCs["CrossroadsKid"].RecX = 1815;
                game.ChapterTwo.NPCs["CrossroadsKid"].PositionX = 1815;
                game.ChapterTwo.NPCs["CrossroadsKid"].FacingRight = false;

                game.ChapterTwo.NPCs["CrossroadsKid"].Dialogue.Clear();
                game.ChapterTwo.NPCs["CrossroadsKid"].Dialogue.Add("I think this is the last one!");

                Crossroads.ToPathFour.IsUseable = true;
            }

            #region Fourth Horizontal Area

            for (int i = 0; i < tripLasers.Count; i++)
            {
                tripLasers[i].Update();


                if (tripLasers[i].tripped)
                {
                    fourthWallLasersTripped = true;
                    tripLasers.Clear();
                    break;
                }
                fourthWallLasersTripped = false;
            }

            if (fourthWallLasersTripped && !platforms.Contains(wallSeven) && !spawnedFourthHorizontalMonsters)
            {
                platforms.Add(wallSeven);
                platforms.Add(wallEight);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallSeven.Rec.X + wallSeven.Rec.Width / 2 - 75, wallSeven.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallEight.Rec.X + wallEight.Rec.Width / 2 - 75, wallEight.Rec.Y - 50, 150, 150), 2);
            }

            if (!spawnedFourthHorizontalMonsters && platforms.Contains(wallSeven))
            {
                if (enemiesInMap.Count < 4)
                    SpawnEnemies(4);
                else
                    spawnedFourthHorizontalMonsters = true;
            }

            if (spawnedFourthHorizontalMonsters && enemiesInMap.Count == 0 && platforms.Contains(wallSeven))
            {
                platforms.Remove(wallSeven);
                platforms.Remove(wallEight);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallSeven.Rec.X + wallSeven.Rec.Width / 2 - 75, wallSeven.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallEight.Rec.X + wallEight.Rec.Width / 2 - 75, wallEight.Rec.Y - 50, 150, 150), 2);
            }
            #endregion

            #region First Horizontal Area
            if (player.PositionY > -2210 && player.PositionX < 4800 && player.PositionY < -950 && player.PositionX > 1500)
            {
                for (int i = 0; i < firstWallLasers.Count; i++)
                {
                    firstWallLasers[i].Update();


                    if (firstWallLasers[i].tripped)
                    {
                        firstWallLasersTripped = true;
                        firstWallLasers.Clear();
                        break;
                    }
                    firstWallLasersTripped = false;
                }
            }

            if (firstWallLasersTripped && !platforms.Contains(wallOne) && !spawnedFirstHorizontalMonsters)
            {
                platforms.Add(wallOne);
                platforms.Add(wallTwo);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallOne.Rec.X + wallOne.Rec.Width / 2 - 75, wallOne.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallTwo.Rec.X + wallTwo.Rec.Width / 2 - 75, wallTwo.Rec.Y - 50, 150, 150), 2);
            }
            if(!spawnedFirstHorizontalMonsters && platforms.Contains(wallOne))
            {
                if (enemiesInMap.Count < 3)
                    SpawnEnemies(1);
                else
                    spawnedFirstHorizontalMonsters = true;
            }

            if (spawnedFirstHorizontalMonsters && enemiesInMap.Count == 0 && platforms.Contains(wallOne))
            {
                platforms.Remove(wallOne);
                platforms.Remove(wallTwo);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallOne.Rec.X + wallOne.Rec.Width / 2 - 75, wallOne.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallTwo.Rec.X + wallTwo.Rec.Width / 2 - 75, wallTwo.Rec.Y - 50, 150, 150), 2);
            }
            #endregion

            #region Second Horizontal Area
            if (Player.PositionY > -3500 && player.PositionY < -2026 && player.PositionX > 3500)
            {
                for (int i = 0; i < secondWallLasers.Count; i++)
                {
                    secondWallLasers[i].Update();


                    if (secondWallLasers[i].tripped)
                    {
                        secondWallLasersTripped = true;
                        secondWallLasers.Clear();
                        break;
                    }
                    secondWallLasersTripped = false;
                }
            }


                if (secondWallLasersTripped && !platforms.Contains(wallThree) && !spawnedSecondHorizontalMonsters)
            {
                platforms.Add(wallThree);
                platforms.Add(wallFour);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallThree.Rec.X + wallThree.Rec.Width / 2 - 75, wallThree.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallFour.Rec.X + wallFour.Rec.Width / 2 - 75, wallFour.Rec.Y - 50, 150, 150), 2);
            }
                if (!spawnedSecondHorizontalMonsters && platforms.Contains(wallThree))
            {
                if (enemiesInMap.Count < 4)
                    SpawnEnemies(2);
                else
                    spawnedSecondHorizontalMonsters = true;
            }

                if (spawnedSecondHorizontalMonsters && enemiesInMap.Count == 0 && platforms.Contains(wallThree))
            {
                platforms.Remove(wallThree);
                platforms.Remove(wallFour);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallThree.Rec.X + wallThree.Rec.Width / 2 - 75, wallThree.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(wallFour.Rec.X + wallFour.Rec.Width / 2 - 75, wallFour.Rec.Y - 50, 150, 150), 2);

            }
            #endregion

            #region Third Horizontal Area

                if (Player.PositionY < -3500 && player.PositionX > 2700)
                {
                    for (int i = 0; i < thirdWallLasers.Count; i++)
                    {
                        thirdWallLasers[i].Update();


                        if (thirdWallLasers[i].tripped)
                        {
                            thirdWallLasersTripped = true;
                            thirdWallLasers.Clear();
                            break;
                        }
                        thirdWallLasersTripped = false;
                    }
                }


                if (thirdWallLasersTripped && !platforms.Contains(wallFive) && !spawnedThirdHorizontalMonsters)
                {
                    platforms.Add(wallFive);
                    platforms.Add(wallSix);

                    Chapter.effectsManager.AddSmokePoof(new Rectangle(wallFive.Rec.X + wallFive.Rec.Width / 2 - 75, wallFive.Rec.Y - 50, 150, 150), 2);
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(wallSix.Rec.X + wallSix.Rec.Width / 2 - 75, wallSix.Rec.Y - 50, 150, 150), 2);
                }
                if (!spawnedThirdHorizontalMonsters && platforms.Contains(wallFive))
                {
                    if (enemiesInMap.Count < 4)
                        SpawnEnemies(3);
                    else
                        spawnedThirdHorizontalMonsters = true;
                }

                if (spawnedThirdHorizontalMonsters && enemiesInMap.Count == 0 && platforms.Contains(wallFive))
                {
                    platforms.Remove(wallFive);
                    platforms.Remove(wallSix);

                    Chapter.effectsManager.AddSmokePoof(new Rectangle(wallFive.Rec.X + wallFive.Rec.Width / 2 - 75, wallFive.Rec.Y - 50, 150, 150), 2);
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(wallSix.Rec.X + wallSix.Rec.Width / 2 - 75, wallSix.Rec.Y - 50, 150, 150), 2);


                }
                #endregion

            #region First Steps
            if (player.PositionX < 3000 && player.PositionY > -1800)
            {
                for (int i = 0; i < firstLasers.Count; i++)
                {
                    firstLasers[i].Update();


                    if (firstLasers[i].tripped)
                    {
                        firstLasersTripped = true;
                        break;
                    }
                    firstLasersTripped = false;
                }
            }


                if (firstLasersTripped && platforms.Contains(firstStep))
                {
                    player.KnockPlayerDown();

                    platforms.Remove(firstStep);
                    platforms.Remove(secondStep);

                    Chapter.effectsManager.AddSmokePoof(new Rectangle(firstStep.Rec.X + firstStep.Rec.Width / 2 - 75, firstStep.Rec.Y - 50, 150, 150), 2);
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(secondStep.Rec.X + secondStep.Rec.Width / 2 - 75, secondStep.Rec.Y - 50, 150, 150), 2);
                }
                if (!firstLasersTripped && !platforms.Contains(firstStep) && player.CurrentPlat != null)
                {
                    platforms.Add(firstStep);
                    platforms.Add(secondStep);

                    Chapter.effectsManager.AddSmokePoof(new Rectangle(firstStep.Rec.X + firstStep.Rec.Width / 2 - 75, firstStep.Rec.Y - 50, 150, 150), 2);
                    Chapter.effectsManager.AddSmokePoof(new Rectangle(secondStep.Rec.X + secondStep.Rec.Width / 2 - 75, secondStep.Rec.Y - 50, 150, 150), 2);
                }
                #endregion
            
            #region Second Steps

            if (player.PositionX > 3450 && player.PositionX < 5150 && player.PositionY > -2800)
            {
                for (int i = 0; i < secondLasers.Count; i++)
                {
                    secondLasers[i].Update();


                    if (secondLasers[i].tripped)
                    {
                        secondLasersTripped = true;
                        break;
                    }
                    secondLasersTripped = false;
                }
            }


            if (secondLasersTripped && platforms.Contains(thirdStep))
            {
                player.KnockPlayerDown();

                platforms.Remove(thirdStep);
                platforms.Remove(fourthStep);
                platforms.Remove(fifthStep);
                platforms.Remove(sixthStep);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(thirdStep.Rec.X + thirdStep.Rec.Width / 2 - 75, thirdStep.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(fourthStep.Rec.X + fourthStep.Rec.Width / 2 - 75, fourthStep.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(fifthStep.Rec.X + fifthStep.Rec.Width / 2 - 75, fifthStep.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(sixthStep.Rec.X + sixthStep.Rec.Width / 2 - 75, sixthStep.Rec.Y - 50, 150, 150), 2);
            }
            if (!secondLasersTripped && !platforms.Contains(thirdStep) && player.CurrentPlat != null)
            {
                platforms.Add(thirdStep);
                platforms.Add(fourthStep);
                platforms.Add(fifthStep);
                platforms.Add(sixthStep);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(thirdStep.Rec.X + thirdStep.Rec.Width / 2 - 75, thirdStep.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(fourthStep.Rec.X + fourthStep.Rec.Width / 2 - 75, fourthStep.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(fifthStep.Rec.X + fifthStep.Rec.Width / 2 - 75, fifthStep.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(sixthStep.Rec.X + sixthStep.Rec.Width / 2 - 75, sixthStep.Rec.Y - 50, 150, 150), 2);

            }
            #endregion

            #region Third Steps
            if (Player.PositionX > 5000 && Player.PositionY < 2700 && Player.PositionY > -4150)
            {
                for (int i = 0; i < thirdLasers.Count; i++)
                {
                    thirdLasers[i].Update();


                    if (thirdLasers[i].tripped)
                    {
                        thirdLasersTripped = true;
                        break;
                    }
                    thirdLasersTripped = false;
                }
            }
            if (thirdLasersTripped && platforms.Contains(step7))
            {
                player.KnockPlayerDown();

                platforms.Remove(step7);
                platforms.Remove(step8);
                platforms.Remove(step9);
                platforms.Remove(step10);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(step7.Rec.X + step7.Rec.Width / 2 - 75, step7.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step8.Rec.X + step8.Rec.Width / 2 - 75, step8.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step9.Rec.X + step9.Rec.Width / 2 - 75, step9.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step10.Rec.X + step10.Rec.Width / 2 - 75, step10.Rec.Y - 50, 150, 150), 2);
            }

            if (!thirdLasersTripped && !platforms.Contains(step7) && player.CurrentPlat != null)
            {
                platforms.Add(step7);
                platforms.Add(step8);
                platforms.Add(step9);
                platforms.Add(step10);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(step7.Rec.X + step7.Rec.Width / 2 - 75, step7.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step8.Rec.X + step8.Rec.Width / 2 - 75, step8.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step9.Rec.X + step9.Rec.Width / 2 - 75, step9.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(step10.Rec.X + step10.Rec.Width / 2 - 75, step10.Rec.Y - 50, 150, 150), 2);
            }
            #endregion

            #region Moving Platforms
            for (int i = 0; i < movingPlatLasers.Count; i++)
            {
                movingPlatLasers[i].Update();


                if (movingPlatLasers[i].tripped)
                {
                    movingPlatLaserTripped = true;
                    break;
                }
                movingPlatLaserTripped = false;
            }

            if (movingPlatLaserTripped && platforms.Contains(move1))
            {
                player.KnockPlayerDown();

                platforms.Remove(move1);
                platforms.Remove(move2);
                platforms.Remove(move3);
                platforms.Remove(move4);
                platforms.Remove(move5);
                platforms.Remove(move6);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(move1.Rec.X + move1.Rec.Width / 2 - 75, move1.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move2.Rec.X + move2.Rec.Width / 2 - 75, move2.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move3.Rec.X + move3.Rec.Width / 2 - 75, move3.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move4.Rec.X + move4.Rec.Width / 2 - 75, move4.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move5.Rec.X + move5.Rec.Width / 2 - 75, move5.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move6.Rec.X + move6.Rec.Width / 2 - 75, move6.Rec.Y - 50, 150, 150), 2);
            }
            if (!movingPlatLaserTripped && !platforms.Contains(move1) && player.CurrentPlat != null)
            {
                platforms.Add(move1);
                platforms.Add(move2);
                platforms.Add(move3);
                platforms.Add(move4);
                platforms.Add(move5);
                platforms.Add(move6);

                Chapter.effectsManager.AddSmokePoof(new Rectangle(move1.Rec.X + move1.Rec.Width / 2 - 75, move1.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move2.Rec.X + move2.Rec.Width / 2 - 75, move2.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move3.Rec.X + move3.Rec.Width / 2 - 75, move3.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move4.Rec.X + move4.Rec.Width / 2 - 75, move4.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move5.Rec.X + move5.Rec.Width / 2 - 75, move5.Rec.Y - 50, 150, 150), 2);
                Chapter.effectsManager.AddSmokePoof(new Rectangle(move6.Rec.X + move6.Rec.Width / 2 - 75, move6.Rec.Y - 50, 150, 150), 2);
            }
            #endregion
        }

        public override void LoadContent()
        {
            background.Add(Game1.portalTexture);
            blocks = content.Load<Texture2D>(@"Maps\Chelseas\CrossroadsBushes");
            game.NPCSprites["Paul"] = content.Load<Texture2D>(@"NPC\Main\paul");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Field Goblin", content.Load<Texture2D>(@"EnemySprites\FieldGoblinSheet"));
        }


        public override void SetPortals()
        {
            base.SetPortals();

            toCrossroads = new Portal(20, -5122, "EmptyField");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toCrossroads, Crossroads.ToPathThree);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            for (int i = 0; i < tripLasers.Count; i++)
            {
                tripLasers[i].Draw(s);
            }

            for (int i = 0; i < movingPlatLasers.Count; i++)
            {
                movingPlatLasers[i].Draw(s);
            }

            if (player.PositionX < 3000 && player.PositionY > -1800)
            {
                for (int i = 0; i < firstLasers.Count; i++)
                {
                    firstLasers[i].Draw(s);
                }
            }

            if (player.PositionX > 3450 && player.PositionX < 5150 && player.PositionY > -2800)
            {
                for (int i = 0; i < secondLasers.Count; i++)
                {
                    secondLasers[i].Draw(s);
                }
            }

            if (player.PositionY > -2210 && player.PositionX < 4800 && player.PositionY < -950 && player.PositionX > 1500)
            {
                for (int i = 0; i < firstWallLasers.Count; i++)
                {
                    firstWallLasers[i].Draw(s);
                }
            }

            if(Player.PositionY > -3500 && player.PositionY < -2026 && player.PositionX > 3500)
            {
                for (int i = 0; i < secondWallLasers.Count; i++)
                {
                    secondWallLasers[i].Draw(s);
                }
            }

            if (Player.PositionY < -3500 && player.PositionX > 2700)
            {
                for (int i = 0; i < thirdWallLasers.Count; i++)
                {
                    thirdWallLasers[i].Draw(s);
                }
            }

            if (Player.PositionX > 5000 && Player.PositionY < 2700 && Player.PositionY > -4150)
            {
                for (int i = 0; i < thirdLasers.Count; i++)
                {
                    thirdLasers[i].Draw(s);
                }
            }

            //Only draw the cage if it hasn't been opened
            if (!kidCage.Finished)
            {
                kidCage.Draw(s);
            }

            s.Draw(Game1.whiteFilter, lava, Color.Red);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, Game1.camera.GetTransform(1.25f, this, game));

            s.End();
        }
    }
}

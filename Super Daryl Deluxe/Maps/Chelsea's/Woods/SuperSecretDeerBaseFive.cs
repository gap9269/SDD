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

    public class BaltoCutOut : InteractiveObject
    {
        Boolean activated = false;
        Boolean drawFButton;
        Rectangle frec;

        public BaltoCutOut(int x, int y, Game1 g) :base(g, false)
        {
            rec = new Rectangle(x, y, 210, 283);
            vitalRec = new Rectangle(x + 110, y, 100, 283);
            frameTimer = 5;
        }

        public override Rectangle GetSourceRec()
        {
            return new Rectangle(frameState * 210, 0, 210, 283);
        }

        public override void Update()
        {
            base.Update();

            vitalRec.X = rec.X + 100;

            if (last.IsKeyDown(Keys.F) && current.IsKeyUp(Keys.F))
            {
                if(Game1.Player.VitalRec.Intersects(vitalRec))
                    activated = true;
            }

            if (activated && !finished)
            {
                frameTimer--;

                if (frameTimer <= 0)
                {
                    frameState++;
                    frameTimer = 5;

                    if (frameState == 2)
                        finished = true;
                }
            }
        }

        public void Draw(SpriteBatch s, Texture2D tex)
        {
            base.Draw(s);

            s.Draw(tex, rec, GetSourceRec(), Color.White);

            Rectangle fRec = new Rectangle(rec.X + rec.Width / 2 - 43 / 2, rec.Y - 50, 43,
65);

            if (!finished || drawFButton)
            {
                #region Draw NPC names

                //--Get the distance from daryl to the NPC
                Point distanceFromNPC = new Point(Math.Abs(Game1.Player.VitalRec.Center.X - vitalRec.Center.X),
                Math.Abs(Game1.Player.VitalRec.Center.Y - vitalRec.Center.Y));

                if (distanceFromNPC.X < 70 && distanceFromNPC.Y < 130 && !activated)
                    drawFButton = true;
                else
                    drawFButton = false;

                //--If it is less than 250 pixels
                if (distanceFromNPC.X < 250 && distanceFromNPC.Y < 250 && game.CurrentChapter.state == Chapter.GameState.Game && !game.CurrentChapter.TalkingToNPC && !drawFButton && !activated)
                {
                    s.DrawString(Game1.HUDFont, "Balto", new Vector2(rec.X + ((rec.Width / 2) - (Game1.HUDFont.MeasureString("Balto").X / 2)) + 50, rec.Y + Game1.npcHeightFromRecTop["Balto"] - 200), Color.Black);
                }
                #endregion

                int fButtonOffset = (int)(43 - 43 * .9f) / 2;

                frec = new Rectangle((rec.X + rec.Width / 2) - (43 / 2) + fButtonOffset + 50, rec.Y - 65 + Game1.npcHeightFromRecTop["Balto"] - 160, (int)(43 * .9f), (int)(65 * .9f));

                if (drawFButton)
                {
                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);
                }

                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);
                }
            }
        }
    }

    class SuperSecretDeerBaseAlpha : MapClass
    {
        static Portal toDirtyPath;

        public static Portal ToDirtyPath { get { return toDirtyPath; } }

        List<TripLaser> roomOneLasers, roomTwoLasers, roomThreeLasers;

        ButtonState previous;
        ButtonState buttonCurrent;

        KeyboardState keyLast, keyCurrent;

        List<TripLaser> newLasers;
        List<Vector2> laserPoints;

        int tempTime = 0;

        Platform floor, doorOne, doorTwo, doorThree, doorFour;

        public static Boolean laserTripped;

        Boolean lockCamera;

        WallSwitch switchOne, switchTwo, switchThree;

        KidCage kidCage;

        BaltoCutOut cardboardBalto;

        Texture2D baltoCutout;

        public SuperSecretDeerBaseAlpha(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1680;
            mapWidth = 7000;
            mapName = "Super Secret Deer Base Alpha";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 8;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            lockCamera = true;

            roomOneLasers = new List<TripLaser>();
            roomTwoLasers = new List<TripLaser>();
            roomThreeLasers = new List<TripLaser>();

            newLasers = new List<TripLaser>();
            laserPoints = new List<Vector2>();

            floor = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(-24, 60, 7050, 50), false, false, false);
            platforms.Add(floor);

            doorOne = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(376, 61 - 720, 135, 670), false, false, false);

            doorTwo = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1784, 61 - 720, 135, 670), false, false, false);

            doorThree = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3980, 61 - 720, 135, 670), false, false, false);

            doorFour = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(5810, 61 - 720, 135, 670), false, false, false);

            
            //FIRST ROOM
            roomOneLasers.Add(new TripLaser(724, 555 - 720));
            roomOneLasers.Add(new TripLaser(1066, 390 - 720));
            roomOneLasers.Add(new TripLaser(1265, 232 - 720));
            roomOneLasers.Add(new TripLaser(934, 114 - 720));
            roomOneLasers.Add(new TripLaser(581, 250 - 720));
            roomOneLasers.Add(new TripLaser(1332, 605 - 720));
            roomOneLasers.Add(new TripLaser(1632, 260 - 720));

            roomOneLasers.Add(new TripLaser(934, 605 - 720));
            roomOneLasers.Add(new TripLaser(1187, 614 - 720));
            roomOneLasers.Add(new TripLaser(860, 307 - 720));
            roomOneLasers.Add(new TripLaser(1450, 430 - 720));
            roomOneLasers.Add(new TripLaser(1600, 530 - 720));
            roomOneLasers.Add(new TripLaser(1600, 600 - 720));

            //SECOND ROOM
            roomTwoLasers.Add(new TripLaser(2093, 341 - 720, new List<Vector2>() { new Vector2(2730, 341 - 720), new Vector2(2093, 341 - 720) }, 3, 3));
            roomTwoLasers.Add(new TripLaser(2300, 250 - 720, new List<Vector2>() { new Vector2(2300, 605 - 720), new Vector2(2300, 250 - 720) }, 3, 3));

            roomTwoLasers.Add(new TripLaser(2200, 600 - 720, new List<Vector2>() { new Vector2(2600, 600 - 720), new Vector2(2200, 600 - 720) }, 4, 3));

            roomTwoLasers.Add(new TripLaser(4481, -701));
            //roomTwoLasers.Add(new TripLaser(3646, 161));
            roomTwoLasers.Add(new TripLaser(3894, -271));
            roomTwoLasers.Add(new TripLaser(3530, -381));
            roomTwoLasers.Add(new TripLaser(4102, -701));
            roomTwoLasers.Add(new TripLaser(2504, -57));

            roomTwoLasers.Add(new TripLaser(2393, -290, new List<Vector2>() { new Vector2(2739, -290), new Vector2(2393, -290) }, 3, 3));
            roomTwoLasers.Add(new TripLaser(2698, -200));
            roomTwoLasers.Add(new TripLaser(2885, -80, new List<Vector2>() { new Vector2(2885, -354), new Vector2(2885, -80) }, 3, 3));
            roomTwoLasers.Add(new TripLaser(3018, -355, new List<Vector2>() { new Vector2(3018, -80), new Vector2(3018, -355) }, 4, 3));

            roomTwoLasers.Add(new TripLaser(3115, -384, new List<Vector2>() { new Vector2(3115, -110), new Vector2(3115, -384) }, 3, 3));
            roomTwoLasers.Add(new TripLaser(3218, -42, new List<Vector2>() { new Vector2(3218, -125), new Vector2(3218, -42) }, 4, 3));
            roomTwoLasers.Add(new TripLaser(2955, -401, new List<Vector2>() { new Vector2(2955, -117), new Vector2(2955, -401) }, 3, 3));
            roomTwoLasers.Add(new TripLaser(3173, -217, new List<Vector2>() { new Vector2(3579, -217), new Vector2(3173, -217) }, 4, 3));
            roomTwoLasers.Add(new TripLaser(2809, -280));

            roomTwoLasers.Add(new TripLaser(3379, -46, new List<Vector2>() { new Vector2(3586, -305), new Vector2(3379, -46) }, 3, 3));
            roomTwoLasers.Add(new TripLaser(3523, -44, new List<Vector2>() { new Vector2(3624, -234), new Vector2(3523, -44) }, 4, 3));
            roomTwoLasers.Add(new TripLaser(3513, -382, new List<Vector2>() { new Vector2(3524, -37), new Vector2(3513, -382) }, 3, 3));
            roomTwoLasers.Add(new TripLaser(3358, -364));
            roomTwoLasers.Add(new TripLaser(3853, -404, new List<Vector2>() { new Vector2(3853, -50), new Vector2(3853, -404) }, 4, 3));

            roomTwoLasers.Add(new TripLaser(3940, -432, new List<Vector2>() { new Vector2(3940, -150), new Vector2(3940, -432) }, 3, 3));
            roomTwoLasers.Add(new TripLaser(4006, 0, new List<Vector2>() { new Vector2(4006, -390), new Vector2(4006, 0) }, 4, 3));
            roomTwoLasers.Add(new TripLaser(3828, -90, new List<Vector2>() { new Vector2(4040, -90), new Vector2(3828, -90) }, 3, 3));
            roomTwoLasers.Add(new TripLaser(4481, -701));
            //roomTwoLasers.Add(new TripLaser(3646, 161));
            roomTwoLasers.Add(new TripLaser(3894, -270));
            roomTwoLasers.Add(new TripLaser(3530, -381));
            roomTwoLasers.Add(new TripLaser(4102, -701)); //43

            //ROOM THREE

            roomThreeLasers.Add(new TripLaser(4218, -265));
            roomThreeLasers.Add(new TripLaser(4390, -125));

            roomThreeLasers.Add(new TripLaser(4332, -230, 80, 80, true, 0));
            roomThreeLasers.Add(new TripLaser(4519, -260));
            roomThreeLasers.Add(new TripLaser(4560, -120, 60, 60, true, 0));
            roomThreeLasers.Add(new TripLaser(4773, -320, new List<Vector2>() { new Vector2(4733, 30), new Vector2(4773, -320) }, 3, 3));
            roomThreeLasers.Add(new TripLaser(4661, -239, 40, 40, true, 0));

            roomThreeLasers.Add(new TripLaser(4789, -36));

            roomThreeLasers.Add(new TripLaser(4393, -11, 30, 30, true, 10));
            roomThreeLasers.Add(new TripLaser(4967, 10, 30, 30, true, 23));
            roomThreeLasers.Add(new TripLaser(5154, -221, 30, 30, true, 6));
            roomThreeLasers.Add(new TripLaser(5066, -100, 60, 60, true, 42));

            roomThreeLasers.Add(new TripLaser(4955, -248, 60, 60, true, 15));
            roomThreeLasers.Add(new TripLaser(5272, 20, 30, 30, true, 0));
            roomThreeLasers.Add(new TripLaser(5344, -90, 30, 30, true, 7));
            roomThreeLasers.Add(new TripLaser(5437, -128));

            roomThreeLasers.Add(new TripLaser(5265, -203, 70, 70, true, 4));
            roomThreeLasers.Add(new TripLaser(5611, -142, 30, 30, true, 0));
            roomThreeLasers.Add(new TripLaser(5775, -37, 20, 20, true, 17));
            roomThreeLasers.Add(new TripLaser(5609, -228));
            roomThreeLasers.Add(new TripLaser(5770, -308, 20, 20, true, 12));
            roomThreeLasers.Add(new TripLaser(5823, -90));

            //roomThreeLasers.Add(new TripLaser(5249, -340));
            //roomThreeLasers.Add(new TripLaser(5713, 185));
            //roomThreeLasers.Add(new TripLaser(5251, -347));


            switchOne = new WallSwitch(Game1.switchTexture, new Rectangle(1950, 460 - 600, 42, 83));
            switches.Add(switchOne);

            switchTwo = new WallSwitch(Game1.switchTexture, new Rectangle(4165, 460 - 600, 42, 83));
            switches.Add(switchTwo);

            switchThree = new WallSwitch(Game1.switchTexture, new Rectangle(6000, 460 - 600, 42, 83));
            switches.Add(switchThree);

            kidCage = new KidCage(Game1.whiteFilter, 6600, 426 - 600, player);

            cardboardBalto = new BaltoCutOut(-6600, -226, game);
            interactiveObjects.Add(cardboardBalto);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\LaserRoom"));
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\LaserRoom1"));
            baltoCutout = content.Load<Texture2D>(@"Maps\Chelseas\BaltoCutOut");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            game.EnemySpriteSheets.Add("Field Goblin", content.Load<Texture2D>(@"EnemySprites\FieldGoblinSheet"));
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();

            Goblin en = new Goblin(pos, "Field Goblin", game, ref player, this);
            monsterY = platforms[platformNum].Rec.Y - en.Rec.Height - 1;
            en.Position = new Vector2(monsterX, monsterY);
            en.TimeBeforeSpawn = 120;
            en.Hostile = true;

            Rectangle testRec = new Rectangle(en.RecX, monsterY, en.Rec.Width, en.Rec.Height);
            if (testRec.Intersects(player.Rec))
            {
            }
            else
            {
                AddEnemyToEnemyList(en);
            }

        }

        public override void Update()
        {
            base.Update();

            if (spawnEnemies && enemiesInMap.Count < enemyAmount)
            {
                RespawnGroundEnemies();
            }

            if (player.CurrentPlat == floor && floor.Passable)
            {
                floor.Passable = false;
                lockCamera = true;
            }

            if (player.CurrentPlat != null && !platforms.Contains(floor))
            {
                floor.Passable = true;
                platforms.Add(floor);

                if (platforms.Contains(doorOne))
                {
                    platforms.Remove(doorOne);
                }
                if (platforms.Contains(doorTwo))
                {
                    platforms.Remove(doorTwo);
                }
                if (platforms.Contains(doorThree))
                {
                    platforms.Remove(doorThree);
                }
                if (platforms.Contains(doorFour))
                {
                    platforms.Remove(doorFour);
                }
            }

            if (laserTripped && platforms.Contains(floor))
            {
                player.KnockPlayerDown();
                platforms.Remove(floor);
                lockCamera = false;
                spawnEnemies = false;

                if (player.VitalRecX < doorTwo.Rec.X && !platforms.Contains(doorTwo))
                {
                    platforms.Add(doorOne);
                    platforms.Add(doorTwo);
                }
                else if (player.VitalRecX < doorThree.Rec.X && !platforms.Contains(doorThree))
                {
                    platforms.Add(doorThree);
                    platforms.Add(doorTwo);
                }
                else if (player.VitalRecX < doorFour.Rec.X && !platforms.Contains(doorFour))
                {
                    platforms.Add(doorThree);
                    platforms.Add(doorFour);
                }
            }

            if (laserTripped && player.CurrentPlat != null)
            {
                laserTripped = false;
            }

            if (player.CurrentPlat == floor && spawnEnemies == false)
                spawnEnemies = true;

            if (player.VitalRecY < 0 && lockCamera)
            {
                game.Camera.center.Y = -260;
            }


            for (int i = 0; i < roomOneLasers.Count; i++)
            {
                roomOneLasers[i].Update();
            }

            for (int i = 0; i < roomTwoLasers.Count; i++)
            {
                roomTwoLasers[i].Update();
            }

            for (int i = 0; i < roomThreeLasers.Count; i++)
            {
                roomThreeLasers[i].Update();
            }


            if (!game.MapBooleans.chapterTwoMapBooleans["TurnedLaserOneOff"])
                CheckSwitch(switchOne);
            if (!game.MapBooleans.chapterTwoMapBooleans["TurnedLaserTwoOff"])
                CheckSwitch(switchTwo);
            if (!game.MapBooleans.chapterTwoMapBooleans["TurnedLaserThreeOff"])
                CheckSwitch(switchThree);

            if (switchOne.Active && !game.MapBooleans.chapterTwoMapBooleans["TurnedLaserOneOff"])
            {
                game.MapBooleans.chapterTwoMapBooleans["TurnedLaserOneOff"] = true;
                roomOneLasers.Clear();
            }

            if (switchTwo.Active && !game.MapBooleans.chapterTwoMapBooleans["TurnedLaserTwoOff"])
            {
                game.MapBooleans.chapterTwoMapBooleans["TurnedLaserTwoOff"] = true;
                roomTwoLasers.Clear();
            }

            if (switchThree.Active && !game.MapBooleans.chapterTwoMapBooleans["TurnedLaserThreeOff"])
            {
                game.MapBooleans.chapterTwoMapBooleans["TurnedLaserThreeOff"] = true;
                roomThreeLasers.Clear();
            }



            kidCage.Update();



            //If the cage is gone but the boolean hasn't be activated, activate it
            if (!game.ChapterTwo.ChapterTwoBooleans["kidThreeSaved"] && kidCage.Finished)
            {
                game.ChapterTwo.ChapterTwoBooleans["kidThreeSaved"] = true;

                cardboardBalto.NewPosition(6600, -150);

                game.ChapterTwo.NPCs["CrossroadsKid"].RecX = 958;
                game.ChapterTwo.NPCs["CrossroadsKid"].PositionX = 958;
                game.ChapterTwo.NPCs["CrossroadsKid"].FacingRight = false;

                game.ChapterTwo.NPCs["CrossroadsKid"].Dialogue.Clear();
                game.ChapterTwo.NPCs["CrossroadsKid"].Dialogue.Add("I found another path!");

                Crossroads.ToPathThree.IsUseable = true;
            }

            //If the game has loaded and the boolean is activated, but the cage isn't finished, set it to finished
            if (game.ChapterTwo.ChapterTwoBooleans["kidThreeSaved"] && !kidCage.Finished)
                kidCage.Finished = true;

        }

        public override void SetPortals()
        {
            base.SetPortals();

            toDirtyPath = new Portal(50, 60, "Super Secret Deer Base Alpha");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            for (int i = 0; i < roomOneLasers.Count; i++)
            {
                roomOneLasers[i].Draw(s);
            }

            for (int i = 0; i < roomTwoLasers.Count; i++)
            {
                roomTwoLasers[i].Draw(s);
            }

            for (int i = 0; i < roomThreeLasers.Count; i++)
            {
                roomThreeLasers[i].Draw(s);
            }

            //Only draw the cage if it hasn't been opened
            if (!kidCage.Finished)
            {
                kidCage.Draw(s);
            }

            cardboardBalto.Draw(s, baltoCutout);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
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
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toDirtyPath, DirtyPath.ToBuilding);
        }
    }
}

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

    public class FenceSpark
    {
        int timer = 5;
        public bool finished = false;
        int frame;
        int type; //1 or 2
        Vector2 position;

        /// <summary>
        /// Takes in '1' or '2' for sparkType
        /// </summary>
        /// <param name="sparkType"></param>
        public FenceSpark(int sparkType, Vector2 pos)
        {
            type = sparkType;
            position = pos;
        }

        public Rectangle GetSourceRectangle()
        {
            if (type == 1)
                return new Rectangle(69 * frame, 0, 69, 78);
            else
                return new Rectangle(69 * frame, 78, 69, 78);
        }

        public void Update()
        {
            timer--;

            if (timer == 0)
            {
                frame++;
                timer = 5;

                if (frame > 6)
                {
                    finished = true;
                }
            }
        }

        public void Draw(SpriteBatch s, Texture2D tex)
        {
            s.Draw(tex, new Rectangle((int)position.X, (int)position.Y, 69, 78), GetSourceRectangle(), Color.White);
        }
    }


    class TheGoats : MapClass
    {
        static Portal toOutsideTheParty;
        static Portal toChelseasField;
        static Portal toTreeHouse;

        public static Portal ToTreeHouse { get { return toTreeHouse; } }
        public static Portal ToOutsideTheParty { get { return toOutsideTheParty; } }
        public static Portal ToChelseasField { get { return toChelseasField; } }

        Texture2D foreground, sky, back, field, lowBranch, highBranch, sparks;

        Platform lowBranchPlat, highBranchPlat;

        Vector2 lowVelocity, upperVelocity;

        float lowOffsetY, upperOffsetY;

        Boolean lowVelocityIncreasing = false;
        Boolean upperVelocityIncreasing = false;

        float maxLowVelocity = 8;
        float maxHighVelocity = 6;

        Boolean lowBranchMoved = false;
        Boolean highBranchMoved = false;

        List<FenceSpark> fenceSparks;

        int spark1Time, spark2Time, spark3Time;
        static Random randomSparkTime;

        Rectangle hatRec;

        public TheGoats(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {

            mapHeight = 720;
            mapWidth = 2068;
            mapName = "The Goats";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            lowBranchPlat = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1702, 347, 100, 50), true, false, true);
            highBranchPlat = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(1766, 191, 150, 50), true, false, true);

            platforms.Add(lowBranchPlat);
            platforms.Add(highBranchPlat);

            fenceSparks = new List<FenceSpark>();
            randomSparkTime = new Random();

            spark1Time = 120;
            spark2Time = 180;
            spark3Time = 300;
            hatRec = new Rectangle(1000, 475, 50, 50);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\TheGoats"));
            foreground = content.Load<Texture2D>(@"Maps\Chelseas\TheGoatsFore");
            sky = content.Load<Texture2D>(@"Maps\Chelseas\TheGoatsSky");
            back = content.Load<Texture2D>(@"Maps\Chelseas\TheGoatsBarn");
            field = content.Load<Texture2D>(@"Maps\Chelseas\TheGoatsField");
            sparks = content.Load<Texture2D>(@"Maps\Chelseas\TheGoatsSparks");
            lowBranch = content.Load<Texture2D>(@"Maps\Chelseas\TheGoatsLowBranch");
            highBranch = content.Load<Texture2D>(@"Maps\Chelseas\TheGoatsUpperBranch");

            game.NPCSprites["Pelt Kid"] = content.Load<Texture2D>(@"NPC\Party\peltKid");
            Game1.npcFaces["Pelt Kid"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Party\PeltKid");

            ////If the last map does not have the same music
            if (Chapter.lastMap == "Tree House")
            {
                //SoundEffect bg1 = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Hidden Agenda");
                //SoundEffectInstance backgroundMusic1 = bg1.CreateInstance();
                //backgroundMusic1.IsLooped = true;
                //Sound.music.Add("Exploring", backgroundMusic1);

                SoundEffect am = Sound.ambienceContent.Load<SoundEffect>(@"Sound\Ambience\ambience_outdoors_night");
                SoundEffectInstance amb = am.CreateInstance();
                amb.IsLooped = true;
                Sound.ambience.Add("ambience_outdoors_night", amb);
            }

            //Sound.backgroundVolume = 1f;
        }

        public override void PlayAmbience()
        {
            Sound.PlayAmbience("ambience_outdoors_night");
        }
        public override void PlayBackgroundMusic()
        {
            //Sound.PlayBackGroundMusic("Exploring");
        }

        public override void Update()
        {
            base.Update();

            //The pelt kid's hat
            if (player.VitalRec.Intersects(hatRec) && last.IsKeyDown(Keys.Space) && current.IsKeyUp(Keys.Space) && game.MapBooleans.chapterTwoMapBooleans["FoundPeltHat"] == false && (game.ChapterTwo.NPCs["PeltKidOne"] as PeltKid).Dead)
            {
                player.AddHatToInventory(new PeltKidsHat());
                game.MapBooleans.chapterTwoMapBooleans["FoundPeltHat"] = true;

                Chapter.effectsManager.AddFoundItem("Pelt Kid's Hat", Game1.equipmentTextures["Pelt Kid's Hat"]);
            }

            PlayBackgroundMusic();

            #region Make the braches shake when you jump on them

            lowOffsetY += lowVelocity.Y;
            upperOffsetY += upperVelocity.Y;

            if (lowVelocity == Vector2.Zero && player.CurrentPlat == lowBranchPlat && lowBranchMoved == false)
            {
                lowVelocity.Y = 8;
                lowVelocityIncreasing = true;
                maxLowVelocity = 8;
                lowBranchMoved = true;
            }
            if ((player.CurrentPlat == null || player.CurrentPlat != lowBranchPlat) && lowBranchMoved == true)
                lowBranchMoved = false;

            if (lowVelocity != Vector2.Zero)
            {
                if (lowVelocityIncreasing)
                {
                    lowVelocity.Y += .8f;

                    if (lowVelocity.Y >= 0)
                    {
                        maxLowVelocity /= 2;
                        lowVelocity.Y = (maxLowVelocity);
                        lowVelocityIncreasing = false;
                        if (Math.Abs(maxLowVelocity) <= 1)
                        {
                            lowVelocity = Vector2.Zero;
                        }
                    }
                }
                else
                {
                    lowVelocity.Y -= .8f;

                    if (lowVelocity.Y <= 0)
                    {
                        lowVelocity.Y = -(maxLowVelocity);
                        lowVelocityIncreasing = true;

                        if (Math.Abs(maxLowVelocity) <= 1)
                        {
                            lowVelocity = Vector2.Zero;
                        }
                    }
                }

            }

            if (upperVelocity == Vector2.Zero && player.CurrentPlat == highBranchPlat && highBranchMoved == false)
            {
                upperVelocity.Y = 6;
                upperVelocityIncreasing = true;
                maxHighVelocity = 6;
                highBranchMoved = true;
            }
            if ((player.CurrentPlat == null || player.CurrentPlat != highBranchPlat) && highBranchMoved == true)
                highBranchMoved = false;

            if (upperVelocity != Vector2.Zero)
            {
                if (upperVelocityIncreasing)
                {
                    upperVelocity.Y += .6f;

                    if (upperVelocity.Y >= 0)
                    {
                        maxHighVelocity /= 2;
                        upperVelocity.Y = (maxHighVelocity);
                        upperVelocityIncreasing = false;
                        if (Math.Abs(maxHighVelocity) <= 1)
                        {
                            upperVelocity = Vector2.Zero;
                        }
                    }
                }
                else
                {
                    upperVelocity.Y -= .6f;

                    if (upperVelocity.Y <= 0)
                    {
                        upperVelocity.Y = -(maxHighVelocity);
                        upperVelocityIncreasing = true;

                        if (Math.Abs(maxHighVelocity) <= 1)
                        {
                            upperVelocity = Vector2.Zero;
                        }
                    }
                }

            }
            #endregion

            #region Fence sparks
            spark1Time--;
            spark2Time--;
            spark3Time--;

            //Add a spark to the map if a timer is up
            if (spark1Time == 0)
            {
                spark1Time = randomSparkTime.Next(30, 600);

                fenceSparks.Add(new FenceSpark(randomSparkTime.Next(1, 3), new Vector2(580, 430)));
            }
            if (spark2Time == 0)
            {
                spark2Time = randomSparkTime.Next(30, 600);

                fenceSparks.Add(new FenceSpark(randomSparkTime.Next(1, 3), new Vector2(1160, 441)));
            }
            if (spark3Time == 0)
            {
                spark3Time = randomSparkTime.Next(30, 600);

                fenceSparks.Add(new FenceSpark(randomSparkTime.Next(1, 3), new Vector2(1420, 458)));
            }

            //Update the sparks
            for (int i = 0; i < fenceSparks.Count; i++)
            {
                fenceSparks[i].Update();

                if (fenceSparks[i].finished == true)
                {
                    fenceSparks.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            #endregion
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Pelt Kid"] = Game1.whiteFilter;

            Game1.npcFaces["Pelt Kid"].faces["Normal"] = Game1.whiteFilter;

            if (Chapter.theNextMap == "Tree House")
            {
                //Sound.UnloadBackgroundMusic();
                Sound.UnloadAmbience();
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toOutsideTheParty = new Portal(1875, platforms[0], "The Goats");
            toChelseasField = new Portal(10, platforms[0], "The Goats");
            toTreeHouse = new Portal(1740, 100, "The Goats");

            toTreeHouse.FButtonYOffset = 220;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();
            
            portals.Add(toOutsideTheParty, OutsideTheParty.ToTheGoats);
            portals.Add(toChelseasField, ChelseasField.ToTheGoats);
            portals.Add(toTreeHouse, TreeHouse.ToTheGoats);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            //Update the sparks
            for (int i = 0; i < fenceSparks.Count; i++)
            {
                fenceSparks[i].Draw(s, sparks);
            }
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.05f, this, game));

            s.Draw(sky, new Rectangle(-300, mapRec.Y, sky.Width, sky.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.1f, this, game));
            s.Draw(back, new Rectangle(-90, 169, back.Width, back.Height), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.7f, this, game));
            s.Draw(field, new Rectangle(0, 213, field.Width, field.Height), Color.White);
            s.End();

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(0, 635), Color.White);
            s.Draw(lowBranch, new Vector2(1618, 250 + lowOffsetY), Color.White);
            s.Draw(highBranch, new Vector2(1626, 52 + upperOffsetY), Color.White);

            s.End();
        }
    }
}

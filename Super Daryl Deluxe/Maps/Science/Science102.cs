using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    class Science102 : MapClass
    {
        static Portal toScience101;
        static Portal toScience103;
        static Portal toBathroom;
        Platform doorOne;
        Platform doorTwo;

        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToScience101 { get { return toScience101; } }
        public static Portal ToScience103 { get { return toScience103; } }

        public Dictionary<String, Texture2D> giantErlTextures;

        Texture2D outhouse, fore, pulse, bubbles, panels1, panels2, flask, erl, erlBottom;

        int pulseX = -4000;
        float bubbleY;

        Boolean spawnGiantErl = false;
        Boolean destroyGiantErl = false;
        int erlFrame, erlDelay;

        float v1; //float velocities

        float y1; //float Y positions

        Boolean up1;

        public Science102(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapWidth = 5700;
            mapHeight = 2000;
            mapName = "Science 102";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 1;

            yScroll = true;
            zoomLevel = .85f;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            doorOne = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(3960, -800, 50, 750), false, false, false);
            doorTwo = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(4640, -800, 50, 750), false, false, false);

            erlDelay = 5;

            giantErlTextures = new Dictionary<String, Texture2D>();
        }

        public override void RespawnGroundEnemies()
        {
            if (game.MapBooleans.prologueMapBooleans["enemyKilled"] == false && game.MapBooleans.prologueMapBooleans["doorsAdded"])
            {
                switch (game.chapterState)
                {
                    case Game1.ChapterState.prologue:
                        ErlTheFlask en = new ErlTheFlask(pos, "Erl The Flask", game, ref player, this);
                        en.TimeBeforeSpawn = 60;
                        monsterY = -20 - en.Rec.Height - 1;
                        en.Position = new Vector2(4380, monsterY);

                        Rectangle testRec = new Rectangle(en.RecX, monsterY, en.VitalRec.Width, en.VitalRec.Height);
                        if (testRec.Intersects(player.Rec))
                        {
                        }
                        else
                        {
                            enemiesInMap.Add(en);
                            game.MapBooleans.prologueMapBooleans["enemyAdded"] = true;
                        }
                        break;
                }
            }
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Science\102\background1"));
            background.Add(content.Load<Texture2D>(@"Maps\Science\102\background2"));
            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");
            pulse = content.Load<Texture2D>(@"Maps\Science\102\pulse");
            fore = content.Load<Texture2D>(@"Maps\Science\102\fore");
            bubbles = content.Load<Texture2D>(@"Maps\Science\102\bubbles");
            panels1 = content.Load<Texture2D>(@"Maps\Science\102\panels1");
            panels2 = content.Load<Texture2D>(@"Maps\Science\102\panels2");
            flask = content.Load<Texture2D>(@"Maps\Science\102\flask");
            erl = content.Load<Texture2D>(@"Maps\Science\102\erl");
            erlBottom = content.Load<Texture2D>(@"Maps\Science\102\erlBottom");

            if (game.MapBooleans.prologueMapBooleans["doorsAdded"] == false)
                giantErlTextures = ContentLoader.LoadContent(content, "Maps\\Science\\102\\Erl");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();

            EnemyContentLoader.ErlTheFlask(content);
        }

        public override void Update()
        {
            base.Update();

            pulseX += 50;

            if (pulseX > mapWidth * 2)
            {
                pulseX = -2000;
            }

            if (player.PositionX > 3970 && game.MapBooleans.prologueMapBooleans["doorsAdded"] == false)
            {
                platforms.Add(doorOne);
                platforms.Add(doorTwo);
                game.Camera.ShakeCamera(30, 4);
                game.MapBooleans.prologueMapBooleans["doorsAdded"] = true;
                spawnGiantErl = true;
            }

            if (spawnGiantErl)
            {
                erlDelay--;

                if (erlDelay <= 0)
                {
                    erlFrame++;

                    erlDelay = 6;

                    if(erlFrame > 3)
                        erlDelay = 4;

                    if (erlFrame > 13)
                    {
                        spawnGiantErl = false;
                    }
                }
            }
            else if (destroyGiantErl)
            {
                erlDelay--;

                if (erlDelay <= 0)
                {
                    erlFrame++;

                    if (erlFrame > 3)
                        erlDelay = 4;

                    if (erlFrame > 21)
                    {
                        destroyGiantErl = false;
                        platforms.Remove(doorOne);
                        platforms.Remove(doorTwo);

                    }
                }
            }

            if (game.MapBooleans.prologueMapBooleans["doorsAdded"] == true && game.MapBooleans.prologueMapBooleans["enemyAdded"] == false && !spawnGiantErl)
            {
                //--If there aren't max enemies on the screen, spawn more
                if (enemiesInMap.Count < enemyAmount)
                    RespawnGroundEnemies();
            }

            if (game.MapBooleans.prologueMapBooleans["doorsAdded"] && enemiesInMap.Count == 0 && game.MapBooleans.prologueMapBooleans["enemyAdded"] && !game.MapBooleans.prologueMapBooleans["enemyKilled"])
            {
                game.MapBooleans.prologueMapBooleans["enemyKilled"] = true;
                game.Prologue.PrologueBooleans["removeNPCs"] = true;
                destroyGiantErl = true;
                game.Camera.ShakeCamera(20, 4);
            }
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            base.DrawBackgroundAndParallax(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(Game1.whiteFilter, mapRec, new Color(25,25,25));
            s.Draw(pulse, new Vector2(pulseX, mapRec.Y), Color.White);
            s.Draw(panels1, new Vector2(0, mapRec.Y), Color.White);
            s.Draw(panels2, new Vector2(panels1.Width, mapRec.Y), Color.White);
            s.End();

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
SamplerState.LinearWrap, null, null, null, Game1.camera.Transform);
            bubbleY += 1;
            s.Draw(bubbles, new Rectangle(2195, mapRec.Y - 100, bubbles.Width, bubbles.Height), new Rectangle(0, (int)bubbleY, bubbles.Width, bubbles.Height), Color.White);
            s.End();
        
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            if (erlFrame < 22 && (spawnGiantErl || destroyGiantErl))
            {
                String textureString = "science102";
                if (erlFrame < 10)
                    textureString += "0" + erlFrame.ToString();
                else
                    textureString += erlFrame.ToString();

                s.Draw(giantErlTextures.ElementAt(erlFrame).Value, new Vector2(3509, mapRec.Y + 20), Color.White);

                if (erlFrame < 4)
                    s.Draw(flask, new Vector2(4278, mapRec.Y + 695), Color.White);

            }

            if (game.MapBooleans.prologueMapBooleans["doorsAdded"] == false)
            {
                if (!up1)
                {
                    v1 += .008f;
                    y1 += v1;

                    if (v1 >= .45)
                        up1 = true;
                }
                else
                {
                    v1 -= .008f;
                    y1 += v1;

                    if (v1 <= -.45)
                        up1 = false;
                }

                s.Draw(flask, new Vector2(4278, mapRec.Y + 655 + y1), Color.White);
            }
            else if (game.MapBooleans.prologueMapBooleans["doorsAdded"] == true && game.MapBooleans.prologueMapBooleans["enemyKilled"] == false && !spawnGiantErl)
            {
                //s.Draw(giantErlTextures.ElementAt(13).Value, new Vector2(3509, mapRec.Y + 20), Color.White);
                s.Draw(erl, new Vector2(3509, mapRec.Y + 20), Color.White);
            }

            s.Draw(fore, new Vector2(3561, mapRec.Y + 744), Color.White);
            s.End();
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
            s.Draw(outhouse, new Rectangle(4890, -outhouse.Height + 8, outhouse.Width, outhouse.Height), Color.White);

            if (game.MapBooleans.prologueMapBooleans["doorsAdded"] == true && game.MapBooleans.prologueMapBooleans["enemyKilled"] == false || (spawnGiantErl && erlFrame > 4))
            {
                s.Draw(erlBottom, new Vector2(3784, mapRec.Y + 859), Color.White);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toScience101 = new Portal(-20, platforms[0], "Science102");
            toScience101.FButtonYOffset = -10;
            toScience101.PortalNameYOffset = -10;

            toScience103 = new Portal(5500, 14, "Science102");
            toBathroom = new Portal(5000, 14, "Science102");
            toBathroom.PortalRecY = -230; //Keep this here
            toScience103.PortalRecY = -230; //this one too

            toBathroom.FButtonYOffset = -40;
            toBathroom.PortalNameYOffset = -40;

            toScience103.FButtonYOffset = -40;
            toScience103.PortalNameYOffset = -40;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toScience101, Science101.ToScience102);
            portals.Add(toScience103, Science103.ToScience102);
            portals.Add(ToBathroom, Bathroom.ToLastMap);
        }
    }
}

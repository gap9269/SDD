using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    class GeneratorRoom : MapClass
    {
        static Portal toBasement;

        public static Portal ToBasement { get { return toBasement; } }

        WallSwitch doorSwitch;
        Platform door;
        public GeneratorRoom(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2050;
            mapName = "Generator Room";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            doorSwitch = new WallSwitch(Game1.switchTexture, new Rectangle(700, (int)(Game1.aspectRatio * 1280 * .6), 42, 83));
            door = new Platform(Game1.platformTextures.ElementAt(0).Value, new Rectangle(560, 0, 50, 720), false, false, false);
            platforms.Add(door);

            switches.Add(doorSwitch);
        }

        public override void Update()
        {
            base.Update();

            if (CheckSwitch(doorSwitch) && platforms.Contains(door))
            {
                game.Camera.ShakeCamera(5, 10);
            }

            if (platforms.Contains(door) && doorSwitch.Active)
            {
                platforms.Remove(door);
            }
        }

        public override void RespawnGroundEnemies()
        {
            base.RespawnGroundEnemies();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toBasement = new Portal(100, platforms[0], "GeneratorRoom");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toBasement, Basement.ToGeneratorRoom);
        }
    }
}
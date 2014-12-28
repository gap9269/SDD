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
    class OokySpookyBarn : MapClass
    {
        static Portal toIrrigationCanal;
        static Portal toSilo1;

        public static Portal ToIrrigationCanal { get { return toIrrigationCanal; } }
        public static Portal ToSilo1 { get { return toSilo1; } }

        public OokySpookyBarn(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2500;
            mapWidth = 2550;
            mapName = "Ooky Spooky Barn";

            mapRec = new Rectangle(0, -mapHeight + 720 + 360, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            Barrel bar = new Barrel(game, 824, 630, Game1.interactiveObjects["Barrel"], true, 5, 0, 1.38f, false, Barrel.BarrelType.WoodenLeft);
            interactiveObjects.Add(bar);

            Barrel bar1 = new Barrel(game, 963, 624, Game1.interactiveObjects["Barrel"], true, 5, 0, .16f, false, Barrel.BarrelType.WoodenRight);
            interactiveObjects.Add(bar1);

            Barrel bar2 = new Barrel(game, 910, 640, Game1.interactiveObjects["Barrel"], true, 5, 0, .16f, false, Barrel.BarrelType.WoodenLeft);
            interactiveObjects.Add(bar2);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\barn"));
            game.NPCSprites["Paul"] = content.Load<Texture2D>(@"NPC\Main\paul");
            game.NPCSprites["Callyn"] = content.Load<Texture2D>(@"NPC\Party\BeerGogglesKid");
        }

        public override void Update()
        {
            base.Update();

            zoomLevel = 1f;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toIrrigationCanal = new Portal(100, platforms[0], "OokySpookyBarn");
            toSilo1 = new Portal(80, -480, "OokySpookyBarn");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toIrrigationCanal, IrrigationCanal.ToOokySpookyBarn);
            portals.Add(toSilo1, Silo1.ToOokySpookyBarn);
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
    }
}
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
    class TunnelOfCertainDeath : MapClass
    {
        static Portal toPrisonChamber;
        public static Portal ToPrisonChamber { get { return toPrisonChamber; } }

        static Portal toChamberOfCorruption;
        public static Portal ToChamberOfCorruption { get { return toChamberOfCorruption; } }

        Texture2D foreground;

        public TunnelOfCertainDeath(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2600;
            mapName = "Tunnel of Certain Death";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            SpikeTrap spikeTrap1 = new SpikeTrap(180, 2053, 470, game, 60);
            SpikeTrap spikeTrap2 = new SpikeTrap(180, 1745, 470, game, 60);
            SpikeTrap spikeTrap3 = new SpikeTrap(180, 1437, 470, game, 60);
            SpikeTrap spikeTrap4 = new SpikeTrap(180, 1129, 470, game, 60);
            SpikeTrap spikeTrap5 = new SpikeTrap(180, 821, 470, game, 60);
            SpikeTrap spikeTrap6 = new SpikeTrap(180, 513, 470, game, 60);
            SpikeTrap spikeTrap7 = new SpikeTrap(180, 205, 470, game, 60);

            spikeTrap1.Timer = 170;
            spikeTrap2.Timer = 110;
            spikeTrap3.Timer = 50;
            spikeTrap4.Timer = 170;
            spikeTrap5.Timer = 50;
            spikeTrap6.Timer = 110;
            spikeTrap7.Timer = 170;

            mapHazards.Add(spikeTrap1);
            mapHazards.Add(spikeTrap2);
            mapHazards.Add(spikeTrap3);
            mapHazards.Add(spikeTrap4);
            mapHazards.Add(spikeTrap5);
            mapHazards.Add(spikeTrap6);
            mapHazards.Add(spikeTrap7);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\TunnelOfCertainDeath\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\TunnelOfCertainDeath\foreground");
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toPrisonChamber = new Portal(50, platforms[0], "Tunnel of Certain Death");
            toChamberOfCorruption = new Portal(2400, platforms[0], "Tunnel of Certain Death");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toPrisonChamber, PrisonChamber.ToTunnelOfCertainDeath);
            portals.Add(toChamberOfCorruption, ChamberOfCorruption.ToTunnelOfCertainDeath);
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

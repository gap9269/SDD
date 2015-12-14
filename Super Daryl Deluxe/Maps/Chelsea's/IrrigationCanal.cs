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
    class IrrigationCanal : MapClass
    {
        static Portal toOokySpookyBarn;
        static Portal toWorkersField;
        static Portal toHut;

        public static Portal ToHut { get { return toHut; } }
        public static Portal ToOokySpookyBarn { get { return toOokySpookyBarn; } }
        public static Portal ToWorkersField { get { return toWorkersField; } }

        Texture2D bridge;

        public IrrigationCanal(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1400;
            mapName = "Irrigation Canal";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Chelseas\IrrigationCanal"));
            bridge = content.Load<Texture2D>(@"Maps\Chelseas\IrrigationCanalBridge");
            game.NPCSprites["Alan"] = content.Load<Texture2D>(@"NPC\Main\alan");
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toWorkersField = new Portal(100, platforms[0], "Irrigation Canal");
            toOokySpookyBarn = new Portal(1000, platforms[0], "Irrigation Canal");
            toHut = new Portal(400, platforms[0], "Irrigation Canal");
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

           // if(game.ChapterTwo.buildBridgeOne.CompletedQuest && !game.CurrentQuests.Contains(game.ChapterTwo.buildBridgeOne))
             //   s.Draw(bridge, mapRec, Color.White);
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toWorkersField, WorkersField.ToIrrigationCanal);
            portals.Add(toOokySpookyBarn, OokySpookyBarn.ToIrrigationCanal);
            portals.Add(toHut, TrollsHut.ToIrrigationCanal);
        }
    }
}

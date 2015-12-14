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
    class TheCliffOfIle : MapClass
    {
        static Portal toFalseRoom;

        public static Portal ToFalseRoom { get { return toFalseRoom; } }

        Texture2D foreground;

        public TheCliffOfIle(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 1500;
            mapWidth = 1400;
            mapName = "The Cliff of Ile";

            mapRec = new Rectangle(0, -410, mapWidth, mapHeight);
            enemyAmount = 3;

            yScroll = true;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\CliffOfIle\background"));
            background.Add(Game1.whiteFilter);
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\CliffOfIle\foreground");


            game.NPCSprites["Bob the Construction Guy"] = content.Load<Texture2D>(@"NPC\Party\ConstructionBob");

            Game1.npcFaces["Bob the Construction Guy"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Bob");
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Bob the Construction Guy"] = Game1.whiteFilter;

            Game1.npcFaces["Bob the Construction Guy"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();

            game.ChapterTwo.NPCs["BobTheConstructionGuyOne"].RecX = 500;
            game.ChapterTwo.NPCs["BobTheConstructionGuyOne"].RecY = -148;

            if (player.VitalRecY > 800)
            {
                ForceToNewMap(new Portal(0, 0, "The Cliff of Ile"), PyramidChute.fromCliffOfIle);
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();
            toFalseRoom = new Portal(120, platforms[0], "The Cliff of Ile");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);
        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toFalseRoom, FalseRoom.ToCliffOfIle);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);

            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {

        }
    }
}

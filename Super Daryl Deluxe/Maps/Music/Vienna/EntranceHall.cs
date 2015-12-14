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
    class EntranceHall : MapClass
    {
        static Portal toIntroRoom;
        static Portal toTheStage;
        static Portal toSecondFloor;

        public static Portal ToIntroRoom { get { return toIntroRoom; } }
        public static Portal ToTheStage { get { return toTheStage; } }
        public static Portal ToSecondFloor { get { return toSecondFloor; } }

        Texture2D foreground;

        public EntranceHall(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 2000;
            mapName = "Entrance Hall";

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
            background.Add(content.Load<Texture2D>(@"Maps\Music\Theater Entrance\background"));
            foreground = content.Load<Texture2D>(@"Maps\Music\Theater Entrance\foreground");
            game.NPCSprites["Beethoven"] = content.Load<Texture2D>(@"NPC\Music\Beethoven");
            Game1.npcFaces["Beethoven"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Music\BeethovenDeaf");
            Game1.npcFaces["Beethoven"].faces["Horn"] = content.Load<Texture2D>(@"NPCFaces\Music\BeethovenHorn");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Beethoven"] = Game1.whiteFilter;
            Game1.npcFaces["Beethoven"].faces["Normal"] = Game1.whiteFilter;
            Game1.npcFaces["Beethoven"].faces["Horn"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toIntroRoom = new Portal(0, platforms[0], "Entrance Hall");
            toTheStage = new Portal(1800, platforms[0], "Entrance Hall");
            toSecondFloor = new Portal(500, platforms[0], "Entrance Hall");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toIntroRoom, MusicIntroRoom.ToEntranceHall);
            portals.Add(ToSecondFloor, SecondFloor.ToEntranceHall);
            portals.Add(toTheStage, TheStage.ToEntranceHall);
        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            s.End();
        }

    }
}

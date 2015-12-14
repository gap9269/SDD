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
    class TheStage : MapClass
    {
        static Portal toBackstage;
        static Portal toEntranceHall;
        static Portal toSecondFloor;
        static Portal toBathroom;

        public static Portal ToBathroom { get { return toBathroom; } }
        public static Portal ToBackstage { get { return toBackstage; } }
        public static Portal ToEntranceHall { get { return toEntranceHall; } }
        public static Portal ToSecondFloor { get { return toSecondFloor; } }

        Texture2D foreground, cloud, outhouseTexture;
        float foregroundAlpha = 1f;

        public TheStage(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 2300;
            mapWidth = 3600;
            mapName = "The Stage";

            mapRec = new Rectangle(0, 720 + 360 - mapHeight, mapWidth, mapHeight);
            enemyAmount = 0;

            yScroll = true;
            zoomLevel = .9f;
            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            TreasureChest rafterChest = new TreasureChest(Game1.treasureChestSheet, 3000, -283, player, 5.00f, new BandUniform(), this);
            treasureChests.Add(rafterChest);

        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\Music\The Stage\background"));
            foreground = content.Load<Texture2D>(@"Maps\Music\The Stage\foreground");
            cloud = content.Load<Texture2D>(@"Maps\Music\The Stage\cloud");
            outhouseTexture = content.Load<Texture2D>(@"Maps\Outhouse");

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

            toBackstage = new Portal(3400, platforms[0], "The Stage", "Backstage Key");
            toEntranceHall = new Portal(50, platforms[0], "The Stage");
            toBathroom = new Portal(2500, platforms[0], "The Stage");
            toSecondFloor = new Portal(50, -510 + Game1.portalTexture.Height, "TheStage");
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEntranceHall, EntranceHall.ToTheStage);
            portals.Add(toBathroom, Bathroom.ToLastMap);
            portals.Add(toSecondFloor, SecondFloor.ToStage);
            portals.Add(ToBackstage, Backstage.ToTheStage);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouseTexture, new Vector2(2395, platforms[0].RecY - outhouseTexture.Height + 20), Color.White);

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);
            s.Draw(cloud, new Vector2(684, mapRec.Y + 1097), Color.White);

            //Front door
            if (player.VitalRec.X > 3000 && player.VitalRecY > 150)
            {

                if (foregroundAlpha > 0)
                    foregroundAlpha -= .05f;
            }
            else
            {
                if (foregroundAlpha < 1f)
                    foregroundAlpha += .05f;
            }

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White * foregroundAlpha);
            s.End();
        }
    }
}

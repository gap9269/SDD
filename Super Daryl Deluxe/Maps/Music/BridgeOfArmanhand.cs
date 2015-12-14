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
    class BridgeOfArmanhand : MapClass
    {
        static Portal toEntrance;
        static Portal toRiver;
        static Portal toRift;

        public static Portal ToRift { get { return toRift; } }
        public static Portal ToRiver { get { return toRiver; } }
        public static Portal ToEntrance { get { return toEntrance; } }

        Texture2D sky, foreground;
        Dictionary<String, Texture2D> rip;
        int ripFrame;
        int ripDelay = 5;

        public BridgeOfArmanhand(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 3000;
            mapName = "Bridge of Armanhand";

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
            background.Add(content.Load<Texture2D>(@"Maps\Music\Bridge of Armanhand\background"));
            foreground = content.Load<Texture2D>(@"Maps\Music\Bridge of Armanhand\foreground");

            sky = content.Load<Texture2D>(@"Maps\Music\Bridge of Armanhand\parallax");

            rip = ContentLoader.LoadContent(content, @"Maps\Music\Bridge of Armanhand\rip");
            game.NPCSprites["Portal Repair Specialist"] = content.Load<Texture2D>(@"NPC\Main\Portal Repair Specialist");
            Game1.npcFaces["Portal Repair Specialist"].faces["Normal"] = content.Load<Texture2D>(@"NPCFaces\Main Characters\Portal Repair Specialist Normal");
        }
        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();

            game.NPCSprites["Portal Repair Specialist"] = Game1.whiteFilter;
            Game1.npcFaces["Portal Repair Specialist"].faces["Normal"] = Game1.whiteFilter;
        }

        public override void Update()
        {
            base.Update();
            ripDelay--;

            if (ripDelay <= 0)
            {
                ripFrame++;
                ripDelay = 1;

                if (ripFrame > 31)
                    ripFrame = 0;
            }

            if (game.CurrentQuests.Contains(game.ChapterOne.portalRepairman) && game.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftCompleted"] == false)
                ToRift.IsUseable = true;
            else
                ToRift.IsUseable = false;
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toEntrance = new Portal(2800, platforms[0], "Bridge of Armanhand", Portal.DoorType.none);
            toRift = new Portal(1400, platforms[0], "Bridge of Armanhand", Portal.DoorType.movement_portal_enter);
            toRiver = new Portal(100, platforms[0], "Bridge of Armanhand", "Bronze Key", Portal.DoorType.none);

            toRift.FButtonYOffset = -56;
            toRift.PortalNameYOffset = -56;

            ToRiver.FButtonYOffset = -18;
            ToRiver.PortalNameYOffset = -18;
        }

        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toEntrance, MusicIntroRoom.ToBridge);
            portals.Add(toRift, BridgeOfArmanhandRift.ToBridge);
            portals.Add(toRiver, GrandCanal.ToBridge);
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            if(game.ChapterOne.ChapterOneBooleans["bridgeOfArmanhandRiftCompleted"] == false)
                s.Draw(rip.ElementAt(ripFrame).Value, new Vector2(1076, 116), Color.White);
            else
                s.Draw(rip["dimensional rip healed"], new Vector2(1076, 116), Color.White);

        }

        public override void DrawParallaxAndForeground(SpriteBatch s)
        {
            base.DrawParallaxAndForeground(s);

            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.Transform);

            s.Draw(foreground, new Vector2(0, mapRec.Y), Color.White);
            //s.Draw(foreground2, new Vector2(mapRec.Width - foreground2.Width, mapRec.Y), Color.White);
            s.End();
        }

        public override void DrawBackgroundAndParallax(SpriteBatch s)
        {
            s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
null, null, null, null, Game1.camera.GetTransform(.15f, this, game));
            s.Draw(sky, new Vector2(-915, mapRec.Y), Color.White);
            s.End();
        }
    }
}

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
    class ForgottenChamberII : MapClass
    {
        static Portal toForgottenChamberI;
        static Portal toEmperorBoomBoomsTomb;
        public static Portal toBathroom;

        public static Portal ToEmperorBoomBoomsTomb { get { return toEmperorBoomBoomsTomb; } }
        public static Portal ToForgottenChamberI { get { return toForgottenChamberI; } }

        Texture2D foreground, healthyWall, crumblingWall, outhouse;
        BreakableObject breakableWall;
        public ForgottenChamberII(List<Texture2D> bg, Game1 g, ref Player play)
            : base(bg, g, ref play)
        {
            mapHeight = 720;
            mapWidth = 1600;
            mapName = "Forgotten Chamber II";

            mapRec = new Rectangle(0, 0, mapWidth, mapHeight);
            enemyAmount = 5;

            yScroll = false;

            AddPlatforms();
            AddBounds();
            AddNPCs();
            SetPortals();

            interactiveObjects.Add(new LivingLocker(game, new Rectangle(300, 100, 600, 500)));
            breakableWall = new BreakableObject(game, 1440, 340, Game1.whiteFilter, false, 8, 0, 0, false);
            breakableWall.Rec = new Rectangle(1440, 340, 300, 720);
            breakableWall.VitalRec = new Rectangle(1460, 340, 300, 300);
            interactiveObjects.Add(breakableWall);
        }

        public override void LoadContent()
        {
            background.Add(content.Load<Texture2D>(@"Maps\History\Pyramid\ForgottenChamberII\background"));
            foreground = content.Load<Texture2D>(@"Maps\History\Pyramid\ForgottenChamberII\foreground");
            healthyWall = content.Load<Texture2D>(@"Maps\History\Pyramid\ForgottenChamberII\healthyWall");
            crumblingWall = content.Load<Texture2D>(@"Maps\History\Pyramid\ForgottenChamberII\crumblingWall");
            outhouse = content.Load<Texture2D>(@"Maps\Outhouse");
        }

        public override void LoadEnemyData()
        {
            base.LoadEnemyData();
        }

        public override void UnloadNPCContent()
        {
            base.UnloadNPCContent();
        }

        public override void Update()
        {
            base.Update();

            if (!breakableWall.Finished && toEmperorBoomBoomsTomb.IsUseable)
                toEmperorBoomBoomsTomb.IsUseable = false;
            else if (breakableWall.Finished && !toEmperorBoomBoomsTomb.IsUseable)
                toEmperorBoomBoomsTomb.IsUseable = true;

            if (breakableWall.Health <= 0 && breakableWall.Finished == false)
            {
                Chapter.effectsManager.AddSmokePoof(new Rectangle(1300, 320, 350, 350), 2);
                breakableWall.Finished = true;
            }
        }

        public override void SetPortals()
        {
            base.SetPortals();

            toForgottenChamberI = new Portal(50, platforms[0], "Forgotten Chamber II");
            toEmperorBoomBoomsTomb = new Portal(1385, platforms[0], "Forgotten Chamber II");
            toBathroom = new Portal(1100, platforms[0], "Forgotten Chamber II");
        }
        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.Draw(outhouse, new Vector2(1000, 370), Color.White);

            if (!breakableWall.Finished)
            {
                if(breakableWall.Health > breakableWall.MaxHealth / 2)
                    s.Draw(healthyWall, new Vector2(1380, 0), Color.White);
                else if (breakableWall.Health <= breakableWall.MaxHealth / 2)
                    s.Draw(crumblingWall, new Vector2(1380, 0), Color.White);
            }

        }
        public override void SetDestinationPortals()
        {
            base.SetDestinationPortals();

            portals.Add(toForgottenChamberI, ForgottenChamberI.ToForgottenChamberII);
            portals.Add(toEmperorBoomBoomsTomb, EmperorBoomBoomsTomb.ToForgottenChamberII);
            portals.Add(toBathroom, Bathroom.ToLastMap);
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

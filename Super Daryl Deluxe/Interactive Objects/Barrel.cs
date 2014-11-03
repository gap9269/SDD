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
    public class Barrel : BreakableObject
    {
        //int barrelType;

        public enum BarrelType
        {
            WoodenRight, WoodenLeft, Radioactive, MetalLabel, MetalBlank,
            TimBarrel,
            ScienceBarrel, ScienceFlask, ScienceTube, ScienceJar
        }
        public BarrelType barrelType;

        public Barrel(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, Object content, float mon, Boolean fore, BarrelType barrlType)
            : base(g, x, y, s, pass, hlth, content, mon, fore)
        {
            rec = new Rectangle(x, y - 155, 105, 155);
            barrelType = barrlType;
            vitalRec = rec;
        }

        public Barrel(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, int hlthDrop, float mon, Boolean fore, BarrelType barrlType)
            : base(g, x, y, s, pass, hlth, hlthDrop, mon, fore)
        {
            rec = new Rectangle(x, y - 155, 105, 155);
            barrelType = barrlType;
            vitalRec = rec;
        }

        public Barrel(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, String content, float mon, Boolean fore, BarrelType barrlType)
            : base(g, x, y, s, pass, hlth, content, mon, fore)
        {
            rec = new Rectangle(x, y - 155, 105, 155);
            barrelType = barrlType;
            vitalRec = rec;
        }

        public Barrel(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, StoryItem story, float mon, Boolean fore, BarrelType barrlType)
            : base(g, x, y, s, pass, hlth, story, mon, fore)
        {
            rec = new Rectangle(x, y - 155, 105, 155);
            barrelType = barrlType;
            vitalRec = rec;
        }


        public override Rectangle GetSourceRec()
        {
            switch (barrelType)
            {
                case BarrelType.WoodenLeft:
                    return new Rectangle(0, 0, 105, 155);
                case BarrelType.WoodenRight:
                    return new Rectangle(0, 155, 105, 155);
                case BarrelType.Radioactive:
                    return new Rectangle(0, 310, 105, 155);
                case BarrelType.MetalBlank:
                    return new Rectangle(0, 465, 105, 155);
                case BarrelType.MetalLabel:
                    return new Rectangle(0, 620, 105, 155);
                case BarrelType.TimBarrel:
                    return new Rectangle(0, 775, 105, 155);
                case BarrelType.ScienceBarrel:
                    return new Rectangle(105, 155, 105, 155);
                case BarrelType.ScienceFlask:
                    return new Rectangle(105, 0, 105, 155);
                case BarrelType.ScienceJar:
                    return new Rectangle(105, 465, 105, 155);
                case BarrelType.ScienceTube:
                    return new Rectangle(105, 310, 105, 155);
            }

            return new Rectangle();
        }

        public override void Update()
        {

            if (!finished)
            {
                vitalRec = rec;

                if (health <= 0 && finished == false)
                {
                    Chapter.effectsManager.AddSmokePoof(rec, 2);
                    finished = true;

                    if (frameState == 0)
                    {
                        DropHealth();
                        DropMoney();

                        if (drop != null)
                        {
                            EnemyDrop tempDrop = new EnemyDrop(drop as Equipment, new Rectangle(rec.Center.X, rec.Center.Y, 50, 50));
                            game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                        }
                        else if (enemyDrop != "" && enemyDrop != null)
                        {
                            EnemyDrop tempDrop = new EnemyDrop(enemyDrop, new Rectangle(rec.Center.X, rec.Center.Y, 50, 50));
                            game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                        }
                        else if (storyItem != null)
                        {
                            EnemyDrop tempDrop = new EnemyDrop(storyItem, new Rectangle(rec.Center.X, rec.Center.Y, 50, 50));
                            game.CurrentChapter.CurrentMap.Drops.Add(tempDrop);
                        }
                    }
                }
            }

            base.Update();
        }
    }
}
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
    public class Vase : BreakableObject
    {
        //int barrelType;

        public enum VaseType
        {
            claySmall, clayMedium, clayLarge
        }
        public VaseType vaseType;

        public Vase(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, Object content, float mon, Boolean fore, VaseType barrlType)
            : base(g, x, y, s, pass, hlth, content, mon, fore)
        {
            rec = new Rectangle(x, y - 159, 83, 159);
            vaseType = barrlType;
            vitalRec = rec;
        }

        public Vase(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, int hlthDrop, float mon, Boolean fore, VaseType barrlType)
            : base(g, x, y, s, pass, hlth, hlthDrop, mon, fore)
        {
            rec = new Rectangle(x, y - 159, 83, 159);
            vaseType = barrlType;
            vitalRec = rec;
        }

        public Vase(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, String content, float mon, Boolean fore, VaseType barrlType)
            : base(g, x, y, s, pass, hlth, content, mon, fore)
        {
            rec = new Rectangle(x, y - 159, 83, 159);
            vaseType = barrlType;
            vitalRec = rec;
        }

        public Vase(Game1 g, int x, int y, Texture2D s, Boolean pass, int hlth, StoryItem story, float mon, Boolean fore, VaseType barrlType)
            : base(g, x, y, s, pass, hlth, story, mon, fore)
        {
            rec = new Rectangle(x, y - 159, 83, 159);

            vaseType = barrlType;
            vitalRec = rec;
        }


        public override Rectangle GetSourceRec()
        {
            switch (vaseType)
            {
                case VaseType.claySmall:
                    return new Rectangle(0, 0, 83, 159);
                case VaseType.clayMedium:
                    return new Rectangle(0, 159, 83, 159);
                case VaseType.clayLarge:
                    return new Rectangle(0, 318, 83, 159);
            }

            return new Rectangle();
        }


        /// <summary>
        /// 0 is hit, 1 is destroyed
        /// </summary>
        /// <param name="destroyedOrHit"></param>
        public void PlayVaseSound(int destroyedOrHit)
        {
            int hitSound;
            switch (vaseType)
            {
                case VaseType.claySmall:
                    hitSound = Game1.randomNumberGen.Next(1, 4);
                    if (destroyedOrHit == 0)
                    {
                        String soundEffectName = "object_barrel_wood_hit_0" + hitSound;
                        Sound.PlaySoundInstance(Sound.permanentSoundEffects[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                    }
                    else
                    {
                        String soundEffectName = "object_barrel_wood_destroy_0" + hitSound;
                        Sound.PlaySoundInstance(Sound.permanentSoundEffects[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                    }
                    break;
            }
        }

        public override void TakeHit(int damage = 1)
        {
            base.TakeHit(damage);

            PlayVaseSound(0);
        }

        public override void Update()
        {
            if (!finished)
            {
                vitalRec = rec;

                if (health <= 0 && finished == false)
                {
                    PlayVaseSound(1);
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
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
    public class Barrel : BreakableObject
    {
        //int barrelType;

        public enum BarrelType
        {
            WoodenRight, WoodenLeft, MetalRadioactive, MetalLabel, MetalBlank,
            TimBarrel,
            ScienceBarrel, ScienceFlask, ScienceTube, ScienceJar,
            pyramidBirdJar, pyramidPitcher, pyramidUrn
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
                case BarrelType.MetalRadioactive:
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
                case BarrelType.pyramidBirdJar:
                    return new Rectangle(210, 0, 105, 155);
                case BarrelType.pyramidUrn:
                    return new Rectangle(210, 155, 105, 155);
                case BarrelType.pyramidPitcher:
                    return new Rectangle(210, 310, 105, 155);
            }

            return new Rectangle();
        }


        /// <summary>
        /// 0 is hit, 1 is destroyed
        /// </summary>
        /// <param name="destroyedOrHit"></param>
        public void PlayBarrelSound(int destroyedOrHit)
        {
            int hitSound;
            switch (barrelType)
            {
                case BarrelType.WoodenLeft:
                case BarrelType.WoodenRight:
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
                case BarrelType.MetalBlank:
                case BarrelType.MetalLabel:
                case BarrelType.MetalRadioactive:
                    hitSound = Game1.randomNumberGen.Next(1, 4);

                    if (destroyedOrHit == 0)
                    {
                        String soundEffectName = "object_barrel_metal_hit_0" + hitSound;
                        Sound.PlaySoundInstance(Sound.permanentSoundEffects[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                    }

                    else
                    {
                        String soundEffectName = "object_barrel_metal_destroy_0" + hitSound;
                        Sound.PlaySoundInstance(Sound.permanentSoundEffects[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                    }

                    break;
                case BarrelType.ScienceBarrel:
                case BarrelType.ScienceFlask:
                case BarrelType.ScienceJar:
                case BarrelType.ScienceTube:
                    hitSound = Game1.randomNumberGen.Next(1, 4);

                    if (destroyedOrHit == 0)
                    {
                        String soundEffectName = "object_barrel_science_hit_0" + hitSound;
                        Sound.PlaySoundInstance(Sound.mapZoneSoundEffects[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                    }
                    else
                    {
                        String soundEffectName = "object_barrel_science_destroy_0" + hitSound;
                        Sound.PlaySoundInstance(Sound.mapZoneSoundEffects[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                    }
                    break;
                case BarrelType.TimBarrel:
                    if (GorillaTim.gorillaSounds != null)
                    {
                        if (destroyedOrHit == 0)
                        {
                            String soundEffectName = "object_tim_fight_barrel_hit";
                            Sound.PlaySoundInstance(GorillaTim.gorillaSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                        }
                        else
                        {
                            String soundEffectName = "object_tim_fight_barrel_destroy";
                            Sound.PlaySoundInstance(GorillaTim.gorillaSounds[soundEffectName], soundEffectName, false, rec.Center.X, rec.Center.Y, 600, 500, 2000);
                        }
                    }
                    break;
            }
        }

        public override void TakeHit(int damage = 1)
        {
            base.TakeHit(damage);

            PlayBarrelSound(0);
        }

        public override void Update()
        {
            if (!finished)
            {
                vitalRec = rec;

                if (health <= 0 && finished == false)
                {
                    PlayBarrelSound(1);
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
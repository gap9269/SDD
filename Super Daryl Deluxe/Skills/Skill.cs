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

    public class AttackType
    {
        public enum AttackTypes
        {
            none,
            Fire,
            Ice,
            Lightning,
            Wind,
            Water,
            Blunt,
            Cut,
            Honors
        }

        public enum RangedOrMelee
        {
            none,
            Melee,
            Ranged
        }
    }

    public class Skill
    {

        protected AttackType.AttackTypes skillType;
        protected AttackType.RangedOrMelee rangedOrMelee;
        protected float damage; //% of player's strength
        protected int animationLength;
        protected int skillRank;
        protected int levelToUse;
        protected int experience;
        protected int experienceUntilLevel;
        public int currentCooldown;
        protected int fullCooldown;
        public bool canUse;
        protected Texture2D animationSheet;
        protected KeyboardState current;
        protected KeyboardState last;
        protected Player player;
        protected List<Enemy> enemies;
        protected Boss currentBoss;
        protected List<Platform> platforms;
        protected String name;
        protected Texture2D icon;
        protected bool equipped;
        protected bool justPressed;
        protected String description;
        protected int hitPauseTime;
        protected int moveFrame;
        protected Game1 game;
        protected int costToBuy;
        protected Dictionary<String, SoundEffect> skillHitSounds;
        protected Dictionary<String, SoundEffect> skillUseSounds;

        protected List<InteractiveObject> interactiveObjectsInMap;
        protected int frameDelay;

        protected List<int> playerLevelRequiredToLevel;

        static Random randomNumberGenerator;
        protected int hitSoundsLeftThisAttack = 2; //Decrease this number by 1 each time an attack is landed, and reset on Use()

        protected int maxHit;
        protected int hitThisTime = 0;
        protected List<Enemy> enemiesHitThisAttack;
        protected List<Boss> bossesHitThisAttack;

        protected int stunnedThisTime = 0;
        protected List<Enemy> enemiesStunnedThisAttack;
        protected List<Boss> bossesStunnedThisAttack;

        protected List<InteractiveObject> interactiveObjectsThisAttack;
        protected Color skillBarColor;

        protected ContentManager content;

        //Charge skills
        protected int chargeTime;
        protected Keys useKey;
        protected Boolean released = false;

        protected List<Boolean> useNext;
        protected Boolean holdToUse;
        protected Boolean canUseInAir = true;

        public int LevelToUse { get { return levelToUse; } }
        public Texture2D Icon { get { return icon; } set { icon = value; } }
        public bool Equipped { get { return equipped; } set { equipped = value; } }
        public String Name { get { return name; } set { name = value; } }
        public String Description { get { return description; } set { description = value; } }
        public int AnimationLength { get { return animationLength; } set { animationLength = value; } }
        public int SkillRank { get { return skillRank; } set { skillRank = value; } }
        public float Damage { get { return damage; } set { damage = value; } }
        public int Experience { get { return experience; } set { experience = value; } }
        public int ExperienceUntilLevel { get { return experienceUntilLevel; } set { experienceUntilLevel = value; } }
        public int FullCooldown { get { return fullCooldown; } set { fullCooldown = value; } }
        public List<Boolean> UseNext { get { return useNext; } set { useNext = value; } }
        public Boolean HoldToUse { get { return holdToUse; } }
        public Boolean CanUseInAir { get { return canUseInAir; } }
        public int CostToBuy { get { return costToBuy; } set { costToBuy = value; } }
        public List<Enemy> EnemiesHitThisAttack { get { return enemiesHitThisAttack; } }
        public Color SkillBarColor { get { return skillBarColor; } }
        public List<int> PlayerLevelsRequiredToLevel { get { return playerLevelRequiredToLevel; } set { playerLevelRequiredToLevel = value; } }
        public AttackType.AttackTypes SkillType { get { return skillType; } }
        public AttackType.RangedOrMelee RangedOrMelee { get { return rangedOrMelee; } }

        public Skill() { }

        public Skill(Texture2D animSheet, Player play, Texture2D ico, Boolean hold)
        {
            icon = ico;
            animationSheet = animSheet;
            player = play;
            equipped = false;
            justPressed = false;

            useNext = new List<bool>();
            useNext.Add(false);
            useNext.Add(false);
            useNext.Add(false);
            useNext.Add(false);
            useNext.Add(false);
            useNext.Add(false);

            holdToUse = hold;
            enemiesHitThisAttack = new List<Enemy>();
            bossesHitThisAttack = new List<Boss>();
            bossesStunnedThisAttack = new List<Boss>();
            enemiesStunnedThisAttack = new List<Enemy>();

            interactiveObjectsThisAttack = new List<InteractiveObject>();

            skillBarColor = new Color(255, 255, 255);
            playerLevelRequiredToLevel = new List<int>();

            content = new ContentManager(Game1.g.Services);
            content.RootDirectory = "Content";

            randomNumberGenerator = new Random();
        }

        public virtual void Use(Game1 game, Keys key)
        {
            this.game = game;
            platforms = game.CurrentChapter.CurrentMap.Platforms;
            enemies =  game.CurrentChapter.CurrentMap.EnemiesInMap;
            currentBoss = game.CurrentChapter.CurrentBoss;
            interactiveObjectsInMap = game.CurrentChapter.CurrentMap.InteractiveObjects;

            hitSoundsLeftThisAttack = 2;

            last = current;
            current = Keyboard.GetState();
        }

        public virtual Rectangle GetSourceRec()
        {
            return new Rectangle();
        }

        public virtual void StayInAir()
        {
            if (animationLength > 0)
            {
                player.AttackFloating = true;
                player.VelocityY = 0;
            }
            else
                player.AttackFloating = false;
        }

        //--Make this draw the entire character's skill spritesheet for each specific skill
        public virtual void Draw(SpriteBatch s)
        {
        }

        public virtual void Update()
        {
            //DELETE THIS FOR A BETTER WAY OF LOADING SOUNDS PLS OMG
            if (skillHitSounds == null)
            {
                skillHitSounds = ContentLoader.LoadSoundContent(content, "Sound\\Skills\\" + name + "\\HitSounds");
                skillUseSounds = ContentLoader.LoadSoundContent(content, "Sound\\Skills\\" + name + "\\UseSounds");
            }

            //--Decrease the timer, cooldown, and animationLength every frame
            if (animationLength > -1)
                animationLength--;

            if (currentCooldown > 0)
                currentCooldown--;

            //--Once the cooldown is over, make it so you can use it again
            if (currentCooldown <= 0)
            {
                canUse = true;
                released = false;
                chargeTime = 0;
            }

            if (experience >= experienceUntilLevel)
            {
                skillRank++;
                ApplyLevelUp();

                Game1.currentChapter.HUD.SkillsHidden = false;
            }
        }

        public void PlayRandomHitSound()
        {
            if (hitSoundsLeftThisAttack > 0)
            {
                hitSoundsLeftThisAttack--;

                skillHitSounds.ElementAt(randomNumberGenerator.Next(skillHitSounds.Count)).Value.CreateInstance().Play();
            }
        }

        public void PlayRandomUseSound(int startingIndex = 0, int endingExclusiveIndex = 0)
        {
            if (endingExclusiveIndex == 0)
                endingExclusiveIndex = skillUseSounds.Count;

            skillUseSounds.ElementAt(randomNumberGenerator.Next(startingIndex, endingExclusiveIndex)).Value.CreateInstance().Play();
        }

        public virtual void ApplyLevelUp()
        {
            Chapter.effectsManager.AddSkillLevelUp(skillBarColor, name);
        }

        public virtual void StunEnemy(Rectangle attackRec, int time)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                //--If the skill's attack hits the enemy vitals
                if (attackRec.Intersects(enemies[i].VitalRec) && enemies[i].Respawning == false)
                {
                    enemies[i].Stun(time);
                    stunnedThisTime++;
                    enemiesStunnedThisAttack.Add(enemies[i]);
                }

                if (stunnedThisTime == maxHit && maxHit > 0)
                    break;
            }

            //--If the skill's attack hits the enemy vitals
            if (game.CurrentChapter.BossFight && attackRec.Intersects(currentBoss.VitalRec))
            {
                currentBoss.Stun(time);
                bossesStunnedThisAttack.Add(currentBoss);
            }
        }


        public virtual void CheckFiniteCollisions(Rectangle attackRec, float damage, Vector2 kbvel, int shakeTime, int shakeMag)
        {
            damage *= player.Strength;

            //--Bosses
            if (game.CurrentChapter.BossFight && attackRec.Intersects(currentBoss.VitalRec) && !bossesHitThisAttack.Contains(currentBoss))
            {
                float kbX = 0;

                if (shakeTime > 0)
                {
                    Game1.camera.ShakeCamera(shakeTime, shakeMag);
                    MyGamePad.SetRumble(shakeTime, (float)((float)shakeMag / 100f) * 10f);
                }

                //--Knock them back
                if (player.VitalRec.Center.X < currentBoss.VitalRec.X)
                    kbX = Math.Abs(kbvel.X);
                else if (player.VitalRec.Center.X > currentBoss.VitalRec.X)
                {
                    kbX = -(kbvel.X); //Set the x value equal to the negative of the vector2.X, but dont change the vector2
                }

                Chapter.effectsManager.AddDamageFXForSkill(10, Rectangle.Intersect(attackRec, currentBoss.VitalRec), name);
                PlayRandomHitSound();
                currentBoss.TakeHit((int)damage, new Vector2(kbX, kbvel.Y), Rectangle.Intersect(attackRec, currentBoss.VitalRec));
                currentBoss.HitPauseTimer = hitPauseTime;
                player.HitPauseTimer = hitPauseTime;
                bossesHitThisAttack.Add(currentBoss);
            }

            for (int i = 0; i < interactiveObjectsInMap.Count; i++)
            {
                //--If the skill's attack hits the enemy vitals
                if (attackRec.Intersects(interactiveObjectsInMap[i].VitalRec) && !interactiveObjectsThisAttack.Contains(interactiveObjectsInMap[i]) && interactiveObjectsInMap[i].Finished == false)
                {
                    if (shakeTime > 0)
                    {
                        Game1.camera.ShakeCamera(shakeTime, shakeMag);
                        MyGamePad.SetRumble(shakeTime, (float)((float)shakeMag / 100f) * 10f);
                    }

                    Chapter.effectsManager.AddDamageFXForSkill(10, Rectangle.Intersect(attackRec, interactiveObjectsInMap[i].VitalRec), name);
                    PlayRandomHitSound();
                    interactiveObjectsInMap[i].TakeHit();
                    interactiveObjectsThisAttack.Add(interactiveObjectsInMap[i]);
                }
            }

            //--For every enemy
            for (int i = 0; i < enemies.Count; i++)
            {
                float kbX = 0;

                //--If the skill's attack hits the enemy vitals
                if (attackRec.Intersects(enemies[i].VitalRec) && !enemiesHitThisAttack.Contains(enemies[i]) && enemies[i].CanBeHit && enemies[i].Respawning == false)
                {
                    //--Shake camera
                    if (shakeTime > 0)
                    {
                        Game1.camera.ShakeCamera(shakeTime, shakeMag);
                        MyGamePad.SetRumble(shakeTime, (float)((float)shakeMag / 100f) * 10f);
                    }

                    //--Knock them back
                    if (player.VitalRec.Center.X < enemies[i].VitalRec.X)
                        kbX = Math.Abs(kbvel.X);
                    else if (player.VitalRec.Center.X > enemies[i].VitalRec.X)
                    {
                        kbX = -(kbvel.X); //Set the x value equal to the negative of the vector2.X, but dont change the vector2
                    }

                    Chapter.effectsManager.AddDamageFXForSkill(10, Rectangle.Intersect(attackRec, enemies[i].VitalRec), name);
                    PlayRandomHitSound();
                    enemies[i].TakeHit((int)damage, new Vector2(kbX, kbvel.Y), Rectangle.Intersect(attackRec, enemies[i].VitalRec), SkillType, RangedOrMelee);
                    enemies[i].HitPauseTimer = hitPauseTime;
                    player.HitPauseTimer = hitPauseTime;
                    hitThisTime++;
                    enemiesHitThisAttack.Add(enemies[i]);
                }

                if (hitThisTime == maxHit && maxHit > 0)
                    break;
            }
        }

        public virtual void CheckCollisions(Rectangle attackRec, float damage, Vector2 kbvel, int shakeTime, int shakeMag)
        {
            damage *= player.Strength;

            //--For every enemy
            for (int i = 0; i < enemies.Count; i++)
            {
                //--This is here because the KB vector has to be the same for every monster hit, so you
                //--have to change a different variable and not the original vector. Changing the vector2 for every enemy
                //--hit was making it so every other enemy would fly in the opposite direction
                float kbX = 0;

                //--If the skill's attack hits the enemy vitals
                if (attackRec.Intersects(enemies[i].VitalRec) && enemies[i].CanBeHit && enemies[i].Respawning == false)
                {
                    if (shakeTime > 0)
                    {
                        Game1.camera.ShakeCamera(shakeTime, shakeMag);
                        MyGamePad.SetRumble(shakeTime, (float)((float)shakeMag / 100f) * 10f);
                    }

                    //--Knock them back
                    if (player.VitalRec.Center.X < enemies[i].VitalRec.X)
                        kbX = Math.Abs(kbvel.X);
                    else if (player.VitalRec.Center.X > enemies[i].VitalRec.X)
                    {
                        kbX = -(kbvel.X); //Set the x value equal to the negative of the vector2.X, but dont change the vector2
                    }

                    Chapter.effectsManager.AddDamageFXForSkill(10, Rectangle.Intersect(attackRec, enemies[i].VitalRec), name);
                    PlayRandomHitSound();
                    enemies[i].TakeHit((int)damage, new Vector2(kbX, kbvel.Y), Rectangle.Intersect(attackRec, enemies[i].VitalRec), SkillType, RangedOrMelee);
                    enemies[i].HitPauseTimer = hitPauseTime;
                    player.HitPauseTimer = hitPauseTime;
                }
            }

            for (int i = 0; i < interactiveObjectsInMap.Count; i++)
            {
                //--If the skill's attack hits the enemy vitals
                if (attackRec.Intersects(interactiveObjectsInMap[i].VitalRec) && interactiveObjectsInMap[i].Finished == false)
                {
                    if (shakeTime > 0)
                    {
                        Game1.camera.ShakeCamera(shakeTime, shakeMag);
                        MyGamePad.SetRumble(shakeTime, (float)((float)shakeMag / 100f) * 10f);
                    }


                    Chapter.effectsManager.AddDamageFXForSkill(10, Rectangle.Intersect(attackRec, interactiveObjectsInMap[i].VitalRec), name);
                    PlayRandomHitSound();
                    interactiveObjectsInMap[i].TakeHit();
                }
            }

            if (game.CurrentChapter.BossFight && attackRec.Intersects(currentBoss.VitalRec))
            {
                float kbX = 0;

                if (shakeTime > 0)
                {
                    Game1.camera.ShakeCamera(shakeTime, shakeMag);
                    MyGamePad.SetRumble(shakeTime, (float)((float)shakeMag / 100f) * 10f);
                }

                //--Knock them back
                if (player.VitalRec.Center.X < currentBoss.VitalRec.X)
                    kbX = Math.Abs(kbvel.X);
                else if (player.VitalRec.Center.X > currentBoss.VitalRec.X)
                {
                    kbX = -(kbvel.X); //Set the x value equal to the negative of the vector2.X, but dont change the vector2
                }

                Chapter.effectsManager.AddDamageFXForSkill(10, Rectangle.Intersect(attackRec, currentBoss.VitalRec), name);
                PlayRandomHitSound();
                currentBoss.TakeHit((int)damage, new Vector2(kbX, kbvel.Y), Rectangle.Intersect(attackRec, currentBoss.VitalRec));
                currentBoss.HitPauseTimer = hitPauseTime;
                player.HitPauseTimer = hitPauseTime;
            }
        }
    }
}
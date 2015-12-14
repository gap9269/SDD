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
    public class TreasureChest
    {
        Object contents;
        float money;
        Rectangle rec;
        Texture2D spriteSheet;
        int timeToOpen;
        Boolean opened = false;
        Boolean empty = false;
        Boolean opening = false;
        Player player;
        KeyboardState last;
        KeyboardState current;
        Rectangle openBar;
        float openBarWidth;
        protected int animationTime = 0;
        protected int maxAnimationTime = 20;
        MapClass currentMap;
        StoryItem sItem;
        int moveFrame = 1; //1, 2, or 3
        int frameTimer;
        static Random ranTime;
        static Random ranNum;
        String skillName;

        float glowAlpha = 0f;
        Boolean increaseGlow = true;

        int popUpMoveframe;
        int flashTimer = 3;
        int popUpTimer;
        public Boolean popUpFinised = false;

        public static SoundEffectInstance object_chest_open, object_chest_unlock;

        public Boolean Opening { get { return opening; } set { opening = value; } }
        public Boolean Opened { get { return opened; } set { opened = value; } }
        public Boolean Empty { get { return empty; } set { empty = value; } }
        public Rectangle Rec { get { return rec; } set { rec = value; } }
        public Rectangle OpenBar { get { return openBar; } set { openBar = value; } }

        public int RecY { get { return rec.Y; } set { rec.Y = value; } }
        public int RecX { get { return rec.X; } set { rec.X = value; } }

        public Texture2D Spritesheet { get { return spriteSheet; } set { spriteSheet = value; } }
        public TreasureChest(Texture2D sprite, int x, int platY, Player p, float mon, Object con, MapClass m)
        {
            currentMap = m;
            spriteSheet = sprite;
            rec = new Rectangle(x, platY - 180, 234, 190);
            timeToOpen = 180;
            player = p;
            money = mon;
            contents = con;
            openBar = new Rectangle(rec.X + rec.Width / 2 - 50, rec.Y, 0, 20);
            ranTime = new Random();
            ranNum = new Random();
        }

        public TreasureChest(Texture2D sprite, int x, int platY, Player p, float mon, String skill, MapClass m)
        {
            currentMap = m;
            spriteSheet = sprite;
            rec = new Rectangle(x, platY - 180, 234, 190);
            timeToOpen = 180;
            player = p;
            money = mon;
            skillName = skill;
            openBar = new Rectangle(rec.X + rec.Width / 2 - 50, rec.Y, 0, 20);
            ranTime = new Random();
            ranNum = new Random();
            contents = new object();
        }

        public Rectangle GetSourceRectangle()
        {
            //THIS IS FOR THE TUTORIAL CHEST IN THE DEMO. WIDTH IS AN AWFUL WAY TO CHOOSE THIS, BUT IT WORKS
            if (spriteSheet.Width == 440)
            {
                if (opened)
                    return new Rectangle(220, 0, 220, 199);
                else
                    return new Rectangle(0, 0, 220, 199);
            }

            if (opened)
                return new Rectangle(0, 0, 234, 190);
            else if (Opening)
                return new Rectangle(936, 0, 234, 190);
            else
                return new Rectangle((234 * moveFrame), 0, 234, 190);
        }

        public void Update()
        {
            last = current;
            current = Keyboard.GetState();

            #region Chest glow
            if (!opened)
            {
                if (increaseGlow)
                {
                    glowAlpha += .02f;

                    if (opening)
                    {
                        glowAlpha += .03f;
                    }

                    if (glowAlpha > 1f)
                        increaseGlow = false;
                }
                else
                {
                    glowAlpha -= .02f;

                    if (opening)
                    {
                        glowAlpha -= .03f;
                    }

                    if (glowAlpha < 0f)
                        increaseGlow = true;
                }
            }
            #endregion

            //Animate the chest
            if (!opened && !opening)
            {
                frameTimer--;

                if (frameTimer <= 0)
                {
                    moveFrame = ranNum.Next(1, 4);

                    if (moveFrame == 1)
                        frameTimer = 5;
                    else
                        frameTimer = ranTime.Next(20, 120);
                }
            }

            if (!opened)
            {
                if (((last.IsKeyDown(Keys.F) && current.IsKeyDown(Keys.F)) || MyGamePad.currentState.Buttons.LeftShoulder == ButtonState.Pressed) && nearPlayer() && player.playerState == Player.PlayerState.standing)
                {
                    if (!opening)
                        Sound.PlaySoundInstance(object_chest_unlock, Game1.GetFileName(() => object_chest_unlock));

                    openBar.X = rec.X + rec.Width / 2 - 50;
                    openBar.Y = rec.Y;
                    opening = true;
                    OpenChest();
                    openBarWidth += 100f / 60f;
                    openBar.Width = (int)openBarWidth;
                }
                else
                {
                    if (timeToOpen != 60 && !opened)
                        timeToOpen = 60;

                    if (opening)
                    {
                        object_chest_unlock.Stop();
                        openBarWidth = 0;
                        openBar.Width = 0;
                        opening = false;
                    }
                }
            }
            else if (opened && !empty)
            {
                opening = false;
                player.Money += money;

                Sound.PlaySoundInstance(object_chest_open, Game1.GetFileName(() => object_chest_open));


                if (contents is Equipment)
                {
                    Equipment eq = contents as Equipment;

                    if (eq is Weapon)
                    {
                        player.AddWeaponToInventory(eq as Weapon);
                    }
                    if (eq is Hat)
                    {
                        player.AddHatToInventory(eq as Hat);
                    }
                    if (eq is Outfit)
                    {
                        player.AddShirtToInventory(eq as Outfit);
                    }
                    if (eq is Accessory)
                    {
                        player.AddAccessoryToInventory(eq as Accessory);
                    }
                    //Chapter.effectsManager.AddFoundItem("a " + eq.Name);
                }
                else if (contents is StoryItem)
                {
                    sItem = contents as StoryItem;
                    if (player.StoryItems.ContainsKey(sItem.Name))
                        player.StoryItems[sItem.Name]++;
                    else
                        player.StoryItems.Add(sItem.Name, 1);

                    sItem.PickedUp = true;
                    //Chapter.effectsManager.AddFoundItem(sItem.PickUpName);
                }
                else if (contents is Collectible)
                {
                    if (contents is Textbook)
                        player.Textbooks++;
                    else if (contents is BronzeKey)
                        player.BronzeKeys++;
                    else if (contents is SilverKey)
                        player.SilverKeys++;
                    else if (contents is GoldKey)
                        player.GoldKeys++;

                    //Chapter.effectsManager.AddFoundItem("a Physics Textbook");
                }
                else if (contents is EnemyDrop)
                {
                    player.AddLoot((contents as EnemyDrop).Name, 1);
                }
                else if (skillName != "" && skillName != null)
                {
                    player.LearnedSkills.Add(SkillManager.AllSkills[skillName]);

                    // Chapter.effectsManager.AddFoundItem("a skill page: " + "\"" + skillName + "\"");
                }

                empty = true;
            }
        }

        public bool nearPlayer()
        {
            Point distanceFromPlayer = new Point(Math.Abs(player.VitalRec.Center.X - rec.Center.X),
Math.Abs(player.VitalRec.Center.Y - rec.Center.Y));


            if (distanceFromPlayer.X < 150 && distanceFromPlayer.Y < 100)
                return true;

            return false;
        }

        public void OpenChest()
        {
            if (timeToOpen > 0)
                timeToOpen--;
            if (timeToOpen <= 0)
                opened = true;
            
        }

        public void Draw(SpriteBatch s)
        {

            
            if (!opened)
                s.Draw(spriteSheet, rec, new Rectangle((234 * 5), 0, 234, 190), Color.White * glowAlpha);

            s.Draw(spriteSheet, rec, GetSourceRectangle(), Color.White);


            if (opened && flashTimer < 400 && !popUpFinised)
            {

                flashTimer--;
                popUpTimer++;
                if (flashTimer <= 0)
                {
                    flashTimer = 2;
                    popUpMoveframe++;

                }

                if (popUpTimer >= 220)
                    popUpFinised = true;


                s.Draw(Game1.treasureChestFlash, new Rectangle(rec.X + rec.Width / 2 - 250, rec.Y + rec.Height / 2 - 275, 550, 550), new Rectangle(391 * popUpMoveframe, 0, 391, 391), Color.White);

                if (popUpMoveframe > 4)
                {
                    s.Draw(Game1.treasureChestBox, new Rectangle(rec.X + 10, rec.Y - 90, Game1.treasureChestBox.Width, Game1.treasureChestBox.Height), Color.White);

                    //If there is money in the chest
                    if (money > 0)
                    {
                        if (contents != null || (skillName != "" && skillName != null))
                        {
                            s.DrawString(Game1.twConQuestHudInfo, money.ToString("N2"), new Vector2(rec.X + 60, rec.Y - 33), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Rectangle(rec.X + 36, rec.Y - 33, 20, 20), Color.White);
                        }
                        else
                        {
                            s.DrawString(Game1.twConQuestHudInfo, money.ToString("N2"), new Vector2(rec.X + 60, rec.Y - 60), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallMoneyIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                        }
                    }

                    //If there is an item in the chest, draw the name and type-icon
                    if (contents != null)
                    {
                        if (contents is Equipment)
                        {
                            Equipment eq = contents as Equipment;
                            s.DrawString(Game1.font, eq.Name, new Vector2(rec.X + 60, rec.Y - 60), Color.Black);

                            if (eq is Weapon)
                                s.Draw(Game1.smallTypeIcons["smallWeaponIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                            if (eq is Hat)
                                s.Draw(Game1.smallTypeIcons["smallHatIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                            if (eq is Outfit)
                                s.Draw(Game1.smallTypeIcons["smallShirtIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                            if (eq is Accessory)
                                s.Draw(Game1.smallTypeIcons["smallAccessoryIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                        }
                        else if (contents is StoryItem)
                        {
                            sItem = contents as StoryItem;
                            s.DrawString(Game1.twConQuestHudInfo, sItem.PickUpName, new Vector2(rec.X + 60, rec.Y - 60), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallStoryItemIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                        }
                        else if (contents is Collectible)
                        {
                            if (contents is Textbook)
                            {
                                s.DrawString(Game1.font, "Textbook", new Vector2(rec.X + 60, rec.Y - 60), Color.Black);
                                s.Draw(Game1.smallTypeIcons["smallTextbookIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                            }
                            else if (contents is BronzeKey)
                            {
                                s.DrawString(Game1.twConQuestHudInfo, "Bronze Key", new Vector2(rec.X + 60, rec.Y - 60), Color.Black);
                                s.Draw(Game1.smallTypeIcons["smallBronzeKeyIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                            }
                            else if (contents is SilverKey)
                            {
                                s.DrawString(Game1.font, "Silver Key", new Vector2(rec.X + 60, rec.Y - 60), Color.Black);
                                s.Draw(Game1.smallTypeIcons["smallSilverKeyIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                            }
                            else if (contents is GoldKey)
                            {
                                s.DrawString(Game1.font, "Gold Key", new Vector2(rec.X + 60, rec.Y - 60), Color.Black);
                                s.Draw(Game1.smallTypeIcons["smallGoldKeyIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                            }
                        }
                        else if (contents is EnemyDrop)
                        {
                            EnemyDrop item = contents as EnemyDrop;
                            s.DrawString(Game1.twConQuestHudInfo, item.Name, new Vector2(rec.X + 60, rec.Y - 60), Color.Black);
                            s.Draw(Game1.smallTypeIcons["smallStoryItemIcon"], new Rectangle(rec.X + 36, rec.Y - 60, 20, 20), Color.White);
                        }
                    }
                    //If the object in the chest is a skill page
                    else if (skillName != "" && skillName != null)
                    {
                        s.DrawString(Game1.font, skillName, new Vector2(rec.X + 60, rec.Y - 60), Color.Black);
                    }
                }
            }
                Rectangle fRec = new Rectangle(rec.X + rec.Width / 2 - 43 / 2, rec.Y - 50, 43, 65);

                if (opening)
                {
                    Game1.OutlineFont(Game1.font, s, "Opening...", 1, rec.X + rec.Width / 2 - 43, rec.Y - 25, Color.Black, Color.White);
                    s.Draw(Game1.emptyBox, new Rectangle(rec.X + rec.Width / 2 - 50, rec.Y, 100, 20), Color.DarkGray * .8f);
                    s.Draw(Game1.emptyBox, openBar, new Color(241, 107, 79));

                    if (Chapter.effectsManager.fButtonRecs.Contains(fRec))
                        Chapter.effectsManager.fButtonRecs.Remove(fRec);
                }

                else if (opened)
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(fRec))
                        Chapter.effectsManager.fButtonRecs.Remove(fRec);
                }
                else
                {
                    if (nearPlayer())
                    {

                        if (!Chapter.effectsManager.fButtonRecs.Contains(fRec))
                            Chapter.effectsManager.AddFButton(fRec);
                    }
                    else
                    {
                        if (Chapter.effectsManager.fButtonRecs.Contains(fRec))
                            Chapter.effectsManager.fButtonRecs.Remove(fRec);
                    }
                }
            }

        }
    }

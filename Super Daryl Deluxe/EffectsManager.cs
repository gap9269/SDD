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
    public class QuestReceivedNotification : Notification
    {

        int questReceivedTimer;
        int sideQuestReceivedTimer;

        //Constructor, sets a timer depending on what type of quest is accepted
        public QuestReceivedNotification(bool story)
        {
            sideQuestReceivedTimer = 0;
            questReceivedTimer = 0;

            if (story)
            {
                questReceivedTimer = 120;
            }
            else
            {
                sideQuestReceivedTimer = 120;
            }
        }

        //--Decreases timer
        public override void Update()
        {
            //Story quest
            if (questReceivedTimer > 0)
            {
                questReceivedTimer--;

                //Finish the notification when the timer hits 0
                if (questReceivedTimer == 0)
                    finished = true;
            }


            //Side quest
            if (sideQuestReceivedTimer > 0)
            {
                sideQuestReceivedTimer--;

                //Finish the notification when the timer hits 0
                if (sideQuestReceivedTimer == 0)
                    finished = true;
            }
        }

        //Draws the appropriate graphic depending on type of quest
        public override void Draw(SpriteBatch s)
        {
            if (sideQuestReceivedTimer > 0)
                s.Draw(Game1.sideQuestReceived, new Rectangle(449, 127, Game1.sideQuestReceived.Width, Game1.sideQuestReceived.Height), Color.White);
            else if (questReceivedTimer > 0)
                s.Draw(Game1.storyQuestReceived, new Rectangle(449, 127, Game1.sideQuestReceived.Width, Game1.sideQuestReceived.Height), Color.White);
        }
    }

    public class QuestCompleteNotification : Notification
    {
        int questTimer;
        float questAlpha = 0f;
        List<Texture2D> rewards;
        Quest quest;

        float mon;
        int exp, karm;

        Boolean story;

        List<Object> questRewardObjects;

        List<Button> rewardBoxes;

        DescriptionBoxManager descriptionBox;

        //Constructor takes in a quest and saves its rewards
        public QuestCompleteNotification(Quest q)
        {
            rewards = new List<Texture2D>();
            rewardBoxes = new List<Button>();

            for (int i = 0; i < 5; i++)
            {
                rewardBoxes.Add(new Button(new Rectangle(456 + (i * 80), 353, 70, 70)));
            }
            descriptionBox = Game1.descriptionBoxManager;
            quest = q;
            story = quest.StoryQuest;
            //Save rewards
            for (int i = 0; i < q.RewardObjects.Count; i++)
            {
                if (q.RewardObjects[i] is Equipment)
                {
                    rewards.Add(Game1.equipmentTextures[(q.RewardObjects[i] as Equipment).Name]);
                }
                if (q.RewardObjects[i] is Collectible)
                {
                    if(q.RewardObjects[i] is Textbook)
                        rewards.Add(Game1.equipmentTextures["Textbook"]);
                }
            }

            questRewardObjects = q.RewardObjects;
            questTimer = 300;
        }

        //Constructor takes in rewards. Only for map signs so far
        public QuestCompleteNotification(List<Object> con, Boolean isStory)
        {
            rewards = new List<Texture2D>();
            questRewardObjects = con;
            story = isStory;
            rewardBoxes = new List<Button>();

            for (int i = 0; i < 5; i++)
            {
                rewardBoxes.Add(new Button(new Rectangle(456 + (i * 80), 353, 70, 70)));
            }
            descriptionBox = Game1.descriptionBoxManager;

            //Save rewards
            for (int i = 0; i < con.Count; i++)
            {
                if (con[i] is Equipment)
                {
                    rewards.Add(Game1.equipmentTextures[(con[i] as Equipment).Name]);

                    if (con[i] is Money)
                        mon = (con[i] as Money).Amount;

                    if (con[i] is Karma)
                        karm = (con[i] as Karma).Amount;

                    if (con[i] is Experience)
                        exp = (con[i] as Experience).Amount;
                }
                if (con[i] is Collectible)
                {
                    if (con[i] is Textbook)
                        rewards.Add(Game1.equipmentTextures["Textbook"]);
                }
                if (con[i] is StoryItem)
                {
                    rewards.Add(Game1.storyItems[(con[i] as StoryItem).Name]);
                }
            }
            questTimer = 300;
        }

        public override void Update()
        {
            //Fade the graphic in
            if (questTimer >= 0)
            {
                if (questTimer == 300)
                {
                    questAlpha = 0f;
                }
                if (questAlpha < 1)
                {
                    questAlpha += (1f / 100f);
                }

                questTimer--;

            }

            if (questTimer <= 0 && !(Cursor.last.CursorRec.Intersects(new Rectangle(449, 127, Game1.storyQuestComplete.Width, Game1.storyQuestComplete.Height)) && Cursor.last.active))
            {
                rewards.Clear();
                questAlpha = 0f;
                finished = true;
            }
        }

        public void DrawRewardDescriptionBoxes()
        {
            for (int i = 0; i < rewardBoxes.Count; i++)
            {
                if (rewardBoxes[i].IsOver() && rewards.Count > i)
                {
                    if (questRewardObjects[i] is Equipment && !(questRewardObjects[i] is Experience) && !(questRewardObjects[i] is Money) && !(questRewardObjects[i] is Karma))
                    {
                        descriptionBox.DrawEquipDescriptions(questRewardObjects[i] as Equipment, rewardBoxes[i].ButtonRec);
                    }
                    if (questRewardObjects[i] is StoryItem)
                    {
                        descriptionBox.DrawStoryItemDescriptions((questRewardObjects[i] as StoryItem).Name, rewardBoxes[i].ButtonRec);
                    }
                    if (questRewardObjects[i] is Collectible)
                    {
                        descriptionBox.DrawCollectibleDescriptions((questRewardObjects[i] as Collectible).collecName, (questRewardObjects[i] as Collectible).Description, rewardBoxes[i].ButtonRec);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            if(story)
                s.Draw(Game1.storyQuestComplete, new Rectangle(449, 127, Game1.storyQuestComplete.Width, Game1.storyQuestComplete.Height), Color.White * questAlpha);
            else
                s.Draw(Game1.sideQuestComplete, new Rectangle(449, 127, Game1.storyQuestComplete.Width, Game1.storyQuestComplete.Height), Color.White * questAlpha);

            for (int i = 0; i < rewards.Count; i++)
            {
                Rectangle temp = new Rectangle(456 + (i * 80), 353, rewards[i].Width, rewards[i].Height);

                s.Draw(rewards[i], temp, Color.White);

                if (questRewardObjects[i] is Money)
                    s.DrawString(Game1.twConQuestHudInfo, "$" + (questRewardObjects[i] as Money).Amount.ToString("N2"), new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString("$" + (questRewardObjects[i] as Money).Amount.ToString("N2")).X / 2, 428), Color.Black);
                else if (questRewardObjects[i] is Experience)
                    s.DrawString(Game1.twConQuestHudInfo, (questRewardObjects[i] as Experience).Amount.ToString(), new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString((questRewardObjects[i] as Experience).Amount.ToString()).X / 2 - 2, 428), Color.Black);
                else if (questRewardObjects[i] is Karma)
                    s.DrawString(Game1.twConQuestHudInfo, (questRewardObjects[i] as Karma).Amount.ToString(), new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString((questRewardObjects[i] as Karma).Amount.ToString()).X / 2 - 2, 428), Color.Black);
                else if (questRewardObjects[i] is Textbook)
                    s.DrawString(Game1.twConQuestHudInfo, "Textbook", new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString("Textbook").X / 2, 428), Color.Black);
                else if (questRewardObjects[i] is Weapon)
                    s.DrawString(Game1.twConQuestHudInfo, "Weapon", new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString("Weapon").X / 2 - 2, 428), Color.Black);
                else if (questRewardObjects[i] is Hat)
                    s.DrawString(Game1.twConQuestHudInfo, "Hat", new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString("Hat").X / 2, 428), Color.Black);
                else if (questRewardObjects[i] is Hoodie)
                    s.DrawString(Game1.twConQuestHudInfo, "Outfit", new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString("Outfit").X / 2, 428), Color.Black);
                else if (questRewardObjects[i] is Accessory)
                    s.DrawString(Game1.twConQuestHudInfo, "Accessory", new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString("Accessory").X / 2, 428), Color.Black);
                else if (questRewardObjects[i] is StoryItem)
                    s.DrawString(Game1.twConQuestHudInfo, "Story Item", new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString("Story Item").X / 2, 428), Color.Black);
                else if (questRewardObjects[i] is BronzeKey || questRewardObjects[i] is SilverKey || questRewardObjects[i] is GoldKey)
                    s.DrawString(Game1.twConQuestHudInfo, "Key", new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString("Key").X / 2, 428), Color.Black);
                else if (questRewardObjects[i] is Collectible)
                    s.DrawString(Game1.twConQuestHudInfo, "Collectible", new Vector2(temp.X + 35 - Game1.twConQuestHudInfo.MeasureString("Collectible").X / 2, 428), Color.Black);
            }

            DrawRewardDescriptionBoxes();
        }
    }

    public class JournalUpdateNotification : Notification
    {
        int timer;
        public int typeOfEntryAdded; //1 is synopsis, 2 is story quest, 3 is side quest
        public Quest quest;

        /// <summary>
        /// Entry type is 1 for synopsis, 2 for story quest, and 3 for side quests
        /// </summary>
        public JournalUpdateNotification(int entryType)
        {
            timer = 300;
            typeOfEntryAdded = entryType;
        }

        //Journal notification for quest entry
        public JournalUpdateNotification(int entryType, Quest q)
        {
            timer = 300;
            typeOfEntryAdded = entryType;
            quest = q;
        }

        public override void Update()
        {
            if (timer > 0)
            {
                timer--;
                if (timer == 0)
                    finished = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(Game1.journalUpdatedTexture, new Rectangle(547, 10, Game1.journalUpdatedTexture.Width, Game1.journalUpdatedTexture.Height), Color.White);
        }
    }

    public class BioUnlockNotification : Notification
    {
        int timer;
        public int typeOfEntryAdded; //1 is NPC, 2 is Monster

        /// <summary>
        ///  //1 is NPC, 2 is Monster
        /// </summary>
        public BioUnlockNotification(int entryType)
        {
            timer = 300;
            typeOfEntryAdded = entryType;
        }

        public override void Update()
        {
            if (timer > 0)
            {
                timer--;
                if (timer == 0)
                    finished = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(Game1.newBioNotification, new Rectangle(547, 10, Game1.newBioNotification.Width, Game1.newBioNotification.Height), Color.White);
        }
    }

    public class SocialRankUpNotification : Notification
    {
        float alpha = 0f;
        int timer;
        String rank, passive;
        int socialLevel;
        int frameDelay = 5;
        int moveFrame = 0;

        public SocialRankUpNotification(String r, String p, int lv)
        {
            rank = r;
            passive = p;
            socialLevel = lv;
        }

        public override void Update()
        {
            frameDelay--;
            timer++;

            if (frameDelay <= 0 && moveFrame < 14)
            {
                frameDelay = 4;
                moveFrame++;

                if (moveFrame > 14)
                {
                    moveFrame = 14;
                }
            }
            if (timer > 250)
            {
                finished = true;
            }
        }

        public override void Draw(SpriteBatch s)
        {
            s.Draw(Game1.socialRankUpTexture["SocialRankUp" + moveFrame], new Rectangle(365, 0, Game1.socialRankUpTexture.ElementAt(0).Value.Width, Game1.socialRankUpTexture.ElementAt(0).Value.Height), Color.White);

            if (moveFrame >= 4)
            {
                s.DrawString(Game1.bioPageNameFont, "Rank " + socialLevel + ": " + rank, new Vector2(635 - (Game1.bioPageNameFont.MeasureString("Rank " + socialLevel + ": " + rank).X * .63f / 2), 73), Color.White, 0, Vector2.Zero, .7f, SpriteEffects.None, 0);
                s.DrawString(Game1.twConRegularSmall, "NEW PASSIVE UNLOCKED:", new Vector2(558, 131), Color.Black, 0, Vector2.Zero, 1.05f, SpriteEffects.None, 0);
                s.DrawString(Game1.twConRegularSmall, passive, new Vector2(655 - Game1.twConRegularSmall.MeasureString(passive).X / 2, 161), new Color(255, 98, 65), 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
        }
    }

    public class Notification
    {
        protected Boolean finished = false;


        public Boolean Finished { get { return finished; } }

        public Notification() { }
        public virtual void Update()
        {
        }

        public virtual void Draw(SpriteBatch s)
        {
        }
    }

    public class JumpPoof
    {
        int frame = 0;
        int frameDelay = 5;
        Boolean facingRight;
        Rectangle rec;
        Boolean finished = false;

        public Boolean Finished { get { return finished; } }

        public JumpPoof(Rectangle r, Boolean faceRight)
        {
            facingRight = faceRight;

            rec = new Rectangle(r.X + 190, r.Y + 398 - (int)(80), (int)(364 * .35f), (int)(142 *.35f));
        }

        public void Update()
        {
            frameDelay--;

            if (frameDelay <= 0)
            {
                frame++;
                frameDelay = 5;
            }

            if (frame > 3)
                finished = true;

        }

        public void Draw(SpriteBatch s)
        {
            if (facingRight)
            {
                s.Draw(Game1.jumpPoofSprite, rec, new Rectangle(364 * frame, 0, 364, 142), Color.White);
            }
            else
            {
                s.Draw(Game1.jumpPoofSprite, rec, new Rectangle(364 * frame, 0, 364, 142), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

            }
        }
    }

    public class DustPoof
    {
        int frame = 0;
        int frameDelay = 5;
        Boolean facingRight;
        Rectangle rec;
        int startingFrame;
        Boolean sprintingPoof = false;
        Boolean finished = false;

        public Boolean Finished { get { return finished; } }

        public DustPoof(int start, Rectangle r, Boolean faceRight, Boolean sprinting)
        {
            sprintingPoof = sprinting;
            startingFrame = start;
            rec = r;
            r.Y += 37;
            facingRight = faceRight;
        }

        public void Update()
        {
            if (sprintingPoof)
            {
                frameDelay--;

                if (frameDelay <= 0)
                {
                    frame++;
                    frameDelay = 5;
                }

                if (frame > 4)
                    finished = true;
            }
            else
            {
                frameDelay--;

                if (frameDelay <= 0)
                {
                    frame++;
                    frameDelay = 5;
                }

                if (frame > 2)
                    finished = true;
            }

        }

        public void Draw(SpriteBatch s)
        {
            if (sprintingPoof)
            {
                if (facingRight)
                    s.Draw(Game1.danceSprite, rec, new Rectangle(530 + (530 * frame), 1194, 530, 398), Color.White);
                else
                    s.Draw(Game1.danceSprite, rec, new Rectangle(530 + (530 * frame), 1194, 530, 398), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                if (facingRight)
                {
                    //If it starts at the player's fifth running frame, draw the according poof
                    if (startingFrame == 5)
                    {
                        s.Draw(Game1.dustSprite, rec, new Rectangle(530 * frame, 398 * 2, 530, 398), Color.White);
                    }
                    else //Draw the other
                    {
                        s.Draw(Game1.dustSprite, rec, new Rectangle(530 * frame, 398 * 3, 530, 398), Color.White);
                    }
                }
                else
                {
                    //If it starts at the player's fifth running frame, draw the according poof
                    if (startingFrame == 5)
                    {
                        s.Draw(Game1.dustSprite, rec, new Rectangle(530 * frame, 398 * 2, 530, 398), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                    else //Draw the other
                    {
                        s.Draw(Game1.dustSprite, rec, new Rectangle(530 * frame, 398 * 3, 530, 398), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                }
            }
        }


    }

    public class EffectsManager
    {
        protected List<int> damageFXTimes;
        protected List<Rectangle> damageFXRecs;
        protected List<String> skillImpactNames;

        protected List<int> deathTimes;
        protected List<int> deathFrames;
        protected List<Rectangle> deathRecs;
        protected List<int> deathTypes; //1 is death, 2 is smoke poof

        public Queue<Notification> notificationQueue;
        public Queue<Notification> secondNotificationQueue; //This is for other notifications that can pop up alongside the first queue of notifications


        float fAlpha = 0f;
        bool alphaGoingUp = true;

        String foundItemName;
        int foundItemTimer;
        Texture2D foundItemIcon;
        float foundItemAlpha;

        public List<Rectangle> fButtonRecs;
        public List<Rectangle> foregroundFButtonRecs;

        public List<Rectangle> spaceButtonRecs;
        public List<Rectangle> foregroundSpaceButtonRecs;

        public static List<SoundEffectInstance> allCurrentSounds;

        public static Texture2D deathSpriteSheet;

        double timerStart;
        int timerIncrement;

        List<String> inGameDialogue;
        List<String> dialogueSpeakerName;
        List<String> dialogueSpeakerEmotion;
        List<int> inGameDialogueTimer;
        List<int> inGameDialogueMaxTimer;
        int dialogueState;

        List<String> announcementDialogue;
        List<int> announcementTimer;
        List<int> announcementMaxTimer;
        int announcementState;

        int lockedTimer;
        float lockedDoorMessagePosX;

        int decisionState;
        String decisionText;
        KeyboardState current;
        KeyboardState last;

        //LEVELING UP
        int levelUpFrame;
        int frameDelay = 5;
        int blinkAmount;
        int levelUpTextYDifference = 0;
        int levelUpTextTimer;
        int offsetNum;
        int levelUpBoxTimer = 0;
        float statBoxAlpha = 0f;

        //DUST POOFS
        List<DustPoof> dustPoofs;
        List<JumpPoof> jumpPoofs;

        //NOTIFICATIONS
        public Queue<Notification> NotificationQueue { get { return notificationQueue; } set { notificationQueue = value; } }

        //TOOL TIPS
        String toolTip;
        int toolTipX, toolTipY;
        Texture2D customToolTipBox;

        //Skill level up
        Color skillLevelColor = Color.White;
        int skillLevelFrame;
        int skillLevelTimer = 5;
        public int skillMessageTime;
        float skillBoxPosY = -80;
        public Color skillMessageColor;
        String skillLevelName;

        //TEXT MESSAGES
        struct TextMessage
        {
            public String sender, message;
            public Boolean showing;
        }
        TextMessage textMessage;
        public int timeUntilNextMessage;
        Button phoneButton;

        public struct SenderAndText
        {
            public String sender, message;
        }
        List<SenderAndText> SenderAndTexts;

        float phonePosX = -195;

        static Random ranText;

        public EffectsManager()
        {
            damageFXRecs = new List<Rectangle>();
            damageFXTimes = new List<int>();
            skillImpactNames = new List<string>();
            deathRecs = new List<Rectangle>();
            deathTimes = new List<int>();
            deathFrames = new List<int>();
            deathTypes = new List<int>();
            allCurrentSounds = new List<SoundEffectInstance>();
            fButtonRecs = new List<Rectangle>();
            spaceButtonRecs = new List<Rectangle>();
            foregroundSpaceButtonRecs = new List<Rectangle>();
            foregroundFButtonRecs = new List<Rectangle>();
            inGameDialogue = new List<string>();
            inGameDialogueMaxTimer = new List<int>();
            inGameDialogueTimer = new List<int>();
            dialogueSpeakerEmotion = new List<string>();
            dialogueSpeakerName = new List<string>();

            dustPoofs = new List<DustPoof>();
            jumpPoofs = new List<JumpPoof>();

            announcementDialogue = new List<string>();
            announcementTimer = new List<int>();
            announcementMaxTimer = new List<int>();

            notificationQueue = new Queue<Notification>();
            secondNotificationQueue = new Queue<Notification>();

            textMessage = new TextMessage();
            textMessage.showing = false;
            SenderAndTexts = new List<SenderAndText>();
            PopulateTextsAndSenders();
            ranText = new Random();

            timeUntilNextMessage = ranText.Next(12000, 65000);
            phoneButton = new Button(new Rectangle(0, 190, 50, 98));
        }

        public void AddSkillLevelUp(Color skillCol, String skillLevelName)
        {
            skillLevelColor = skillCol;
            skillMessageColor = skillCol;
            skillLevelFrame = 0;
            skillLevelTimer = 2;
            skillMessageTime = 160;
            skillBoxPosY = -80;
            this.skillLevelName = skillLevelName;
        }

        public void DrawSkillLevelUpEffect(SpriteBatch s)
        {
            if (skillLevelColor != Color.White)
            {
                s.Draw(Game1.skillLevelUpTexture, new Rectangle(Game1.Player.Rec.X + 110, Game1.Player.Rec.Y + 39, 300, 320), new Rectangle(2400, 0, 300, 320), skillLevelColor);
                s.Draw(Game1.skillLevelUpTexture, new Rectangle(Game1.Player.Rec.X + 110, Game1.Player.Rec.Y + 39, 300, 320), new Rectangle(skillLevelFrame * 300, 0, 300, 320), skillLevelColor);
            }
        }

        public void DrawSkillLevelUpBox(SpriteBatch s)
        {
            if (skillMessageTime > 0)
            {
                if (skillBoxPosY != 108 && skillMessageTime >= 50)
                {
                    if (skillBoxPosY < 108)
                    {
                        float distance = skillBoxPosY - 148;

                        skillBoxPosY -= 8 * (distance / 40);

                        if (skillBoxPosY > 108)
                        {
                            skillBoxPosY = 108;
                        }

                    }
                }
                else if (skillMessageTime < 50)
                {
                    if (skillBoxPosY != -80)
                    {
                        if (skillBoxPosY > -80)
                        {
                            float distance = skillBoxPosY + 120;

                            skillBoxPosY -= 8 * (distance / 40);

                            if (skillBoxPosY < -78)
                            {
                                skillBoxPosY = -80;
                            }

                        }
                    }
                }

                s.Draw(Game1.skillLevelUpBox, new Rectangle(496, (int)skillBoxPosY, Game1.skillLevelUpBox.Width, Game1.skillLevelUpBox.Height), Color.White);
                s.Draw(SkillManager.AllSkills[skillLevelName].Icon, new Rectangle(494, (int)skillBoxPosY + 5, SkillManager.AllSkills[skillLevelName].Icon.Width, SkillManager.AllSkills[skillLevelName].Icon.Height), Color.White);
            }
        }

        public SenderAndText NewSenderAndText(String name, String message)
        {
            SenderAndText temp = new SenderAndText();
            temp.sender = name;
            temp.message = message;
            return temp;
        }

        public void PopulateTextsAndSenders()
        {
            SenderAndTexts.Add(NewSenderAndText("Greg", "Yo Balto, wats up?"));
            SenderAndTexts.Add(NewSenderAndText("Johnny", "Sup fatass"));
            SenderAndTexts.Add(NewSenderAndText("Mom", "Don't forget! Grandma's birthday is today. \nText her something nice :-)"));
            SenderAndTexts.Add(NewSenderAndText("Chelsea", "I hate you Balto."));
            SenderAndTexts.Add(NewSenderAndText("Chris", "Hey bro did you ever get your phone back from \nthat ugly weird kid?"));
            SenderAndTexts.Add(NewSenderAndText("Dad", "You're a disappointment, son."));
            SenderAndTexts.Add(NewSenderAndText("Dad", "I'm drunk again."));
            SenderAndTexts.Add(NewSenderAndText("Dad", "The bus driver told me you were licking the windows again."));
            SenderAndTexts.Add(NewSenderAndText("Cat", "Meow"));
            SenderAndTexts.Add(NewSenderAndText("Uncle Rob", "Your Aunt went and fell down the stairs again. Got her good this time"));
            SenderAndTexts.Add(NewSenderAndText("Kenny T.", "Yo man i sold u the wrong shit this mornin. Dont smoke it wutever u do"));
            SenderAndTexts.Add(NewSenderAndText("Old McDonald.", "E I E I O"));
            SenderAndTexts.Add(NewSenderAndText("Mom", "You forgot to unclog the toilet again"));
            SenderAndTexts.Add(NewSenderAndText("Chubby Cheez Pizza", "Text CHUBBY4ME on your next order to \nreceive a free case of Chubby's famous Fried \nGrease Balls!"));
            SenderAndTexts.Add(NewSenderAndText("Mr. Lard Boy's Donuts", "Thank you for your loyalty to Mr. Lard Boy's! To show our appreciation, here's a coupon for a free box of Mayonnaise Donuts: EXV3T480"));
            SenderAndTexts.Add(NewSenderAndText("Bucket Fried Chicken", "Happy birthday! Show this text to your cashier today and receive an extra ladle of gravy on your order!"));
        }

        public void AddTextMessage(String sender, String message)
        {
            textMessage.message = message;
            textMessage.sender = sender;
            textMessage.showing = true;
        }

        //Add a poof and a new timer/frame
        public void AddRunningDustPoof(Rectangle rec, int startFrame, Boolean faceRight, Boolean sprinting)
        {
            dustPoofs.Add(new DustPoof(startFrame, rec, faceRight, sprinting));
        }

        //Add a poof and a new timer/frame
        public void AddJumpDustPoof(Rectangle rec, Boolean faceRight)
        {
            jumpPoofs.Add(new JumpPoof(rec, faceRight));
        }

        public void DrawDustPoofsWhileRunning(SpriteBatch s)
        {
            //For every run poof
            for (int i = 0; i < dustPoofs.Count; i++)
            {
                dustPoofs[i].Draw(s);
            }

            //Jump poofs
            for (int i = 0; i < jumpPoofs.Count; i++)
            {
                jumpPoofs[i].Draw(s);
            }
        }

        public void ResetEffects()
        {
            damageFXRecs = new List<Rectangle>();
            damageFXTimes = new List<int>();
            deathRecs = new List<Rectangle>();
            deathTimes = new List<int>();
            deathFrames = new List<int>();
            deathTypes = new List<int>();
            allCurrentSounds = new List<SoundEffectInstance>();
            fButtonRecs = new List<Rectangle>();
            inGameDialogue = new List<string>();
            inGameDialogueMaxTimer = new List<int>();
            inGameDialogueTimer = new List<int>();
            dialogueSpeakerEmotion = new List<string>();
            dialogueSpeakerName = new List<string>();

            dustPoofs = new List<DustPoof>();
            jumpPoofs = new List<JumpPoof>();

            announcementDialogue = new List<string>();
            announcementTimer = new List<int>();
            announcementMaxTimer = new List<int>();

            notificationQueue = new Queue<Notification>();
        }

        public void DrawLevelUp(SpriteBatch s)
        {
            frameDelay--;

            //Increase frame
            if (frameDelay == 0)
            {
                levelUpFrame++;

                //Change delay based on frame
                if (levelUpFrame != 8 && levelUpFrame != 9)
                {
                    if (levelUpFrame < 4)
                        frameDelay = 5;
                    else
                    {
                        frameDelay = 4;
                    }
                }
                else
                {
                    frameDelay = 10;
                }
            }

            if (levelUpFrame == 10 && blinkAmount < 3)
            {
                levelUpFrame = 8;
                blinkAmount++;
            }


            //Blink eight times before finishing the animation
            if (levelUpFrame == 10 && blinkAmount == 3)
            {
                statBoxAlpha = 0f;
                levelUpBoxTimer = 150;
                Game1.Player.LevelingUp = false;
                frameDelay = 6;
                levelUpFrame = 0;
                blinkAmount = 0;
                offsetNum = 0;
                levelUpTextYDifference = 0;
            }

            //Raise the timer to raise the level up text over time
            if (levelUpFrame > 5)
                levelUpTextTimer++;

            Rectangle rec = Game1.Player.Rec;

            if (offsetNum >= -250)
            {
                //Number that makes the level up text rise upward
                offsetNum -= 6;
            }


            //Draws the text higher with a base offset of 100, so it starts lower than the character's rectangle by 100ish
            levelUpTextYDifference = 200 + offsetNum;

            if (levelUpFrame > 5 && levelUpFrame != 9 && blinkAmount == 0)
            {
                if (!Game1.Player.FacingRight)
                {
                    Rectangle textRec = rec;
                    textRec.X -= 70;
                    textRec.Y += levelUpTextYDifference;

                    s.Draw(Game1.levelUpAnimation, rec, new Rectangle(0, 796, 530, 398), Color.White);
                }
                else
                {
                    Rectangle textRec = rec;
                    textRec.Y += levelUpTextYDifference;

                    s.Draw(Game1.levelUpAnimation, rec, new Rectangle(0, 796, 530, 398), Color.White);
                }
            }

            if (!Game1.Player.FacingRight)
            {

                if (levelUpFrame < 5)
                    s.Draw(Game1.levelUpAnimation, rec, new Rectangle(levelUpFrame * 530, 0, 530, 398), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                else
                    s.Draw(Game1.levelUpAnimation, rec, new Rectangle((levelUpFrame - 5) * 530, 398, 530, 398), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            }
            else
            {

                if (levelUpFrame < 5)
                    s.Draw(Game1.levelUpAnimation, rec, new Rectangle(levelUpFrame * 530, 0, 530, 398), Color.White);

                else
                    s.Draw(Game1.levelUpAnimation, rec, new Rectangle((levelUpFrame - 5) * 530, 398, 530, 398), Color.White);
            }
        }
        
        public void AddInGameDialogue(String dialogue, String name, String emotion, int maxTime)
        {
            inGameDialogue.Add(dialogue);
            dialogueSpeakerName.Add(name);
            dialogueSpeakerEmotion.Add(emotion);
            inGameDialogueMaxTimer.Add(maxTime);
            inGameDialogueTimer.Add(0);
            dialogueState = 0;
        }

        public void AddAnnouncement(String dialogue, int maxTime)
        {

            announcementDialogue.Add(dialogue);
            announcementMaxTimer.Add(maxTime);
            announcementTimer.Add(0);
            announcementState = 0;
        }

        public void ClearDialogue()
        {
            inGameDialogue.Clear();
            dialogueSpeakerName.Clear();
            dialogueSpeakerEmotion.Clear();
            inGameDialogueMaxTimer.Clear();
            inGameDialogueTimer.Clear();
            dialogueState = 0;
        }

        public void AddLockedDoorMessage()
        {
            if (lockedTimer <= 0)
            {
                lockedTimer = 150;
                lockedDoorMessagePosX = -306; //306 is the width of the texture
            }
        }

        public void AddTimer(double time)
        {
            timerStart = time;
        }

        public void AddDamageFX(int time, Rectangle rec)
        {
            damageFXRecs.Add(rec);
            damageFXTimes.Add(time);
            skillImpactNames.Add("");
        }

        public void AddDamageFXForSkill(int time, Rectangle rec, String skillName)
        {
            damageFXRecs.Add(rec);
            damageFXTimes.Add(time);
            skillImpactNames.Add(skillName);

        }

        //Adds a tooltip to the screen. Must call the RemoveToolTip function to remove it
        public void AddToolTip(String text, int x, int y)
        {
            toolTip = null;
            customToolTipBox = null;
            toolTip = text;
            toolTipX = x;
            toolTipY = y;
        }

        //Adds a tooltip to the screen with a custom image for the box. Must call the RemoveToolTip function to remove it
        public void AddToolTipWithImage(String text, int x, int y, Texture2D img)
        {
            toolTip = null;
            customToolTipBox = null;
            toolTip = text;
            toolTipX = x;
            toolTipY = y;
            customToolTipBox = img;
        }

        //Removes the tooltip
        public void RemoveToolTip()
        {
            toolTip = null;
            customToolTipBox = null;
        }

        public void AddFoundItem(String name, Texture2D icon)
        {
            foundItemName = name;
            foundItemTimer = 150;
            foundItemIcon = icon;
            foundItemAlpha = 1f;
        }

        /// <summary>
        /// Type 1 is enemy death, type 2 is normal poof
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="type"></param>
        public void AddSmokePoof(Rectangle rec, int type)
        {
            Rectangle newRec = rec;

            int size;

            if (rec.Width > rec.Height)
                size = rec.Width;
            else
                size = rec.Height;
            
            newRec.Width += (int)(size * 2);
            newRec.Height += (int)(size * 2);
            newRec.X = rec.X -(int)(size);
            newRec.Y = rec.Y -(int)(size);

            deathTimes.Add(0);
            deathRecs.Add(newRec);
            deathFrames.Add(0);
            deathTypes.Add(type);

        }

        public Rectangle GetDeathSource(int recNum)
        {
            //Death
            if(deathTypes[recNum] == 1)
                return new Rectangle(deathFrames[recNum] * 400, 0, 400, 300);

            //SMoke poof
            else
                return new Rectangle(deathFrames[recNum] * 400, 300, 400, 300);
        }

        public int UpdateDecision(String text)
        {
            decisionText = text;
            //last = current;
           // current = Keyboard.GetState();

            if (decisionState == 0)
                decisionState = 1;

            if ((last.IsKeyDown(Keys.Enter) && current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed())
            {
                if (decisionState == 1)
                    return 1;
                else
                    return 2;
            }

            if (decisionState == 1 && (last.IsKeyDown(Keys.Down) && current.IsKeyUp(Keys.Down)) || MyGamePad.DownPadPressed())
                decisionState = 2;
            else if (decisionState == 2 && (last.IsKeyDown(Keys.Up) && current.IsKeyUp(Keys.Up)) || MyGamePad.UpPadPressed())
                decisionState = 1;

            return 0;
        }

        public void Update()
        {
            last = current;
            current = Keyboard.GetState();

            #region skill level up

            if (Game1.Player.LevelingUp == false)
            {
                skillLevelTimer--;

                if (skillLevelTimer == 0)
                {
                    skillLevelFrame++;
                    skillLevelTimer = 2;

                    if (skillLevelFrame == 8)
                    {
                        skillLevelColor = Color.White;
                    }
                }

                skillMessageTime--;
            }
            #endregion

            //Timer for showing the stat box after leveling up
            if (levelUpBoxTimer > 0)
                levelUpBoxTimer--;

            #region TEXT MESSAGES
            if (Game1.Player.HasCellPhone)
            {
                if (timeUntilNextMessage <= 0)
                {
                    int mesNum = ranText.Next(SenderAndTexts.Count - 1);
                    AddTextMessage(SenderAndTexts[mesNum].sender, SenderAndTexts[mesNum].message);

                    timeUntilNextMessage = ranText.Next(12000, 65000);
                }

                //Close the text if you press 'Enter' while an NPC isn't talking
                if (last.IsKeyDown(Keys.Enter) && current.IsKeyUp(Keys.Enter) && textMessage.showing == true && !Game1.currentChapter.TalkingToNPC)
                {
                    textMessage.showing = false;
                }

                //MAKE THE PHONE SLIDE OUT
                if (textMessage.showing && phonePosX < 0)
                {
                    if (phonePosX < 0)
                    {
                        float distance = phonePosX - 25;

                        phonePosX -= 5 * (distance / 75);

                        if (phonePosX >= -1)
                        {
                            phonePosX = 0;
                            phoneButton.ButtonRec = new Rectangle(0, 190, 236, 98);
                        }

                    }
                } //SLIDE BACK IN
                else if (textMessage.showing == false && phonePosX > -197)
                {
                    if (phonePosX > -197)
                    {
                        float distance = phonePosX + 215;

                        phonePosX -= 5 * (distance / 75);

                        if (phonePosX <= -196)
                        {
                            phonePosX = -197;
                            phoneButton = new Button(new Rectangle(0, 190, 50, 98));
                        }

                    }
                }//CLICK THE PHONE TO OPEN IT
                else if (textMessage.showing == false && phonePosX == -197)
                {
                    if (phoneButton.Clicked())
                    {
                        phoneButton.ButtonRec = new Rectangle(0, 190, 236, 98);
                        textMessage.showing = true;
                    }
                }//CLICK THE PHONE TO CLOSE IT
                else if (textMessage.showing && phonePosX == 0)
                {
                    if (phoneButton.Clicked())
                    {
                        textMessage.showing = false;
                        phoneButton = new Button(new Rectangle(0, 190, 50, 98));
                    }
                }
            }
            #endregion

            //Updates each of the running poofs on the screen
            for (int i = 0; i < dustPoofs.Count; i++)
            {
                dustPoofs[i].Update();

                if (dustPoofs[i].Finished)
                {
                    dustPoofs.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            //Updates each of the jump poofs on the screen
            for (int i = 0; i < jumpPoofs.Count; i++)
            {
                jumpPoofs[i].Update();

                if (jumpPoofs[i].Finished)
                {
                    jumpPoofs.RemoveAt(i);
                    i--;
                    continue;
                }
            }

            if (notificationQueue.Count > 0)
            {
                notificationQueue.ElementAt(0).Update();

                if (notificationQueue.ElementAt(0).Finished)
                    notificationQueue.Dequeue();
            }

            if (secondNotificationQueue.Count > 0)
            {
                secondNotificationQueue.ElementAt(0).Update();

                if (secondNotificationQueue.ElementAt(0).Finished)
                    secondNotificationQueue.Dequeue();
            }


            if (lockedTimer > 0)
                lockedTimer--;

            if (fButtonRecs.Count > 0 || foregroundFButtonRecs.Count > 0 || spaceButtonRecs.Count > 0 || foregroundSpaceButtonRecs.Count > 0)
            {

                if (alphaGoingUp == false)
                {
                    fAlpha -= .01f;

                    if (fAlpha <= 0)
                        alphaGoingUp = true;
                }

                if (alphaGoingUp)
                {
                    fAlpha += .01f;
                    if (fAlpha >= 1)
                        alphaGoingUp = false;
                }

            }

            if (announcementDialogue.Count > 0 && inGameDialogue.Count == 0)
            {
                announcementTimer[announcementState]++;

                if (announcementTimer[announcementState] == announcementMaxTimer[announcementState])
                {
                    announcementState++;
                    if (announcementState > announcementDialogue.Count - 1)
                    {
                        announcementDialogue.Clear();
                        announcementState = 0;
                        announcementMaxTimer.Clear();
                        announcementTimer.Clear();
                    }
                }
            }

            if (inGameDialogue.Count > 0)
            {
                inGameDialogueTimer[dialogueState]++;

                if (inGameDialogueTimer[dialogueState] == inGameDialogueMaxTimer[dialogueState])
                {
                    dialogueState++;
                    if (dialogueState > inGameDialogue.Count - 1)
                    {
                        inGameDialogue.Clear();
                        dialogueState = 0;
                        inGameDialogueMaxTimer.Clear();
                        inGameDialogueTimer.Clear();
                        dialogueSpeakerEmotion.Clear();
                        dialogueSpeakerName.Clear();
                    }
                }
            }

            if (timerStart > 0)
            {
                if (timerIncrement < 60)
                    timerIncrement++;
                else
                {
                    timerIncrement = 0;
                    timerStart -= .01;
                }
            }
            else
            {
                timerStart = 0;
                timerIncrement = 0;
            }

            if (foundItemTimer > 0)
                foundItemTimer--;

            for (int i = 0; i < damageFXRecs.Count; i++)
            {
                damageFXTimes[i]--;
                if (damageFXTimes[i] <= 0)
                {
                    skillImpactNames.RemoveAt(i);
                    damageFXRecs.RemoveAt(i);
                    damageFXTimes.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < deathRecs.Count; i++)
            {
                deathTimes[i]++;

                if (deathTimes[i] == 4)
                {
                    deathFrames[i]++;
                    deathTimes[i] = 0;
                }

                if (deathFrames[i] > 8)
                {
                    deathRecs.RemoveAt(i);
                    deathTimes.RemoveAt(i);
                    deathFrames.RemoveAt(i);
                    deathTypes.RemoveAt(i);
                    i--;
                } 
            }
        }

        public void DrawNotification(SpriteBatch s)
        {
            if (notificationQueue.Count > 0)
            {
                notificationQueue.ElementAt(0).Draw(s);
            }

            if (secondNotificationQueue.Count > 0)
            {
                secondNotificationQueue.ElementAt(0).Draw(s);
            }
        }

        public void DrawLevelUpInfo(SpriteBatch s)
        {

            if (levelUpBoxTimer > 0)
            {
                if (levelUpBoxTimer > 100)
                {
                    statBoxAlpha += .04f;

                    if (statBoxAlpha > 1f)
                        statBoxAlpha = 1f;
                }
                else if (levelUpBoxTimer < 26)
                    statBoxAlpha -= .04f;

                s.Draw(Game1.levelUpStatBox, new Rectangle(560, 134, Game1.levelUpStatBox.Width, Game1.levelUpStatBox.Height), Color.White * statBoxAlpha);

                s.DrawString(Game1.twConRegularSmall, "+  " + Game1.Player.strengthAddedDuringLevel, new Vector2(651, 161), Color.Black * statBoxAlpha);
                s.DrawString(Game1.twConRegularSmall, "+  " + Game1.Player.healthAddedDuringLevel, new Vector2(651, 190), Color.Black * statBoxAlpha);
                s.DrawString(Game1.twConRegularSmall, "+  " + Game1.Player.defenseAddedDuringLevel, new Vector2(651, 219), Color.Black * statBoxAlpha);

            }
        }

        public void DrawToolTip(SpriteBatch s)
        {
            if (toolTip != null)
            {
                //Normal box
                if (customToolTipBox == null)
                {
                    Vector2 stringMeas = Game1.twConQuestHudInfo.MeasureString(toolTip);
                    s.Draw(Game1.toolTipTexture, new Rectangle(toolTipX, toolTipY, Game1.toolTipTexture.Width, Game1.toolTipTexture.Height), Color.White);
                    s.DrawString(Game1.twConQuestHudInfo, toolTip, new Vector2(toolTipX + 520 / 2 - (stringMeas.X / 2), toolTipY + 99 / 2 - stringMeas.Y / 2), Color.Black);
                }
                //Custom box
                else
                {
                    s.Draw(customToolTipBox, new Rectangle(toolTipX, toolTipY, customToolTipBox.Width, customToolTipBox.Height), Color.White);
                    //Draw the tooltip. The Y position is measured from the bottom in case a custom tooltip texture is used, which could potentially alter the
                    //height of the image. However, the height from the bottom would remain the same
                    s.DrawString(Game1.dialogueFont, toolTip, new Vector2(toolTipX + 150, toolTipY +  99 - 40), Color.Black);
                }
            }


        }

        public void DrawDecision(SpriteBatch s)
        {
            s.Draw(Game1.decisionBox, new Rectangle(187, 483, Game1.decisionBox.Width, Game1.decisionBox.Height), Color.White);

            Vector2 meas = Game1.dialogueFont.MeasureString(decisionText);

            s.DrawString(Game1.dialogueFont, decisionText, new Vector2(610 - meas.X / 2, 610), Color.Black);

            //--Draw the choices
            if (decisionState == 1)
            {

                s.Draw(Game1.notificationTextures, new Rectangle(922, 532, 151, 115), new Rectangle(3137, 0, 151, 115), Color.White);
                s.Draw(Game1.notificationTextures, new Rectangle(922, 588, 151, 115), new Rectangle(2676, 0, 151, 115), Color.White);
            }
            else if(decisionState == 2)
            {
                s.Draw(Game1.notificationTextures, new Rectangle(922, 532, 151, 115), new Rectangle(3288, 0, 151, 115), Color.White);
                s.Draw(Game1.notificationTextures, new Rectangle(922, 588, 151, 115), new Rectangle(2525, 0, 151, 115), Color.White);
            }
        }

        public void DrawLockedDoorMessage(SpriteBatch s)
        {

            if (lockedDoorMessagePosX < 496 && lockedTimer > 120)
            {
                float distance = lockedDoorMessagePosX - 500;

                lockedDoorMessagePosX -= 8 * (distance / 40);

                if (lockedDoorMessagePosX > 496)
                {
                    lockedDoorMessagePosX = 496;
                }

            }
            else if (lockedTimer < 50)
            {
                float distance = lockedDoorMessagePosX - 1300;

                lockedDoorMessagePosX -= 8 * (distance / 40);

                if (lockedDoorMessagePosX > 1290)
                {
                    lockedDoorMessagePosX = 1290;
                }
            }

            if (lockedTimer > 0)
            {
                s.Draw(Game1.lockedDoorMessageTexture, new Rectangle((int)lockedDoorMessagePosX, 248, Game1.lockedDoorMessageTexture.Width, Game1.lockedDoorMessageTexture.Height), Color.White);
            }

        }

        public void DrawAnnouncement(SpriteBatch s)
        {
            if (announcementDialogue.Count > 0 && inGameDialogue.Count == 0)
            {
                Vector2 meas = Game1.dialogueFont.MeasureString(announcementDialogue[announcementState]);

                s.Draw(Game1.decisionBox, new Rectangle(187, 483, Game1.decisionBox.Width, Game1.decisionBox.Height), Color.White);
                s.DrawString(Game1.dialogueFont, announcementDialogue[announcementState], new Vector2(610 - meas.X/2, 610), Color.Black);
            }
        }

        public void DrawDialogue(SpriteBatch s)
        {
            if (inGameDialogue.Count > 0 && lockedTimer <= 0)
            {
                s.Draw(Game1.notificationTextures, new Rectangle(38, 393, 1080, 327), new Rectangle(0, 155, 1080, 327), Color.White);
                //s.Draw(Game1.npcFaces[dialogueSpeakerName[dialogueState]][dialogueSpeakerEmotion[dialogueState]], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);

                //Oh fuck this code blows
                s.Draw(Game1.npcFaces[dialogueSpeakerName[dialogueState]].faces[dialogueSpeakerEmotion[dialogueState]], new Rectangle(0, (int)(Game1.aspectRatio * 1280) - Game1.npcFaces[dialogueSpeakerName[dialogueState]].faces[dialogueSpeakerEmotion[dialogueState]].Height, Game1.npcFaces[dialogueSpeakerName[dialogueState]].faces[dialogueSpeakerEmotion[dialogueState]].Width, Game1.npcFaces[dialogueSpeakerName[dialogueState]].faces[dialogueSpeakerEmotion[dialogueState]].Height), Color.White);

                s.DrawString(Game1.dialogueFont, Game1.WrapText(Game1.dialogueFont, inGameDialogue[dialogueState], 660), new Vector2(360, (int)(Game1.aspectRatio * 1280 * .8f)), Color.Black);

                
            }
        }

        public void DrawFoundItem(SpriteBatch s)
        {
            if (foundItemTimer > 0)
            {

                if (foundItemTimer < 100)
                    foundItemAlpha -= .01f;
                Vector2 meas = Game1.twConRegularSmall.MeasureString(foundItemName + "!");

                s.Draw(Game1.youFoundItemTexture, new Rectangle(496, 248, Game1.youFoundItemTexture.Width, Game1.youFoundItemTexture.Height), Color.White * foundItemAlpha);
                s.DrawString(Game1.twConRegularSmall, "You found", new Vector2(680 - Game1.twConRegularSmall.MeasureString("You found").X / 2, 280), Color.Black * foundItemAlpha);
                s.DrawString(Game1.twConRegularSmall, foundItemName + "!", new Vector2(680 - (int)(meas.X / 2), 305), Color.Black * foundItemAlpha);

                s.Draw(foundItemIcon, new Rectangle(520, 270, 70, 70), Color.White * foundItemAlpha);
            }
        }

        public void DrawCutsceneItem(SpriteBatch s, String text)
        {
            Vector2 meas = Game1.twConRegularSmall.MeasureString(text + "!");

            s.Draw(Game1.youFoundItemTexture, new Rectangle(496, 248, Game1.youFoundItemTexture.Width, Game1.youFoundItemTexture.Height), Color.White);
            s.DrawString(Game1.twConRegularSmall, "You found", new Vector2(650 - Game1.twConRegularSmall.MeasureString("You found").X / 2, 280), Color.Black);
            s.DrawString(Game1.twConRegularSmall, text + "!", new Vector2(650 - (int)(meas.X / 2), 305), Color.Black);
            
        }

        public void DrawSkillEffects(SpriteBatch s)
        {
            #region Draw Impact Effects
            for (int i = 0; i < damageFXRecs.Count; i++)
            {
                if (skillImpactNames[i] != "" && SkillManager.skillImpactEffects[skillImpactNames[i]] != Game1.bangFX1)
                    s.Draw(SkillManager.skillImpactEffects[skillImpactNames[i]], new Rectangle(damageFXRecs[i].X - 125, damageFXRecs[i].Y - 125, SkillManager.skillImpactEffects[skillImpactNames[i]].Width, SkillManager.skillImpactEffects[skillImpactNames[i]].Height), Color.White);
                else
                    s.Draw(Game1.bangFX1, new Rectangle(damageFXRecs[i].X - 125, damageFXRecs[i].Y - 125, 250, 250), Color.White);


            }
            #endregion
        }

        public void DrawPoofs(SpriteBatch s)
        {
            for (int i = 0; i < deathRecs.Count; i++)
            {
                s.Draw(deathSpriteSheet, new Rectangle(deathRecs[i].X, deathRecs[i].Y, deathRecs[i].Width, (int)(deathRecs[i].Width * .75f)), 
                    GetDeathSource(i), Color.White);
            }
        }

        public void DrawTimer(SpriteBatch s)
        {
            if (timerStart > 0)
                s.DrawString(Game1.HUDFont, timerStart.ToString(), new Vector2(600, 100), Color.Black);
        }

        public void AddForeroundFButton(Rectangle rec)
        {
            foregroundFButtonRecs.Add(rec);
        }

        public void DrawForegroundFButtons(SpriteBatch s)
        {
            for (int i = 0; i < foregroundFButtonRecs.Count; i++)
            {

                s.Draw(Game1.fInner, foregroundFButtonRecs[i], Color.White * fAlpha);
                if (Game1.gamePadConnected)
                    s.Draw(Game1.rightBumperTexture, foregroundFButtonRecs[i], Color.White);
                else
                    s.Draw(Game1.fOuter, foregroundFButtonRecs[i], Color.White * .7f);

            }
        }

        public void AddFButton(Rectangle rec)
        {
             fButtonRecs.Add(rec);
        }

        public void AddSpaceButton(Rectangle rec)
        {
            spaceButtonRecs.Add(rec);
        }

        public void DrawFButtons(SpriteBatch s)
        {
            for (int i = 0; i < fButtonRecs.Count; i++)
            {

                s.Draw(Game1.fInner, fButtonRecs[i],Color.White * fAlpha);
                if (Game1.gamePadConnected)
                    s.Draw(Game1.rightBumperTexture, fButtonRecs[i], Color.White);
                else
                    s.Draw(Game1.fOuter, fButtonRecs[i], Color.White * .7f);

            }
        }

        public void DrawSpaceButtons(SpriteBatch s)
        {
            for (int i = 0; i < spaceButtonRecs.Count; i++)
            {

                s.Draw(Game1.spaceInner, spaceButtonRecs[i], Color.White * fAlpha);
                if (Game1.gamePadConnected)
                    s.Draw(Game1.rightBumperTexture, spaceButtonRecs[i], Color.White);
                else
                    s.Draw(Game1.spaceOuter, spaceButtonRecs[i], Color.White * .7f);

            }
        }

        public void DrawTextMessage(SpriteBatch s)
        {
            if (Game1.Player.HasCellPhone)
            {
                s.Draw(Game1.phoneTexture, new Rectangle((int)phonePosX, 190, Game1.phoneTexture.Width, Game1.phoneTexture.Height), Color.White);
                if (textMessage.message != null)
                {
                    s.DrawString(Game1.expMoneyFloatingNumFont, "From : " + textMessage.sender, new Vector2(29 + (int)phonePosX, 200), Color.White);
                    s.DrawString(Game1.phoneTextFont, Game1.WrapText(Game1.phoneTextFont, textMessage.message, 170), new Vector2(25 + (int)phonePosX, 218), Color.White);
                }
                else
                {
                    s.DrawString(Game1.expMoneyFloatingNumFont, "No Messages!", new Vector2(70 + (int)phonePosX, 210), Color.White);
                }
            }
        }

        public void ClearDustPoofs()
        {
            dustPoofs.Clear();
            jumpPoofs.Clear();
        }
    }
}

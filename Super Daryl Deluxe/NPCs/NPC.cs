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
    public class NPC : GameObject
    {
        protected Texture2D spriteSheet;
        protected String currentDialogueFace;
        protected Player player;
        protected List<String> dialogue;
        protected List<String> questDialogue;
        protected int dialogueState;
        protected Boolean talking;
        protected Boolean facingRight;
        protected Boolean staticNPC = false;
        public int moveFrame;
        protected Game1 game;
        protected int choice = 0;
        protected String mapName;
        protected Quest quest;
        protected Boolean acceptQuestPage;
        protected Boolean currentlyMoving = false;
        protected int frameDelay = 5;
        protected String name;
        protected Boolean acceptedQuest = false;
        protected Boolean updatingInteraction = false;
        protected Boolean drawFButton = false;
        protected Rectangle frec;
        public Boolean canTalk = true;

        protected float enterAlpha = 1f;
        protected Boolean enterAlphaIncreasing = false;

        protected Boolean drawEnter = true;
        protected Boolean updateFaster = false;

        protected float notificationPos;
        protected float notificationVelocity;
        protected int notificationTimer;
        protected int notificationState = 1;


        protected String scrollDialogue;
        protected int scrollDialogueNum;
        protected int stringNum;
        int textScrollSoundIndex;

        //--Wander attributes
        static Random moveRandom;
        int tempWanderTimer;
        int wanderTimer;
        int wanderState;

        public Boolean FacingRight { get { return facingRight; } set { facingRight = value; } }
        public List<String> Dialogue { get { return dialogue; } set { dialogue = value; } }
        public String Name { get { return name; } set { name = value; } }
        public String CurrentDialogueFace { get { return currentDialogueFace; } set { currentDialogueFace = value; } }
        public Boolean Talking { get { return talking; } set { talking = value; } }
        public List<String> QuestDialogue { get { return questDialogue; } set { questDialogue = value; } }
        public int Choice { get { return choice; } set { choice = value; } }
        public int DialogueState { get { return dialogueState; } set { dialogueState = value; } }
        public Quest Quest { get { return quest; } set { quest = value; } }
        public Boolean AcceptedQuest { get { return acceptedQuest; } set { acceptedQuest = value; } }
        public String MapName { get { return mapName; } set { mapName = value; } }
        public Texture2D Spritesheet { get { return spriteSheet; } set { spriteSheet = value; } }

        public enum MoveState
        {
            standing,
            moving
        }
        public MoveState moveState;

        public NPC(Texture2D sprite, Game1 g, String name)
        {
            spriteSheet = sprite;
            game = g;
            dialogue = new List<string>();
            this.name = name;
            currentDialogueFace = "Normal";
            moveRandom = new Random();
        }

        //--Constructor for quest NPC
        public NPC(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, 
            Boolean stat)
        {
            dialogue = d;
            quest = q;
            dialogueState = 0;
            rec = r;
            player = play;
            talking = false;
            font = f;
            spriteSheet = sprite;
            game = g;
            mapName = mName;
            this.name = name;
            questDialogue = new List<string>();
            questDialogue = quest.QuestDialogue;
            acceptQuestPage = false;
            staticNPC = stat;
            moveRandom = new Random();

            //--If the NPC only has a single texture, make the rectangle that size
            //--If it isn't static, the size must be passed in
            if (staticNPC)
            {
                rec.Width = spriteSheet.Width;
                rec.Height = spriteSheet.Height;
            }

            currentDialogueFace = "Normal";

            position.X = rec.X;
            position.Y = rec.Y;
        }

        //--Constructor for non-Quest NPC
        public NPC(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
        {
            dialogue = d;
            dialogueState = 0;
            rec = r;
            player = play;
            talking = false;
            font = f;
            spriteSheet = sprite;
            game = g;
            mapName = mName;
            acceptQuestPage = false;
            this.name = name;
            staticNPC = stat;
            questDialogue = new List<string>();
            //--If the NPC only has a single texture, make the rectangle that size
            //--If it isn't static, the size must be passed in
            if (staticNPC)
            {
                rec.Width = spriteSheet.Width;
                rec.Height = spriteSheet.Height;
            }
            currentDialogueFace = "Normal";
            moveRandom = new Random();

            position.X = rec.X;
            position.Y = rec.Y;
        }

        public virtual Rectangle GetSourceRectangle(int frame)
        {
            if (!staticNPC)
            {
                switch (moveState)
                {
                    case MoveState.standing:
                        return new Rectangle(0, 0, 516, 388);

                    case MoveState.moving:
                        if (frame < 5)
                        {
                            return new Rectangle(frame * 516, 388, 516, 388);
                        }
                        else
                        {
                            int column = frame - 5;
                            return new Rectangle(column * 516, 388 * 2, 516, 388);
                        }
                }
            }
            else
            {
                return new Rectangle(0, 0, spriteSheet.Width, spriteSheet.Height);
            }

                return new Rectangle(0, 0, 0, 0);
        }

        //--Check if the player is talking to the NPC
        public virtual void CheckInteraction()
        {

            //--Get the distance from daryl to the NPC
            Point distanceFromNPC = new Point(Math.Abs(player.VitalRec.Center.X - rec.Center.X),
            Math.Abs(player.VitalRec.Center.Y - rec.Center.Y));

            if (distanceFromNPC.X < 70 && distanceFromNPC.Y < 130)
            {
                if (((game.last.IsKeyDown(Keys.F) && game.current.IsKeyUp(Keys.F)) || MyGamePad.LeftBumperPressed()) && Game1.spokeThisFrame == false && player.CurrentPlat != null && canTalk)
                {

                    //--Once the quest is completed, but you haven't gotten the reward, change the dialogue to the finished quest string
                    if (quest != null && quest.CompletedQuest && questDialogue != quest.CompletedDialogue)
                    {
                        dialogueState = 0;
                        questDialogue = quest.CompletedDialogue;
                    }

                    game.CurrentChapter.TalkingToNPC = true;
                    talking = true;
                    player.Sprinting = false;

                    if(rec.Center.X < player.VitalRec.Center.X)
                        facingRight = true;
                    else
                        facingRight = false;

                    Game1.spokeThisFrame = true;
                }

                drawFButton = true;
            }
            else
                drawFButton = false;
        }

        public virtual void ClearDialogue()
        {
            talking = false;
            updatingInteraction = false;
            textScrollSoundIndex = 0;
            stringNum = 0;
            scrollDialogueNum = 0;
            scrollDialogue = "";

            dialogue.Clear();

            if(questDialogue != null)
                questDialogue.Clear();
        }

        public void UpdateRecAndPosition()
        {
            position.X += velocity.X;
            position.Y += velocity.Y;
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;
        }

        public override void Update()
        {
            base.Update();

            if (game.CurrentChapter.CurrentMap.MapName == mapName || talking)
            {
                if (game.CurrentChapter.TalkingToNPC == false)
                {
                    CheckInteraction();
                }

                if (talking)
                {
                    UpdateInteraction();
                }
            }

            if (quest != null && (quest.npcName == null || quest.npcName == ""))
            {
                quest.npcName = name;
            }
        }

        public virtual void Move(Vector2 velocity, Platform.PlatformType floorType = Platform.PlatformType.rock, int footstepFrame1 = 1, int footstepFrame2 = 6)
        {
            if (velocity.X > 0)
            {
                facingRight = true;
            }
            if (velocity.X < 0)
            {
                facingRight = false;
            }
            moveState = MoveState.moving;

            if (currentlyMoving == false)
                moveFrame = 0;

            frameDelay--;
            if (frameDelay == 0)
            {
                moveFrame++;
                frameDelay = 6;
            }

            if ((moveFrame == footstepFrame1 || moveFrame == footstepFrame2) && frameDelay == 3)
            {
                Sound.PlayNPCSteppingSound(floorType, rec.Center.X, rec.Center.Y);
            }

            if (moveFrame == Game1.numberOfNPCWalkingFrames[name])
                moveFrame = 0;

            currentlyMoving = true;

            position += velocity;
            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

        }


        /// <summary>
        /// Use this for changing the dialogue when you go back to talk to an NPC whose quest you have accepted but haven't finished
        /// This is really only useful for story quests
        /// </summary>
        public void ChangeQuestDialogueForAcceptedQuest(String dia)
        {
            questDialogue[questDialogue.Count - 1] = dia;
        }

        public void AcceptQuestRemotely(Quest q)
        {
            if (quest == q)
            {
                acceptedQuest = true;
                dialogueState = questDialogue.Count - 2;
                acceptQuestPage = false;
                game.CurrentQuests.Add(quest);

                if (!quest.StoryQuest)
                {
                    game.CurrentSideQuests.Add(quest);
                    Chapter.effectsManager.NotificationQueue.Enqueue(new QuestReceivedNotification(false));
                }
                else
                {
                    Chapter.effectsManager.NotificationQueue.Enqueue(new QuestReceivedNotification(true));

                    //Remove the old story quest from the quest helper
                    for (int j = 0; j < Game1.questHUD.questHelperQuests.Count; j++)
                    {
                        if (Game1.questHUD.questHelperQuests[j].StoryQuest)
                        {
                            Game1.questHUD.RemoveQuestFromHelper(Game1.questHUD.questHelperQuests[j]);
                        }
                    }
                }

                Game1.questHUD.AddQuestToHelper(quest);
            }
            else
            {
                throw new Exception("This quest does not match the NPC's quest");
            }
        }

        //--Update dialogue when you press enter
        public virtual void UpdateInteraction()
        {
            updatingInteraction = true;

            #region Non-Quest
            //--If there isn't a quest, or a quest has been accepted that isn't this one
            //--Only play the standard dialogue
            if (questDialogue == null || questDialogue.Count == 0/* || (game.CurrentChapter.CurrentQuests != null & game.CurrentChapter.CurrentQuests != quest)*/)
            {
                if (((game.last.IsKeyDown(Keys.Enter) && game.current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed()) && scrollDialogueNum >= dialogue[dialogueState].Length && updateFaster == false)
                {
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                    scrollDialogueNum = 0;
                    dialogueState++;
                    scrollDialogue = "";
                    textScrollSoundIndex = 0;
                        if (dialogueState == dialogue.Count)
                        {
                            game.CurrentChapter.TalkingToNPC = false;
                            talking = false;
                            MyGamePad.ResetStates();
                            dialogueState = 0;
                            updatingInteraction = false;
                        }
                        
                }

                //--Update the text faster
                if (scrollDialogueNum < dialogue[dialogueState].Length && (game.current.IsKeyDown(Keys.Enter) || MyGamePad.currentState.Buttons.A == ButtonState.Pressed))
                {
                    updateFaster = true;
                }
                else if (game.current.IsKeyUp(Keys.Enter) && MyGamePad.currentState.Buttons.A == ButtonState.Released)
                {
                    updateFaster = false;
                }

            }
            #endregion

            #region Quest
            else
            {

                #region Yes or No Page
                if (acceptQuestPage == true)
                {
                    
                    //--Change choices between yes and no
                    if (choice == 0 && ((game.current.IsKeyUp(Keys.Down) && game.last.IsKeyDown(Keys.Down)) || MyGamePad.DownPadPressed()))
                    {
                        choice = 1;
                    }
                    else if (choice == 1 && ((game.current.IsKeyUp(Keys.Up) && game.last.IsKeyDown(Keys.Up)) || MyGamePad.UpPadPressed()))
                    {
                        choice = 0;
                    }

                    if (quest.StoryQuest)
                        choice = 0;

                    //--If you say yes, accept the quest, increment the dialogue state, stop talking to the NPC
                    //--Set the chapter's current quest
                    if (choice == 0 && ((game.last.IsKeyDown(Keys.Enter) && game.current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed()) && scrollDialogueNum >= questDialogue[dialogueState].Length && updateFaster == false)
                    {
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                        scrollDialogueNum = 0;
                        scrollDialogue = "";
                        textScrollSoundIndex = 0;

                        acceptedQuest = true;
                        dialogueState++;
                        game.CurrentChapter.TalkingToNPC = false;
                        talking = false;
                        acceptQuestPage = false;
                        updatingInteraction = false;
                        game.CurrentQuests.Add(quest);
                        MyGamePad.ResetStates();

                        if (!quest.StoryQuest)
                        {
                            game.CurrentSideQuests.Add(quest);
                            Chapter.effectsManager.NotificationQueue.Enqueue(new QuestReceivedNotification(false));
                        }
                        else
                        {
                            if(quest.QuestName != "Daryl's New Friends")
                                Chapter.effectsManager.NotificationQueue.Enqueue(new QuestReceivedNotification(true));

                            //Remove the old story quest from the quest helper
                            for (int j = 0; j < Game1.questHUD.questHelperQuests.Count; j++)
                            {
                                if (Game1.questHUD.questHelperQuests[j].StoryQuest)
                                {
                                    Game1.questHUD.RemoveQuestFromHelper(Game1.questHUD.questHelperQuests[j]);
                                }
                            }
                        }

                        Game1.questHUD.AddQuestToHelper(quest);
                    }
                    else if (choice == 1 && ((game.last.IsKeyDown(Keys.Enter) && game.current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed()) && scrollDialogueNum >= questDialogue[dialogueState].Length && updateFaster == false)
                    {
                        scrollDialogueNum = 0;
                        textScrollSoundIndex = 0;
                        scrollDialogue = "";
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                        dialogueState = 0;
                        game.CurrentChapter.TalkingToNPC = false;
                        talking = false;
                        MyGamePad.ResetStates();

                        updatingInteraction = false;
                        acceptQuestPage = false;
                        choice = 0;
                    }
                }
                #endregion

                #region Cancel Quest
                if (acceptQuestPage == false && acceptedQuest == true && quest.CompletedQuest == false)
                {
                    if (choice == 0 && ((game.current.IsKeyUp(Keys.Down) && game.last.IsKeyDown(Keys.Down)) || MyGamePad.DownPadPressed()))
                    {
                        choice = 1;
                    }
                    else if (choice == 1 && ((game.current.IsKeyUp(Keys.Up) && game.last.IsKeyDown(Keys.Up)) || MyGamePad.UpPadPressed()))
                    {
                        choice = 0;
                    }

                    if (quest.StoryQuest)
                        choice = 0;

                    if (choice == 0 && ((game.last.IsKeyDown(Keys.Enter) && game.current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed()) && scrollDialogueNum >= questDialogue[dialogueState].Length && updateFaster == false)
                    {
                        scrollDialogueNum = 0;
                        textScrollSoundIndex = 0;
                        scrollDialogue = "";
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);
                        talking = false;
                        MyGamePad.ResetStates();

                        acceptQuestPage = false;
                        game.CurrentChapter.TalkingToNPC = false;
                        updatingInteraction = false;
                    }
                    else if (choice == 1 && ((game.last.IsKeyDown(Keys.Enter) && game.current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed()) && scrollDialogueNum >= questDialogue[dialogueState].Length && updateFaster == false)
                    {
                        scrollDialogueNum = 0;
                        textScrollSoundIndex = 0;
                        scrollDialogue = "";
                        Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);

                        dialogueState = 0;
                        game.CurrentChapter.TalkingToNPC = false;
                        talking = false;
                        MyGamePad.ResetStates();

                        acceptQuestPage = false;
                        game.CurrentQuests.Remove(quest);
                        if(!quest.StoryQuest)
                            game.CurrentSideQuests.Remove(quest);

                        Game1.questHUD.RemoveQuestFromHelper(quest);

                        acceptedQuest = false;
                        updatingInteraction = false;
                        choice = 0;
                    }
                }
                #endregion

                #region Talk to NPC about quest
                if (talking && ((game.last.IsKeyDown(Keys.Enter) && game.current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed()) && acceptQuestPage == false && scrollDialogueNum >= questDialogue[dialogueState].Length && updateFaster == false)
                {
                    scrollDialogueNum = 0;
                    textScrollSoundIndex = 0;
                    scrollDialogue = "";
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_advance);

                    //--If you haven't completed the quest
                    if (quest.CompletedQuest == false)
                    {
                        //--If you don't have a quest yet, keep talking
                        if (acceptedQuest == false)
                        {
                            dialogueState++;
                        }

                        //--Once you reach the second to last line, make them choose yes or no
                        if (dialogueState == questDialogue.Count - 2)
                        {
                            acceptQuestPage = true;
                        }
                    }

                    //--If completed
                    else
                    {
                        if (dialogueState == questDialogue.Count - 1)
                        {
                            scrollDialogueNum = 0;
                            textScrollSoundIndex = 0;
                            scrollDialogue = "";
                            quest.RewardPlayer();
                            dialogueState = 0;
                            questDialogue = null;
                            acceptedQuest = false;
                            game.CurrentQuests.Remove(quest);

                            if (!quest.StoryQuest)
                                game.CurrentSideQuests.Remove(quest);

                            Boolean storyQuestInHelper = false;

                            if (Game1.questHUD.questHelperQuests.Contains(quest))
                                storyQuestInHelper = true;

                            Game1.questHUD.RemoveQuestFromHelper(quest);

                            if (quest.StoryQuest && storyQuestInHelper)
                            {
                                for (int i = game.CurrentQuests.Count - 1; i >= 0; i--)
                                {
                                    if (game.CurrentQuests[i].StoryQuest)
                                    {
                                        Game1.questHUD.AddQuestToHelper(game.CurrentQuests[i]);
                                        break;
                                    }
                                }
                            }

                            quest = null;
                            game.CurrentChapter.TalkingToNPC = false;
                            talking = false;
                            MyGamePad.ResetStates();

                            updatingInteraction = false;
                        }
                        else
                        {
                            scrollDialogueNum = 0;
                            scrollDialogue = "";
                            dialogueState++;
                        }
                    }
                }
                #endregion

                if (quest != null && scrollDialogueNum < questDialogue[dialogueState].Length && (game.current.IsKeyDown(Keys.Enter) || MyGamePad.currentState.Buttons.A == ButtonState.Pressed))
                {
                    updateFaster = true;
                }
                else if (game.current.IsKeyUp(Keys.Enter) && MyGamePad.currentState.Buttons.A == ButtonState.Released)
                {
                    updateFaster = false;
                }

            }
            #endregion
        }

        public void SkipQuestDialogue()
        {
            if (quest != null)
            {
                scrollDialogueNum = 0;
                scrollDialogue = "";
                textScrollSoundIndex = 0;

                acceptedQuest = true;
                dialogueState = questDialogue.Count - 1;
                game.CurrentChapter.TalkingToNPC = false;
                talking = false;
                acceptQuestPage = false;
                updatingInteraction = false;
                game.CurrentQuests.Add(quest);
                MyGamePad.ResetStates();

                if (!quest.StoryQuest)
                {
                    game.CurrentSideQuests.Add(quest);
                    Chapter.effectsManager.NotificationQueue.Enqueue(new QuestReceivedNotification(false));
                }
                else
                {
                    if (quest.QuestName != "Daryl's New Friends")
                        Chapter.effectsManager.NotificationQueue.Enqueue(new QuestReceivedNotification(true));

                    //Remove the old story quest from the quest helper
                    for (int j = 0; j < Game1.questHUD.questHelperQuests.Count; j++)
                    {
                        if (Game1.questHUD.questHelperQuests[j].StoryQuest)
                        {
                            Game1.questHUD.RemoveQuestFromHelper(Game1.questHUD.questHelperQuests[j]);
                        }
                    }
                }

                Game1.questHUD.AddQuestToHelper(quest);
            }
        }

        public void AddQuest(Quest q)
        {
            quest = q;
            questDialogue = q.QuestDialogue;
            acceptedQuest = false;
        }

        public void RemoveQuest(Quest q)
        {
            if (quest == q)
            {
                dialogueState = 0;
                questDialogue = null;
                acceptedQuest = false;
                game.CurrentQuests.Remove(quest);

                if (!quest.StoryQuest)
                    game.CurrentSideQuests.Remove(quest);

                quest = null;
                game.CurrentChapter.TalkingToNPC = false;
                talking = false;
            }
            else
            {
                throw new Exception("This quest does not match the NPC's quest");
            }
        }

        public void Wander(int maxToLeft, int maxToRight, Platform.PlatformType floorType = Platform.PlatformType.rock, int footstepFrame1 = 1, int footstepFrame2 = 6)
        {
            if (game.CurrentChapter.CurrentMap.MapName == mapName)
            {
                if (wanderTimer <= 0)
                {
                    wanderState = moveRandom.Next(0, 3);
                    wanderTimer = moveRandom.Next(70, 160);
                }

                wanderTimer--;

                if (tempWanderTimer > 1)
                {
                    tempWanderTimer--;

                    if (tempWanderTimer == 1)
                    {
                        tempWanderTimer = 0;
                        wanderState = 0;
                        wanderTimer = 60;
                    }
                }

                switch (wanderState)
                {
                    case 0:
                        moveState = MoveState.standing;
                        break;
                    case 1:
                        if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                            Chapter.effectsManager.fButtonRecs.Remove(frec);
                        Move(new Vector2(3, 0), floorType, footstepFrame1, footstepFrame2);
                        break;
                    case 2:
                        if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                            Chapter.effectsManager.fButtonRecs.Remove(frec);
                        Move(new Vector2(-3, 0), floorType, footstepFrame1, footstepFrame2);
                        break;
                }

                if (PositionX < maxToLeft)
                {
                    wanderTimer = 20;
                    tempWanderTimer = 20;
                    wanderState = 1;
                }
                if (PositionX > maxToRight)
                {
                    wanderTimer = 20;
                    tempWanderTimer = 20;
                    wanderState = 2;
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {

            //--If the player is in the map, draw the NPC
            if (game.CurrentChapter.state != Chapter.GameState.Cutscene && game.CurrentChapter.CurrentMap.MapName == mapName)
            {
                    if (spriteSheet == Game1.whiteFilter)
                    {
                        spriteSheet = game.NPCSprites[name];
                    }

                if (facingRight)
                {
                    s.Draw(game.NPCSprites[name], rec, GetSourceRectangle(moveFrame), Color.White);
                }
                else
                {
                    s.Draw(game.NPCSprites[name], rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }

                //Don't draw their name or the F button if they cannot talk
                if (canTalk && !game.CurrentChapter.MakingDecision)
                {
                    #region Draw NPC names

                    //--Get the distance from daryl to the NPC
                    Point distanceFromNPC = new Point(Math.Abs(player.VitalRec.Center.X - rec.Center.X),
                    Math.Abs(player.VitalRec.Center.Y - rec.Center.Y));

                    //--If it is less than 250 pixels
                    if (distanceFromNPC.X < 250 && distanceFromNPC.Y < 250 && game.CurrentChapter.state == Chapter.GameState.Game && !game.CurrentChapter.TalkingToNPC && !drawFButton)
                    {

                        Game1.OutlineFont(Game1.font, s, name, 1, (int)(rec.X + ((rec.Width / 2) - (Game1.font.MeasureString(name).X / 2)) - 2), (int)(rec.Y + Game1.npcHeightFromRecTop[name] - 40 - 2), Color.Black, Color.White);

                    }
                    #endregion
                }

                int fButtonOffset = (int)(43 - 43 * .9f) / 2;

                frec = new Rectangle((rec.X + rec.Width / 2) - (43 / 2) + fButtonOffset, rec.Y - 65 + Game1.npcHeightFromRecTop[name] - 30, (int)(43), (int)(65));

                if (drawFButton && canTalk && !game.CurrentChapter.MakingDecision)
                {
                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);
                }

                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);

                    float notiWidth = 123;
                    float notiHeight = 111;

                    if (game.CurrentChapter.TalkingToNPC == false)
                    {
                        #region Draw notifications

                        if (quest != null && quest.StoryQuest)
                            s.Draw(Game1.notificationTextures, new Rectangle((rec.X + rec.Width / 2) - ((int)notiWidth / 2) - 12, rec.Y + Game1.npcHeightFromRecTop[name] - (int)notiHeight - 30 + (int)notificationPos, (int)(notiWidth * 1.2f), (int)(notiHeight * 1.2f)), new Rectangle(737, 0, 123, 111), Color.White);

                        else if (quest != null && !quest.StoryQuest)
                            s.Draw(Game1.notificationTextures, new Rectangle((rec.X + rec.Width / 2) - ((int)notiWidth / 2) - 12, rec.Y + Game1.npcHeightFromRecTop[name] - (int)notiHeight - 30 + (int)notificationPos, (int)(notiWidth * 1.2f), (int)(notiHeight * 1.2f)), new Rectangle(615, 0, 123, 111), Color.White);

                        if (quest != null && !game.CurrentQuests.Contains(quest))
                        {
                            if (notificationPos >= 0)
                            {
                                notificationState++;
                                notificationPos = 0;


                                if (notificationState == 5)
                                    notificationState = 1;
                                else if (notificationState == 1)
                                    notificationVelocity = -8;
                                else if (notificationState == 2)
                                    notificationVelocity = -4;
                                else if (notificationState == 3)
                                    notificationVelocity = -2;
                            }

                            notificationPos += notificationVelocity;
                            notificationVelocity += GameConstants.GRAVITY / 3f;

                            s.Draw(Game1.notificationTextures, new Rectangle((rec.X + rec.Width / 2) - ((int)notiWidth / 2) - 12, rec.Y + Game1.npcHeightFromRecTop[name] - (int)notiHeight - 30 + (int)notificationPos, (int)(notiWidth * 1.2f), (int)(notiHeight * 1.2f)), new Rectangle(0, 0, 123, 111), Color.White);
                        }

                        else if (quest != null && game.CurrentQuests.Contains(quest) && quest.CompletedQuest)
                        {

                            if (notificationPos >= 0)
                            {
                                notificationState++;
                                notificationPos = 0;


                                if (notificationState == 5)
                                    notificationState = 1;
                                else if (notificationState == 1)
                                    notificationVelocity = -8;
                                else if (notificationState == 2)
                                    notificationVelocity = -4;
                                else if (notificationState == 3)
                                    notificationVelocity = -2;
                            }

                            notificationPos += notificationVelocity;
                            notificationVelocity += GameConstants.GRAVITY / 3f;

                            s.Draw(Game1.notificationTextures, new Rectangle((rec.X + rec.Width / 2) - ((int)notiWidth / 2) - 12, rec.Y + Game1.npcHeightFromRecTop[name] - (int)notiHeight - 30 + (int)notificationPos, (int)(notiWidth * 1.2f), (int)(notiHeight * 1.2f)), new Rectangle(123, 0, 123, 111), Color.White);
                        }

                        else if (quest != null && game.CurrentQuests.Contains(quest) && !quest.CompletedQuest)
                        {
                            if (notificationState > 3)
                                notificationState = 1;

                            if (notificationTimer <= 0)
                            {
                                notificationTimer = 30;
                                notificationState++;

                                if (notificationState > 3)
                                    notificationState = 1;
                            }
                            else
                                notificationTimer--;

                            notificationPos = 0;

                            int sourceRecPosX = 246 + (123* (notificationState - 1));

                            s.Draw(Game1.notificationTextures, new Rectangle((rec.X + rec.Width / 2) - ((int)notiWidth / 2) - 12, rec.Y + Game1.npcHeightFromRecTop[name] - (int)notiHeight - 30, (int)(notiWidth * 1.2f), (int)(notiHeight * 1.2f)), new Rectangle(sourceRecPosX, 0, 123, 111), Color.White);
                        }
                        #endregion
                    }
                }
            }

            else if (game.CurrentChapter.state == Chapter.GameState.Cutscene && game.CurrentChapter.CurrentMap.MapName == mapName)
            {
                if (spriteSheet == Game1.whiteFilter)
                {
                    spriteSheet = game.NPCSprites[name];
                }

                if (facingRight)
                {
                    s.Draw(game.NPCSprites[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
                }
                else
                {
                    s.Draw(game.NPCSprites[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }

        public void CompleteQuestSilently(Quest q)
        {
            scrollDialogueNum = 0;
            textScrollSoundIndex = 0;
            scrollDialogue = "";
            q.RewardPlayer();
            dialogueState = 0;
            questDialogue = null;
            acceptedQuest = false;
            game.CurrentQuests.Remove(q);

            if (!q.StoryQuest)
                game.CurrentSideQuests.Remove(q);

            Boolean storyQuestInHelper = false;

            if (Game1.questHUD.questHelperQuests.Contains(q))
                storyQuestInHelper = true;

            Game1.questHUD.RemoveQuestFromHelper(q);

            if (q.StoryQuest && storyQuestInHelper)
            {
                for (int i = game.CurrentQuests.Count - 1; i >= 0; i--)
                {
                    if (game.CurrentQuests[i].StoryQuest)
                    {
                        Game1.questHUD.AddQuestToHelper(game.CurrentQuests[i]);
                        break;
                    }
                }
            }

            quest = null;
            game.CurrentChapter.TalkingToNPC = false;
            talking = false;
            updatingInteraction = false;
        }

        public virtual void DrawDialogue(SpriteBatch s)
        {
            //--When talking to the NPC
            if (talking)
            {
                moveState = MoveState.standing;

                s.Draw(Game1.notificationTextures, new Rectangle(38, (int)(Game1.aspectRatio * 1280 * .55) - 3, 1080, 327), new Rectangle(0, 155, 1080, 327), Color.White);

                    s.Draw(Game1.npcFaces[name].faces[currentDialogueFace], new Rectangle(0, (int)(Game1.aspectRatio * 1280) - Game1.npcFaces[name].faces[currentDialogueFace].Height, Game1.npcFaces[name].faces[currentDialogueFace].Width, Game1.npcFaces[name].faces[currentDialogueFace].Height), Color.White);

                Game1.OutlineFont(Game1.questNameFont, s, name, 1, 351, 538, Color.Black, Color.White);

                #region Non Quest
                if ((questDialogue == null || questDialogue.Count == 0))
                {
                    String currentLine = Game1.WrapText(Game1.dialogueFont, dialogue[dialogueState], 660);
                    stringNum = currentLine.Length;


                    //--Scroll text
                    if (scrollDialogueNum < stringNum)
                    {
                        scrollDialogue += currentLine.ElementAt(scrollDialogueNum);
                        scrollDialogueNum++;
                        textScrollSoundIndex++;

                        //--Add a second letter for updating faster
                        if (updateFaster && scrollDialogueNum < stringNum)
                        {
                            scrollDialogue += currentLine.ElementAt(scrollDialogueNum);
                            scrollDialogueNum++;
                            textScrollSoundIndex++;

                        }

                        if (textScrollSoundIndex > 4)
                            textScrollSoundIndex = 0;
                        //--Scroll text noise. Faster then you're updating text quickly
                        if (textScrollSoundIndex == 0)
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_scroll);
                    }

                    if (updatingInteraction && scrollDialogueNum == stringNum)
                    {
                        if (enterAlphaIncreasing)
                        {
                            enterAlpha += .05f;

                            if (enterAlpha >= 1)
                                enterAlphaIncreasing = false;
                        }
                        else
                        {
                            enterAlpha -= .05f;

                            if (enterAlpha <= 0)
                                enterAlphaIncreasing = true;
                        }

                        if (!Game1.gamePadConnected)
                            s.Draw(Game1.notificationTextures, new Rectangle(974, 639, 50, 22), new Rectangle(1774, 0, 50, 22), Color.White);
                        else
                            s.Draw(Game1.AButton, new Vector2(0, 0), Color.White);
                    }

                    s.DrawString(Game1.dialogueFont, Game1.WrapText(Game1.dialogueFont, scrollDialogue, 660), new Vector2(360, (int)(Game1.aspectRatio * 1280 * .8f)), Color.Black);
                }
                #endregion

                #region Quest
                else
                {
                    //--Measure the name of the quest to center it
                    Vector2 nameLength = Game1.questNameFont.MeasureString(quest.QuestName);
                    float vecX = (nameLength.X / 2);


                    String currentLine = Game1.WrapText(Game1.dialogueFont, questDialogue[dialogueState], 660);
                    stringNum = currentLine.Length;
                    if (scrollDialogueNum < stringNum)
                    {
                        scrollDialogue += currentLine.ElementAt(scrollDialogueNum);
                        scrollDialogueNum++;
                        textScrollSoundIndex++;

                        if (updateFaster && scrollDialogueNum < stringNum)
                        {
                            scrollDialogue += currentLine.ElementAt(scrollDialogueNum);
                            scrollDialogueNum++;
                            textScrollSoundIndex++;
                        }


                        if (textScrollSoundIndex > 4)
                            textScrollSoundIndex = 0;
                        //--Scroll text noise. Faster then you're updating text quickly
                        if (textScrollSoundIndex == 0)
                            Sound.PlaySoundInstance(Sound.SoundNames.ui_general_text_scroll);
                    }

                    if (updatingInteraction && scrollDialogueNum == stringNum && drawEnter)
                    {
                        if(!Game1.gamePadConnected)
                            s.Draw(Game1.notificationTextures, new Rectangle(974, 639, 50, 22), new Rectangle(1774, 0, 50, 22), Color.White);
                        else
                            s.Draw(Game1.AButton, new Vector2(0, 0), Color.White);

                    }
                    //--Draw the text
                    s.DrawString(Game1.dialogueFont, Game1.WrapText(Game1.dialogueFont, scrollDialogue, 660), new Vector2(360, (int)(Game1.aspectRatio * 1280 * .8f)), Color.Black);

                    #region Cancelling a quest
                    if (acceptQuestPage == false && acceptedQuest == true && quest.CompletedQuest == false && quest.StoryQuest == false)
                    {
                        #region Flier
                        s.Draw(Game1.questWantedTexture, new Rectangle(444, 37, Game1.questWantedTexture.Width, Game1.questWantedTexture.Height), Color.White);
                        s.DrawString(Game1.questNameFont, quest.QuestName, new Vector2(444 + (Game1.questWantedTexture.Width / 2) - vecX, 185), new Color(102, 45, 144));
                        s.DrawString(Game1.twConQuestHudInfo, Game1.WrapText(Game1.twConQuestHudInfo, quest.ConditionsToComplete, 350), new Vector2(505, 265), Color.Black);


                        if (!(quest.RewardObjects.OfType<Money>().Any()))
                        {
                            s.DrawString(Game1.twConQuestHudInfo, "0.00", new Vector2(610, 487), Color.Black);
                        }
                        if (!(quest.RewardObjects.OfType<Experience>().Any()))
                        {
                            s.DrawString(Game1.twConQuestHudInfo, "0", new Vector2(700, 487), Color.Black);
                        }
                        if (!(quest.RewardObjects.OfType<Karma>().Any()))
                        {
                            s.DrawString(Game1.twConQuestHudInfo, "0", new Vector2(533, 487), Color.Black);
                        }

                        for (int i = 0; i < quest.RewardObjects.Count; i++)
                        {
                            if (quest.RewardObjects[i] is Equipment)
                            {
                                if (quest.RewardObjects[i] is Money)
                                {
                                    s.DrawString(Game1.twConQuestHudInfo, (quest.RewardObjects[i] as Money).Amount.ToString("N2"), new Vector2(610, 487), Color.Black);
                                }
                                else if (quest.RewardObjects[i] is Karma)
                                {
                                    s.DrawString(Game1.twConQuestHudInfo, (quest.RewardObjects[i] as Karma).Amount.ToString(), new Vector2(533, 487), Color.Black);
                                }
                                else if (quest.RewardObjects[i] is Experience)
                                {
                                    s.DrawString(Game1.twConQuestHudInfo, (quest.RewardObjects[i] as Experience).Amount.ToString(), new Vector2(700, 485), Color.Black);

                                }
                                else
                                {
                                    if (quest.RewardObjects[i] is Weapon)
                                        s.Draw(Game1.smallTypeIcons["smallWeaponIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                    if (quest.RewardObjects[i] is Hat)
                                        s.Draw(Game1.smallTypeIcons["smallHatIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                    if (quest.RewardObjects[i] is Outfit)
                                        s.Draw(Game1.smallTypeIcons["smallShirtIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                    if (quest.RewardObjects[i] is Accessory)
                                        s.Draw(Game1.smallTypeIcons["smallAccessoryIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);

                                    s.DrawString(Game1.twConQuestHudName, "     " + (quest.RewardObjects[i] as Equipment).Name, new Vector2(503, 405 + (i * 25)), Color.Black * .8f);
                                }
                            }
                            if (quest.RewardObjects[i] is Collectible)
                            {
                                if (quest.RewardObjects[i] is Textbook)
                                    s.Draw(Game1.smallTypeIcons["smallTextbookIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                if (quest.RewardObjects[i] is BronzeKey)
                                    s.Draw(Game1.smallTypeIcons["smallBronzeKeyIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                if (quest.RewardObjects[i] is SilverKey)
                                    s.Draw(Game1.smallTypeIcons["smallSilverKeyIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                if (quest.RewardObjects[i] is GoldKey)
                                    s.Draw(Game1.smallTypeIcons["smallGoldKeyIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                if (quest.RewardObjects[i] is LockerCombo)
                                    s.Draw(Game1.smallTypeIcons["smallComboIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);

                                if (!(quest.RewardObjects[i] is LockerCombo))
                                    s.DrawString(Game1.twConQuestHudName, "     " + (quest.RewardObjects[i] as Collectible).collecName, new Vector2(503, 405 + (i * 25)), Color.Black * .8f);
                                else
                                    s.DrawString(Game1.twConQuestHudName, "     " + (quest.RewardObjects[i] as LockerCombo).name + "\'s locker combination", new Vector2(503, 405 + (i * 25)), Color.Black * .8f);
                            }
                        }
                        #endregion

                        //--Draw the choices
                        if (choice == 0)
                        {

                            s.Draw(Game1.notificationTextures, new Rectangle(817, 475, 350, 115), new Rectangle(1825, 0, 350, 115), Color.White);
                            s.Draw(Game1.notificationTextures, new Rectangle(1012, 536, 155, 115), new Rectangle(2982, 4, 155, 115), Color.White);
                        }
                        else
                        {
                            s.Draw(Game1.notificationTextures, new Rectangle(817, 475, 350, 115), new Rectangle(2175, -4, 350, 115), Color.White);
                            s.Draw(Game1.notificationTextures, new Rectangle(1012, 536, 155, 115), new Rectangle(2827, 0, 155, 115), Color.White);
                        }

                        drawEnter = false;
                    }
                    #endregion

                    #region Accepting a quest
                    else if (!quest.StoryQuest && (acceptQuestPage || (acceptedQuest == true && quest.CompletedQuest == false && quest.StoryQuest == false)))
                    {
                        #region Flier
                        s.Draw(Game1.questWantedTexture, new Rectangle(444, 37, Game1.questWantedTexture.Width, Game1.questWantedTexture.Height), Color.White);
                        s.DrawString(Game1.questNameFont, quest.QuestName, new Vector2(444 + (Game1.questWantedTexture.Width / 2) - vecX, 185), new Color(102, 45, 144));
                        s.DrawString(Game1.twConQuestHudInfo, Game1.WrapText(Game1.twConQuestHudInfo, quest.ConditionsToComplete, 350), new Vector2(505, 265), Color.Black);


                        if (!(quest.RewardObjects.OfType<Money>().Any()))
                        {
                            s.DrawString(Game1.twConQuestHudInfo, "0.00", new Vector2(610, 487), Color.Black);
                        }
                        if (!(quest.RewardObjects.OfType<Experience>().Any()))
                        {
                            s.DrawString(Game1.twConQuestHudInfo, "0", new Vector2(700, 487), Color.Black);
                        }
                        if (!(quest.RewardObjects.OfType<Karma>().Any()))
                        {
                            s.DrawString(Game1.twConQuestHudInfo, "0", new Vector2(533, 487), Color.Black);
                        }

                        for (int i = 0; i < quest.RewardObjects.Count; i++)
                        {
                            if (quest.RewardObjects[i] is Equipment)
                            {
                                if (quest.RewardObjects[i] is Money)
                                {
                                    s.DrawString(Game1.twConQuestHudInfo, (quest.RewardObjects[i] as Money).Amount.ToString("N2"), new Vector2(610, 487), Color.Black);
                                }
                                else if (quest.RewardObjects[i] is Karma)
                                {
                                    s.DrawString(Game1.twConQuestHudInfo, (quest.RewardObjects[i] as Karma).Amount.ToString(), new Vector2(533, 487), Color.Black);
                                }
                                else if (quest.RewardObjects[i] is Experience)
                                {
                                    s.DrawString(Game1.twConQuestHudInfo, (quest.RewardObjects[i] as Experience).Amount.ToString(), new Vector2(700, 485), Color.Black);

                                }
                                else
                                {
                                    if (quest.RewardObjects[i] is Weapon)
                                        s.Draw(Game1.smallTypeIcons["smallWeaponIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                    if (quest.RewardObjects[i] is Hat)
                                        s.Draw(Game1.smallTypeIcons["smallHatIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                    if (quest.RewardObjects[i] is Outfit)
                                        s.Draw(Game1.smallTypeIcons["smallShirtIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                    if (quest.RewardObjects[i] is Accessory)
                                        s.Draw(Game1.smallTypeIcons["smallAccessoryIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);

                                    s.DrawString(Game1.twConQuestHudName, "     " + (quest.RewardObjects[i] as Equipment).Name, new Vector2(503, 405 + (i * 25)), Color.Black * .8f);
                                }
                            }
                            if (quest.RewardObjects[i] is Collectible)
                            {
                                if (quest.RewardObjects[i] is Textbook)
                                    s.Draw(Game1.smallTypeIcons["smallTextbookIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                if (quest.RewardObjects[i] is BronzeKey)
                                    s.Draw(Game1.smallTypeIcons["smallBronzeKeyIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                if (quest.RewardObjects[i] is SilverKey)
                                    s.Draw(Game1.smallTypeIcons["smallSilverKeyIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                if (quest.RewardObjects[i] is GoldKey)
                                    s.Draw(Game1.smallTypeIcons["smallGoldKeyIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);
                                if (quest.RewardObjects[i] is LockerCombo)
                                    s.Draw(Game1.smallTypeIcons["smallComboIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);

                                if (!(quest.RewardObjects[i] is LockerCombo))
                                    s.DrawString(Game1.twConQuestHudName, "     " + (quest.RewardObjects[i] as Collectible).collecName, new Vector2(503, 405 + (i * 25)), Color.Black * .8f);
                                else
                                    s.DrawString(Game1.twConQuestHudName, "     " + (quest.RewardObjects[i] as LockerCombo).name +"\'s locker combination", new Vector2(503, 405 + (i * 25)), Color.Black * .8f);
                            }
                            if (quest.RewardObjects[i] is StoryItem)
                            {
                                    s.Draw(Game1.smallTypeIcons["smallStoryItemIcon"], new Rectangle(503, 405 + (i * 25), Game1.smallTypeIcons["smallExperienceIcon"].Width, Game1.smallTypeIcons["smallExperienceIcon"].Height), Color.White);

                                    s.DrawString(Game1.twConQuestHudName, "     " + (quest.RewardObjects[i] as StoryItem).Name, new Vector2(503, 405 + (i * 25)), Color.Black * .8f);
                            }
                        }
                        #endregion

                        //--Draw the choices
                        if (choice == 0)
                        {

                            s.Draw(Game1.notificationTextures, new Rectangle(1015, 533, 151, 115), new Rectangle(3137, 0, 151, 115), Color.White);
                            s.Draw(Game1.notificationTextures, new Rectangle(1015, 589, 151, 115), new Rectangle(2676, 0, 151, 115), Color.White);
                        }
                        else
                        {
                            s.Draw(Game1.notificationTextures, new Rectangle(1015, 533, 151, 115), new Rectangle(3288, 0, 151, 115), Color.White);
                            s.Draw(Game1.notificationTextures, new Rectangle(1015, 589, 151, 115), new Rectangle(2525, 0, 151, 115), Color.White);
                        }

                        drawEnter = false;
                    }
                    else
                        drawEnter = true;
                    #endregion

                }
                #endregion
            }
        }
    }
}

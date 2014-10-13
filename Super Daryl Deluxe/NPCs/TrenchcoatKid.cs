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
    public class TrenchcoatKid : NPC
    {
        List<ItemForSale> itemsOnSale;
        List<Boolean> soldOut;


        public List<ItemForSale> ItemsOnSale { get { return itemsOnSale; } set { itemsOnSale = value; } }
        public List<Boolean> SoldOut { get { return soldOut; } set { soldOut = value; } }
        public Texture2D SpriteSheet { get { return spriteSheet; } set { spriteSheet = value; } }

        public TrenchcoatKid(Texture2D sprite, List<String> d, int x, int y, Player play, Game1 g, String mName, List<ItemForSale> items)
            :base (sprite, g, "Trenchcoat Employee")
        {
            dialogue = d;
            dialogueState = 0;
            player = play;
            talking = false;
            game = g;
            mapName = mName;
            name = "Trenchcoat Employee";
            rec = new Rectangle();
            itemsOnSale = items;
            soldOut = new List<bool>();

            for (int i = 0; i < itemsOnSale.Count; i++)
            {
                soldOut.Add(false);
            }

            rec.Width = 516;
            rec.Height = 388;
            rec.X = x;
            rec.Y = y - rec.Height;
            position.X = x;
            position.Y = y - rec.Height;

            currentDialogueFace = "Normal";
            staticNPC = false;
        }

        public override Rectangle GetSourceRectangle(int frame)
        {
            if (!staticNPC)
            {
                switch (moveState)
                {
                    case MoveState.standing:
                        return new Rectangle(0, 0, 516, 360);

                    case MoveState.moving:
                        if (frame < 5)
                        {
                            return new Rectangle(frame * 518, 388, 518, 388);
                        }
                        else
                        {
                            int column = frame - 5;
                            return new Rectangle(column * 518, 388 * 2, 518, 388);
                        }
                }
            }
            else
            {
                return new Rectangle(0, 0, spriteSheet.Width, spriteSheet.Height);
            }

            return new Rectangle(0, 0, 0, 0);
        }

        public override void Update()
        {
            base.Update();

            if (game.CurrentChapter.CurrentMap.MapName == mapName)
            {
                if (talking == false)
                {
                    CheckInteraction();
                }

                if (talking)
                {
                    UpdateInteraction();
                }
            }
        }

        //--Update dialogue when you press enter
        public override void UpdateInteraction()
        {

            last = current;
            current = Keyboard.GetState();

            updatingInteraction = true;

            if (((last.IsKeyDown(Keys.Enter) && current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed()) && scrollDialogueNum >= dialogue[dialogueState].Length && updateFaster == false)
            {
                if(game.CurrentChapter.state != Chapter.GameState.Cutscene)
                    game.Shop.LoadContent();

                scrollDialogueNum = 0;
                dialogueState++;
                scrollDialogue = "";
                if (dialogueState == dialogue.Count)
                {
                    game.CurrentChapter.TalkingToNPC = false;
                    talking = false;
                    dialogueState = 0;
                    updatingInteraction = false;

                    if (game.CurrentChapter.state != Chapter.GameState.Cutscene)
                    {
                        game.CurrentChapter.state = Chapter.GameState.shop;
                        game.Shop.TrenchcoatKid = this;
                    }
                }

            }

            //--Update the text faster
            if (scrollDialogueNum < dialogue[dialogueState].Length && (current.IsKeyDown(Keys.Enter) || MyGamePad.currentState.Buttons.A == ButtonState.Pressed))
            {
                updateFaster = true;
            }
            else if (current.IsKeyUp(Keys.Enter) && MyGamePad.currentState.Buttons.A == ButtonState.Released)
            {
                updateFaster = false;
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
                    s.Draw(spriteSheet, rec, GetSourceRectangle(moveFrame), Color.White);
                }
                else
                {
                    s.Draw(spriteSheet, rec, GetSourceRectangle(moveFrame), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }

                #region Draw NPC names

                //--Get the distance from daryl to the NPC
                Point distanceFromNPC = new Point(Math.Abs(player.VitalRec.Center.X - rec.Center.X),
                Math.Abs(player.VitalRec.Center.Y - rec.Center.Y));

                //--If it is less than 250 pixels
                if (distanceFromNPC.X < 250 && distanceFromNPC.Y < 250 && game.CurrentChapter.state == Chapter.GameState.Game && !game.CurrentChapter.TalkingToNPC && !drawFButton)
                {
                    s.DrawString(Game1.font, name, new Vector2(rec.X + ((rec.Width / 2) - (Game1.font.MeasureString(name).X / 2)) - 2, rec.Y + Game1.npcHeightFromRecTop["Trenchcoat Employee"] - 40 - 2), Color.Black);

                    s.DrawString(Game1.font, name, new Vector2(rec.X + ((rec.Width / 2) - (Game1.font.MeasureString(name).X / 2)), rec.Y + Game1.npcHeightFromRecTop["Trenchcoat Employee"] - 40), Color.White);//new Color(241, 107, 79));
                }
                #endregion

                int fButtonOffset = (int)(43 - 43 * .9f) / 2;

                frec = new Rectangle((rec.X + rec.Width / 2) - (43 / 2) + fButtonOffset, rec.Y - 55 + Game1.npcHeightFromRecTop["Paul"] - 30, (int)(43), (int)(65));

                if (drawFButton)
                {
                    if (!Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.AddFButton(frec);
                }

                else
                {
                    if (Chapter.effectsManager.fButtonRecs.Contains(frec))
                        Chapter.effectsManager.fButtonRecs.Remove(frec);

                    //Size of the texture
                    float notiWidth = 123;
                    float notiHeight = 111;

                    #region Draw notifications

                    if (quest != null && quest.StoryQuest)
                        s.Draw(Game1.notificationTextures, new Rectangle((rec.X + rec.Width / 2) - ((int)notiWidth / 2) - 12, rec.Y + Game1.npcHeightFromRecTop[name] - (int)notiHeight - 30 + (int)notificationPos, (int)(notiWidth * 1.2f), (int)(notiHeight * 1.2f)), new Rectangle(738, 0, 123, 111), Color.White);

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

                        s.Draw(Game1.notificationTextures, new Rectangle((rec.X + rec.Width / 2) - ((int)notiWidth / 2) - 12, rec.Y + Game1.npcHeightFromRecTop[name] - (int)notiHeight - 30 + (int)notificationPos, (int)(notiWidth * 1.2f), (int)(notiHeight * 1.2f)),  new Rectangle(0, 0, 123, 111), Color.White);
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
                        String notNum = "current" + notificationState;

                        int sourceRecPosX = 246 * notificationState;

                        s.Draw(Game1.notificationTextures, new Rectangle((rec.X + rec.Width / 2) - ((int)notiWidth / 2) - 12, rec.Y + Game1.npcHeightFromRecTop[name] - (int)notiHeight - 30, (int)(notiWidth * 1.2f), (int)(notiHeight * 1.2f)), new Rectangle(sourceRecPosX, 0, 123, 111), Color.White);
                    }
                    #endregion
                }

            }
            else if (game.CurrentChapter.state == Chapter.GameState.Cutscene && game.CurrentChapter.CurrentMap.MapName == mapName)
            {
                if (facingRight)
                {
                    s.Draw(spriteSheet, rec, GetSourceRectangle(moveFrame), Color.White * alpha);
                }
                else
                {
                    s.Draw(spriteSheet, rec, GetSourceRectangle(moveFrame), Color.White * alpha, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }

        public override void DrawDialogue(SpriteBatch s)
        {
            //--When talking to the NPC
            if (talking)
            {

                //s.Draw(Game1.whiteFilter, faceRec, Color.LightGray);
                s.Draw(Game1.notificationTextures, new Rectangle(38, 393, 1080, 327), new Rectangle(0, 155, 1080, 327), Color.White);

                s.Draw(Game1.npcFaces[name].faces[currentDialogueFace], new Rectangle(0, (int)(Game1.aspectRatio * 1280) - Game1.npcFaces[name].faces[currentDialogueFace].Height, Game1.npcFaces[name].faces[currentDialogueFace].Width, Game1.npcFaces[name].faces[currentDialogueFace].Height), Color.White);

                String currentLine = Game1.WrapText(Game1.dialogueFont, dialogue[dialogueState], 660);
                stringNum = currentLine.Length;

                //--Scroll text
                if (scrollDialogueNum < stringNum)
                {
                    scrollDialogue += currentLine.ElementAt(scrollDialogueNum);
                    scrollDialogueNum++;

                    //--Scroll text noise. Faster then you're updating text quickly
                    if (scrollDialogueNum % 5 == 0 && updateFaster == false)
                        Sound.PlaySoundInstance(Sound.SoundNames.TextScroll);
                    else if (scrollDialogueNum % 4 == 0 && updateFaster == true)
                        Sound.PlaySoundInstance(Sound.SoundNames.TextScroll);

                    //--Add a second letter for updating faster
                    if (updateFaster && scrollDialogueNum < stringNum)
                    {
                        scrollDialogue += currentLine.ElementAt(scrollDialogueNum);
                        scrollDialogueNum++;

                    }

                }

                if (updatingInteraction && scrollDialogueNum == stringNum)
                    s.Draw(Game1.notificationTextures, new Rectangle(974, 639, 50, 22), new Rectangle(1774, 0, 50, 22), Color.White);

                s.DrawString(Game1.dialogueFont, Game1.WrapText(Game1.dialogueFont, scrollDialogue, 660), new Vector2(360, (int)(Game1.aspectRatio * 1280 * .8f)), Color.Black);
            }
        }
    }
}

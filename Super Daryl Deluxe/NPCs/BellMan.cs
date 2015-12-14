using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    class BellMan : NPC
    {
        //--Constructor for quest NPC
        public BellMan(Texture2D sprite, List<String> d, Quest q, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name,
            Boolean stat)
            : base(sprite, d, q, r, play, f, g, mName, name, stat)
        {

        }

        //--Constructor for non-Quest NPC
        public BellMan(Texture2D sprite, List<String> d, Rectangle r, Player play, SpriteFont f, Game1 g, String mName, String name, Boolean stat)
            : base(sprite, d, r, play, f, g, mName, name, stat)
        {

        }

        public override Rectangle GetSourceRectangle(int frame)
        {
            if (facingRight)
            {
                return new Rectangle(0, 0, 516, 388);
            }

            else
            {
                return new Rectangle(516, 0, 516, 388);
            }
        }

        public override void Update()
        {
            base.Update();
            if (game.CurrentChapter.CurrentMap.MapName == mapName)
            {
                frameDelay--;

                if (frameDelay <= 0)
                {
                    moveFrame++;
                    frameDelay = 5;

                    if (moveFrame > 1)
                        moveFrame = 0;
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

                    s.Draw(game.NPCSprites[name], rec, GetSourceRectangle(moveFrame), Color.White);

                //Don't draw their name or the F button if they cannot talk
                if (canTalk)
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

                if (drawFButton && canTalk)
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

                            int sourceRecPosX = 246 + (123 * (notificationState - 1));

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

                    s.Draw(game.NPCSprites[name], rec, GetSourceRectangle(moveFrame), Color.White * alpha);
            }
        }
    }
}

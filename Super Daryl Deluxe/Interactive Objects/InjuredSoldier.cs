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
    public class InjuredSoldier : InteractiveObject
    {
        int timeToHeal;
        Boolean healed = false;
        Boolean healing = false;
        Player player;
        Rectangle openBar;
        float openBarWidth;
        public Boolean showFButton = true;

        public Boolean Healing { get { return healing; } set { healing = value; } }
        public Boolean Healed { get { return healed; } set { healed = value; } }
        public Rectangle OpenBar { get { return openBar; } set { openBar = value; } }
        List<String> thanks;

        public enum HealedState
        {
            normal, skeleton, goblin
        }
        public HealedState healedState;

        public InjuredSoldier(int x, int y, Player p, Boolean foreground, HealedState healedState = HealedState.normal, Boolean faceRight = true) 
            : base(Game1.g, foreground)
        {
            rec = new Rectangle(x, y, 516, 398);
            timeToHeal = 350;
            player = p;
            openBar = new Rectangle(rec.X + rec.Width / 2 - 50, rec.Y, 0, 20);
            this.healedState = healedState;
            facingRight = faceRight;

            thanks = new List<string>()
            {
                "Thanks, soldier!",
                "Well that was weird.",
                "I feel...funny.",
                "It's a miracle!",
                "Who knew Death had a gambling problem?"
            };
        }

        public override void Update()
        {
            if (!healed && player.StoryItems.ContainsKey("Goblin Morphine") && game.CurrentSideQuests.Contains(game.SideQuestManager.soldierRepairs))
            {
                if (game.last.IsKeyDown(Keys.F) && game.current.IsKeyDown(Keys.F) && nearPlayer() && player.playerState == Player.PlayerState.standing)
                {
                    openBar.Height = game.EnemySpriteSheets["HealthBar"].Height;
                    openBar.Y = rec.Y + 227;
                    openBar.X = rec.X + rec.Width / 2 - 48;
                    healing = true;
                    HealSoldier();
                    openBarWidth += 100f / 240f;
                    openBar.Width = (int)openBarWidth;
                }
                else
                {
                    if (timeToHeal != 240 && !healed)
                        timeToHeal = 240;

                    openBarWidth = 0;
                    openBar.Width = 0;
                    healing = false;
                }
            }
            else if (healed && !finished)
            {
                healing = false;
                finished = true;
                Chapter.effectsManager.AddSmokePoof(new Rectangle(rec.Center.X - 125, rec.Center.Y - 75, 250, 250), 2);
            }

            if (finished && !healed)
                healed = true;
        }

        public bool nearPlayer()
        {
            Point distanceFromPlayer = new Point(Math.Abs(player.VitalRec.Center.X - rec.Center.X),
Math.Abs(player.VitalRec.Center.Y - rec.Center.Y));

            
            if (distanceFromPlayer.X < 175 && distanceFromPlayer.Y < 100)
                return true;

            return false;
        }

        public void HealSoldier()
        {
            if (timeToHeal > 0)
                timeToHeal--;
            if (timeToHeal <= 0)
            {
                healed = true;

                player.RemoveStoryItem("Goblin Morphine", 1);

                if (healedState == HealedState.normal)
                    Chapter.effectsManager.AddInGameDialogue(thanks[Game1.randomNumberGen.Next(thanks.Count - 1)], "French Soldier", "Normal", 100);

                if (healedState == HealedState.goblin)
                {
                    //The rest
                    Goblin ben = new Goblin(new Vector2(), "Goblin", game, ref player, game.CurrentChapter.CurrentMap);
                    ben.Hostile = true;
                    ben.StopAttack();
                    ben.SpawnWithPoof = false;
                    if (player.CurrentPlat != null)
                        ben.Position = new Vector2(rec.Center.X - (ben.Rec.Width / 2), player.CurrentPlat.Rec.Y - ben.Rec.Height - 1);
                    else
                        ben.Position = new Vector2(rec.Center.X - (ben.Rec.Width / 2), rec.Y + 70);
                   game.CurrentChapter.CurrentMap.AddEnemyToEnemyList(ben);

                    if(game.CurrentChapter.CurrentMap.EnemyNamesAndNumberInMap.ContainsKey("Goblin"))
                        game.CurrentChapter.CurrentMap.EnemyNamesAndNumberInMap["Goblin"]++;
                }

            }
        }
        

        public override Rectangle GetSourceRec()
        {
            if (!healed)
                return new Rectangle(516, 0, 516, 398);
            else
            {
                switch (healedState)
                {
                    case HealedState.normal:
                        return new Rectangle(0, 0, 516, 398);
                    case HealedState.skeleton:
                        return new Rectangle(1032, 0, 516, 398);

                }
            }

            return new Rectangle(0, 0, 0, 0);

        }

        public override void Draw(SpriteBatch s)
        {
            if(facingRight)
                s.Draw(sprite, rec, GetSourceRec(), Color.White);
            else
                s.Draw(sprite, rec, GetSourceRec(), Color.White,0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);


            Rectangle fRec = new Rectangle(rec.X + rec.Width / 2 - 43 / 2, rec.Y + 200, 43,
65);

            if (healing)
            {
                float greenColor, redColor;

                if (openBar.Width > (100 / 2))
                {
                    greenColor = 1;
                    redColor = (1f - ((float)openBar.Width / (float)100));
                }
                else
                {
                    redColor = 1;
                    greenColor = (((float)openBar.Width / ((float)100 / 2f)));
                }

                Game1.OutlineFont(Game1.font, s, "Healing...", 2, rec.X + rec.Width / 2 - 32, rec.Y + 200, Color.White, Color.DarkOrange);
                //s.Draw(Game1.emptyBox, new Rectangle(rec.X + rec.Width / 2 - 50, rec.Y + 175, 100, 20), Color.DarkGray * .8f);
                //s.Draw(Game1.emptyBox, openBar, new Color(redColor, greenColor, 0));

                s.Draw(game.EnemySpriteSheets["HealthBox"], new Rectangle(rec.X + rec.Width / 2 - 50, rec.Y + 225, game.EnemySpriteSheets["HealthBox"].Width, game.EnemySpriteSheets["HealthBox"].Height), Color.White);
                s.Draw(game.EnemySpriteSheets["HealthBar"], openBar, new Color(redColor, greenColor, 0));
                s.Draw(game.EnemySpriteSheets["HealthBar"], openBar, Color.Gray * .4f);

                if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                    Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);
            }

            else if (healed)
            {
                if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                    Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);
            }
            else
            {
                if (nearPlayer() && player.StoryItems.ContainsKey("Goblin Morphine") && game.CurrentSideQuests.Contains(game.SideQuestManager.soldierRepairs))
                {

                    if (showFButton)
                    {
                        if (!Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                            Chapter.effectsManager.AddForeroundFButton(fRec);
                    }
                }
                else
                {
                    if (Chapter.effectsManager.foregroundFButtonRecs.Contains(fRec))
                        Chapter.effectsManager.foregroundFButtonRecs.Remove(fRec);
                }
            }
        }

    }
}

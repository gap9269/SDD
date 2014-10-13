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
    public class LevelUpScreen : BaseMenu
    {
        Button strUp;
        Button defUp;
        Button motUp;

        Button strDown;
        Button defDown;
        Button motDown;

        Button exit;

        int addedStr;
        int addedDef;
        int addedMot;

        List<Button> plusButtons;
        List<Button> minusButtons;

        Dictionary<String, Texture2D> textures;
    
        public LevelUpScreen(Texture2D b, Game1 g, Dictionary<String,Texture2D> textures)
            : base(b, g)
        {

            this.textures = textures;

            int plusWidth = textures["Plus"].Width;
            int plusHeight = textures["Plus"].Height;

            strUp = new Button(textures["Plus"], new Rectangle(200, 450, plusWidth, plusHeight));
            defUp = new Button(textures["Plus"], new Rectangle(550, 420, plusWidth, plusHeight));
            motUp = new Button(textures["Plus"], new Rectangle(900, 450, plusWidth, plusHeight));

            strDown = new Button(textures["Minus"], new Rectangle(300, 450, plusWidth, plusHeight));
            defDown = new Button(textures["Minus"], new Rectangle(650, 420, plusWidth, plusHeight));
            motDown = new Button(textures["Minus"], new Rectangle(1000, 450, plusWidth, plusHeight));

            exit = new Button(Game1.emptyBox, new Rectangle(400, 650, 480, 50));

            plusButtons = new List<Button>();
            minusButtons = new List<Button>();


            plusButtons.Add(strUp);
            plusButtons.Add(defUp);
            plusButtons.Add(motUp);

            minusButtons.Add(strDown);
            minusButtons.Add(defDown);
            minusButtons.Add(motDown);

            buttons.Add(strUp);
            buttons.Add(defUp);
            buttons.Add(motUp);
            buttons.Add(strDown);
            buttons.Add(defDown);
            buttons.Add(motDown);

            buttons.Add(exit);
            UpdateResolution();
        }

        public void UpdateResolution()
        {

            strUp.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .6) + 18;//450
            defUp.ButtonRecY = (int)(Game1.aspectRatio * 1280  * .6) - 12;//420
            motUp.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .6) + 18;//450

            strDown.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .6) + 18;//450
            defDown.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .6) - 12;//420
            motDown.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .6) + 18;//450

            exit.ButtonRecY = (int)(Game1.aspectRatio * 1280 * .9) + 2;//650
        }

        public override void Update()
        {
            base.Update();

            if ((previous.IsKeyDown(Keys.Back) && current.IsKeyUp(Keys.Back)) || exit.Clicked())
            {
                addedDef = 0;
                addedMot = 0;
                addedStr = 0;
                game.CurrentChapter.state = Chapter.GameState.Game;
            }

            #region Change Minus Textures
            if (addedStr == 0)
                strDown.ButtonTexture = textures["MinusOff"];
            else
            {
                if (strDown.MouseDown())
                    strDown.ButtonTexture = textures["MinusDown"];
                else
                    strDown.ButtonTexture = textures["Minus"];
            }

            if (addedDef == 0)
                defDown.ButtonTexture = textures["MinusOff"];
            else
            {
                if(defDown.MouseDown())
                    defDown.ButtonTexture = textures["MinusDown"];
                else
                    defDown.ButtonTexture = textures["Minus"];
            }

            if (addedMot == 0)
                motDown.ButtonTexture = textures["MinusOff"];
            else
            {
                if(motDown.MouseDown())
                    motDown.ButtonTexture = textures["MinusDown"];
                else
                    motDown.ButtonTexture = textures["Minus"];
            }
            #endregion

            for (int i = 0; i < plusButtons.Count; i++)
            {
                if (plusButtons[i].MouseDown())
                    plusButtons[i].ButtonTexture = textures["PlusDown"];
                else
                    plusButtons[i].ButtonTexture = textures["Plus"];
            }

            if (addedStr > 0 && strDown.Clicked())
            {
                if (Game1.Player.StrengthPoints % 3 == 0)
                    Game1.Player.Strength -= 3;
                else if (Game1.Player.StrengthPoints % 2 == 0)
                    Game1.Player.Strength -= 2;
                else
                    Game1.Player.Strength -= 1;

                Game1.Player.StrengthPoints--;
                addedStr--;
                Game1.Player.StatPoints++;
            }

            if (addedDef > 0 && defDown.Clicked())
            {
                if (Game1.Player.TolerancePoints % 3 == 0)
                    Game1.Player.Defense -= 3;
                else if (Game1.Player.TolerancePoints % 2 == 0)
                    Game1.Player.Defense -= 2;
                else
                    Game1.Player.Defense -= 1;

                Game1.Player.TolerancePoints--;
                addedDef--;
                Game1.Player.StatPoints++;
            }

            if (addedMot > 0 && motDown.Clicked())
            {
                if (Game1.Player.MotivationPoints % 3 == 0)
                    Game1.Player.MaxHealth -= 5;
                else if (Game1.Player.MotivationPoints % 2 == 0)
                    Game1.Player.MaxHealth -= 3;
                else
                    Game1.Player.MaxHealth -= 1;

                Game1.Player.MotivationPoints--;
                addedMot--;
                Game1.Player.StatPoints++;
            }

            if (Game1.Player.StatPoints > 0)
            {
                if (strUp.Clicked())
                {
                    Game1.Player.StrengthPoints++;
                    addedStr++;
                    Game1.Player.StatPoints--;

                    if (Game1.Player.StrengthPoints % 3 == 0)
                        Game1.Player.Strength += 3;
                    else if (Game1.Player.StrengthPoints % 2 == 0)
                        Game1.Player.Strength += 2;
                    else
                        Game1.Player.Strength += 1;
                }

                if (defUp.Clicked())
                {
                    Game1.Player.TolerancePoints++;
                    addedDef++;
                    Game1.Player.StatPoints--;

                    if (Game1.Player.TolerancePoints % 3 == 0)
                        Game1.Player.Defense += 3;
                    else if (Game1.Player.TolerancePoints % 2 == 0)
                        Game1.Player.Defense += 2;
                    else
                        Game1.Player.Defense += 1;
                }

                if (motUp.Clicked())
                {
                    Game1.Player.MotivationPoints++;
                    addedMot++;
                    Game1.Player.StatPoints--;

                    if (Game1.Player.MotivationPoints % 3 == 0)
                        Game1.Player.MaxHealth += 5;
                    else if (Game1.Player.MotivationPoints % 2 == 0)
                        Game1.Player.MaxHealth += 3;
                    else
                        Game1.Player.MaxHealth += 1;
                }
            }
        }

        public override void Draw(SpriteBatch s)
        {
            base.Draw(s);

            s.DrawString(Game1.HUDFont, "Points Left: " + Game1.Player.StatPoints, new Vector2(550, (int)(Game1.aspectRatio * 1280 * .8) + 22), Color.Black);
            s.DrawString(Game1.HUDFont, "Backspace or Click Here to exit", new Vector2(450, (int)(Game1.aspectRatio * 1280 * .9) + 12), Color.Black);

            s.DrawString(Game1.font, "Strength: " + Game1.Player.Strength, new Vector2(200, (int)(Game1.aspectRatio * 1280 * .2) - 4), Color.Black);
            s.DrawString(Game1.font, "Strength Points: " + Game1.Player.StrengthPoints + " (+" + addedStr + ")", new Vector2(175, (int)(Game1.aspectRatio * 1280) / 2 + 5), Color.Black);

            s.DrawString(Game1.font, "Tolerance: " + Game1.Player.Defense, new Vector2(570, (int)(Game1.aspectRatio * 1280 * .2) - 14), Color.Black);
            s.DrawString(Game1.font, "Tolerance Points: " + Game1.Player.TolerancePoints + " (+" + addedDef + ")", new Vector2(530, (int)(Game1.aspectRatio * 1280) / 2 - 15), Color.Black);

            s.DrawString(Game1.font, "Motivation: " + Game1.Player.MaxHealth, new Vector2(940, (int)(Game1.aspectRatio * 1280 * .2) - 4), Color.Black);
            s.DrawString(Game1.font, "Motivation Points: " + Game1.Player.MotivationPoints + " (+" + addedMot + ")", new Vector2(905, (int)(Game1.aspectRatio * 1280) / 2 + 5), Color.Black);
        }
    }
}

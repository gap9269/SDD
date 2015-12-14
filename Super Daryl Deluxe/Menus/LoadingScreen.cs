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
    public class LoadingScreen : BaseMenu
    {
        Boolean alphaIncreasing = false;
        float textAlpha = 1f;
        float rayRotation;

        public LoadingScreen(Game1 g) 
            : base(Game1.whiteFilter, g)
        {
        }

        public override void Update()
        {

        }

        public override void Draw(SpriteBatch s)
        {
            rayRotation+= .5f;

            if (rayRotation == 360)
                rayRotation = 0;

            if (alphaIncreasing)
            {
                textAlpha += .03f;

                if (textAlpha >= 1)
                {
                    textAlpha = 1f;
                    alphaIncreasing = false;
                }
            }
            else
            {
                textAlpha -= .01f;

                if (textAlpha <= 0)
                {
                    textAlpha = 0;
                    alphaIncreasing = true;
                }
            }

            s.Draw(Game1.loadingScreenRays, new Rectangle(1280 / 2, 720 / 2, 1939, 1939), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(1280 / 2, 720 / 2), SpriteEffects.None, 0f);

            s.Draw(Game1.loadingScreenLogo, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);
            s.Draw(Game1.loadingScreenText, new Rectangle(1280 - Game1.loadingScreenText.Width, (int)(Game1.aspectRatio * 1280) - Game1.loadingScreenText.Height, Game1.loadingScreenText.Width, Game1.loadingScreenText.Height), Color.White * textAlpha);
            s.Draw(Game1.loadingScreenGradient, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);

            s.DrawString(Game1.twConMedium, Game1.WrapText(Game1.twConMedium, game.loadingTips[MapClass.currentLoadingTipNum], 990), new Vector2(640 - Game1.twConMedium.MeasureString(Game1.WrapText(Game1.twConMedium, game.loadingTips[MapClass.currentLoadingTipNum], 990)).X / 2, 330), Color.White);

        }
    }
}

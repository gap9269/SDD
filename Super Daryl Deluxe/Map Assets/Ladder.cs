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
    public class Ladder
    {
        Rectangle rec;
        Rectangle topOfLadder;
        Rectangle middleOfLadder;
        Texture2D texture;


        public Rectangle Rec { get { return rec; } }
        public Rectangle TopOfLadder { get { return topOfLadder; } }
        public Rectangle MiddleOfLadder { get { return middleOfLadder; } }

        public Ladder(int x, int y, int height)
        {
            texture = Game1.ladderTexture;
            rec = new Rectangle(x, y, 100, height);
            topOfLadder = new Rectangle(x, y, 100, 50);
            middleOfLadder = new Rectangle(x + 35, y, 30, height); 
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(texture, rec, Color.White);
        }
    }
}

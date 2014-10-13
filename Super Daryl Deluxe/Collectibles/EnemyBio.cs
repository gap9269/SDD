using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class EnemyBio : EnemyDrop
    {
        float rayRotation;
        String characterName;
       // String info;

        public String CharacterName { get { return characterName; } }
       // public String Info { get { return info; } }

        public EnemyBio(String drop, Rectangle r, String name)
            : base
                (drop, r)
        {
            characterName = name;
            rec = r;
           // info = CharacterMonsterBioDictionary.nameAndInfo[characterName];
            tex = Game1.bioTexture;
        }

        public override void Update(List<Platform> plats)
        {
            platforms = plats;

            #region FALL
            //--Drop the item until it hits the ground
            if (colliding == false)
            {
                velocity.Y += GameConstants.GRAVITY;
                if (velocity.Y > 25)
                    velocity.Y = 25;

                //--Check to see if it is colliding with a platform
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (rec.Intersects(platforms[i].Rec))
                    {
                        velocity.Y = -velocity.Y / 2;
                        velocity.X = velocity.X / 2;


                        Vector2 temp = velocity;
                        if (Math.Abs(temp.X) < 1.5f)
                        {
                            velocity.X = 0;
                        }

                        if (Math.Abs(temp.Y) < 1.5f)
                        {
                            velocity.Y = 0;
                            colliding = true;
                        }
                    }
                }

                rec.X += (int)velocity.X;
                rec.Y += (int)velocity.Y;
            }
            #endregion

            #region FLOAT UP AND DOWN
            //--Once it hits the ground, make it float up and down
            else
            {
                //--Every 20 frames it changes direction
                //--It floats at 1 pixel per frame, every 2 frames
                if (floatCycle < 20)
                {
                    if (floatCycle % 2 == 0)
                        rec.Y -= 1; floatCycle++;

                }
                else
                {
                    if (floatCycle % 2 == 0)
                        rec.Y += 1; floatCycle++;

                    if (floatCycle >= 40)
                    {
                        floatCycle = 0;
                    }
                }
            }
            #endregion

            rayRotation++;

            if (rayRotation == 360)
                rayRotation = 0;

            //--Check to see if it is colliding with a platform
            for (int i = 0; i < platforms.Count; i++)
            {
                if (rec.Intersects(platforms[i].Rec))
                {
                    colliding = true;
                    rec.Y = platforms[i].Rec.Y - rec.Height;
                }
            }

            if (Game1.Player.VitalRec.Intersects(rec) && colliding)
                PickUpCollectible();
        }

        public void PickUpCollectible()
        {
            Chapter.effectsManager.AddFoundItem("a Monster Biography : " + characterName, Game1.storyItemIcons["Piece of Paper"]);

            Game1.Player.AllMonsterBios[CharacterName] = true;

            //Make it disappear instantly
            PickedUpTimer = 0;
        }


        public override void Draw(SpriteBatch s)
        {

            s.Draw(Game1.textbookRay, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, Game1.textbookRay.Width, Game1.textbookRay.Height), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(Game1.textbookRay.Width / 2, Game1.textbookRay.Height / 2), SpriteEffects.None, 0f);
            s.Draw(tex, rec, Color.White);

        }

    }
}

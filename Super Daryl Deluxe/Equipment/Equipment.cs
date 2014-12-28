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
    public class Equipment
    {
        protected int upgradeSlots;
        protected int health;
        protected int strength;
        protected int defense;
        protected int moveSpeed;
        protected int jumpHeight;
        protected string name;
        protected int level;
        protected Texture2D icon;
        protected string description;
        protected int sellPrice;
        protected Random randomStats;
        protected Passive passiveAbility;

        public Passive PassiveAbility { get { return passiveAbility; } set { passiveAbility = value; } }
        public int SellPrice { get { return sellPrice; } set { sellPrice = value; } }
        public int Health { get { return health; } set { health = value; if (health < 0) health = 0; } }
        public int Strength { get { return strength; } set { strength = value; if (strength < 0) strength = 0; } }
        public int Defense { get { return defense; } set { defense = value; if (defense < 0) defense = 0; } }
        public int MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
        public int JumpHeight { get { return jumpHeight; } set { jumpHeight = value; } }
        public int Level { get { return level; } set { level = value; } }
        public String Name { get { return name; } set { name = value; } }
        public String Description { get { return description; } set { description = value; } }
        public int UpgradeSlots { get { return upgradeSlots; } set { upgradeSlots = value; } }
        public Texture2D Icon { get { return icon; } set { icon = value; } }

        public Equipment()
        {
            randomStats = new Random();
        }

        public virtual void SetRandomDropStats()
        {

        }

        public int NextRandomStat(int low, int high)
        {
            int random = randomStats.Next(2);

            int stat = (int)(randomStats.Next(low, high) + .1f);

            if (random == 0)
                return -stat;
            else
                return stat;
        }

        public String GetStatDescription()
        {
            String statDescription = "";

            if (health > 0)
                statDescription += "Health +" + health + "\n";
            if (strength > 0)
                statDescription += "Strength +" + strength + "\n";
            if(defense > 0)
                statDescription += "Defense +" + defense + "\n";
            if (passiveAbility != null && passiveAbility.Name != "")
            {
                statDescription += "Passive +" + passiveAbility.Name + "\n";
            }

            return statDescription;
        }

        //public virtual void DrawEquipDescription(SpriteBatch s, SpriteFont font, Vector2 vec)
        //{
        //    s.DrawString(font, description, vec, Color.Black);

        //    if (!(this is Money) && !(this is Karma) && !(this is Experience))
        //    {
        //        s.DrawString(font, "Health +" + health, vec + new Vector2(0, font.MeasureString("Health +" + health).Y), Color.Black);
        //        s.DrawString(font, "Strength +" + health, vec + new Vector2(0, font.MeasureString("Health +" + health + "\nStrength +" + health).Y), Color.Black);
        //        s.DrawString(font, "Defense +" + health, vec + new Vector2(0, font.MeasureString("Health +" + health + "\nStrength +" + health + "\nDefense +" + health).Y), Color.Black);
        //    }
        //}

        public virtual void UpdateDescription()
        {

        }
    }
}

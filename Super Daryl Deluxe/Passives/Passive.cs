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
    public class Passive
    {

        protected String name;
        protected Game1 game;
        protected Texture2D spriteSheet;
        protected ContentManager content;
        protected float healthModifier, strengthModifer, defenseModifier, moneyModifier, experienceModifier, damageModifier, pickUpRectangleModifier, extraExperiencePerKill;
        protected int luck, specialDefense;

        public String Name { get { return name; } set { name = value; } }

        public Passive(Game1 g)
        {
            game = g;
            content = new ContentManager(g.Services);
            content.RootDirectory = "Content";
        }

        /// <summary>
        /// Call this when the skill is added to Daryl's passive list, or when the game is loaded and Daryl has this passive equipped already
        /// </summary>
        public virtual void LoadPassive()
        {
            Game1.Player.strengthModifier += strengthModifer;
            Game1.Player.defenseModifier += defenseModifier;
            Game1.Player.healthModifier += healthModifier;
            Game1.Player.moneyModifier += moneyModifier;
            Game1.Player.pickUpRectangleModifier += pickUpRectangleModifier;
            Game1.Player.Luck += luck;
            Game1.Player.extraExperiencePerKill += extraExperiencePerKill;
            Game1.Player.UpdateStats();
        }

        //Call this when the passive is removed from Daryl's list
        public virtual void UnloadPassive()
        {
            Game1.Player.strengthModifier -= strengthModifer;
            Game1.Player.defenseModifier -= defenseModifier;
            Game1.Player.healthModifier -= healthModifier;
            Game1.Player.moneyModifier -= moneyModifier;
            Game1.Player.pickUpRectangleModifier -= pickUpRectangleModifier;
            Game1.Player.extraExperiencePerKill -= extraExperiencePerKill;
            Game1.Player.Luck -= luck;

            Game1.Player.UpdateStats();
            spriteSheet = null;
            content.Unload();
        }

        public virtual void CheckEnemyCollisions(Enemy en)
        {

        }

        public virtual void Update()
        {
        }

        public virtual void Draw(SpriteBatch s)
        { 
        }

        public virtual void DrawBehindPlayer(SpriteBatch s)
        {
        }

    }
}

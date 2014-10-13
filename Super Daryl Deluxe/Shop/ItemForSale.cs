using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ISurvived
{
    public class ItemForSale
    {
        public Texture2D icon;
        public float cost;
        public String description, name, passive;
        protected int health, defense, strength, level;

        public ItemForSale(float c)
        {
            cost = c;
        }

        public virtual void DrawEquipDescription(SpriteBatch s, SpriteFont font, Vector2 vec)
        {
            //Only draw stats if it is equipment
            if (!(this is TextbookForSale) && !(this is StoryItemForSale) && !(this is KeyForSale))
            {
                if (Game1.Player.Level >= level)
                    s.DrawString(font, level.ToString(), new Vector2(885, 439), Color.Black);
                else
                    s.DrawString(font, level.ToString(), new Vector2(885, 439), Color.Red);
                s.DrawString(font, Game1.WrapText(Game1.descriptionFont, description, 315), new Vector2(794, 461), Color.Black);

                Vector2 healthVec = new Vector2(788, 538);
                Vector2 defenseVec = new Vector2(985, 538);
                Vector2 strengthVec = new Vector2(795, 580);

                s.DrawString(font, "+ " + health.ToString(), new Vector2(788, 538), Color.White);
                s.DrawString(font, "+ " + strength.ToString(), new Vector2(795, 580), Color.White);
                s.DrawString(font, "+ " + defense.ToString(), new Vector2(985, 538), Color.White);

                if (passive != null && passive != "")
                {
                    s.DrawString(font, passive, new Vector2(823, 500), Color.DarkCyan);
                }
                else
                    s.DrawString(font, "No Passive", new Vector2(823, 500), Color.Red);

                #region Draw the stat differences for Accessories
                if (this is AccessoryForSale)
                {
                    if (Game1.Player.EquippedAccessory != null)
                    {
                        //Get the difference between the equipped accessory and the one the player is viewing in the shop
                        int healthDiff = health - Game1.Player.EquippedAccessory.Health;
                        int defenseDiff = defense - Game1.Player.EquippedAccessory.Defense;
                        int strengthDiff = strength - Game1.Player.EquippedAccessory.Strength;

                        #region Draw the bonus differences for the first accessory
                        if (healthDiff >= 0)
                            s.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+  " + Math.Abs(health)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+  " + Math.Abs(health)).X, 0), Color.Red);

                        if (strengthDiff >= 0)
                            s.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+  " + Math.Abs(strength)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+  " + Math.Abs(strength)).X, 0), Color.Red);

                        if (defenseDiff >= 0)
                            s.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+  " + Math.Abs(defense)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+  " + Math.Abs(defense)).X, 0), Color.Red);
                        #endregion

                        //Get the difference between the second equipped accessory and the one the player is viewing in the shop
                        if (Game1.Player.SecondAccessory != null)
                        {
                            int secondHealthDiff = health - Game1.Player.SecondAccessory.Health;
                            int secondDefenseDiff = defense - Game1.Player.SecondAccessory.Defense;
                            int secondStrengthDiff = strength - Game1.Player.SecondAccessory.Strength;

                            #region Draw the bonus differences for the second accessory
                            if (secondHealthDiff >= 0)
                                s.DrawString(font, "(+" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Red);


                            if (secondStrengthDiff >= 0)
                                s.DrawString(font, "(+" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Red);


                            if (secondDefenseDiff >= 0)
                                s.DrawString(font, "(+" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Red);
                            #endregion
                        }
                        else //The player has no second accessory, so it's all positive stat differences
                        {
                            #region Draw the positive stat differences next to the first accessory differences
                            s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString("+ " + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);

                            s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString("+ " + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);

                            s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString("+ " + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                            #endregion
                        }

                    }
                    else //No accessories, so it's all positive
                    {
                        #region Draw the stat differences, all positive
                        s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Green);
                        #endregion
                    }
                }
                #endregion

                #region Draw the stat differences for Weapons
                if (this is WeaponForSale)
                {
                    //Draw whether or not dual wielding is allowed

                    if ((this as WeaponForSale).dualWield)
                    {
                        s.Draw(Game1.dualWieldIcon, new Vector2(935, 576), Color.White);
                        s.DrawString(font, "Dual Wield Allowed", new Vector2(977, 582), Color.White, 0, Vector2.Zero, .93f, SpriteEffects.None, 0);
                    }

                    if (Game1.Player.EquippedWeapon != null)
                    {
                        //Get the difference between the equipped weapon and the one the player is viewing in the shop
                        int healthDiff = health - Game1.Player.EquippedWeapon.Health;
                        int defenseDiff = defense - Game1.Player.EquippedWeapon.Defense;
                        int strengthDiff = strength - Game1.Player.EquippedWeapon.Strength;

                        #region Draw the bonus differences for the first Weapon
                        if (healthDiff >= 0)
                            s.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(health)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(health)).X, 0), Color.Red);

                        if (strengthDiff >= 0)
                            s.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(strength)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(strength)).X, 0), Color.Red);

                        if (defenseDiff >= 0)
                            s.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(defense)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + " " + Math.Abs(defense)).X, 0), Color.Red);
                        #endregion

                        //Get the difference between the second equipped weapon and the one the player is viewing in the shop
                        if (Game1.Player.SecondWeapon != null)
                        {
                            int secondHealthDiff = health - Game1.Player.SecondWeapon.Health;
                            int secondDefenseDiff = defense - Game1.Player.SecondWeapon.Defense;
                            int secondStrengthDiff = strength - Game1.Player.SecondWeapon.Strength;

                            #region Draw the bonus differences for the second Weapon
                            if (secondHealthDiff >= 0)
                                s.DrawString(font, "(+" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondHealthDiff + ")", healthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Red);


                            if (secondStrengthDiff >= 0)
                                s.DrawString(font, "(+" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondStrengthDiff + ")", strengthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Red);


                            if (secondDefenseDiff >= 0)
                                s.DrawString(font, "(+" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                            else
                                s.DrawString(font, "(" + secondDefenseDiff + ")", defenseVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Red);
                            #endregion
                        }
                        else //The player has no second weapon, so it's all positive stat differences
                        {
                            #region Draw the positive stat differences next to the first accessory differences
                            s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(health)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(healthDiff) + ")").X + 10, 0), Color.Green);

                            s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(strength)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(strengthDiff) + ")").X + 10, 0), Color.Green);

                            s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString("+ " + "" + Math.Abs(defense)).X, 0) + new Vector2(font.MeasureString("(+" + Math.Abs(defenseDiff) + ")").X + 10, 0), Color.Green);
                            #endregion
                        }

                    }
                    else //No weapons, so it's all positive
                    {
                        #region Draw the stat differences, all positive
                        s.DrawString(font, "(+" + health + ")", new Vector2(788, 538) + new Vector2(font.MeasureString("+ " + " " + Math.Abs(health)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + strength + ")", new Vector2(795, 580) + new Vector2(font.MeasureString("+ " + " " + Math.Abs(strength)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + defense + ")", new Vector2(985, 538) + new Vector2(font.MeasureString("+ " + " " + Math.Abs(defense)).X, 0), Color.Green);
                        #endregion
                    }
                }
                #endregion

                #region Draw the stat differences for Hats
                if (this is HatForSale)
                {
                    if (Game1.Player.EquippedHat != null)
                    {
                        //Get the difference between the equipped hat and the one the player is viewing in the shop
                        int healthDiff = health - Game1.Player.EquippedHat.Health;
                        int defenseDiff = defense - Game1.Player.EquippedHat.Defense;
                        int strengthDiff = strength - Game1.Player.EquippedHat.Strength;

                        #region Draw the bonus differences for the first hat
                        if (healthDiff >= 0)
                            s.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Red);

                        if (strengthDiff >= 0)
                            s.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Red);

                        if (defenseDiff >= 0)
                            s.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Red);
                        #endregion
                    }
                    else //No hat, so it's all positive
                    {
                        #region Draw the stat differences, all positive
                        s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Green);
                        #endregion
                    }
                }
                #endregion

                #region Draw the stat differences for Outfits
                if (this is HoodieForSale)
                {
                    if (Game1.Player.EquippedHoodie != null)
                    {
                        //Get the difference between the equipped outfit and the one the player is viewing in the shop
                        int healthDiff = health - Game1.Player.EquippedHoodie.Health;
                        int defenseDiff = defense - Game1.Player.EquippedHoodie.Defense;
                        int strengthDiff = strength - Game1.Player.EquippedHoodie.Strength;

                        #region Draw the bonus differences for the first outfit
                        if (healthDiff >= 0)
                            s.DrawString(font, "(+" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + healthDiff + ")", healthVec + new Vector2(font.MeasureString(" + " + Math.Abs(health)).X, 0), Color.Red);

                        if (strengthDiff >= 0)
                            s.DrawString(font, "(+" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + strengthDiff + ")", strengthVec + new Vector2(font.MeasureString(" + " + Math.Abs(strength)).X, 0), Color.Red);

                        if (defenseDiff >= 0)
                            s.DrawString(font, "(+" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Green);
                        else
                            s.DrawString(font, "(" + defenseDiff + ")", defenseVec + new Vector2(font.MeasureString(" + " + Math.Abs(defense)).X, 0), Color.Red);
                        #endregion
                    }
                    else //No outfit, so it's all positive
                    {
                        #region Draw the stat differences, all positive
                        s.DrawString(font, "(+" + health + ")", healthVec + new Vector2(font.MeasureString("  +" + Math.Abs(health)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + strength + ")", strengthVec + new Vector2(font.MeasureString("  +" + Math.Abs(strength)).X, 0), Color.Green);
                        s.DrawString(font, "(+" + defense + ")", defenseVec + new Vector2(font.MeasureString("  +" + Math.Abs(defense)).X, 0), Color.Green);
                        #endregion
                    }
                }
                #endregion
            }
            else
            {
                s.DrawString(font, Game1.WrapText(Game1.descriptionFont, description, 315), new Vector2(794, 495), Color.Black);
            }
        }
    }

    public class TextbookForSale : ItemForSale
    {
        public int textureNum;

        public TextbookForSale(float c, int texture)
            : base(c)
        {
            name = "Textbook";
            description = "Our baseline product. Top quality stuff, \nguaranteed.";

            textureNum = texture;
            icon = Game1.textbookTextures;
        }


        public Rectangle GetSourceRec()
        {
            switch (textureNum)
            {
                case 0:
                    return new Rectangle(0, 0, 94, 90);
                case 1:
                    return new Rectangle(94, 0, 94, 90);
                case 2:
                    return new Rectangle(188, 0, 94, 90);
                case 3:
                    return new Rectangle(188 + 94, 0, 94, 90);
            }

            return new Rectangle();
        }
    }

    public class KeyForSale : ItemForSale
    {
        public enum KeyType
        {
            Bronze, Silver, Gold
        }
        public KeyType keyType;

        public KeyForSale(float c, KeyType type)
            : base(c)
        {
            keyType = type;

            if (keyType == KeyType.Bronze)
            {
                name = "Bronze Key";
                description = "A bronze room key. Opens bronze locks.";
                icon = Game1.storyItemIcons["Bronze Key"];
            }
            if (keyType == KeyType.Silver)
            {
                name = "Silver Key";
                description = "A silver room key. Opens silver locks.";
                icon = Game1.storyItemIcons["Silver Key"];
            }
            if (keyType == KeyType.Gold)
            {
                name = "Gold Key";
                description = "A gold room key. Opens gold locks.";
                icon = Game1.storyItemIcons["Gold Key"];
            }
        }
    }

    public class WeaponForSale : ItemForSale
    {
        public Weapon weapon;
        public Boolean dualWield;
        public WeaponForSale(Weapon w, float c)
            : base(c)
        {
            weapon = w;
            name = weapon.Name;
            description = weapon.Description;
            icon = weapon.Icon;
            health = w.Health;
            defense = w.Defense;
            strength = w.Strength;
            dualWield = w.CanHoldTwo;
            level = w.Level;
            if (w.PassiveAbility != null)
                passive = w.PassiveAbility.Name;
        }
    }

    public class AccessoryForSale : ItemForSale
    {
        public Accessory access;

        public AccessoryForSale(Accessory a, float c)
            : base(c)
        {
            access = a;
            name = access.Name;
            description = access.Description;
            icon = access.Icon;
            health = a.Health;
            defense = a.Defense;
            strength = a.Strength;
            level = a.Level;
            if (a.PassiveAbility != null)
                passive = a.PassiveAbility.Name;
        }
    }

    public class HatForSale : ItemForSale
    {
        public Hat hat;

        public HatForSale(Hat h, float c)
            : base(c)
        {
            hat = h;
            name = hat.Name;
            description = hat.Description;
            icon = hat.Icon;
            health = h.Health;
            defense = h.Defense;
            strength = h.Strength;
            level = h.Level;
            if (h.PassiveAbility != null)
                passive = h.PassiveAbility.Name;
        }
    }

    public class HoodieForSale : ItemForSale
    {
        public Hoodie hoodie;

        public HoodieForSale(Hoodie h, float c)
            : base(c)
        {
            hoodie = h;
            name = hoodie.Name;
            description = hoodie.Description;
            icon = hoodie.Icon;
            health = h.Health;
            defense = h.Defense;
            strength = h.Strength;
            level = h.Level;
            if (h.PassiveAbility != null)
                passive = h.PassiveAbility.Name;
        }
    }


    public class StoryItemForSale : ItemForSale
    {
        public StoryItem item;

        public StoryItemForSale(StoryItem i, float c)
            : base(c)
        {
            item = i;
            name = item.Name;
            description = item.Description;
            icon = item.Icon;
        }
    }
}

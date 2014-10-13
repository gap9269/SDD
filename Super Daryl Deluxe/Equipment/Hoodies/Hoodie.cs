using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class Hoodie : Equipment
    {

        public Hoodie()
            : base()
        {
        }
    }

    class IHateMelons : Hoodie
    {
        public IHateMelons()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 4;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "'I Hate Melons' Band Tee";
            level = 2;
            description = "Nothing says \"I Hate Melons\" quite like this \nT-Shirt. \n\n";
            icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Nothing says \"I Hate Melons\" quite like this \nT-Shirt. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Nothing says \"I Hate Melons\" quite like this \nT-Shirt. \n\n";
        }
    }

    class ILoveMelons : Hoodie
    {
        public ILoveMelons()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 4;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "'I Love Melons' Band Tee";
            level = 2;
            description = "I'm not sure how anyone could actually enjoy \na melon. \n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "I'm not sure how anyone could actually enjoy \na melon. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "I'm not sure how anyone could actually enjoy \na melon. \n\n";
        }
    }

    class Toga : Hoodie
    {
        public Toga()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 5;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Toga";
            level = 1;
            description = "Julius Caesar's own personal toga. \n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Julius Caesar's own personal toga. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Julius Caesar's own personal toga. \n\n";
        }
    }

    class ScarecrowVest : Hoodie
    {
        public ScarecrowVest()
            : base()
        {
            health = 0;
            strength = 0;
            defense = 6;
            moveSpeed = 0;
            jumpHeight = 0;
            name = "Scarecrow Vest";
            level = 4;
            description = "Full of worms. \n\n";
icon = Game1.equipmentTextures[name];
        }

        public override void SetRandomDropStats()
        {
            Strength += NextRandomStat(0, 2);
            Defense += NextRandomStat(0, 3);
            Health += NextRandomStat(0, 4);

            description = "Full of worms. \n\n";
        }

        public override void UpdateDescription()
        {
            base.UpdateDescription();
            description = "Full of worms. \n\n";
        }
    }
}

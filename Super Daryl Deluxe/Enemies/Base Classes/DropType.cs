using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class DropType
    {
        public String name;
        public Texture2D texture;
        public float sellCost;
        public float buyCost;
        public String description;
        public int successRate;
        public String type;

        public DropType(String n, String d, Texture2D tex, float sell, float buy, int suc, String t)
        {
            name = n;
            description = d;
            texture = tex;
            sellCost = sell;
            buyCost = buy;
            type = t;
            successRate = suc;
        }

    }
}

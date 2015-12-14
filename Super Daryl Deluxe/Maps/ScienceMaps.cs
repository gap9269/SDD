﻿using System;
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
    public class ScienceMaps : MapZone
    {
        Game1 game;
        Player player;


        public ScienceMaps(Game1 g, Player p) : base(g)
        {
            game = g;
            player = p;
        }

        public override void LoadEnemyData()
        {
            #region MONSTERS
            //PROLOGUE
            game.ResetEnemySpriteList();
            #endregion
        }

        public void LoadSchoolZone()
        {

            LoadEnemyData();



            //--Keep this at the end of MAPS
            for (int i = 0; i < maps.Count; i++)
            {
                maps.ElementAt(i).Value.SetDestinationPortals();
            }
        }
    }
}

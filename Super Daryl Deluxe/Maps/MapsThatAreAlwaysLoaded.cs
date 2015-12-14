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
    public class MapsThatAreAlwaysLoaded : MapZone
    {

        Game1 game;
        Player player;

        public MapsThatAreAlwaysLoaded(Game1 g, Player p) : base(g)
        {

            game = g;
            player = p;
        }


        public void LoadSchoolZone()
        {


            //--Keep this at the end of MAPS
            for (int i = 0; i < maps.Count; i++)
            {
                maps.ElementAt(i).Value.SetDestinationPortals();
            }
        }

        //This is necessary because it needs to be called AFTER any other map set is loaded
        public void SetAlwaysLoadedMapsDestinationPortals()
        {
            for (int i = 0; i < maps.Count; i++)
            {
                maps.ElementAt(i).Value.Portals.Clear();
                maps.ElementAt(i).Value.SetDestinationPortals();
            }
        }
    }
}

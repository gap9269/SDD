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
    public class ViennaMaps : MapZone
    {
        Game1 game;
        Player player;

        public ViennaMaps(Game1 g, Player p) : base(g)
        {
            game = g;
            player = p;
        }


        public override void LoadEnemyData()
        {
            //MONSTERS
            game.ResetEnemySpriteList();
            game.EnemySpriteSheets.Add("Bat", content.Load<Texture2D>(@"EnemySprites\bat"));
            game.EnemySpriteSheets.Add("Tuba Ghost", this.content.Load<Texture2D>(@"EnemySprites\TubaGhost"));
            game.EnemySpriteSheets.Add("Bill Baton", this.content.Load<Texture2D>(@"EnemySprites\ViennaGuard"));
            game.EnemySpriteSheets.Add("Crossbow Carl", this.content.Load<Texture2D>(@"EnemySprites\CrossbowSprite"));
        }

        public void LoadSchoolZone()
        {
            //--Keep this at the end of MAPS
            for (int i = 0; i < maps.Count; i++)
            {
                maps.ElementAt(i).Value.SetDestinationPortals();
            }
        }
    }
}

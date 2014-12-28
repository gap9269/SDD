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
    public class TutorialMaps : MapZone
    {
        Game1 game;
        Player player;


        public TutorialMaps(Game1 g, Player p) : base(g)
        {
            game = g;
            player = p;
        }


        public override void LoadEnemyData()
        {
            //TUTORIAL
            game.ResetEnemySpriteList();
            game.EnemySpriteSheets.Add("Garden Beast", content.Load<Texture2D>(@"Tutorial\EnemieSheet"));
            //--DEMO
            game.EnemySpriteSheets.Add("AustinFace", this.content.Load<Texture2D>(@"Bosses\DemoAustin\austinFace"));
            game.EnemySpriteSheets.Add("AustinName", this.content.Load<Texture2D>(@"Bosses\DemoAustin\austinName"));
        }

        public void LoadSchoolZone()
        {


        }
    }
}

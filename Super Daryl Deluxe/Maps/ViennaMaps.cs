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
            List<Texture2D> science101Back = new List<Texture2D>();
            science101Back.Add(content.Load<Texture2D>(@"Maps\hall"));

            #region Vienna
            TheaterAnDerWien theaterAnDerWien = new TheaterAnDerWien(science101Back, game, ref player);
            maps.Add("TheaterAnDerWien", theaterAnDerWien);

            EntranceHall entranceHall = new EntranceHall(science101Back, game, ref player);
            maps.Add("EntranceHall", entranceHall);

            TheStage stage = new TheStage(science101Back, game, ref player);
            maps.Add("TheStage", stage);

            SecondFloor secondFloor = new SecondFloor(science101Back, game, ref player);
            maps.Add("SecondFloor", secondFloor);

            ThirdFloor thirdFloor = new ThirdFloor(science101Back, game, ref player);
            maps.Add("ThirdFloor", thirdFloor);

            TenantHallway tenantHallway = new TenantHallway(science101Back, game, ref player);
            maps.Add("TenantHallway", tenantHallway);

            RoomTwo roomTwo = new RoomTwo(science101Back, game, ref player);
            maps.Add("TenantRoom#2", roomTwo);

            RoomOne roomOne = new RoomOne(science101Back, game, ref player);
            maps.Add("TenantRoom#1", roomOne);

            RoomThree roomThree = new RoomThree(science101Back, game, ref player);
            maps.Add("TenantRoom#3", roomThree);

            RoomFour roomFour = new RoomFour(science101Back, game, ref player);
            maps.Add("TenantRoom#4", roomFour);

            BeethovensRoom beethovensRoom = new BeethovensRoom(science101Back, game, ref player);
            maps.Add("Beethoven'sRoom", beethovensRoom);

            Backstage backStage = new Backstage(science101Back, game, ref player);
            maps.Add("Backstage", backStage);

            ManagersFloor managersFloor = new ManagersFloor(science101Back, game, ref player);
            maps.Add("Manager'sFloor", managersFloor);
            #endregion

            //--Keep this at the end of MAPS
            for (int i = 0; i < maps.Count; i++)
            {
                maps.ElementAt(i).Value.SetDestinationPortals();
            }
        }
    }
}

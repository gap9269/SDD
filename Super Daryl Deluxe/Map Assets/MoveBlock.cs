using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    public class MoveBlock : Platform
    {
        int moveSpeedWhenPushed;
        Platform currentPlat;
        KeyboardState last;
        KeyboardState current;
        bool falling = false;

        bool touchingPlat = false;

        float terminalVelocity = 25;

        public Platform CurrentPlat { get { return currentPlat; } set { currentPlat = value; } }
        
        public MoveBlock(Texture2D tex, Rectangle rec, int moveSpeed):base
            (tex, rec, false, false, false)
        {
            moveSpeedWhenPushed = moveSpeed;
            position.X = rec.X;
            position.Y = rec.Y;
        }

        public void Update(Player player, MapClass currentMap)
        {
            last = current;
            current = Keyboard.GetState();

            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            //Rectangles that represent the sides of the character
            Rectangle feet = new Rectangle((int)player.VitalRec.X, (int)player.VitalRec.Y + player.VitalRec.Height + 20, player.VitalRec.Width, 20);
            Rectangle rightPlay = new Rectangle((int)player.VitalRec.X + player.VitalRec.Width, (int)player.VitalRec.Y + 5, 15, player.VitalRec.Height);
            Rectangle leftPlay = new Rectangle((int)player.VitalRec.X, (int)player.VitalRec.Y + 5, 15, player.VitalRec.Height);
            Rectangle topPlay = new Rectangle((int)player.VitalRec.X + 5, (int)player.VitalRec.Y, player.VitalRec.Width - 5, 10);


            Rectangle top = new Rectangle(rec.X + 5, rec.Y, rec.Width - 5, 20);
            Rectangle left = new Rectangle(rec.X, rec.Y + 5, 10, rec.Height - 3);
            Rectangle right = new Rectangle(rec.X + rec.Width, rec.Y + 5, 10, rec.Height - 3);
            Rectangle bottom = new Rectangle(rec.X + 5, rec.Y + rec.Height - 10, rec.Width - 5, 10);

            //--If you run into the side of a nonpassable wall
            //--Depending on what side you're on, and if you're jumping or not, move the player back a frame
            if ((rightPlay.Intersects(left) || leftPlay.Intersects(right)))
            {
                if (rightPlay.Intersects(left) && current.IsKeyDown(Keys.Right))
                {
                    if (player.playerState == Player.PlayerState.running)
                    {
                        player.playerState = Player.PlayerState.pushingBlock;
                        player.PushBlockSpeed = moveSpeedWhenPushed;
                    }
                    else if (player.playerState != Player.PlayerState.pushingBlock)
                    {
                        if (player.playerState == Player.PlayerState.jumping || player.playerState == Player.PlayerState.attackJumping)
                            player.PositionX -= player.AirMoveSpeed;
                        else
                            player.PositionX -= player.MoveSpeed;
                    }
                    else
                    {
                        position.X += moveSpeedWhenPushed;
                    }
                }

                if (leftPlay.Intersects(right))
                {
                    if (player.playerState == Player.PlayerState.running)
                    {
                        player.playerState = Player.PlayerState.pushingBlock;
                        player.PushBlockSpeed = moveSpeedWhenPushed;
                    }
                    else if (player.playerState != Player.PlayerState.pushingBlock)
                    {
                        if (player.playerState == Player.PlayerState.jumping || player.playerState == Player.PlayerState.attackJumping)
                            player.PositionX += player.AirMoveSpeed;
                        else
                            player.PositionX += player.MoveSpeed;
                    }
                    else
                    {
                        position.X -= moveSpeedWhenPushed;
                    }
                }
            }

            InteractWithPlatforms(currentMap, player);
        }

        public void InteractWithPlatforms(MapClass currentMap, Player player)
        {
            if(currentPlat == null)
                velocity.Y += GameConstants.GRAVITY;

            position += velocity;

            if (velocity.Y > terminalVelocity)
            {
                velocity.Y = terminalVelocity;
            }

            Rectangle top = new Rectangle(rec.X + 5, rec.Y, rec.Width - 5, 20);
            Rectangle left = new Rectangle(rec.X, rec.Y + 5, 10, rec.Height - 3);
            Rectangle right = new Rectangle(rec.X + rec.Width, rec.Y + 5, 10, rec.Height - 3);
            Rectangle bottom = new Rectangle(rec.X + 5, rec.Y + rec.Height - 20, rec.Width - 5, 20);

            for (int i = 0; i < currentMap.Platforms.Count; i++)
            {
                Platform plat = currentMap.Platforms[i];
                Rectangle platTop = new Rectangle(plat.Rec.X, plat.Rec.Y, plat.Rec.Width, 10);
                Rectangle platLeft = new Rectangle(plat.Rec.X, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);
                Rectangle platRight = new Rectangle(plat.Rec.X + plat.Rec.Width, plat.Rec.Y + 5, 10, plat.Rec.Height - 3);

                if (plat == currentPlat)
                {
                    if (!bottom.Intersects(platTop))
                    {
                        currentPlat = null;
                    }
                }

                if (velocity.Y >= GameConstants.GRAVITY * 2)
                    falling = true;

                if (bottom.Intersects(platTop) && velocity.Y > 0)
                {
                    position.Y = currentMap.Platforms[i].Rec.Y - rec.Height + 5;
                    velocity.Y = 0;
                    currentPlat = currentMap.Platforms[i];
                    falling = false;
                }

                if (right.Intersects(platLeft) && plat.Passable == false)
                {
                    position.X -= moveSpeedWhenPushed;
                    player.PositionX -= moveSpeedWhenPushed;
                }
                else if (left.Intersects(platRight) && plat.Passable == false)
                {
                    position.X += moveSpeedWhenPushed;
                    player.PositionX += moveSpeedWhenPushed;
                }
            }
        }
    }
}

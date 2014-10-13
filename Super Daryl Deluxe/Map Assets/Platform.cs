using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISurvived
{
    public class SinkingDisappearingPlatform : MovingPlatform
    {
        int maxSinkDepth;

        int sinkSpeed;
        int riseSpeed;

        int maxHealth;
        int health;
        int timeBeforeRespawn;
        int maxTimeBeforeRespawn;

        int timeBeforeRise = 0;

        Boolean atRestAtBottom = false;
        Boolean atRestAtTop = true;

        public SinkingDisappearingPlatform(Texture2D t, Rectangle r, bool pass, bool spawn, bool invis, int speedOfSink, int maxSink, int speedRise, int hel, int respawnTime)
            : base(t, r, pass, spawn, invis, new List<Vector2> { new Vector2(r.X, r.Y + maxSink), new Vector2(r.X, r.Y) }, speedOfSink, 50)
        {
            texture = t;
            rec = r;

            position.X = rec.X;
            position.Y = rec.Y;

            passable = pass;
            spawnOnTop = spawn;
            invisible = invis;

            maxSinkDepth = maxSink;

            sinkSpeed = speedOfSink;
            riseSpeed = speedRise;

            health = maxHealth = hel;

            maxTimeBeforeRespawn = timeBeforeRespawn = respawnTime;

            if (texture == Game1.platformTextures["RockFloor2"] || texture == Game1.platformTextures["RockFloor"])
                type = "Normal";
        }

        //--So this method is basically stolen from MovingPlatform, but with a few changes
        public override void Update()
        {

            if (exists)
            {
                //--Update position
                position.X += velocity.X;
                position.Y += velocity.Y;

                rec.X = (int)position.X;
                rec.Y = (int)position.Y;

                //--Update center -- This is for calculating when you hit the point you are seeking
                centerPos.X += velocity.X;
                centerPos.Y += velocity.Y;

                centerOfPlat.X = (int)centerPos.X;
                centerOfPlat.Y = (int)centerPos.Y;

                //--If the platform is at the first point in its path, it is at the bottom and can be stopped
                if (centerOfPlat.Intersects(new Rectangle((int)path[0].X, (int)path[0].Y, 10, 10)) && atRestAtBottom == false)
                {
                    atRestAtBottom = true;
                    velocity.Y = 0;
                }

                //If the player is touching the platform and the platform isn't resting on the bottom of its path
                if (Game1.Player.CurrentPlat == this && !atRestAtBottom)
                {
                    //It can't be at rest at the top since it is not moving
                    atRestAtTop = false;
                    //Reset the time buffer for rising
                    timeBeforeRise = 0;

                    //Seek the bottom point
                    Vector2 nextPoint = path[0];
                    velocity = Seek(nextPoint);

                    //Get the distance between the points
                    float dist = (centerPos - nextPoint).Length();
                    float radius = stopMag;

                    //If you are farther away than the stopping magnitude
                    if (dist < radius)
                    {
                        velocity.Normalize();
                        velocity *= (sinkSpeed * dist / radius);
                    }
                }
                //If the player isn't standing on it and it isn't at the top, increment the time buffer
                else if (Game1.Player.CurrentPlat != this && !atRestAtTop)
                {
                    timeBeforeRise++;

                    if (timeBeforeRise > 5)
                        velocity.Y = 0;
                }

                //If the buffer is greater than 20, or the player is no longer on the platform, but it is at the bottom of its path, make it rise
                if (timeBeforeRise >= 60 || (Game1.Player.CurrentPlat != this && atRestAtBottom))
                {
                    //It can't be at the bottom if it is rising up
                    atRestAtBottom = false;

                    //Select the base point
                    Vector2 nextPoint = path[1];
                    velocity = SeekWithCustomSpeed(nextPoint, riseSpeed);

                    float dist = (centerPos - nextPoint).Length();
                    float radius = stopMag;

                    if (dist < radius)
                    {
                        velocity.Normalize();
                        velocity *= (riseSpeed * dist / radius);
                    }

                    if (centerOfPlat.Intersects(new Rectangle((int)nextPoint.X, (int)nextPoint.Y, 10, 10)))
                    {
                        velocity.Y = 0;
                        atRestAtTop = true;
                    }

                }

                //If the player is standing on it at the bottom of its path, destroy it
                if ((Game1.Player.CurrentPlat == this && atRestAtBottom) || health != maxHealth)
                {

                    health--;

                    if (health <= 0)
                        exists = false;
                }
            }

            //If it doesn't exist, start respawning it
            if (!exists && health < maxHealth)
            {
                timeBeforeRespawn--;

                //--Spawn it and reset its health and respawn timer
                if (timeBeforeRespawn <= 0)
                {
                    health = maxHealth;
                    exists = true;
                    timeBeforeRespawn = maxTimeBeforeRespawn;
                    position = path[0];
                }
            }
        }
    }

    public class SinkingPlatform : MovingPlatform
    {
        int maxSinkDepth;

        int sinkSpeed;
        int riseSpeed;

        int timeBeforeRise = 0;

        Boolean atRestAtBottom = false;
        Boolean atRestAtTop = true;

        public SinkingPlatform(Texture2D t, Rectangle r, bool pass, bool spawn, bool invis, int speedOfSink, int maxSink, int speedRise)
            : base(t, r, pass, spawn, invis, new List<Vector2>{new Vector2(r.X, r.Y + maxSink), new Vector2(r.X, r.Y)}, speedOfSink, 50)
        {
            texture = t;
            rec = r;

            position.X = rec.X;
            position.Y = rec.Y;

            passable = pass;
            spawnOnTop = spawn;
            invisible = invis;

            maxSinkDepth = maxSink;

            sinkSpeed = speedOfSink;
            riseSpeed = speedRise;

            if (texture == Game1.platformTextures["RockFloor2"] || texture == Game1.platformTextures["RockFloor"])
                type = "Normal";
        }

        //--So this method is basically stolen from MovingPlatform, but with a few changes
        public override void Update()
        {

            //--Update position
            position.X += velocity.X;
            position.Y += velocity.Y;

            rec.X = (int)position.X;
            rec.Y = (int)position.Y;

            //--Update center -- This is for calculating when you hit the point you are seeking
            centerPos.X += velocity.X;
            centerPos.Y += velocity.Y;

            centerOfPlat.X = (int)centerPos.X;
            centerOfPlat.Y = (int)centerPos.Y;

            //--If the platform is at the first point in its path, it is at the bottom and can be stopped
            if (centerOfPlat.Intersects(new Rectangle((int)path[0].X, (int)path[0].Y, 10, 10)) && atRestAtBottom == false)
            {
                atRestAtBottom = true;
                velocity.Y = 0;
            }

            //If the player is touching the platform and the platform isn't resting on the bottom of its path
            if (Game1.Player.CurrentPlat == this && !atRestAtBottom)
            {
                //It can't be at rest at the top since it is not moving
                atRestAtTop = false;
                //Reset the time buffer for rising
                timeBeforeRise = 0;

                //Seek the bottom point
                Vector2 nextPoint = path[0];
                velocity = Seek(nextPoint);

                //Get the distance between the points
                float dist = (centerPos - nextPoint).Length();
                float radius = stopMag;

                //If you are farther away than the stopping magnitude
                if (dist < radius)
                {
                    velocity.Normalize();
                    velocity *= (sinkSpeed * dist / radius);
                }
            }
            //If the player isn't standing on it and it isn't at the top, increment the time buffer
            else if (Game1.Player.CurrentPlat != this && !atRestAtTop)
            {
                timeBeforeRise++;

                if(timeBeforeRise > 5)
                    velocity.Y = 0;
            }

            //If the buffer is greater than 20, or the player is no longer on the platform, but it is at the bottom of its path, make it rise
            if (timeBeforeRise >= 60 || (Game1.Player.CurrentPlat != this && atRestAtBottom))
            {
                //It can't be at the bottom if it is rising up
                atRestAtBottom = false;

                //Select the base point
                Vector2 nextPoint = path[1];
                velocity = SeekWithCustomSpeed(nextPoint, riseSpeed);

                float dist = (centerPos - nextPoint).Length();
                float radius = stopMag;

                if (dist < radius)
                {
                    velocity.Normalize();
                    velocity *= (riseSpeed * dist / radius);
                }

                if (centerOfPlat.Intersects(new Rectangle((int)nextPoint.X, (int)nextPoint.Y, 10, 10)))
                {
                    velocity.Y = 0;
                    atRestAtTop = true;
                }
            }
        }
    }

    public class DisappearingPlatform : Platform
    {
        //Used to make the platforms disappear and reappear
        int maxHealth;
        int health;
        int timeBeforeRespawn;
        int maxTimeBeforeRespawn;

        public DisappearingPlatform(Texture2D t, Rectangle r, bool pass, bool spawn, bool invis, int hel, int respawnTime)
            : base(t, r, pass, spawn, invis)
        {
            texture = t;
            rec = r;
            passable = pass;
            spawnOnTop = spawn;
            invisible = invis;

            health = maxHealth = hel;

            maxTimeBeforeRespawn = timeBeforeRespawn = respawnTime;

            if (texture == Game1.platformTextures["RockFloor2"] || texture == Game1.platformTextures["RockFloor"])
                type = "Normal";
        }

        public override void Update()
        {
            base.Update();

            //--If the player is standing on it or has touched it, making its health less than max
            //--Subtract health then make it disappear when it hits 0
            if (Game1.Player.CurrentPlat == this || (health < maxHealth && exists))
            {
                health--;

                if (health <= 0)
                    exists = false;
            }
            //If it doesn't exist, start respawning it
            else if (!exists && health < maxHealth)
            {
                timeBeforeRespawn--;

                //--Spawn it and reset its health and respawn timer
                if (timeBeforeRespawn <= 0)
                {
                    health = maxHealth;
                    exists = true;
                    timeBeforeRespawn = maxTimeBeforeRespawn;
                }
            }
        }
    }

    public class Platform
    {
        protected Texture2D texture;
        protected Rectangle rec;
        protected bool passable;
        protected bool spawnOnTop;
        protected bool invisible = false;
        protected Vector2 velocity;
        protected Vector2 position;
        protected String type;
        protected Boolean exists = true; //used to make platforms disappear without actually removing them from the map
        Boolean drawPlatform = false;

        //MAP EDIT SHIT
        public Button mapEditButton; //Button for map editting in game
        ButtonState previousMouseState;
        ButtonState currentMouseState;

        // PROPERTIES \\
        public Boolean DrawPlatform { get { return drawPlatform; } set { drawPlatform = value; } }
        public String Type { get { return type; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public bool Passable { get { return passable; } set { passable = value; } }
        public bool SpawnOnTop { get { return spawnOnTop; } set { spawnOnTop = value; } }
        public Rectangle Rec { get { return rec; } set { rec = value; } }
        public Texture2D Texture { get { return texture; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Boolean Exists { get { return exists; } set { exists = value; } }
        public int RecX { get { return rec.X; } set { rec.X = value; } }
        public int RecY { get { return rec.Y; } set { rec.Y = value; } }
        public int RecWidth { get { return rec.Width; } set { rec.Width = value; } }
        public int RecHeight { get { return rec.Height; } set { rec.Height = value; } }

        public Platform(Texture2D t, Rectangle r, bool pass, bool spawn, bool invis)
        {
            texture = t;
            rec = r;
            position.X = rec.X;
            position.Y = rec.Y;
            passable = pass;
            spawnOnTop = spawn;
            invisible = invis;

            if (texture == Game1.platformTextures["RockFloor2"] || texture == Game1.platformTextures["RockFloor"])
                type = "Normal";

            mapEditButton = new Button(r);
        }

        public virtual void Update() { }

        public void Draw(SpriteBatch s)
        {
            if (invisible == true || !exists)
                s.Draw(texture, rec, null, Color.White * 0f);
            else
                s.Draw(texture, rec, null, Color.White);
        }

        //--Checks to see if the button has been clicked or not
        public bool Clicked()
        {
            MouseState mouse = Mouse.GetState();
            //--Update the mouse states, so last state is set to "previous"
            previousMouseState = currentMouseState;
            if (Cursor.cursorOnMapRec.Intersects(rec))
            {

                //--If the mouse is clicking, make "current" equal to pressed.
                //--If not, make it released
                if (mouse.LeftButton == ButtonState.Pressed)
                    currentMouseState = ButtonState.Pressed;
                else
                    currentMouseState = ButtonState.Released;


                //--If the previous state was pressed, and it is now released, the user must have clicked
                //--Return true
                if (currentMouseState == ButtonState.Released && previousMouseState == ButtonState.Pressed)
                    return true;
            }

            return false;
        }

        public void Draw(SpriteBatch s, float alpha)
        {
            if (invisible == true || !exists)
                alpha = 0f;

            s.Draw(texture, rec, Color.White * alpha);
        }
    }
}

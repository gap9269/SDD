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

//--Written by Christopher Blackman and Gary Porter

namespace ISurvived
{
    public class Camera
    {
        Matrix transform;
        Matrix staticTransform;
        float zoom = 1f;
        float zoomTarget;
        Viewport view;
        public Vector2 center;
        public Vector2 centerTarget;
        public Vector2 nextCenterTarget; //Used for changing maps. You set the goal target to this, and switch it right before maps change
        Vector2 playerOffset;
        Boolean shaking = false;
        int shakeTimer;
        int shakeDuration;

        float shakeMagnitude;
        Random shakeNum;
        Vector2 shakeOffset;
        Matrix parallaxTransform;
        
        public static float cursorScale;

        public Matrix Transform { get { return transform; } }
        public Matrix StaticTransform { get { return staticTransform; } }
        public Vector2 Center { get { return center; } }
        public Viewport View { get { return view; } }
        public float Width { get { return (720f / Game1.aspectRatio) / zoom; } }

        public Camera(Viewport nview)
        {
            view = nview;
            shakeNum = new Random();
        }

        public void ShakeCamera(int time, float mag)
        {
            if (!shaking || mag > shakeMagnitude)
            {
                shaking = true;
                shakeMagnitude = mag;
                shakeTimer = 0;
                shakeDuration = time;
            }
        }

        public void UpdateShake()
        {

        }

        public void SnapTo(GameObject obj, Game1 g)
        {

        }

        public void Update(GameObject obj, Game1 g)
        {
            /*
            center = new Vector2(obj.Position.X, obj.Position.Y);

            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
            Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));*/

            //--Possibly replace this with a "zoom out" boolean in the mapclass


            center = new Vector2(obj.Position.X, obj.Position.Y);
            

            var gameWorldSize = new Vector2(1280, 720);
            var vp = g.GraphicsDevice.Viewport;

            float scaleX = vp.Width / gameWorldSize.X;
            float scaleY = vp.Height / gameWorldSize.Y;
            float scale = Math.Min(g.res.X / 1280f, g.res.Y / 720f);
            scale = Math.Max(scaleX, scaleY);

            float transX = -center.X * (scaleY);
            float transY = -center.Y * (scaleY);


            transform = Matrix.CreateScale(scaleY, scaleY, 1)
                    * Matrix.CreateTranslation(transX, transY, 0);



            staticTransform = Matrix.CreateScale(scaleX, scaleX, 1);

            cursorScale = scaleX;
        }

        public void Update(GameObject obj, Game1 g, MapClass current)
        {


            //--Possibly replace this with a "zoom out" boolean in the mapclass
            if (current.ZoomLevel != 1)
                zoomTarget = current.ZoomLevel;
            else
            {
                zoomTarget = 1f;
                zoom = 1f;
            }
            if (zoomTarget > zoom)
            {
                zoom = zoomTarget;
            }
            if (obj is Player)
            {
                centerTarget = new Vector2(obj.VitalRecX + (obj.VitalRecWidth / 2), 0);
                playerOffset = new Vector2(g.res.X / 2, 0);


                if (obj.YScroll == true)
                {
                    centerTarget += new Vector2(0, obj.VitalRecY + (obj.VitalRecHeight / 2));
                    playerOffset += new Vector2(0, g.res.Y / 2);
                }
            }
            else
            {
                centerTarget = new Vector2(obj.Position.X + (obj.Rec.Width / 2), 0);
                playerOffset = new Vector2(g.res.X / 2, 0);


                if (obj.YScroll == true)
                {
                    centerTarget += new Vector2(0, obj.Position.Y + (obj.Rec.Height / 2));
                    playerOffset += new Vector2(0, g.res.Y / 2);
                }
            }

            center += (centerTarget - center) * 0.05f;
            zoom += (zoomTarget - zoom) * 0.03f;

            var gameWorldSize = new Vector2(1280, 720);
            var vp = g.GraphicsDevice.Viewport;

            float scaleX = vp.Width / gameWorldSize.X;
            float scaleY = vp.Height / gameWorldSize.Y;
            float scale = Math.Min(g.res.X / 1280f, g.res.Y / 720f);
            scale = Math.Max(scaleX, scaleY);

            float transX = -center.X * (scaleY * zoom) + playerOffset.X;
            float transY = -center.Y * (scaleY * zoom) + playerOffset.Y;

            if (transX > 0)
                transX = 0;
            if (transX < -((current.MapWidth - Width) * scaleY * zoom))
                transX = -((current.MapWidth - Width) * scaleY * zoom);

            if (transY > -current.MapY * scaleY * zoom)
                transY = -current.MapY * scaleY * zoom;

            if (transY < -((current.MapY + current.MapHeight - 720 - 360) * scaleY * zoom) && current.yScroll)
                transY = -((current.MapY + current.MapHeight - 720 - 360) * scaleY * zoom);

            #region Shaking
            if (shaking)
            {
                shakeTimer++;
                if (shakeTimer >= shakeDuration)
                {
                    shaking = false;
                    shakeTimer = 0;
                }

                float progress = shakeTimer / shakeDuration;

                float magnitude = shakeMagnitude * (1f - (progress * progress));

                shakeOffset = new Vector2(NextFloat(), NextFloat()) * magnitude;

                //--Has to be transX and Y now, since we don't move the camera based off "center" anymore
                transX += shakeOffset.X;
                transY += shakeOffset.Y;
            }
            #endregion


            transform = Matrix.CreateScale(scaleY * zoom, scaleY * zoom, 1) 
                    * Matrix.CreateTranslation(transX, transY, 0);

            staticTransform = Matrix.CreateScale(scaleX, scaleX, 1);

            cursorScale = scaleX;

        }

        /// <summary>
        /// This needs to be ran in the chapter's update method every frame, so differing resolutions always have the static screens transform matrix
        /// </summary>
        public void UpdateStaticTransform(Game1 g)
        {
            var gameWorldSize = new Vector2(1280, 720);
            var vp = g.GraphicsDevice.Viewport;

            float scaleX = vp.Width / gameWorldSize.X;
            float scaleY = vp.Height / gameWorldSize.Y;
            float scale = Math.Min(g.res.X / 1280f, g.res.Y / 720f);
            scale = Math.Max(scaleX, scaleY);

            staticTransform = Matrix.CreateScale(scaleX, scaleX, 1);
        }

        public Matrix GetTransform(float parallax, MapClass current, Game1 g)
        {
            var gameWorldSize = new Vector2(1280, 720);
            var vp = g.GraphicsDevice.Viewport;

            float scaleX = vp.Width / gameWorldSize.X;
            float scaleY = vp.Height / gameWorldSize.Y;
            float scale = Math.Min(g.res.X / 1280f, g.res.Y / 720f);
            scale = Math.Max(scaleX, scaleY);

            float transX = -center.X * (scaleY * zoom) + playerOffset.X;
            float transY = -center.Y * (scaleY * zoom) + playerOffset.Y;

            if (transX > 0)
                transX = 0;
            if (transX < -((current.MapWidth - Width) * scaleY * zoom))
                transX = -((current.MapWidth - Width) * scaleY * zoom);

            if (transY > -current.MapY * scaleY * zoom)
                transY = -current.MapY * scaleY * zoom;

            if (transY < -((current.MapY + current.MapHeight - 720 - 360) * scaleY * zoom) && current.yScroll)
                transY = -((current.MapY + current.MapHeight - 720 - 360) * scaleY * zoom);

            parallaxTransform = Matrix.CreateScale(scaleY * zoom, scaleY * zoom, 1)
        * Matrix.CreateTranslation(transX * parallax, transY, 0);

            return parallaxTransform;
        }

        public Matrix GetTransformWithVertParallax(float parallaxX, float parallaxY, MapClass current, Game1 g)
        {
            var gameWorldSize = new Vector2(1280, 720);
            var vp = g.GraphicsDevice.Viewport;

            float scaleX = vp.Width / gameWorldSize.X;
            float scaleY = vp.Height / gameWorldSize.Y;
            float scale = Math.Min(g.res.X / 1280f, g.res.Y / 720f);
            scale = Math.Max(scaleX, scaleY);

            float transX = -center.X * (scaleY * zoom) + playerOffset.X;
            float transY = -center.Y * (scaleY * zoom) + playerOffset.Y;

            if (transX > 0)
                transX = 0;
            if (transX < -((current.MapWidth - Width) * scaleY * zoom))
                transX = -((current.MapWidth - Width) * scaleY * zoom);

            if (transY > -current.MapY * scaleY * zoom)
                transY = -current.MapY * scaleY * zoom;

            if (transY < -((current.MapY + current.MapHeight - 720 - 360) * scaleY * zoom) && current.yScroll)
                transY = -((current.MapY + current.MapHeight - 720 - 360) * scaleY * zoom);

            parallaxTransform = Matrix.CreateScale(scaleY * zoom, scaleY * zoom, 1)
        * Matrix.CreateTranslation(transX * parallaxX, transY * parallaxY, 0);

            return parallaxTransform;
        }

        public float NextFloat()
        {
            return (float)shakeNum.NextDouble() * 2f - 1f;
        }
    }
}

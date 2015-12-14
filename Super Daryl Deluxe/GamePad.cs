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
using SharpDX.XInput;

namespace ISurvived
{
    class MyGamePad
    {
        public static GamePadState currentState;
        public static GamePadState previousState;
        static int rumbleTime;
        static float currentRumbleAmount;
        public static Controller gamePad;
        /// <summary>
        /// Only call this if a gamepad is connected
        /// </summary>
        public void UpdateGamePad()
        {
            previousState = currentState;
            currentState = GamePad.GetState(PlayerIndex.One);

            if (rumbleTime > 0)
            {
                rumbleTime--;

                if (rumbleTime <= 0)
                {
                    gamePad.SetVibration(new Vibration());
                    currentRumbleAmount = 0;
                }
            }
        }

        public static void ResetStates()
        {
            currentState = new GamePadState();
            previousState = new GamePadState();
        }
        
        public static void SetRumble(int time, float amount)
        {
            if (gamePad != null)
            {
                if (amount < .3f)
                    amount = .3f;

                if (time < 3)
                    time = 3;

                if (amount > currentRumbleAmount)
                {
                    Vibration vib = new Vibration();
                    if (amount > 1) amount = 1;
                    vib.LeftMotorSpeed = (ushort)(amount * 65535);
                    vib.RightMotorSpeed = (ushort)(amount * 65535);
                    gamePad.SetVibration(vib);
                    rumbleTime = time;
                    currentRumbleAmount = (ushort)(amount * 65535);
                }
            }
        }

        #region BUTTONS PRESSED

        public static bool SelectPressed()
        {
            if (currentState.Buttons.Back == ButtonState.Released && previousState.Buttons.Back == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool StartPressed()
        {
            if (currentState.Buttons.Start == ButtonState.Released && previousState.Buttons.Start == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool APressed()
        {
            if (currentState.Buttons.A == ButtonState.Released && previousState.Buttons.A == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool BPressed()
        {
            if (currentState.Buttons.B == ButtonState.Released && previousState.Buttons.B == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool XPressed()
        {
            if (currentState.Buttons.X == ButtonState.Released && previousState.Buttons.X == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool YPressed()
        {
            if (currentState.Buttons.Y == ButtonState.Released && previousState.Buttons.Y == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool AHeld()
        {
            if (currentState.Buttons.A == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool BHeld()
        {
            if (currentState.Buttons.B == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool XHeld()
        {
            if (currentState.Buttons.X == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool YHeld()
        {
            if (currentState.Buttons.Y == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool RightTriggerPressed()
        {
            if (currentState.Triggers.Right > 0 && previousState.Triggers.Right == 0)
                return true;

            return false;
        }

        public static bool LeftTriggerPressed()
        {
            if (currentState.Triggers.Left > 0 && previousState.Triggers.Left == 0)
                return true;

            return false;
        }

        public static bool LeftBumperPressed()
        {
            if (currentState.Buttons.LeftShoulder == ButtonState.Released && previousState.Buttons.LeftShoulder == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool RightBumperPressed()
        {
            if (currentState.Buttons.RightShoulder == ButtonState.Released && previousState.Buttons.RightShoulder == ButtonState.Pressed)
                return true;

            return false;
        }


        #endregion

        #region DPAD PRESSED
        public static bool DownPadPressed()
        {
            if (currentState.DPad.Down == ButtonState.Released && previousState.DPad.Down == ButtonState.Pressed)
                return true;

            return false;
        }
        public static bool UpPadPressed()
        {
            if (currentState.DPad.Up == ButtonState.Released && previousState.DPad.Up == ButtonState.Pressed)
                return true;

            return false;
        }
        public static bool LeftPadPressed()
        {
            if (currentState.DPad.Left == ButtonState.Released && previousState.DPad.Left == ButtonState.Pressed)
                return true;

            return false;
        }
        public static bool RightPadPressed()
        {
            if (currentState.DPad.Right == ButtonState.Released && previousState.DPad.Right == ButtonState.Pressed)
                return true;

            return false;
        }



        public static bool DownPadHeld()
        {
            if (currentState.DPad.Down == ButtonState.Pressed)
                return true;

            return false;
        }
        public static bool UpPadHeld()
        {
            if (currentState.DPad.Up == ButtonState.Pressed)
                return true;

            return false;
        }
        public static bool LeftPadHeld()
        {
            if (currentState.DPad.Left == ButtonState.Pressed)
                return true;

            return false;
        }
        public static bool RightPadHeld()
        {
            if (currentState.DPad.Right == ButtonState.Pressed)
                return true;

            return false;
        }

        #endregion

        #region Analogs

        public static bool RightAnalogHeld()
        {
            if (currentState.ThumbSticks.Left.X > 0)
                return true;

            return false;
        }

        public static bool LeftAnalogHeld()
        {
            if (currentState.ThumbSticks.Left.X < 0)
                return true;

            return false;
        }

        public static bool DownAnalogHeld()
        {
            if (currentState.ThumbSticks.Left.Y <= -.9f)
                return true;

            return false;
        }

        public static bool DownAnalogPressed()
        {
            if (currentState.ThumbSticks.Left.Y == 0 && previousState.ThumbSticks.Left.Y <= -.9f)
                return true;

            return false;
        }

        public static bool RightDownAnalogPressed()
        {
            if (currentState.ThumbSticks.Right.Y <= -.7f && (int)(Game1.gameTimeData.TotalGameTime.TotalMilliseconds) % 30 == 0)
                return true;

            return false;
        }

        public static bool RightUpAnalogPressed()
        {
            if (currentState.ThumbSticks.Right.Y >= .9f && (int)(Game1.gameTimeData.TotalGameTime.TotalMilliseconds) % 30 == 0)
                return true;

            return false;
        }

        public static bool RightAnalogPressedIn()
        {
            if (currentState.Buttons.RightStick == ButtonState.Released && previousState.Buttons.RightStick == ButtonState.Pressed)
                return true;

            return false;
        }

        public static bool LeftAnalogPressedIn()
        {
            if (currentState.Buttons.LeftStick == ButtonState.Released && previousState.Buttons.LeftStick == ButtonState.Pressed)
                return true;

            return false;
        }
        #endregion
    }
}

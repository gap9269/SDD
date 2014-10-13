using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISurvived
{
    public class Cutscene
    {
        // ATTRIBUTES \\
        protected int timer;
        protected int state;
        public float alpha;
        protected bool notFirstFrame;
        protected bool fade;
        protected Game1 game;
        protected KeyboardState current;
        protected KeyboardState last;
        protected Rectangle dialogueBox;
        protected Vector2 textBox;
        protected Camera camera;
        protected List<String> dialogue;
        protected int dialogueState;
        protected Dictionary<String, NPC> cutsceneNPCs;
        protected Player player;
        protected Dictionary<String, Texture2D> textures;
        protected Boolean firstFrameOfTheState = true;
        protected String scrollDialogue;
        protected int scrollDialogueNum;
        protected ContentManager content;
        int stringNum;

        protected int topBarPos;
        protected int botBarPos;
        bool firstFrameOfCutscene = true;

        protected int DialogueState
        {
            get { return dialogueState; }
            set
            {
                dialogueState = value; 
                stringNum = dialogue[dialogueState].Length; 
                scrollDialogueNum = 0;
                scrollDialogue = "";
            }
        }

        // CONSTRUCTOR \\
        public Cutscene(Game1 g, Camera cam, Player player)
        {
            state = 0;
            fade = false;
            timer = 0;
            alpha = 1;
            notFirstFrame = false;
            game = g;
            dialogue = new List<string>();
            dialogueState = 0;
            camera = cam;
            cutsceneNPCs = new Dictionary<string, NPC>();
            this.player = player;

            content = new ContentManager(g.Services);
            content.RootDirectory = "Content";
        }

        public Cutscene(Game1 g, Camera cam)
        {
            state = 0;
            fade = false;
            timer = 0;
            alpha = 1;
            notFirstFrame = false;
            game = g;
            dialogue = new List<string>();
            dialogueState = 0;
            camera = cam;
            cutsceneNPCs = new Dictionary<string, NPC>();

            content = new ContentManager(g.Services);
            content.RootDirectory = "Content";
        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public int State { get { return state; } set { state = value; } }

        // METHODS \\


        //
        // IMPORTANT: Play each cutscene in the form of a switch statement based on 'state'
        //

        public void SkipScene()
        {
            last = current;
            current = Keyboard.GetState();

            if (current.IsKeyUp(Keys.Escape) && last.IsKeyDown(Keys.Escape))
            {
                state++;
                timer = 0;
            }

        }

        /// <summary>
        /// Updates the timer and the position of the dialogue and text box
        /// </summary>
        public virtual void Play()
        {
            //SkipScene();
            if (firstFrameOfCutscene)
            {
                firstFrameOfCutscene = false;
                topBarPos = -66;
                botBarPos = (int)(Game1.aspectRatio * 1280);
            }

            timer++;
            dialogueBox = new Rectangle(0, (int)(Game1.aspectRatio * 1280) - 120, 1280, 120);

            if (timer == 1)
                firstFrameOfTheState = true;
            else
                firstFrameOfTheState = false;
        }

        //-- FADES ARE DONE BY USING A BLACK SQUARE PLACED OVER THE SCREEN AND CHANGING THE ALPHA --\\

        //--Fade into the scene
        public virtual void FadeIn(float length)
        {
            //--If it is the first frame, set the timer to 0 and start the fade
            if (notFirstFrame == false)
            {
                timer = 0;
                fade = true;
                alpha = 1;
            }

            notFirstFrame = true;

            //--If the fade hasn't ran the full length yet, decrease the alpha (of the black rectangle)
            //--It decreases by an even amount every frame, so 1/length
            if (timer < length)
            {
                alpha -= (float)(1f / length);
            }
            //--Once the timer has run the full length, reset the attributes and increase the state number
            else
            {
                notFirstFrame = false;
                timer = 0;
                alpha = -1f;
                state++;
                fade = false;
            }
        }

        //--Same as fade in, except backwarrds
        public virtual void FadeOut(float length)
        {
            //--First frame, start the fade out. Alpha is 0 for the black square
            if (notFirstFrame == false)
            {
                alpha = 0;
                timer = 0;
                fade = true;
            }

            notFirstFrame = true;

            //--Increase alpha as the timer increases
            if (timer < length)
            {
                alpha += (float)(1f / length);
            }
            //--At the end, reset the attributes and increase state
            else
            {
                notFirstFrame = false;
                timer = 0;
                alpha = -1f;
                state++;
                fade = false;
            }
        }

        /// <summary>
        /// Empty. Leave this up to the child classes
        /// </summary>
        /// <param name="s"></param>
        public virtual void Draw(SpriteBatch s)
        {

        }

        public void DrawFade(SpriteBatch s, float startAlpha)
        {
            if (notFirstFrame == false)
                alpha = startAlpha;
            s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.Black * alpha);
        }

        public void DrawFadeWhite(SpriteBatch s, float startAlpha)
        {
            if (notFirstFrame == false)
                alpha = startAlpha;
            s.Draw(Game1.whiteFilter, new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White * alpha);
        }

        /// <summary>
        /// Plays the dialogue of the cutscene. State increments at the end of the list, and dialogueState resets
        /// </summary>
        public virtual void PlayDialogue()
        {
            last = current;
            current = Keyboard.GetState();

            //--Hitting enter increases the dialogue state, which reads the next string in the list
            if ((last.IsKeyDown(Keys.Enter) && current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed())
            {
                scrollDialogueNum = 0;
                dialogueState++;
                scrollDialogue = "";
            }

            //--Hitting the end of the list resets the dialogue state and increases the cutscene state
            if (dialogueState == dialogue.Count)
            {
                scrollDialogueNum = 0;
                scrollDialogue = "";

                dialogueState = 0;
                state++;
            }
        }

        public virtual void PlayDialogue(NPC npc)
        {
            last = current;
            current = Keyboard.GetState();

            dialogue = npc.Dialogue;
            npc.Talking = true;
            npc.UpdateInteraction();

            
            //--Hitting enter increases the dialogue state, which reads the next string in the list
            if ((last.IsKeyDown(Keys.Enter) && current.IsKeyUp(Keys.Enter)) || MyGamePad.APressed())
            {
                scrollDialogueNum = 0;
                scrollDialogue = "";
                dialogueState++;
            }

            //--Hitting the end of the list resets the dialogue state and increases the cutscene state
            if (dialogueState == dialogue.Count)
            {
                scrollDialogueNum = 0;
                scrollDialogue = "";
                npc.Talking = false;
                dialogueState = 0;
                state++;
            }
        }

        public virtual void DrawCutsceneBars(SpriteBatch s)
        {
            if (topBarPos < 0)
                topBarPos += 3;
            if (botBarPos > (int)(Game1.aspectRatio * 1280) - 66)
                botBarPos -= 3;

            s.Draw(Game1.whiteFilter, new Rectangle(0, topBarPos, 1280, 66), Color.Black);
            s.Draw(Game1.whiteFilter, new Rectangle(0,botBarPos, 1280, 66), Color.Black);
        }

        public virtual void DrawRemoveCutsceneBars(SpriteBatch s)
        {
            if (topBarPos > -66)
                topBarPos -= 3;
            if (botBarPos < (int)(Game1.aspectRatio * 1280))
                botBarPos += 3;

            s.Draw(Game1.whiteFilter, new Rectangle(0, topBarPos, 1280, 66), Color.Black);
            s.Draw(Game1.whiteFilter, new Rectangle(0, botBarPos, 1280, 66), Color.Black);
        }

        //--Draw the dialogue box using the static white box in game, and the position of the dialoguebox
        //--Draw the string that is found in the list at the index of [dialogueState]
        public virtual void DrawDialogue(SpriteBatch s)
        {
            Vector2 length = Game1.font.MeasureString(dialogue[dialogueState]);
            float lengthX = length.X / 2;
            textBox = new Vector2((dialogueBox.X + dialogueBox.Width / 2) - lengthX, dialogueBox.Y + 20);

            if (scrollDialogueNum < stringNum)
            {
                scrollDialogue += dialogue[dialogueState].ElementAt(scrollDialogueNum);
                scrollDialogueNum++;

                if (scrollDialogueNum % 5 == 0)
                    Sound.PlaySoundInstance(Sound.SoundNames.TextScroll);
            }

            s.Draw(Game1.whiteFilter, dialogueBox, Color.LightGray * .7f);
            s.DrawString(Game1.font, scrollDialogue, textBox, Color.Black);
        }

        public virtual void DrawDialogue(SpriteBatch s, NPC npc)
        {
            npc.DrawDialogue(s);   
        }

        public void DrawGainItemBox(SpriteBatch s, String message)
        {
            Vector2 messageMeasure = Game1.pickUpFont.MeasureString(message);
            float textWidth = messageMeasure.X;
            float textHeight = messageMeasure.Y;

            s.Draw(Game1.emptyBox,
                new Rectangle(1280 / 2 - (int)textWidth / 2 - 25, (int)(Game1.aspectRatio * 1280) / 2 - (int)textHeight / 2 - 10, (int)textWidth + 50, (int)textHeight + 20),
                Color.Gray);
            s.DrawString(Game1.pickUpFont, message, new Vector2(1280 / 2 - (int)textWidth / 2, (int)(Game1.aspectRatio * 1280) / 2 - (int)textHeight / 2), Color.Black);
        }

        public static void DrawGainItemBoxStatic(SpriteBatch s, String message)
        {
            Vector2 messageMeasure = Game1.pickUpFont.MeasureString(message);
            float textWidth = messageMeasure.X;
            float textHeight = messageMeasure.Y;

            s.Draw(Game1.emptyBox,
                new Rectangle(1280 / 2 - (int)textWidth / 2 - 25, (int)(Game1.aspectRatio * 1280) / 2 - (int)textHeight / 2 - 10, (int)textWidth + 50, (int)textHeight + 20),
                Color.Gray);
            s.DrawString(Game1.pickUpFont, message, new Vector2(1280 / 2 - (int)textWidth / 2, (int)(Game1.aspectRatio * 1280) / 2 - (int)textHeight / 2), Color.Black);
        }
    }
}

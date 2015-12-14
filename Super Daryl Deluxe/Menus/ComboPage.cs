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
    public class ComboPage
    {
        Dictionary<String, Texture2D> textures;
        Dictionary<String, String> lockerCombos;

        Button leftArrow, rightArrow;

        int page;

        Game1 game;
        KeyboardState current;
        KeyboardState last;

        public int Page { get; set; }

        public Dictionary<String, String> LockerCombos { get { return lockerCombos; } set { lockerCombos = value; } }

        public ComboPage(Dictionary<String, Texture2D> texts, Game1 g)
        {
            lockerCombos = new Dictionary<string, string>();
            textures = texts;
            game = g;
            UpdateResolution();

            leftArrow = new Button(new Rectangle(641, 648, 40, 28));
            rightArrow = new Button(new Rectangle(758, 644, 35, 29));
        }

        public void UpdateResolution()
        {
        }

        public void Update()
        {
            last = current;
            current = Keyboard.GetState();

            // Use this to test the placement and page stuff
            // if (lockerCombos.Count < 19)
            // {
            //     for (int i = 0; i < 20; i++)
            //     {
            //         AddCombo("Locker" + i.ToString(), "0-0-0");
            //     }
            // }

            if (game.CurrentChapter.state == Chapter.GameState.noteBook)
            {
                if (DarylsNotebook.journalTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.journal;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                    Chapter.effectsManager.RemoveToolTip();
                }

                if (DarylsNotebook.inventoryTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.inventory;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                    Chapter.effectsManager.RemoveToolTip();
                }

                if (DarylsNotebook.bioTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.bios;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                    Chapter.effectsManager.RemoveToolTip();
                }

                if (DarylsNotebook.questsTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.quests;
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                    Chapter.effectsManager.RemoveToolTip();
                }
                if (DarylsNotebook.mapsTab.Clicked())
                {
                    game.Notebook.state = DarylsNotebook.State.maps;
                    Chapter.effectsManager.RemoveToolTip();
                    Sound.PlaySoundInstance(Sound.SoundNames.ui_general_tab);
                }
            }
            if ((leftArrow.Clicked() || MyGamePad.LeftPadPressed())&& page > 0)
            {
                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_page_01);
                page--;
            }
            if ((rightArrow.Clicked()|| MyGamePad.RightPadPressed()) && page < 2)
            {
                Sound.PlaySoundInstance(Sound.SoundNames.ui_inventory_page_02);
                page++;
            }
        }

        public void AddCombo(String namesLocker, String comboWithDashes)
        {
            lockerCombos.Add(namesLocker, comboWithDashes);
        }

        public void Draw(SpriteBatch s)
        {
            s.Draw(textures["Background"], new Rectangle(0, 0, 1280, (int)(Game1.aspectRatio * 1280)), Color.White);

            if (page > 0)
            {
                if (Game1.gamePadConnected)
                    s.Draw(DarylsNotebook.dLeft, new Vector2(643, 644), Color.White);
                else
                {
                    if (leftArrow.IsOver())
                        s.Draw(textures["leftActive"], new Rectangle(626, 640, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
                    else
                        s.Draw(textures["leftStatic"], new Rectangle(626, 640, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
                }
            }
            if (page < 2)
            {
                if (Game1.gamePadConnected)
                    s.Draw(DarylsNotebook.dRight, new Vector2(758, 642), Color.White);
                else
                {
                    if (rightArrow.IsOver())
                        s.Draw(textures["rightActive"], new Rectangle(626, 640, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
                    else
                        s.Draw(textures["rightStatic"], new Rectangle(626, 640, textures["leftActive"].Width, textures["leftActive"].Height), Color.White);
                }
            }

            s.DrawString(Game1.font, (page + 1).ToString() + " / 3", new Vector2(699, 648), Color.Black);

            //18 is the number of combos per page
            for (int i = 0; i < 18; i++)
            {
                if (lockerCombos.Count > i + (page * 18))
                {

                    if (i < 9)
                    {
                        //--Draw the person's name
                        s.DrawString(Game1.twConMedium, lockerCombos.ElementAt(i + (page * 18)).Key, new Vector2(340, (180) + (i * 50)), Color.Black);

                        //--Draw the combo
                        s.DrawString(Game1.twConMedium, lockerCombos.ElementAt(i + (page * 18)).Value, new Vector2(545, (180) + (i * 50)), Color.Black);
                    }
                    else
                    {
                        //--Draw the person's name
                        s.DrawString(Game1.twConMedium, lockerCombos.ElementAt(i + (page * 18)).Key, new Vector2(780, (180) + ((i - 9) * 50)), Color.Black);

                        //--Draw the combo
                        s.DrawString(Game1.twConMedium, lockerCombos.ElementAt(i + (page * 18)).Value, new Vector2(985, (180) + ((i - 9) * 50)), Color.Black);
                    }
                }
            }
        }
    }
}

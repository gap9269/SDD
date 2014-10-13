using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ISurvived
{
    static class Sound
    {
        public static Dictionary<String, SoundEffectInstance> music;
        public static Dictionary<String, SoundEffectInstance> ambience;
        public static Dictionary<String, SoundEffect> permanentSoundEffects;
        public static Dictionary<String, SoundEffect> menuSoundEffects;
        public static Dictionary<String, SoundEffect> enemySoundEffects;
        public static Dictionary<String, SoundEffect> skillSoundEffects;

        public static ContentManager permanentContent, backgroundMusicContent, menuContent, ambienceContent;
        public static float backgroundVolume = .3f;
        public static float soundVolume = .5f;
        public static float ambienceVolume = .3f;

        public static Boolean muted = false;

        public enum SoundNames
        {
            PlayerRunRoom1,
            PlayerRunRoom2,
            PlayerRunRoom3,
            PlayerLanding,
            PlayerJump1,
            PlayerJump2,

            TextScroll,
            CoinPickUp,
            DoorOpen,

            UIBack,
            UIEnter,
            UIOpen,
            UIClose,
            UITab,
            EquipOutfit1,
            EquipOutfit2,
            EquipWeapon1,
            EquipWeapon2,
            UIList1,
            UIList2,
            UIPage1,
            UIPage2,
            UIPaperTab1,
            UIPaperTab2,
            UIPaperTab3,
            UIPaperTab4,
            UIQuestOpen
        }

        static public void ResetSound()
        {
            ambience = new Dictionary<string, SoundEffectInstance>();
            music = new Dictionary<string, SoundEffectInstance>();
            menuSoundEffects = new Dictionary<string, SoundEffect>();
            enemySoundEffects = new Dictionary<string, SoundEffect>();

           //backgroundMusicContent.Unload();
            menuContent.Unload();
            //ambienceContent.Unload();
        }

        static public void LoadMenuSounds()
        {

            menuSoundEffects.Add("UIOpen", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_open"));
            menuSoundEffects.Add("EquipOutfit1", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_equip_clothes_01"));
            menuSoundEffects.Add("EquipOutfit2", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_equip_clothes_02"));
            menuSoundEffects.Add("EquipWeapon1", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_equip_weapon_01"));
            menuSoundEffects.Add("EquipWeapon2", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_equip_weapon_02"));

            menuSoundEffects.Add("UIList1", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_01"));
            menuSoundEffects.Add("UIList2", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_02"));
            menuSoundEffects.Add("UIPage1", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_page_01"));
            menuSoundEffects.Add("UIPage2", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_page_02"));

            menuSoundEffects.Add("UIPaperTab1", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_01"));
            menuSoundEffects.Add("UIPaperTab2", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_02"));
            menuSoundEffects.Add("UIPaperTab3", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_03"));
            menuSoundEffects.Add("UIPaperTab4", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_04"));

            menuSoundEffects.Add("UIQuestOpen", menuContent.Load<SoundEffect>(@"Sound\UI\ui_quest_open"));
        }

        static public void UnloadMenuSounds()
        {
            menuSoundEffects.Clear();
            menuContent.Unload();
        }


        static public void LoadPermanentContent()
        {
            ambience = new Dictionary<string, SoundEffectInstance>();
            music = new Dictionary<string, SoundEffectInstance>();
            menuSoundEffects = new Dictionary<string, SoundEffect>();
            permanentSoundEffects = new Dictionary<string, SoundEffect>();
            enemySoundEffects = new Dictionary<string, SoundEffect>();
            skillSoundEffects = new Dictionary<string, SoundEffect>();

            // Load sound effects
            permanentSoundEffects.Add("PlayerRunRoom1", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_room_01"));
            permanentSoundEffects.Add("PlayerRunRoom2", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_room_02"));
            permanentSoundEffects.Add("PlayerRunRoom3", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_room_03"));
            permanentSoundEffects.Add("PlayerLanding", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_land_room_01"));
            permanentSoundEffects.Add("PlayerJump1", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_room_01"));
            permanentSoundEffects.Add("PlayerJump2", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_room_02"));

            permanentSoundEffects.Add("TextScroll", permanentContent.Load<SoundEffect>("Sound\\textScroll"));
            permanentSoundEffects.Add("CoinPickUp", permanentContent.Load<SoundEffect>("SoundEffects\\CoinPickUp"));
            permanentSoundEffects.Add("DoorOpen", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_door_open"));

            permanentSoundEffects.Add("UIClose", permanentContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_close"));
            permanentSoundEffects.Add("UIEnter", permanentContent.Load<SoundEffect>(@"Sound\UI\ui_general_enter"));
            permanentSoundEffects.Add("UIBack", permanentContent.Load<SoundEffect>(@"Sound\UI\ui_general_back"));
            permanentSoundEffects.Add("UITab", permanentContent.Load<SoundEffect>(@"Sound\UI\ui_general_tab"));

            skillSoundEffects.Add("DiscussDifferencesUse1", permanentContent.Load<SoundEffect>(@"Sound\Skills\DiscussDifferences\weapon_punch_a_01"));
            skillSoundEffects.Add("DiscussDifferencesUse2", permanentContent.Load<SoundEffect>(@"Sound\Skills\DiscussDifferences\weapon_punch_b_01"));
            skillSoundEffects.Add("DiscussDifferencesUse3", permanentContent.Load<SoundEffect>(@"Sound\Skills\DiscussDifferences\weapon_punch_c_01"));

            skillSoundEffects.Add("QuickRetortUse1", permanentContent.Load<SoundEffect>(@"Sound\Skills\QuickRetort\weapon_dash_a_01"));

            skillSoundEffects.Add("SharpCommentsUse1", permanentContent.Load<SoundEffect>(@"Sound\Skills\SharpComments\weapon_sword_a_01"));

            skillSoundEffects.Add("ShockingStatementUse1", permanentContent.Load<SoundEffect>(@"Sound\Skills\ShockingStatement\weapon_zap_a_01"));
            skillSoundEffects.Add("ShockingStatementUse2", permanentContent.Load<SoundEffect>(@"Sound\Skills\ShockingStatement\weapon_zap_a_02"));
        }

        static public void UnloadAmbience()
        {
            StopAmbience();
            ambience.Clear();
            ambienceContent.Unload();
        }

        static public void UnloadBackgroundMusic()
        {
            StopBackgroundMusic();
            music.Clear();
            backgroundMusicContent.Unload();
        }

        static public void StopBackgroundMusic()
        {
            for (int i = 0; i < music.Count; i++)
            {
                music.ElementAt(i).Value.Stop();
            }
        }

        static public void StopAmbience()
        {
            for (int i = 0; i < ambience.Count; i++)
            {
                ambience.ElementAt(i).Value.Stop();
            }
        }

        static public void PauseBackgroundMusic()
        {
            for (int i = 0; i < music.Count; i++)
            {
                if (music.ElementAt(i).Value.State == SoundState.Playing)
                    music.ElementAt(i).Value.Pause();
            }
        }

        static public void ResumeBackgroundMusic()
        {
            for (int i = 0; i < music.Count; i++)
            {
                if (music.ElementAt(i).Value.State == SoundState.Paused)
                    music.ElementAt(i).Value.Resume();
            }
        }

        static public void IncrementBackgroundVolume(float incrementVolume)
        {
            backgroundVolume += incrementVolume;

            if (backgroundVolume < 0)
                backgroundVolume = 0;

            if (backgroundVolume > 1)
                backgroundVolume = 1;
        }

        static public void IncrementAmbienceVolume(float incrementVolume)
        {
            ambienceVolume += incrementVolume;

            if (ambienceVolume < 0)
                ambienceVolume = 0;

            if (ambienceVolume > 1)
                ambienceVolume = 1;
        }

        static public void SetBackgroundVolume(float volume)
        {
            backgroundVolume = volume;

            if (backgroundVolume < 0)
                backgroundVolume = 0;

            if (backgroundVolume > 1)
                backgroundVolume = 1;
        }

        static public void PlayAmbience(String ambienceName)
        {
            if (!muted)
            {
                //Creating a instance allows you to modify the sound playing, eg Change volume.
                if (ambience[ambienceName].State == SoundState.Stopped)
                {
                    ambience[ambienceName].Volume = ambienceVolume;
                    ambience[ambienceName].Play();
                }
                else
                {
                    ambience[ambienceName].Volume = ambienceVolume;
                    ambience[ambienceName].Resume();
                }
            }
        }

        static public void PlayBackGroundMusic(String musicName)
        {
            if (!muted)
            {
                //Creating a instance allows you to modify the sound playing, eg Change volume.
                if (music[musicName].State == SoundState.Stopped)
                {
                    music[musicName].Volume = backgroundVolume;
                    music[musicName].Play();
                }
                else
                {
                    music[musicName].Volume = backgroundVolume;
                    music[musicName].Resume();
                }
            }
        }

        static public void PlayBackGroundMusic(String musicName, float vol, float pan)
        {
            if (!muted)
            {
                if (pan < 0)
                    pan = 0;
                if (pan > 1)
                    pan = 1;

                if (vol < 0)
                    vol = 0;
                if (vol > 1)
                    vol = 1;

                //Creating a instance allows you to modify the sound playing, eg Change volume.
                if (music[musicName].State == SoundState.Stopped)
                {
                    music[musicName].Pan = pan;
                    music[musicName].Volume = vol;
                    music[musicName].Play();
                }
                else
                {
                    music[musicName].Pan = pan;
                    music[musicName].Volume = vol;
                    music[musicName].Resume();
                }
            }
        }

        /// <summary>
        /// Creates and plays a sound effect instance
        /// </summary>
        /// <param name="soundInstanceName"></param>
        static public void PlaySoundInstance(SoundNames soundInstanceName)
        {
            //if (soundEffects[soundInstanceName.ToString()].State != SoundState.Playing)
            if (permanentSoundEffects.ContainsKey(soundInstanceName.ToString()))
            {
                SoundEffectInstance inst = permanentSoundEffects[soundInstanceName.ToString()].CreateInstance();
                inst.Volume = soundVolume;
                inst.Play();
            }
            else if (menuSoundEffects.ContainsKey(soundInstanceName.ToString()))
            {
                SoundEffectInstance inst = menuSoundEffects[soundInstanceName.ToString()].CreateInstance();
                inst.Volume = soundVolume;
                inst.Play();
            }
        }
    }
}
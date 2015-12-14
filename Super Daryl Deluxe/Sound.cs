using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;


namespace ISurvived
{
    public static class Sound
    {
        public static Dictionary<String, SoundEffectInstance> music;
        public static Dictionary<String, SoundEffectInstance> ambience;
        public static Dictionary<String, SoundEffect> permanentSoundEffects;
        public static Dictionary<String, SoundEffect> menuSoundEffects;
        public static Dictionary<String, SoundEffect> enemySoundEffects;
        public static Dictionary<String, SoundEffect> mapZoneSoundEffects;

        static Dictionary<String, float> soundEffectVolumes;

        public static List<SoundEffectInstance> poofSounds;

        public static List<SoundEffectInstance> currentlyPlayingSounds;
        public static List<String> currentlyPlayingSounds_NAMES;

        public static ContentManager permanentContent, backgroundMusicContent, menuContent, ambienceContent, mapZoneContent;

        public static float currentBackgroundVolume = 1f;
        public static float currentSoundVolume = 1f;
        public static float currentAmbienceVolume = 1f;

        public static float setBackgroundVolume = 1f;
        public static float setSoundVolume = 1f;
        public static float setAmbienceVolume = 1f;

        public static Boolean muted = false;

        public enum MusicNames
        {
            none,
            NoirHalls,
            DoingScienceLow,
            DoingScienceMed,
            DoingScienceHigh,
            FortBattle,
            TimFight,
            FurryFuneral,
            PaulAndAlanTheme
        }
        public static MusicNames musicName;

        public enum SoundNames
        {
            movement_footstep_walk_room_01,
            movement_footstep_walk_room_02,
            movement_footstep_walk_room_03,
            movement_land_room_01,
            movement_land_room_02,
            movement_jump_room_01,
            movement_jump_room_02,

            movement_footstep_walk_outside_01,
            movement_footstep_walk_outside_02,
            movement_footstep_walk_outside_03,
            movement_footstep_walk_outside_04,
            movement_footstep_walk_outside_05,
            movement_footstep_walk_outside_06,
            movement_footstep_walk_hallway_01,
            movement_footstep_walk_hallway_02,
            movement_footstep_walk_hallway_03,
            movement_footstep_walk_hallway_04,
            movement_footstep_walk_hallway_05,
            movement_footstep_walk_hallway_06,
            movement_land_outside_01,
            movement_land_outside_02,
            movement_jump_outside_01,
            movement_jump_outside_02,
            movement_land_hallway_01,
            movement_land_hallway_02,
            movement_jump_hallway_01,
            movement_jump_hallway_02,
            movement_unduck,
            movement_duck,

            ui_general_text_advance,
            popup_text_message,
            object_pickup_coin_01,
            object_pickup_coin_02,
            object_pickup_coin_03,
            object_pickup_coin_04,
            object_pickup_coin_05,
            object_pickup_health,
            object_pickup_misc,
            object_pickup_combo,
            object_pickup_loot,
            object_pickup_textbook,
            object_platform_start,
            object_platform_loop,
            object_locker_crash,
            object_locker_hit,
            movement_door_open,
            movement_portal_enter,
            movement_stairs,
            popup_enter,
            popup_exit,
            object_button_large,
            popup_level_up,
            popup_skill_level_up,
            popup_social_rank_up,
            popup_quest_completed,
            popup_save_game,
            popup_load_game,

            ui_general_back,
            ui_general_enter,
            ui_inventory_open,
            ui_inventory_close,
            ui_general_tab,
            ui_inventory_equip_clothes_01,
            ui_inventory_equip_clothes_02,
            ui_inventory_equip_weapon_01,
            ui_inventory_equip_weapon_02,
            ui_inventory_list_01,
            ui_inventory_list_02,
            ui_inventory_page_01,
            ui_inventory_page_02,
            ui_inventory_tab_01,
            ui_inventory_tab_02,
            ui_inventory_tab_03,
            ui_inventory_tab_04,
            ui_quest_open,
            ui_trenchcoat_sell,
            ui_trenchcoat_select,
            ui_trenchcoat_open,
            ui_trenchcoat_buy,
            ui_general_text_scroll,

            ui_locker_player_open,
            ui_locker_playermenu_appear,
            ui_locker_shopmenu_appear,
            ui_locker_equip_skill,
            ui_locker_unequip_skill,
            ui_locker_cant_equip_skill,
            ui_locker_click_skill_01,
            ui_locker_click_skill_02,
            ui_locker_buy_skill,
            ui_locker_change_menu,
            ui_locker_shut,

            ui_locker_other_menu_open,
            ui_locker_other_menu_appear,
            ui_locker_dial_click_01,
            ui_locker_dial_click_02,
            ui_locker_dial_click_03,
            ui_locker_dial_click_04,
            ui_locker_dial_click_05,
            ui_locker_dial_click_06,
            ui_locker_spin_loop,
            ui_locker_spin_loop_end,
            ui_locker_unlock,
            ui_locker_take_item_book,
            ui_locker_take_item_key,
            ui_locker_take_item_money,
            ui_locker_take_item_outfit,
            ui_locker_take_item_weapon,

            //MAP SPECIFIC STUFF
            movement_footstep_walk_science_01,
            movement_footstep_walk_science_02,
            movement_footstep_walk_science_03,
            movement_footstep_walk_science_04,
            movement_footstep_walk_science_05,
            movement_footstep_walk_science_06,
            movement_land_science_01,
            movement_land_science_02,
            movement_jump_science_01,
            movement_jump_science_02,
            object_goblin_hut_damage, object_goblin_hut_destroy,

            //Vents
            movement_footstep_walk_vent_01,
            movement_footstep_walk_vent_02,
            movement_footstep_walk_vent_03,
            movement_footstep_walk_vent_04,
            movement_footstep_walk_vent_05,
            movement_footstep_walk_vent_06,
            movement_land_vent_01,
            movement_land_vent_02,
            movement_jump_vent_01,
            movement_jump_vent_02,
            object_steam_vent_loop,
            object_steam_vent_start,
            object_steam_vent_stop,
            object_coal_break_01,
            object_coal_break_02,
            object_coal_damage_01,
            object_coal_damage_02,
            object_coal_damage_03,
        }

        /// <summary>
        /// distanceStrength higher number makes the pan happen slower. Default is 600
        /// </summary>
        public static float GetSoundPan(int soundXOrigin, int distanceStrength = 600)
        {
            float pan = -((Game1.Player.VitalRec.Center.X - soundXOrigin) / distanceStrength);
            if (pan < -.5f)
                pan = -.5f;
            if (pan > .5f)
                pan = .5f;

            return pan;
        }

        /// <summary>
        /// Higher distancestrength means the sound can be heard from farther away
        /// </summary>
        public static float GetSoundVolume(Vector2 soundOrigin, int maxDistanceToHearSound, int distanceStrength = 500, float maxVolume = 1.0f)
        {
            float dis = Vector2.Distance(soundOrigin, new Vector2(Game1.Player.Rec.Center.X, Game1.Player.Rec.Center.Y));
            float vol = distanceStrength / Vector2.Distance(soundOrigin, new Vector2(Game1.Player.Rec.Center.X, Game1.Player.Rec.Center.Y));

            if (vol > 1)
                vol = 1;

            if (Math.Abs(dis) > maxDistanceToHearSound)
                vol = 0;

            return vol;
        }

        static public void ResetSound()
        {
            currentlyPlayingSounds.Clear();
            UnloadAmbience();
            UnloadBackgroundMusic();
            UnloadMenuSounds();
            UnloadMapZoneSounds();

            ambience = new Dictionary<string, SoundEffectInstance>();
            music = new Dictionary<string, SoundEffectInstance>();
            menuSoundEffects = new Dictionary<string, SoundEffect>();
            enemySoundEffects = new Dictionary<string, SoundEffect>();
            mapZoneSoundEffects = new Dictionary<string, SoundEffect>();
            currentlyPlayingSounds = new List<SoundEffectInstance>();
            currentlyPlayingSounds_NAMES = new List<string>();
            soundEffectVolumes = new Dictionary<string, float>();

            LoadSoundVolumeFile();

        }

        static public void LoadStudentLockerSounds()
        {
            menuSoundEffects.Add("ui_locker_player_open", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_player_open"));
            menuSoundEffects.Add("ui_locker_other_menu_open", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_other_menu_open"));
            menuSoundEffects.Add("ui_locker_other_menu_appear", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_other_menu_appear"));
            menuSoundEffects.Add("ui_locker_dial_click_01", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_dial_click_01"));
            menuSoundEffects.Add("ui_locker_dial_click_02", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_dial_click_02"));
            menuSoundEffects.Add("ui_locker_dial_click_03", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_dial_click_01"));
            menuSoundEffects.Add("ui_locker_dial_click_04", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_dial_click_04"));
            menuSoundEffects.Add("ui_locker_dial_click_05", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_dial_click_05"));
            menuSoundEffects.Add("ui_locker_dial_click_06", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_dial_click_06"));
            menuSoundEffects.Add("ui_locker_spin_loop", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_spin_loop"));
            menuSoundEffects.Add("ui_locker_spin_loop_end", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_spin_loop_end"));
            menuSoundEffects.Add("ui_locker_unlock", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_unlock"));
            menuSoundEffects.Add("ui_locker_take_item_book", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_take_item_book"));
            menuSoundEffects.Add("ui_locker_take_item_key", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_take_item_key"));
            menuSoundEffects.Add("ui_locker_take_item_money", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_take_item_money"));
            menuSoundEffects.Add("ui_locker_take_item_outfit", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_take_item_outfit"));
            menuSoundEffects.Add("ui_locker_take_item_weapon", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_take_item_weapon"));
            menuSoundEffects.Add("ui_inventory_list_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_01"));
            menuSoundEffects.Add("ui_inventory_list_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_02"));
        }

        static public void LoadTrenchcoatSounds()
        {
            menuSoundEffects.Add("ui_trenchcoat_sell", menuContent.Load<SoundEffect>(@"Sound\UI\Trench\ui_trenchcoat_sell"));
            menuSoundEffects.Add("ui_trenchcoat_buy", menuContent.Load<SoundEffect>(@"Sound\UI\Trench\ui_trenchcoat_buy"));
            menuSoundEffects.Add("ui_trenchcoat_open", menuContent.Load<SoundEffect>(@"Sound\UI\Trench\ui_trenchcoat_open"));
            menuSoundEffects.Add("ui_trenchcoat_select", menuContent.Load<SoundEffect>(@"Sound\UI\Trench\ui_trenchcoat_select"));

            menuSoundEffects.Add("ui_inventory_list_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_01"));
            menuSoundEffects.Add("ui_inventory_list_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_02"));
            menuSoundEffects.Add("ui_inventory_page_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_page_01"));
            menuSoundEffects.Add("ui_inventory_page_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_page_02"));

            menuSoundEffects.Add("ui_inventory_tab_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_01"));
            menuSoundEffects.Add("ui_inventory_tab_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_02"));
            menuSoundEffects.Add("ui_inventory_tab_03", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_03"));
            menuSoundEffects.Add("ui_inventory_tab_04", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_04"));
        }

        static public void LoadNotebookSounds()
        {
            menuSoundEffects.Add("ui_inventory_open", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_open"));
            menuSoundEffects.Add("ui_inventory_equip_clothes_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_equip_clothes_01"));
            menuSoundEffects.Add("ui_inventory_equip_clothes_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_equip_clothes_02"));
            menuSoundEffects.Add("ui_inventory_equip_weapon_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_equip_weapon_01"));
            menuSoundEffects.Add("ui_inventory_equip_weapon_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_equip_weapon_02"));

            menuSoundEffects.Add("ui_inventory_list_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_01"));
            menuSoundEffects.Add("ui_inventory_list_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_02"));
            menuSoundEffects.Add("ui_inventory_page_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_page_01"));
            menuSoundEffects.Add("ui_inventory_page_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_page_02"));

            menuSoundEffects.Add("ui_inventory_tab_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_01"));
            menuSoundEffects.Add("ui_inventory_tab_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_02"));
            menuSoundEffects.Add("ui_inventory_tab_03", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_03"));
            menuSoundEffects.Add("ui_inventory_tab_04", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_tab_04"));

            menuSoundEffects.Add("ui_quest_open", menuContent.Load<SoundEffect>(@"Sound\UI\ui_quest_open"));
        }

        static public void LoadDarylLockerSounds()
        {
            menuSoundEffects.Add("ui_locker_player_open", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_player_open"));
            menuSoundEffects.Add("ui_locker_change_menu", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_change_menu"));
            menuSoundEffects.Add("ui_locker_equip_skill", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_equip_skill"));
            menuSoundEffects.Add("ui_locker_unequip_skill", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_unequip_skill"));
            menuSoundEffects.Add("ui_locker_cant_equip_skill", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_cant_equip_skill"));
            menuSoundEffects.Add("ui_locker_click_skill_01", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_click_skill_01"));
            menuSoundEffects.Add("ui_locker_click_skill_02", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_click_skill_02"));
            menuSoundEffects.Add("ui_inventory_list_01", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_01"));
            menuSoundEffects.Add("ui_inventory_list_02", menuContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_list_02"));
            menuSoundEffects.Add("ui_locker_buy_skill", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_buy_skill"));
            menuSoundEffects.Add("ui_locker_playermenu_appear", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_playermenu_appear"));
            menuSoundEffects.Add("ui_locker_shopmenu_appear", menuContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_shopmenu_appear"));
        }

        static public void CreateBinaryVolumeFile()
        {
            String file = "vol.txt";

            if (File.Exists(file))
            {
                StreamReader sr = new StreamReader(file);
                List<String> lines = new List<string>();

                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    lines.Add(line);
                }
                sr.Close();


                BinaryWriter write = new BinaryWriter(File.Open("volBinary.txt", FileMode.Create));

                for (int i = 0; i < lines.Count; i++)
                {
                    write.Write(lines[i]);
                }
                write.Close();
            }
        }

        static public void LoadSoundVolumeFile()
        {
            CreateBinaryVolumeFile();

            //-----Read the binary file and turn it into a regular text file----//
            String file2 = "volReadBinary.txt";
            File.Delete(file2);
            StreamWriter sw2 = File.AppendText(file2);

            BinaryReader reader = new BinaryReader(File.Open("volBinary.txt", FileMode.Open));

            //While the reader's stream position is not the end of the file, add a new line to the text file (Basically a new platform)
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                sw2.WriteLine(reader.ReadString());
            }

            sw2.Close();
            reader.Close();
            //-----------------------------------------------------------------//

            String file = "volReadBinary.txt";
            StreamReader sr = new StreamReader(file);

            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();

                if (line != "" && !line[0].Equals('-'))
                {
                    String[] lines = line.Split(',');
                    soundEffectVolumes.Add(lines[0], float.Parse(lines[1]));
                }
            }
            sr.Close();

            File.Delete("volReadBinary.txt");

        }

        static public void LoadPermanentContent()
        {
            poofSounds = new List<SoundEffectInstance>();
            ambience = new Dictionary<string, SoundEffectInstance>();
            music = new Dictionary<string, SoundEffectInstance>();
            menuSoundEffects = new Dictionary<string, SoundEffect>();
            permanentSoundEffects = new Dictionary<string, SoundEffect>();
            enemySoundEffects = new Dictionary<string, SoundEffect>();
            mapZoneSoundEffects = new Dictionary<string, SoundEffect>();
            currentlyPlayingSounds = new List<SoundEffectInstance>();
            currentlyPlayingSounds_NAMES = new List<string>();
            soundEffectVolumes = new Dictionary<string, float>();

            LoadSoundVolumeFile();

            // Load sound effects
            permanentSoundEffects.Add("movement_footstep_walk_room_01", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_room_01"));
            permanentSoundEffects.Add("movement_footstep_walk_room_02", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_room_02"));
            permanentSoundEffects.Add("movement_footstep_walk_room_03", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_room_03"));
            permanentSoundEffects.Add("movement_land_room_01", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_land_room_01"));
            permanentSoundEffects.Add("movement_land_room_02", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_land_room_02"));
            permanentSoundEffects.Add("movement_jump_room_01", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_room_01"));
            permanentSoundEffects.Add("movement_jump_room_02", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_room_02"));
            permanentSoundEffects.Add("movement_duck", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_duck"));
            permanentSoundEffects.Add("movement_unduck", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_unduck"));

            permanentSoundEffects.Add("movement_footstep_walk_outside_01", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_outside_01"));
            permanentSoundEffects.Add("movement_footstep_walk_outside_02", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_outside_02"));
            permanentSoundEffects.Add("movement_footstep_walk_outside_03", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_outside_03"));
            permanentSoundEffects.Add("movement_footstep_walk_outside_04", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_outside_04"));
            permanentSoundEffects.Add("movement_footstep_walk_outside_05", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_outside_05"));
            permanentSoundEffects.Add("movement_footstep_walk_outside_06", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_outside_06"));
            permanentSoundEffects.Add("movement_land_outside_01", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_land_outside_01"));
            permanentSoundEffects.Add("movement_land_outside_02", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_land_outside_02"));
            permanentSoundEffects.Add("movement_jump_outside_01", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_outside_01"));
            permanentSoundEffects.Add("movement_jump_outside_02", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_outside_02"));

            permanentSoundEffects.Add("ui_general_text_advance", permanentContent.Load<SoundEffect>("Sound\\ui_general_text_advance"));
            permanentSoundEffects.Add("ui_general_text_scroll", permanentContent.Load<SoundEffect>("Sound\\UI\\ui_general_text_scroll"));
            permanentSoundEffects.Add("popup_text_message", permanentContent.Load<SoundEffect>("Sound\\Pop Ups\\popup_text_message"));
            permanentSoundEffects.Add("popup_enter", permanentContent.Load<SoundEffect>("Sound\\Pop Ups\\popup_enter"));
            permanentSoundEffects.Add("popup_exit", permanentContent.Load<SoundEffect>("Sound\\Pop Ups\\popup_exit"));
            permanentSoundEffects.Add("popup_skill_level_up", permanentContent.Load<SoundEffect>("Sound\\Pop Ups\\popup_skill_level_up"));
            permanentSoundEffects.Add("popup_quest_completed", permanentContent.Load<SoundEffect>("Sound\\Pop Ups\\popup_quest_completed"));
            permanentSoundEffects.Add("popup_level_up", permanentContent.Load<SoundEffect>("Sound\\Pop Ups\\popup_level_up"));
            permanentSoundEffects.Add("popup_social_rank_up", permanentContent.Load<SoundEffect>("Sound\\Pop Ups\\popup_social_rank_up"));
            permanentSoundEffects.Add("object_pickup_coin_01", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_coin_01"));
            permanentSoundEffects.Add("object_pickup_coin_02", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_coin_02"));
            permanentSoundEffects.Add("object_pickup_coin_03", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_coin_02"));
            permanentSoundEffects.Add("object_pickup_coin_04", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_coin_04"));
            permanentSoundEffects.Add("object_pickup_coin_05", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_coin_05"));
            permanentSoundEffects.Add("object_pickup_health", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_health"));
            permanentSoundEffects.Add("object_pickup_misc", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_misc"));
            permanentSoundEffects.Add("object_pickup_combo", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_combo"));
            permanentSoundEffects.Add("object_pickup_loot", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_loot"));
            permanentSoundEffects.Add("object_pickup_textbook", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_pickup_textbook"));
            permanentSoundEffects.Add("object_locker_crash", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_locker_crash"));
            permanentSoundEffects.Add("object_locker_hit", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_locker_hit"));
            permanentSoundEffects.Add("movement_door_open", permanentContent.Load<SoundEffect>("Sound\\Movement\\movement_door_open"));
            permanentSoundEffects.Add("movement_portal_enter", permanentContent.Load<SoundEffect>("Sound\\Movement\\Doors\\movement_portal_enter"));
            permanentSoundEffects.Add("movement_stairs", permanentContent.Load<SoundEffect>("Sound\\Movement\\Doors\\movement_stairs"));
            permanentSoundEffects.Add("popup_save_game", permanentContent.Load<SoundEffect>(@"Maps\School\Bathroom\popup_save_game"));
            permanentSoundEffects.Add("popup_load_game", permanentContent.Load<SoundEffect>(@"Maps\School\Bathroom\popup_load_game"));
            permanentSoundEffects.Add("object_platform_start", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_platform_start"));
            permanentSoundEffects.Add("object_platform_loop", permanentContent.Load<SoundEffect>("Sound\\Objects\\object_platform_loop"));

            permanentSoundEffects.Add("ui_locker_shut", permanentContent.Load<SoundEffect>(@"Sound\UI\Lockers\ui_locker_shut"));
            permanentSoundEffects.Add("object_button_large", permanentContent.Load<SoundEffect>(@"Sound\Objects\object_button_large"));
            TreasureChest.object_chest_unlock = permanentContent.Load<SoundEffect>("Sound\\Objects\\object_chest_unlock").CreateInstance();
            TreasureChest.object_chest_open = permanentContent.Load<SoundEffect>("Sound\\Objects\\object_chest_open").CreateInstance();

            permanentSoundEffects.Add("object_barrel_metal_hit_01", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_metal_hit_01"));
            permanentSoundEffects.Add("object_barrel_metal_hit_02", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_metal_hit_02"));
            permanentSoundEffects.Add("object_barrel_metal_hit_03", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_metal_hit_03"));
            permanentSoundEffects.Add("object_barrel_metal_destroy_01", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_metal_destroy_01"));
            permanentSoundEffects.Add("object_barrel_metal_destroy_02", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_metal_destroy_02"));
            permanentSoundEffects.Add("object_barrel_metal_destroy_03", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_metal_destroy_03"));
            permanentSoundEffects.Add("object_barrel_wood_destroy_01", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_wood_destroy_01"));
            permanentSoundEffects.Add("object_barrel_wood_destroy_02", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_wood_destroy_02"));
            permanentSoundEffects.Add("object_barrel_wood_destroy_03", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_wood_destroy_03"));
            permanentSoundEffects.Add("object_barrel_wood_hit_01", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_wood_hit_01"));
            permanentSoundEffects.Add("object_barrel_wood_hit_02", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_wood_hit_02"));
            permanentSoundEffects.Add("object_barrel_wood_hit_03", permanentContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_wood_hit_03"));

            permanentSoundEffects.Add("enemy_generic_spawn_01", permanentContent.Load<SoundEffect>("Sound\\Poofs\\enemy_generic_spawn_01"));
            permanentSoundEffects.Add("enemy_generic_spawn_02", permanentContent.Load<SoundEffect>("Sound\\Poofs\\enemy_generic_spawn_02"));
            permanentSoundEffects.Add("enemy_generic_spawn_03", permanentContent.Load<SoundEffect>("Sound\\Poofs\\enemy_generic_spawn_03"));

            permanentSoundEffects.Add("ui_inventory_close", permanentContent.Load<SoundEffect>(@"Sound\UI\ui_inventory_close"));
            permanentSoundEffects.Add("ui_general_enter", permanentContent.Load<SoundEffect>(@"Sound\UI\ui_general_enter"));
            permanentSoundEffects.Add("ui_general_back", permanentContent.Load<SoundEffect>(@"Sound\UI\ui_general_back"));
            permanentSoundEffects.Add("ui_general_tab", permanentContent.Load<SoundEffect>(@"Sound\UI\ui_general_tab"));
        }

        #region Unload content
        static public void UnloadMenuSounds()
        {
            menuSoundEffects.Clear();
            menuContent.Unload();
        }

        static public void UnloadMapZoneSounds()
        {
            mapZoneSoundEffects.Clear();
            mapZoneContent.Unload();
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
        #endregion

        #region Map Zone Load Sounds
        static public void LoadScienceZoneSounds()
        {
            if (mapZoneSoundEffects.Count == 0)
            {
                mapZoneSoundEffects.Add("movement_footstep_walk_science_01", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_science_01"));
                mapZoneSoundEffects.Add("movement_footstep_walk_science_02", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_science_02"));
                mapZoneSoundEffects.Add("movement_footstep_walk_science_03", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_science_03"));
                mapZoneSoundEffects.Add("movement_footstep_walk_science_04", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_science_04"));
                mapZoneSoundEffects.Add("movement_footstep_walk_science_05", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_science_05"));
                mapZoneSoundEffects.Add("movement_footstep_walk_science_06", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_science_06"));
                mapZoneSoundEffects.Add("movement_land_science_01", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_land_science_01"));
                mapZoneSoundEffects.Add("movement_land_science_02", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_land_science_02"));
                mapZoneSoundEffects.Add("movement_jump_science_01", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_science_01"));
                mapZoneSoundEffects.Add("movement_jump_science_02", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_science_02"));
                mapZoneSoundEffects.Add("movement_footstep_walk_hallway_01", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_hallway_01"));
                mapZoneSoundEffects.Add("movement_footstep_walk_hallway_02", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_hallway_02"));
                mapZoneSoundEffects.Add("movement_footstep_walk_hallway_03", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_hallway_03"));
                mapZoneSoundEffects.Add("movement_footstep_walk_hallway_04", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_hallway_04"));
                mapZoneSoundEffects.Add("movement_footstep_walk_hallway_05", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_hallway_05"));
                mapZoneSoundEffects.Add("movement_footstep_walk_hallway_06", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_hallway_06"));
                mapZoneSoundEffects.Add("movement_land_hallway_01", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_land_hallway_01"));
                mapZoneSoundEffects.Add("movement_land_hallway_02", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_land_hallway_02"));
                mapZoneSoundEffects.Add("movement_jump_hallway_01", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_hallway_01"));
                mapZoneSoundEffects.Add("movement_jump_hallway_02", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_hallway_02"));

                mapZoneSoundEffects.Add("object_barrel_science_destroy_01", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_science_destroy_01"));
                mapZoneSoundEffects.Add("object_barrel_science_destroy_02", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_science_destroy_02"));
                mapZoneSoundEffects.Add("object_barrel_science_destroy_03", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_science_destroy_03"));
                mapZoneSoundEffects.Add("object_barrel_science_hit_01", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_science_hit_01"));
                mapZoneSoundEffects.Add("object_barrel_science_hit_02", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_science_hit_02"));
                mapZoneSoundEffects.Add("object_barrel_science_hit_03", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\Barrels\\object_barrel_science_hit_03"));

                mapZoneSoundEffects.Add("object_portal_loop", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\Doors\\object_portal_loop"));
                mapZoneSoundEffects.Add("object_portal_pulse", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\Doors\\object_portal_pulse"));

            }
        }

        static public void LoadVentZoneSounds()
        {
            if (mapZoneSoundEffects.Count == 0)
            {
                mapZoneSoundEffects.Add("movement_footstep_walk_vent_01", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_vent_01"));
                mapZoneSoundEffects.Add("movement_footstep_walk_vent_02", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_vent_02"));
                mapZoneSoundEffects.Add("movement_footstep_walk_vent_03", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_vent_03"));
                mapZoneSoundEffects.Add("movement_footstep_walk_vent_04", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_vent_04"));
                mapZoneSoundEffects.Add("movement_footstep_walk_vent_05", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_vent_05"));
                mapZoneSoundEffects.Add("movement_footstep_walk_vent_06", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_footstep_walk_vent_06"));
                mapZoneSoundEffects.Add("movement_land_vent_01", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_land_vent_01"));
                mapZoneSoundEffects.Add("movement_land_vent_02", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_land_vent_02"));
                mapZoneSoundEffects.Add("movement_jump_vent_01", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_vent_01"));
                mapZoneSoundEffects.Add("movement_jump_vent_02", mapZoneContent.Load<SoundEffect>("Sound\\Movement\\movement_jump_vent_02"));

                mapZoneSoundEffects.Add("object_steam_vent_start", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\Traps\\object_steam_vent_start"));
                mapZoneSoundEffects.Add("object_steam_vent_stop", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\Traps\\object_steam_vent_stop"));
                mapZoneSoundEffects.Add("object_steam_vent_loop", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\Traps\\object_steam_vent_loop"));
                mapZoneSoundEffects.Add("object_coal_break_01", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\object_coal_break_01"));
                mapZoneSoundEffects.Add("object_coal_break_02", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\object_coal_break_02"));
                mapZoneSoundEffects.Add("object_coal_damage_01", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\object_coal_damage_01"));
                mapZoneSoundEffects.Add("object_coal_damage_02", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\object_coal_damage_02"));
                mapZoneSoundEffects.Add("object_coal_damage_03", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\object_coal_damage_03"));
            }
        }

        static public void LoadFortZoneSounds()
        {
            if (mapZoneSoundEffects.Count == 0)
            {
                mapZoneSoundEffects.Add("object_goblin_hut_damage", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\object_goblin_hut_damage"));
                mapZoneSoundEffects.Add("object_goblin_hut_destroy", mapZoneContent.Load<SoundEffect>("Sound\\Objects\\object_goblin_hut_destroy"));
                mapZoneSoundEffects.Add("popup_fort_alarm", mapZoneContent.Load<SoundEffect>("Sound\\Pop Ups\\popup_fort_alarm"));
                GoblinHut.object_goblin_hut_fire_loop = mapZoneContent.Load<SoundEffect>("Sound\\Objects\\object_goblin_hut_fire_loop");
            }
        }
        #endregion

        static public void ChangeBackgroundMusicWithFade(MusicNames newMusic, float fadeTime)
        {
            Game1.g.CurrentChapter.CurrentMap.PlayBackgroundMusic();
            if (music[newMusic.ToString()].State != SoundState.Playing)
            {
                IncrementBackgroundVolume(-(1 * setBackgroundVolume) / fadeTime);

                if (currentBackgroundVolume <= 0)
                {
                    Game1.g.CurrentChapter.CurrentMap.currentBackgroundMusic = newMusic;
                    StopBackgroundMusic();
                }
            }
            else if (currentBackgroundVolume < setBackgroundVolume)
            {
                IncrementBackgroundVolume((1 * setBackgroundVolume) / fadeTime);

                if (currentBackgroundVolume > setBackgroundVolume)
                    currentBackgroundVolume = setBackgroundVolume;
            }
        }

        static public void PlayRandomRegularPoof(int originx, int originy)
        {
            if (poofSounds.Count < 2)
            {

                int num = Game1.randomNumberGen.Next(1, 4);
                SoundEffectInstance newPoof = permanentSoundEffects["enemy_generic_spawn_0" + num].CreateInstance();

                Sound.PlaySoundInstance(newPoof, "enemy_generic_spawn_0" + num, false, originx, originy, 600, 500, 2000);
                poofSounds.Add(newPoof);
            }
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

        static public void PauseTrack(String trackName)
        {
            if (trackName != "none")
            {
                music[trackName].Pause();
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
            currentBackgroundVolume += incrementVolume;

            if (currentBackgroundVolume < 0)
                currentBackgroundVolume = 0;

            if (currentBackgroundVolume > 1)
                currentBackgroundVolume = 1;
        }

        static public void IncrementAmbienceVolume(float incrementVolume)
        {
            currentAmbienceVolume += incrementVolume;

            if (currentAmbienceVolume < 0)
                currentAmbienceVolume = 0;

            if (currentAmbienceVolume > 1)
                currentAmbienceVolume = 1;

            for (int i = 0; i < ambience.Count; i++)
            {
                ambience.ElementAt(i).Value.Volume = currentAmbienceVolume;
            }
        }

        static public void SetBackgroundVolume(float volume)
        {
            currentBackgroundVolume = volume;

            if (currentBackgroundVolume < 0)
                currentBackgroundVolume = 0;

            if (currentBackgroundVolume > 1)
                currentBackgroundVolume = 1;
        }

        static public void PlayAmbience(String ambienceName)
        {
            if (!muted)
            {
                //Creating a instance allows you to modify the sound playing, eg Change volume.
                if (ambience[ambienceName].State == SoundState.Stopped)
                {
                    ambience[ambienceName].Volume = currentAmbienceVolume * GetSoundVolumeFromFile(ambienceName);
                    ambience[ambienceName].Play();
                }
                else
                {
                    ambience[ambienceName].Volume = currentAmbienceVolume * GetSoundVolumeFromFile(ambienceName);
                    ambience[ambienceName].Resume();
                }
            }
        }

        static public void PlayAmbience(String ambienceName, float vol, float pan)
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
                if (ambience[ambienceName].State == SoundState.Stopped)
                {
                    ambience[ambienceName].Pan = pan;
                    ambience[ambienceName].Volume = currentAmbienceVolume * GetSoundVolumeFromFile(ambienceName) * vol;
                    ambience[ambienceName].Play();
                }
                else
                {
                    ambience[ambienceName].Pan = pan;
                    ambience[ambienceName].Volume = currentAmbienceVolume * GetSoundVolumeFromFile(ambienceName) * vol;
                    ambience[ambienceName].Resume();
                }
            }
        }

        static public void PlayBackGroundMusic(String musicName)
        {
            if (!muted && musicName != "none")
            {
                //Creating a instance allows you to modify the sound playing, eg Change volume.
                if (music[musicName].State == SoundState.Stopped)
                {
                    music[musicName].Volume = currentBackgroundVolume * GetSoundVolumeFromFile(musicName);
                    music[musicName].Play();
                }
                else
                {
                    music[musicName].Volume = currentBackgroundVolume * GetSoundVolumeFromFile(musicName);
                    music[musicName].Resume();
                }
            }
            else if (musicName == "none")
                PauseBackgroundMusic();
        }

        static public void PlayBackGroundMusic(String musicName, float vol, float pan)
        {
            if (!muted && musicName != "none")
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
                    music[musicName].Volume = vol * GetSoundVolumeFromFile(musicName) * currentBackgroundVolume;
                    music[musicName].Play();
                }
                else
                {
                    music[musicName].Pan = pan;
                    music[musicName].Volume = vol * GetSoundVolumeFromFile(musicName) * currentBackgroundVolume;
                    music[musicName].Resume();
                }
            }
            else if (musicName == "none")
                PauseBackgroundMusic();
        }

        static public void CleanUpCurrentSoundsList()
        {
            if (currentlyPlayingSounds.Count > 30)
            {

                List<String> distinctSoundNames = new List<string>();
                for (int i = 0; i < currentlyPlayingSounds_NAMES.Count; i++)
                {
                    if (!distinctSoundNames.Contains(currentlyPlayingSounds_NAMES[i]))
                    {
                        distinctSoundNames.Add(currentlyPlayingSounds_NAMES[i]);
                    }
                    else
                    {
                        currentlyPlayingSounds[i].Stop();
                        currentlyPlayingSounds_NAMES.RemoveAt(i);
                        currentlyPlayingSounds.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
        }

        static public void UpdatePanAndVolume(SoundEffectInstance sound, String name, int xOrigin, int yOrigin, int panStrength = 600, int volumeStrength = 500, int maxHearingDistance = 2000)
        {
            float max = GetSoundVolumeFromFile(name) * currentSoundVolume;

            if (panStrength != 0)
                sound.Pan = GetSoundPan(xOrigin, panStrength);
            if (volumeStrength != 0)
                sound.Volume = GetSoundVolume(new Vector2(xOrigin, yOrigin), maxHearingDistance, volumeStrength, max);
        }

        /// <summary>
        /// Creates and plays a sound effect instance
        /// </summary>
        /// <param name="soundInstanceName"></param>
        static public void PlaySoundInstance(SoundNames soundInstanceName, Boolean isLooped = false, int xOrigin = int.MaxValue, int yOrigin = int.MaxValue, int panStrength = 0, int volumeStrength = 0, int maxHearingDistance = 0, Boolean allowStackingSounds = true, int maxStacks = 1)
        {
            if (allowStackingSounds == true || (allowStackingSounds == false && NumberOfSoundInstancesPlaying(soundInstanceName.ToString()) < maxStacks))
            {
                if (permanentSoundEffects.ContainsKey(soundInstanceName.ToString()))
                {
                    SoundEffectInstance inst = permanentSoundEffects[soundInstanceName.ToString()].CreateInstance();
                    inst.IsLooped = isLooped;
                    inst.Volume = GetSoundVolumeFromFile(soundInstanceName.ToString()) * currentSoundVolume;

                    if (xOrigin != int.MaxValue && yOrigin != int.MaxValue)
                    {
                        if (panStrength != 0)
                            inst.Pan = GetSoundPan(xOrigin, panStrength);
                        if (volumeStrength != 0)
                            inst.Volume *= GetSoundVolume(new Vector2(xOrigin, yOrigin), maxHearingDistance, volumeStrength, inst.Volume);
                    }

                    if (!currentlyPlayingSounds.Contains(inst))
                    {
                        currentlyPlayingSounds.Add(inst);
                        currentlyPlayingSounds_NAMES.Add(soundInstanceName.ToString());
                        CleanUpCurrentSoundsList();
                    }

                    inst.Play();
                }
                else if (menuSoundEffects.ContainsKey(soundInstanceName.ToString()))
                {
                    SoundEffectInstance inst = menuSoundEffects[soundInstanceName.ToString()].CreateInstance();
                    inst.IsLooped = isLooped;
                    inst.Volume = GetSoundVolumeFromFile(soundInstanceName.ToString()) * currentSoundVolume;

                    if (xOrigin != int.MaxValue && yOrigin != int.MaxValue)
                    {
                        if (panStrength != 0)
                            inst.Pan = GetSoundPan(xOrigin, panStrength);
                        if (volumeStrength != 0)
                            inst.Volume *= GetSoundVolume(new Vector2(xOrigin, yOrigin), maxHearingDistance, volumeStrength, inst.Volume);
                    }

                    if (!currentlyPlayingSounds.Contains(inst))
                    {
                        currentlyPlayingSounds.Add(inst);
                        currentlyPlayingSounds_NAMES.Add(soundInstanceName.ToString());
                        CleanUpCurrentSoundsList();

                    }

                    inst.Play();
                }
                else if (mapZoneSoundEffects.ContainsKey(soundInstanceName.ToString()))
                {
                    SoundEffectInstance inst = mapZoneSoundEffects[soundInstanceName.ToString()].CreateInstance();
                    inst.IsLooped = isLooped;
                    inst.Volume = GetSoundVolumeFromFile(soundInstanceName.ToString()) * currentSoundVolume;

                    if (xOrigin != int.MaxValue && yOrigin != int.MaxValue)
                    {
                        if (panStrength != 0)
                            inst.Pan = GetSoundPan(xOrigin, panStrength);
                        if (volumeStrength != 0)
                            inst.Volume *= GetSoundVolume(new Vector2(xOrigin, yOrigin), maxHearingDistance, volumeStrength, inst.Volume);
                    }

                    if (!currentlyPlayingSounds.Contains(inst))
                    {
                        currentlyPlayingSounds.Add(inst);
                        currentlyPlayingSounds_NAMES.Add(soundInstanceName.ToString());
                        CleanUpCurrentSoundsList();

                    }

                    inst.Play();
                }
            }
        }

        static public void PlaySoundInstance(SoundEffect sound, String soundFileName, Boolean isLooped = false, int xOrigin = int.MaxValue, int yOrigin = int.MaxValue, int panStrength = 0, int volumeStrength = 0, int maxHearingDistance = 0, Boolean allowStackingSounds = true, int maxStacks = 1)
        {
            if (allowStackingSounds == true || (allowStackingSounds == false && NumberOfSoundInstancesPlaying(soundFileName) < maxStacks))
            {
                SoundEffectInstance inst = sound.CreateInstance();
                inst.IsLooped = isLooped;
                inst.Volume = GetSoundVolumeFromFile(soundFileName) * currentSoundVolume;

                if (xOrigin != int.MaxValue && yOrigin != int.MaxValue)
                {
                    if (panStrength != 0)
                        inst.Pan = GetSoundPan(xOrigin, panStrength);
                    if (volumeStrength != 0)
                        inst.Volume *= GetSoundVolume(new Vector2(xOrigin, yOrigin), maxHearingDistance, volumeStrength, inst.Volume);
                }

                if (!currentlyPlayingSounds.Contains(inst))
                {
                    currentlyPlayingSounds.Add(inst);
                    currentlyPlayingSounds_NAMES.Add(soundFileName);
                    CleanUpCurrentSoundsList();

                }
                inst.Play();
            }
        }

        static public void PlaySoundInstance(SoundEffectInstance soundInstance, String soundFileName, Boolean isLooped = false, int xOrigin = int.MaxValue, int yOrigin = int.MaxValue, int panStrength = 0, int volumeStrength = 0, int maxHearingDistance = 0, Boolean allowStackingSounds = true, int maxStacks = 1)
        {
            if (allowStackingSounds == true || (allowStackingSounds == false && NumberOfSoundInstancesPlaying(soundFileName) < maxStacks))
            {
                soundInstance.IsLooped = isLooped;
                soundInstance.Volume = GetSoundVolumeFromFile(soundFileName) * currentSoundVolume;

                if (xOrigin != int.MaxValue && yOrigin != int.MaxValue)
                {
                    if (panStrength != 0)
                        soundInstance.Pan = GetSoundPan(xOrigin, panStrength);
                    if (volumeStrength != 0)
                        soundInstance.Volume *= GetSoundVolume(new Vector2(xOrigin, yOrigin), maxHearingDistance, volumeStrength, soundInstance.Volume);
                }

                if (!currentlyPlayingSounds.Contains(soundInstance))
                {
                    currentlyPlayingSounds.Add(soundInstance);
                    currentlyPlayingSounds_NAMES.Add(soundFileName);
                    CleanUpCurrentSoundsList();

                }
                soundInstance.Play();
            }
        }

        static public float GetSoundVolumeFromFile(String fileName)
        {
      //     soundEffectVolumes.Clear();
     //      LoadSoundVolumeFile();
            float vol = soundEffectVolumes[fileName];

            if (vol > 1)
                vol = 1;
            if (vol < 0)
                vol = 0;

            return vol;
        }

        static public void PauseAllSoundEffects()
        {
            for (int i = 0; i < currentlyPlayingSounds.Count; i++)
            {
                currentlyPlayingSounds[i].Pause();
            }
        }

        static public void ResumeAllSoundEffects()
        {
            for (int i = 0; i < currentlyPlayingSounds.Count; i++)
            {
                if (currentlyPlayingSounds[i].State == SoundState.Paused)
                    currentlyPlayingSounds[i].Resume();
            }
        }

        static public void StopAllSoundEffects()
        {
            for (int i = 0; i < currentlyPlayingSounds.Count; i++)
            {
                currentlyPlayingSounds[i].Stop();
            }

            currentlyPlayingSounds.Clear();
            currentlyPlayingSounds_NAMES.Clear();
        }

        static public void UpdateCurrentPlayingSounds()
        {
            for (int i = 0; i < currentlyPlayingSounds.Count; i++)
            {
                if (currentlyPlayingSounds[i].State == SoundState.Stopped)
                {
                    currentlyPlayingSounds_NAMES.RemoveAt(i);
                    currentlyPlayingSounds.RemoveAt(i);
                    i--;
                }
            }
        }

        static public int NumberOfSoundInstancesPlaying(String soundName)
        {
            int num = 0;

            foreach(String s in currentlyPlayingSounds_NAMES)
            {
                if (soundName.Equals(s))
                    num++;
            }

            return num;
        }

        static public void PlayJumpSound(Platform.PlatformType type)
        {

            switch (type)
            {
                case Platform.PlatformType.rock:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_room_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_room_02);
                    break;
                case Platform.PlatformType.grass:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_outside_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_outside_02);
                    break;
                case Platform.PlatformType.science:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_science_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_science_02);
                    break;
                case Platform.PlatformType.echo:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_hallway_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_hallway_02);
                    break;
                case Platform.PlatformType.vents:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_vent_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_jump_vent_02);
                    break;
            }
        }

        static public void PlayLandingSound(Platform.PlatformType type)
        {

            switch (type)
            {
                case Platform.PlatformType.rock:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_room_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_room_02);
                    break;
                case Platform.PlatformType.grass:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_outside_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_outside_02);
                    break;
                case Platform.PlatformType.science:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_science_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_science_02);
                    break;
                case Platform.PlatformType.echo:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_hallway_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_hallway_02);
                    break;
                case Platform.PlatformType.vents:
                    if (Game1.randomNumberGen.Next(2) == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_vent_01);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_land_vent_02);
                    break;
            }
        }

        static public void PlaySteppingSound(Platform.PlatformType type)
        {
            int ran;

            switch (type)
            {
                case Platform.PlatformType.rock:
                    ran = Game1.randomNumberGen.Next(3);
                    if (ran == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_room_01);
                    else if (ran == 1)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_room_02);
                    else if (ran == 2)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_room_03);
                    break;
                case Platform.PlatformType.grass:
                    ran = Game1.randomNumberGen.Next(6);
                    if (ran == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_01);
                    else if (ran == 1)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_02);
                    else if (ran == 2)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_03);
                    else if (ran == 3)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_04);
                    else if (ran == 4)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_05);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_06);
                    break;
                case Platform.PlatformType.science:
                    ran = Game1.randomNumberGen.Next(6);
                    if (ran == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_01);
                    else if (ran == 1)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_02);
                    else if (ran == 2)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_03);
                    else if (ran == 3)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_04);
                    else if (ran == 4)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_05);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_06);
                    break;
                case Platform.PlatformType.echo:
                    ran = Game1.randomNumberGen.Next(6);
                    if (ran == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_hallway_01);
                    else if (ran == 1)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_hallway_02);
                    else if (ran == 2)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_hallway_03);
                    else if (ran == 3)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_hallway_04);
                    else if (ran == 4)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_hallway_05);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_hallway_06);
                    break;
                case Platform.PlatformType.vents:
                    ran = Game1.randomNumberGen.Next(6);
                    if (ran == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_vent_01);
                    else if (ran == 1)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_vent_02);
                    else if (ran == 2)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_vent_03);
                    else if (ran == 3)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_vent_04);
                    else if (ran == 4)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_vent_05);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_vent_06);
                    break;
            }
        }

        static public void PlayNPCSteppingSound(Platform.PlatformType type, int originx, int originy)
        {
            int ran;

            switch (type)
            {
                case Platform.PlatformType.rock:
                    ran = Game1.randomNumberGen.Next(3);
                    if (ran == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_room_01, false, originx, originy, 600, 500, 1500);
                    else if (ran == 1)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_room_02, false, originx, originy, 600, 500, 1500);
                    else if (ran == 2)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_room_03, false, originx, originy, 600, 500, 1500);
                    break;
                case Platform.PlatformType.grass:
                    ran = Game1.randomNumberGen.Next(6);
                    if (ran == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_01, false, originx, originy, 600, 500, 1500);
                    else if (ran == 1)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_02, false, originx, originy, 600, 500, 1500);
                    else if (ran == 2)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_03, false, originx, originy, 600, 500, 1500);
                    else if (ran == 3)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_04, false, originx, originy, 600, 500, 1500);
                    else if (ran == 4)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_05, false, originx, originy, 600, 500, 1500);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_outside_06, false, originx, originy, 600, 500, 1500);
                    break;
                case Platform.PlatformType.science:
                    ran = Game1.randomNumberGen.Next(6);
                    if (ran == 0)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_01, false, originx, originy, 600, 500, 1500);
                    else if (ran == 1)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_02, false, originx, originy, 600, 500, 1500);
                    else if (ran == 2)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_03, false, originx, originy, 600, 500, 1500);
                    else if (ran == 3)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_04, false, originx, originy, 600, 500, 1500);
                    else if (ran == 4)
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_05, false, originx, originy, 600, 500, 1500);
                    else
                        Sound.PlaySoundInstance(Sound.SoundNames.movement_footstep_walk_science_06, false, originx, originy, 600, 500, 1500);
                    break;
            }
        }

    }
}
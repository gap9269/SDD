using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ISurvived
{
    public static class EnemyContentLoader
    {
        #region Prologue
        static public void FezGoblin(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Fez", content.Load<Texture2D>(@"EnemySprites\Prologue\FezGoblin"));
        }

        static public void ErlTheFlask(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Erl The Flask", content.Load<Texture2D>(@"EnemySprites\ErlSprite"));
            if (!Sound.enemySoundEffects.ContainsKey("enemy_glass_hit_generic_01"))
            {
                Sound.enemySoundEffects.Add("enemy_glass_hit_generic_01", content.Load<SoundEffect>(@"Sound\Enemy\Erl\enemy_glass_hit_generic_01"));
                Sound.enemySoundEffects.Add("enemy_glass_hit_generic_02", content.Load<SoundEffect>(@"Sound\Enemy\Erl\enemy_glass_hit_generic_02"));
            }
        }

        static public void BennyBeaker(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Benny Beaker", content.Load<Texture2D>(@"EnemySprites\BennySprite"));
            if (!Sound.enemySoundEffects.ContainsKey("enemy_glass_hit_generic_01"))
            {
                Sound.enemySoundEffects.Add("enemy_glass_hit_generic_01", content.Load<SoundEffect>(@"Sound\Enemy\Erl\enemy_glass_hit_generic_01"));
                Sound.enemySoundEffects.Add("enemy_glass_hit_generic_02", content.Load<SoundEffect>(@"Sound\Enemy\Erl\enemy_glass_hit_generic_02"));
            }
        }

        static public void GorillaTimBoss(ContentManager content)
        {
            GorillaTim.animationTextures = ContentLoader.LoadContent(content, "Bosses\\Tim");
            GorillaTim.animationTextures.Add("BossTitleBar", content.Load<Texture2D>(@"HUD\Boss Title"));
            GorillaTim.gorillaSounds = ContentLoader.LoadSoundContent(content, "Sound\\Enemy\\GorillaTim");

            GorillaTim.timFightTheme =

            GorillaTim.timFightTheme = Sound.backgroundMusicContent.Load<SoundEffect>(@"Sound\Cutscenes\Tim\TimFightLoop").CreateInstance();
            GorillaTim.timFightTheme.IsLooped = true;
            GorillaTim.timFightTheme.Volume = Sound.GetSoundVolumeFromFile("Tim Fight Loop");
            Sound.music.Add("Tim Fight Loop", GorillaTim.timFightTheme);
        }
        #endregion

        #region CH1
        static public void BatEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Vent Bat", content.Load<Texture2D>(@"EnemySprites\BatSprite"));
        }
        static public void SharedRatSounds(ContentManager content)
        {
            FlufflesTheRat.ratSounds = ContentLoader.LoadSoundContent(content, "Sound\\Enemy\\Rat");
        }
        static public void FlufflesRat(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Fluffles the Rat", content.Load<Texture2D>(@"EnemySprites\FlufflesSprite"));
        }

        static public void CaptainSaxEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Captain Sax", content.Load<Texture2D>(@"EnemySprites\CaptainSax"));
        }

        static public void SergeantCymbalEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Sergeant Cymbal", content.Load<Texture2D>(@"EnemySprites\SergeantCymbal"));
        }

        static public void MaracasHermanosEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Maracas Hermanos", content.Load<Texture2D>(@"EnemySprites\MaracasHermanos"));
        }

        static public void TubaGhostEnemy(ContentManager content)
        {
            TubaGhost.tubaSounds = ContentLoader.LoadSoundContent(content, "Sound\\Enemy\\TubaGhost");

            Game1.g.EnemySpriteSheets.Add("Tuba Ghost", content.Load<Texture2D>(@"EnemySprites\TubaGhostSheet"));
        }

        static public void XylophoneEnemy(ContentManager content)
        {
            Dictionary<String, Texture2D> temp = ContentLoader.LoadContent(content, "EnemySprites\\Xylophone");

            Game1.g.EnemySpriteSheets.Add("Lord Glockenspiel", content.Load<Texture2D>(@"EnemySprites\Xylophone\\static0"));
            for (int i = 0; i < temp.Count; i++)
            {
                Game1.g.EnemySpriteSheets.Add(temp.ElementAt(i).Key, temp.ElementAt(i).Value);
            }

        }

        static public void EatballEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Eatball", content.Load<Texture2D>(@"EnemySprites\EatballSprite"));
        }

        static public void SlayDoughEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Slay Dough", content.Load<Texture2D>(@"EnemySprites\SlayDoughSprite"));
        }

        static public void FlufflesBandit(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Fluffles the Bandit", content.Load<Texture2D>(@"EnemySprites\BanditSprite"));
        }

        static public void GoblinMortarEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Goblin Mortar", content.Load<Texture2D>(@"EnemySprites\MortarSprite"));
        }

        #endregion

        #region CH2
        static public void AnubisWarriorEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Anubis Warrior", content.Load<Texture2D>(@"EnemySprites\AnubisWarrior"));
        }

        static public void TreeEntEnemy(ContentManager content)
        {

            Dictionary<String, Texture2D> temp = ContentLoader.LoadContent(content, "EnemySprites\\Tree Ent");
            Game1.g.EnemySpriteSheets.Add("Tree Ent", content.Load<Texture2D>(@"EnemySprites\Bomblin\\stand0"));

            for (int i = 0; i < temp.Count; i++)
            {
                Game1.g.EnemySpriteSheets.Add(temp.ElementAt(i).Key, temp.ElementAt(i).Value);
            }

        }

        static public void SpookyPresentEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Spooky Present", content.Load<Texture2D>(@"EnemySprites\GiftSprite"));
        }

        static public void EerieElfEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Eerie Elf", content.Load<Texture2D>(@"EnemySprites\ElfSprite"));
        }

        static public void HauntedNutcrackerEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Haunted Nutcracker", content.Load<Texture2D>(@"EnemySprites\NutcrackerSprite"));
            Game1.g.EnemySpriteSheets.Add("Haunted Nutcracker Gas", content.Load<Texture2D>(@"EnemySprites\GasSprite"));
        }

        static public void LitGuardian(ContentManager content)
        {
            Dictionary<String, Texture2D> temp = ContentLoader.LoadContent(content, "Bosses\\LitGuardian");

            Game1.g.EnemySpriteSheets.Add("Literature Guardian", content.Load<Texture2D>(@"Bosses\LitGuardian\\frontStatic0"));
            for (int i = 0; i < temp.Count; i++)
            {
                Game1.g.EnemySpriteSheets.Add(temp.ElementAt(i).Key, temp.ElementAt(i).Value);
            }
        }

        static public void SexySaguaroEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Sexy Saguaro", content.Load<Texture2D>(@"EnemySprites\CactusSprite"));
        }

        static public void BurnieBuzzardEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Burnie Buzzard", content.Load<Texture2D>(@"EnemySprites\VultureSprite"));
        }

        static public void ScorpadilloEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Scorpadillo", content.Load<Texture2D>(@"EnemySprites\ScorpadilloSprite"));
        }

        static public void MummyEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Mummy", content.Load<Texture2D>(@"EnemySprites\MummySprite"));
        }

        static public void VileMummyEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Vile Mummy", content.Load<Texture2D>(@"EnemySprites\ExplodingMummySprite"));
        }

        static public void CorruptedCoffinBoss(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Corrupted Coffin", content.Load<Texture2D>(@"Bosses\CorruptedCoffin"));
        }

        static public void AnubisCommanderEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Commander Anubis", content.Load<Texture2D>(@"EnemySprites\CommanderAnubis"));
            Game1.g.EnemySpriteSheets.Add("Commander AnubisAttacking", content.Load<Texture2D>(@"EnemySprites\CommanderAnubisAttacks"));
        }

        static public void SharedAnubisSounds(ContentManager content)
        {
            AnubisWarrior.anubisSounds = ContentLoader.LoadSoundContent(content, "Sound\\Enemy\\Anubis");
        }

        static public void LocustEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Locust", content.Load<Texture2D>(@"EnemySprites\Locust"));
        }

        static public void SharedGoblinSounds(ContentManager content)
        {
            Goblin.goblinSounds = ContentLoader.LoadSoundContent(content, "Sound\\Enemy\\Goblin");
        }

        static public void GoblinEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Goblin", content.Load<Texture2D>(@"EnemySprites\GoblinSheet"));
        }

        static public void NurseGoblinEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Nurse Goblin", content.Load<Texture2D>(@"EnemySprites\NurseGoblinSheet"));
        }

        static public void SoldierGoblinEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Goblin Soldier", content.Load<Texture2D>(@"EnemySprites\SoldierGoblinSheet"));
        }

        static public void CommanderGoblinEnemy(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Commander Goblin", content.Load<Texture2D>(@"EnemySprites\CommanderGoblinSheet"));
        }

        static public void BomblinEnemy(ContentManager content)
        {
            Dictionary<String, Texture2D> temp = ContentLoader.LoadContent(content, "EnemySprites\\Bomblin");

            Game1.g.EnemySpriteSheets.Add("Bomblin", content.Load<Texture2D>(@"EnemySprites\Bomblin\\stand0"));
            for (int i = 0; i < temp.Count; i++)
            {
                Game1.g.EnemySpriteSheets.Add(temp.ElementAt(i).Key, temp.ElementAt(i).Value);
            }

            Bomblin.bomblinSounds = ContentLoader.LoadSoundContent(content, "Sound\\Enemy\\Bomblin");

        }

        static public void Crow(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Crow", content.Load<Texture2D>(@"EnemySprites\CrowSheet"));
        }

        static public void Troll(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Field Troll", content.Load<Texture2D>(@"EnemySprites\TrollSprite"));
            Game1.g.EnemySpriteSheets.Add("TrollFall", content.Load<Texture2D>(@"EnemySprites\TrollFallSprite"));
            Game1.g.EnemySpriteSheets.Add("TrollAttack", content.Load<Texture2D>(@"EnemySprites\TrollAttackSprite"));
            Game1.g.EnemySpriteSheets.Add("TrollClubGone", content.Load<Texture2D>(@"EnemySprites\TrollClubDisappearSprite"));
            FieldTroll.trollSounds = ContentLoader.LoadSoundContent(content, "Sound\\Enemy\\Troll");
        }

        static public void Scarecrow(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Scarecrow", content.Load<Texture2D>(@"EnemySprites\ScarecrowSheet"));
            ScarecrowEnemy.scarecrowSounds = ContentLoader.LoadSoundContent(content, "Sound\\Enemy\\Scarecrow");
        }

        #endregion
    }
}

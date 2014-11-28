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
        static public void ErlTheFlask(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Erl The Flask", content.Load<Texture2D>(@"EnemySprites\ErlSprite"));
            Sound.enemySoundEffects.Add("ErlHit1", content.Load<SoundEffect>(@"Sound\Enemy\Erl\enemy_glass_hit_generic_01"));
            Sound.enemySoundEffects.Add("ErlHit2", content.Load<SoundEffect>(@"Sound\Enemy\Erl\enemy_glass_hit_generic_02"));
        }

        static public void BennyBeaker(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Benny Beaker", content.Load<Texture2D>(@"EnemySprites\BennySprite"));
            Sound.enemySoundEffects.Add("BennyHit1", content.Load<SoundEffect>(@"Sound\Enemy\Erl\enemy_glass_hit_generic_01"));
            Sound.enemySoundEffects.Add("BennyHit2", content.Load<SoundEffect>(@"Sound\Enemy\Erl\enemy_glass_hit_generic_02"));
        }

        static public void FezGoblin(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Fez", content.Load<Texture2D>(@"EnemySprites\Prologue\FezGoblin"));
        }

        static public void Goblin(ContentManager content)
        {
            Game1.g.EnemySpriteSheets.Add("Goblin", content.Load<Texture2D>(@"EnemySprites\GoblinSheet"));
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

        static public void GorillaTimBoss(ContentManager content)
        {
            GorillaTim.animationTextures = ContentLoader.LoadContent(content, "Bosses\\Tim");
            GorillaTim.animationTextures.Add("BossTitleBar", content.Load<Texture2D>(@"HUD\Boss Title"));
        }
    }
}

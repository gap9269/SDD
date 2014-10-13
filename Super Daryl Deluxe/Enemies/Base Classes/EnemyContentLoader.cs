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
    }
}

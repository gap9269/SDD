using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISurvived
{
    public class CharacterMonsterBioDictionary
    {
        public struct CharacterInfo
        {
            public string name, yearbookQuote, funFact, superlative, age;
        }

        public struct EnemyInfo
        {
            public string name, itemDrop, hobby, experienceGiven, level;
        }

        public static Dictionary<String, CharacterInfo> nameAndInfo;
        public static Dictionary<String, EnemyInfo> enemyNameAndInfo;

        public CharacterMonsterBioDictionary()
        {
            nameAndInfo = new Dictionary<string, CharacterInfo>();
            enemyNameAndInfo = new Dictionary<string, EnemyInfo>();

            nameAndInfo.Add("Paul", new CharacterInfo() { name = "Paul Palte", yearbookQuote = "\"Advertise your business here for as low as $5, call (208)555-PAUL\"", funFact = "Is deathly afraid of showing genuine human emotion", superlative = "Most Likely To Disregard Morals And Ethics", age = "16" });

            nameAndInfo.Add("Alan", new CharacterInfo() { name = "Alan Orpter", yearbookQuote = "\"The kids that work on yearbook staff are tools.\"", funFact = "Thinks fun facts are for tools", superlative = "Most Likely To End Up Missing", age = "16" });

            nameAndInfo.Add("Mr. Robatto", new CharacterInfo() { name = "Mr. Robatto", yearbookQuote = "\"Try your best and be yourself.\"", funFact = "Catches fire in rain", superlative = "Longest Battery Life", age = "Middle-Age" });

            nameAndInfo.Add("Chelsea", new CharacterInfo() { name = "Chelsea Lardstal", yearbookQuote = "\"Everyone I hate is going to be there, but at least I'll look great.\"", funFact = "Makes $25000 a year off can deposits", superlative = "Most Likely To Shoot You A Look Of Distaste", age = "16" });

            nameAndInfo.Add("Mark", new CharacterInfo() { name = "Mark", yearbookQuote = "\"To save bearkind, we must eat the enemy from within. As sure as the noble salmon makes his annual passage upstream, so shall the bears reign again.\" - The Great Grizzly Spirit\"", funFact = "Loves honey", superlative = "Most Likely To Be Shot By Pelt Kid", age = "5" });

            nameAndInfo.Add("Bob the Construction Guy", new CharacterInfo() { name = "Bob the Construction Guy", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Trenchcoat Employee", new CharacterInfo() { name = "Trenchcoat Employee", yearbookQuote = "\"The only thing you got in this world is what you can sell.\" - Death of a Salesman", funFact = "Does not reflect light", superlative = "Most Likely to Avoid Airport Security", age = "13-20ish" });

            nameAndInfo.Add("Equipment Instructor", new CharacterInfo() { name = "Weapons Master", yearbookQuote = "\"For I am he who bears the mighty hammer and other objects of mightiness.  Thou shall drink happily from the Magic River of Life because of my weapons.\"", funFact = "Has a rash", superlative = "Most Likely To Hurt Self/Others", age = "16" });

            nameAndInfo.Add("Save Instructor", new CharacterInfo() { name = "Saving Instructor", yearbookQuote = "\"I am the hope of the Universe. I am the answer to all living things who cry out for peace. I am protector of the innocent.\" - Goku", funFact = "Lost interest in playing D&D a few months ago", superlative = "Least Likely To Make It Past Thirty", age = "14" });

            nameAndInfo.Add("Skill Instructor", new CharacterInfo() { name = "Skill Sorceress", yearbookQuote = "\"Time wasted on enhancing skills isn't even wasted. Those with skills will inherit the whole realm.\"", funFact = "Can astonishingly produce life from thin air, collects hats instead", superlative = "Most Likely To Write Fan-Fiction", age = "17" });

            nameAndInfo.Add("Tim", new CharacterInfo() { name = "Tim Laibly", yearbookQuote = "\"Stay out of my locker.\"", funFact = "Is not aware he can transform into massive, violent gorilla", superlative = "Best Bowl Cut", age = "15" });

            nameAndInfo.Add("Gardener", new CharacterInfo() { name = "The Gardener", yearbookQuote = "\"Old friendships make the most fertile garden soil.\"", funFact = "Has secretly converted most of school grounds to pet cemetery", superlative = "Most Likely Lives In Shed", age = "77" });

            nameAndInfo.Add("Journal Instructor", new CharacterInfo() { name = "Keeper of the Quests", yearbookQuote = "\"Elvish society relies not on the slave labor of dwarves alone, but through strict organization, whereby we become great elves.\"", funFact = "Can recall 5000 years of Middle-earth lore, forgets mother's birthday", superlative = "Best Worst Set Of Priorities", age = "17" });

            nameAndInfo.Add("Karma Instructor", new CharacterInfo() { name = "Karma Shaman", yearbookQuote = "\"Great monuments are built upon a foundation of great karma and the most friends.\"", funFact = "Shoplifts", superlative = "Highest Tolerance For Fruit Flies", age = "15" });

            nameAndInfo.Add("Julius Caesar", new CharacterInfo() { name = "Julius Caesar", yearbookQuote = "I love a good 'brew' alongside my 'bros,' more than I fear death.\" - Me", funFact = "Had a child with Cleopatra, reformed calendar", superlative = "Most Likely To Wage War Against Several Gallic Tribes", age = "55" });

            nameAndInfo.Add("Blurso", new CharacterInfo() { name = "Blurso", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most Likely To Wage War Against Several Gallic Tribes", age = "16" });

            nameAndInfo.Add("Pelt Kid", new CharacterInfo() { name = "Pelt Kid", yearbookQuote = "\"There exists no problem large or rare enough that it can't be shot down, eaten and worn.\" – Pa", funFact = "Accidently shot Pa dead while on Colorado Pond Snake hunt", superlative = "Most likely to stay in college until he's 40.", age = "18" });

            nameAndInfo.Add("Squirrel Boy", new CharacterInfo() { name = "Squirrel Boy", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most Likely To Shoot Mark", age = "16" });

            nameAndInfo.Add("Jesse", new CharacterInfo() { name = "Jesse Krigster", yearbookQuote = "\"Any free meal is a good meal, you know?\" - Joe Sakic", funFact = "Has unwittingly ingested human flesh multiple times", superlative = "Most Likely To Fall Ill From Kuru", age = "17" });


            enemyNameAndInfo.Add("Fez", new EnemyInfo() { name = "Fez", experienceGiven = "1", hobby = "Recalling glory days of Ottoman Empire", itemDrop = "World-weary sigh", level = "1" });

            enemyNameAndInfo.Add("Benny Beaker", new EnemyInfo() { name = "Benny Beaker", experienceGiven = "5", hobby = "Sea-shanties", itemDrop = "Broken Glass", level = "3" });

            enemyNameAndInfo.Add("Erl The Flask", new EnemyInfo() { name = "Erl the Flask", experienceGiven = "3", hobby = "Fuckin' swaggin' around", itemDrop = "Broken Glass", level = "2" });

            enemyNameAndInfo.Add("Scarecrow", new EnemyInfo() { name = "Scarecrow", experienceGiven = "45", hobby = "Sneaking up on unsuspecting Daryls", itemDrop = "Corn Stalk", level = "14" });

            enemyNameAndInfo.Add("Crow",  new EnemyInfo() { name = "Crow", experienceGiven = "30", hobby = "Corn muffins", itemDrop = "Feather", level = "13" });

            enemyNameAndInfo.Add("Goblin", new EnemyInfo() { name = "Goblin", experienceGiven = "75", hobby = "Building swingsets", itemDrop = "Invisible Goblin Gold", level = "15" });

            enemyNameAndInfo.Add("Field Goblin", new EnemyInfo() { name = "Field Goblin", experienceGiven = "25", hobby = "Building field swingsets", itemDrop = "Invisible Field Goblin Gold", level = "4" });

            enemyNameAndInfo.Add("Troll", new EnemyInfo() { name = "Troll", experienceGiven = "1000", hobby = "Licking roof of mouth", itemDrop = "Increased land value", level = "16" });

            enemyNameAndInfo.Add("Goblin Gate", new EnemyInfo() { name = "Goblin Gate", experienceGiven = "None", hobby = "Avoiding erosion ", itemDrop = "A lot of dirt and garbage", level = "5" });
        }
    }

    public class CharacterBio : Collectible
    {
        float rayRotation;
        float floatCycle;
        String characterName;
        //String info;
        Boolean isAnEnemy;

        public String CharacterName { get { return characterName; } }
        //public String Info { get { return info; } }
        public CharacterBio(int x, int platY, string name, Boolean isEnemy)
            : base
                (Game1.whiteFilter, x, platY)
        {
            collecName = "Character Bio";
            isAnEnemy = isEnemy;
            characterName = name;
            rec = new Rectangle(x, platY - 94, 94, 90);
            //info = CharacterMonsterBioDictionary.nameAndInfo[characterName];
            textureOnMap = Game1.bioTexture;
        }

        public Rectangle GetSourceRec()
        {
            return new Rectangle();
        }

        public override void Update()
        {
            base.Update();

            if (pickedUp == false && ableToPickUp)
            {
                rayRotation++;

                if (rayRotation == 360)
                    rayRotation = 0;

                #region FLOAT UP AND DOWN
                //--Once it hits the ground, make it float up and down
                else
                {
                    //--Every 20 frames it changes direction
                    //--It floats at 1 pixel per frame, every 2 frames
                    if (floatCycle < 50)
                    {
                        if (floatCycle % 5 == 0)
                            rec.Y -= 1; floatCycle++;

                    }
                    else
                    {
                        if (floatCycle % 5 == 0)
                            rec.Y += 1; floatCycle++;

                        if (floatCycle >= 100)
                        {
                            floatCycle = 0;
                        }
                    }
                }
                #endregion
            }
        }

        public override void PickUpCollectible()
        {
            base.PickUpCollectible();

            Game1.Player.Textbooks++;
            if (!isAnEnemy)
            {
                Chapter.effectsManager.AddFoundItem("a Character Biography", Game1.storyItemIcons["Piece of Paper"]);
                Game1.Player.AllCharacterBios[CharacterName] = true;
            }
            else
                Chapter.effectsManager.AddFoundItem("a Monster Biography", Game1.storyItemIcons["Piece of Paper"]);
        }

        public override void Draw(SpriteBatch s)
        {
            if (pickedUp == false && ableToPickUp)
            {
                s.Draw(Game1.textbookRay, new Rectangle(rec.X + rec.Width / 2, rec.Y + rec.Height / 2, Game1.textbookRay.Width, Game1.textbookRay.Height), null, Color.White, (float)(rayRotation * (Math.PI / 180)), new Vector2(Game1.textbookRay.Width / 2, Game1.textbookRay.Height / 2), SpriteEffects.None, 0f);
                s.Draw(textureOnMap, rec, Color.White);
            }
        }

    }
}

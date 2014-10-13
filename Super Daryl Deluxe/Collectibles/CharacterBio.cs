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

            nameAndInfo.Add("Paul", new CharacterInfo() { name = "Paul Palte", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Alan", new CharacterInfo() { name = "Alan Orpter", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Mr. Robatto", new CharacterInfo() { name = "Mr. Robatto", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Chelsea", new CharacterInfo() { name = "Chelsea Lardstall", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Mark", new CharacterInfo() { name = "Mark", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Bob the Construction Guy", new CharacterInfo() { name = "Bob the Construction Guy", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Trenchcoat Employee", new CharacterInfo() { name = "Trenchcoat Employee", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Equipment Instructor", new CharacterInfo() { name = "Equipment Instructor", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Save Instructor", new CharacterInfo() { name = "Save Instructor", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Skill Instructor", new CharacterInfo() { name = "Skill Instructor", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Tim", new CharacterInfo() { name = "Tim", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Gardener", new CharacterInfo() { name = "Gardener", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Journal Instructor", new CharacterInfo() { name = "Journal Instructor", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Karma Instructor", new CharacterInfo() { name = "Karma Instructor", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Julius Caesar", new CharacterInfo() { name = "Julius Caesar", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Blurso", new CharacterInfo() { name = "Blurso", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Pelt Kid", new CharacterInfo() { name = "Pelt Kid", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Squirrel Boy", new CharacterInfo() { name = "Squirrel Boy", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            nameAndInfo.Add("Jesse", new CharacterInfo() { name = "Jesse", yearbookQuote = "I am the alpha and the omega, the greatest textbook salesman to ever inhabit this ball of floating dirt.", funFact = "Has never sold a textbook.", superlative = "Most likely to stay in college until he's 40.", age = "16" });

            enemyNameAndInfo.Add("Benny Beaker", new EnemyInfo() { name = "Benny Beaker", experienceGiven = "5", hobby = "Eating cabbage", itemDrop = "Broken Glass", level = "3" });

            enemyNameAndInfo.Add("Erl The Flask", new EnemyInfo() { name = "Erl the Flask", experienceGiven = "3", hobby = "Fuckin' swaggin' around", itemDrop = "Broken Glass", level = "2" });

            enemyNameAndInfo.Add("Scarecrow", new EnemyInfo() { name = "Scarecrow", experienceGiven = "5", hobby = "Sneaking up on unsuspecting Daryls", itemDrop = "Corn Stalk", level = "3" });

            enemyNameAndInfo.Add("Crow",  new EnemyInfo() { name = "Crow", experienceGiven = "5", hobby = "Corn muffins", itemDrop = "Feather", level = "2" });

            enemyNameAndInfo.Add("Goblin",  new EnemyInfo() { name = "Goblin", experienceGiven = "25", hobby = "Building swingsets", itemDrop = "Goblin Rags", level = "4" });

            enemyNameAndInfo.Add("Field Goblin", new EnemyInfo() { name = "Field Goblin", experienceGiven = "25", hobby = "Building field swingsets", itemDrop = "Field Goblin Rags", level = "4" });

            enemyNameAndInfo.Add("Troll",  new EnemyInfo() { name = "Troll", experienceGiven = "75", hobby = "Licking roof of mouth", itemDrop = "Ass Feathers", level = "5" });

            enemyNameAndInfo.Add("Goblin Gate", new EnemyInfo() { name = "Goblin Gate", experienceGiven = "75", hobby = "Licking roof of mouth", itemDrop = "Ass Feathers", level = "5" });
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

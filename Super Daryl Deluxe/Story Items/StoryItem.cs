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
    public class AllStoryItems
    {
        public Dictionary<String, String> allStoryItems;

        public AllStoryItems()
        {
            allStoryItems = new Dictionary<string, string>();

            allStoryItems.Add("Bronze Key", "A bronze key.");
            allStoryItems.Add("Silver Key", "A silver key.");
            allStoryItems.Add("Gold Key", "A gold key.");
            allStoryItems.Add("Challenge Room Key", "A key that unlocks challenge rooms.");
            allStoryItems.Add("Key Ring", "The janitor's key ring.");
            allStoryItems.Add("Closet Key", "A key that opens the janitor's closet.");
            allStoryItems.Add("Pyramid Key", "An ancient key made of sandstone.");
            allStoryItems.Add("Christmas Cage Key", "A key that locks the door on Christmas Spirit.");
            allStoryItems.Add("Backstage Key", "Meet the band!");
            allStoryItems.Add("Security Clearance I.D", "An I.D card that gives you high security clearance in the Theater An Der Wien.");
            allStoryItems.Add("Empty Bottle", "Unlike it's older counterparts, this bottle never runs out of precious liquid once it's filled.");
            allStoryItems.Add("Pyramid Water", "Thousand year old water from the musty pyramid springs. Try pouring it on stuff!");
            allStoryItems.Add("Piece of Paper", "A crumpled up piece of paper.");
            allStoryItems.Add("Old painting", "A painting that is centuries old. Probably worth a lot of lunches.");
            allStoryItems.Add("Coal", "A chunk of dirty coal. Try eating it.");
            allStoryItems.Add("Harpsichord", "Wow! What a great gift from Beethoven!");
            allStoryItems.Add("Recorder", "The most precious of fipple flutes.");
            allStoryItems.Add("Ten Dollars", "You should invest.");
            allStoryItems.Add("Napoleon's Supplies", "Barrels full of white flags.");
            allStoryItems.Add("Old Battery", "A cheap, old battery.");
            allStoryItems.Add("250 Ducats", "Also known as arcade tokens.");
            allStoryItems.Add("Vienna Access Card", "An access card to the Vienna portal.");
            allStoryItems.Add("Ear Trumpet", "Beethoven pretends to hear when he has this.");
            allStoryItems.Add("Dandelion", "A beautiful dandelion.");
            allStoryItems.Add("Goblin Morphine", "Try injecting into your favorite test subjects!");
            allStoryItems.Add("Jarred Brain", "The jarred brain of a European Swamp Ape.");
            allStoryItems.Add("Jarred Liver", "The profoundly efficient liver of the legendary ruler, Pharaoh Drinx Ahlut.");
            allStoryItems.Add("Jarred Heart", "Yes, it still beats, but can it still love?");
            allStoryItems.Add("Letter to Caesar", "An invitation to join forces, from Napoleon to Julius.");
            allStoryItems.Add("Letter to Cleopatra", "An invitation to join forces, from Napoleon to Cleopatra.");

            allStoryItems.Add("Beer", "Some beer from Chelsea's party.");
            allStoryItems.Add("Assorted Nuts", "An old, moldy can of assorted nuts.");
            allStoryItems.Add("Scissors", "A pair of 100% conductable metal scissors.");
            allStoryItems.Add("Squiggles the Hostage", "Squiggles family is very concerned.");
        }
    }

    public class StoryItem
    {
        protected Rectangle rec;
        protected Texture2D texture;
        protected String name;
        protected String pickUpName; //This is the name that will display when you pick the item up
        protected Texture2D icon;
        //protected String mapName;
        protected Player player;
        protected Boolean pickedUp = false;
        protected KeyboardState previous;
        protected KeyboardState current;
        protected String description;
        protected int animationTime = 0;
        protected int maxAnimationTime = 20;
        protected Boolean inChest = false;
        protected int moveFrame;
        protected int frameDelay;
        protected bool facingRight = true;
        protected bool showFButton = true;

        public String Name { get { return name; } }
        public String PickUpName { get { return pickUpName; } }
        public String Description { get { return description; } }
        public int RecX { get { return rec.X; } set { rec.X = value; } }
        public int RecY { get { return rec.Y; } set { rec.Y = value; } }
        public Rectangle Rec { get { return rec; } }
        public Boolean PickedUp { get { return pickedUp; } set { pickedUp = value; } }
        public Texture2D Icon { get { return icon; } }
        public Boolean ShowFButton { get { return showFButton; } set { showFButton = value; } }

        public StoryItem(int x, int platY)
        {
            //this.mapName = map;
            player = Game1.Player;
        }
        public StoryItem(Boolean inChest)
        {
            //this.mapName = map;
            player = Game1.Player;
            this.inChest = inChest;
        }


        public virtual Rectangle GetSourceRec()
        {
            return new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public virtual void Update()
        {
            if (!inChest && !pickedUp)
            {
                previous = current;
                current = Keyboard.GetState();

                if (NearPlayer() && ((previous.IsKeyDown(Keys.Space) && current.IsKeyUp(Keys.Space)) || MyGamePad.RightTriggerPressed()) && !pickedUp)
                {
                    if (player.StoryItems.ContainsKey(name))
                        player.StoryItems[name]++;
                    else
                        player.StoryItems.Add(name, 1);

                    pickedUp = true;
                    Chapter.effectsManager.AddFoundItem(pickUpName, Game1.storyItemIcons[name]);

                    Sound.PlaySoundInstance(Sound.SoundNames.object_pickup_misc);
                }
            }
        }

        public virtual void isClicked()
        {

        }

        public virtual void Draw(SpriteBatch s)
        {
            if (!inChest)
            {
                if (!pickedUp)
                {
                    if(facingRight)
                        s.Draw(texture, rec, Color.White);
                    else
                        s.Draw(texture, rec, null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

                }

                Rectangle spaceRec = new Rectangle(rec.X + rec.Width / 2 - 96 / 2, rec.Y - 70, (int)(120 * .8f),
                    (int)(52 * .8f));

                if (NearPlayer() && !pickedUp && showFButton)
                {

                    if (!Chapter.effectsManager.spaceButtonRecs.Contains(spaceRec))
                        Chapter.effectsManager.AddSpaceButton(spaceRec);

                }
                else
                {
                    if (Chapter.effectsManager.spaceButtonRecs.Contains(spaceRec))
                        Chapter.effectsManager.spaceButtonRecs.Remove(spaceRec);
                }
            }

        }

        public virtual bool NearPlayer()
        {
            if (player.VitalRec.Intersects(rec))
                return true;

            return false;
        }
    }
}

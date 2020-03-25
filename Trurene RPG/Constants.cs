/*
 * This file contains a list of variables and constants. These make the software
 * very easy to be fine-tuned and balanced, to make the game the best experience
 * as possible for the user. This also has constants which do not change and won't
 * be "fine-tuned", but instead these constants are just here to make it easy to
 * find their initialisation and understand what they do.
 * As is the common convention, all of these variables are declared in all CAPITALS.
 * 
 */



using System;
using System.Drawing;
using System.Windows.Forms;

namespace Trurene_RPG
{
    class Constants
    {
        // Spells
        public const int SPELL_LEARNT = 2;
        public const int SPELL_AQCUIRED = 1;
        public const int SPELL_NOT_ACQUIRED = 0;
        
        public const int VAMPIRE_INDEX = 0; // The index of the spell in the world.spells array
        public const int TROLL_INDEX = 1;
        public const int GOBLIN_INDEX = 2;
        public const int OMNISCIENCE_INDEX = 3;

        public const double VAMPIRE_HEAL = 0.3; // The multiplier of Aurora's power which she heals every hit

        // Map icons


        public static string[] DEVELOPER_RESOURCE_PACKS = { "classic" };
        public static string RESOURCE_PACK = "Classic";
        public static Image EMPTY_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/empty-icon.png");
        public static Image AURORA_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/aurora-icon.png");
        public static Image TROLL_KING_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/troll-king-icon.png");
        public static Image WOLVES_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/wolves-icon.png"); 
        public static Image INTACT_VILLAGE_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/intact-village-icon.png");
        public static Image DESTROYED_VILLAGE_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/destroyed-village-icon.png");
        public static Image UNSOLVED_SHRINE_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/unsolved-shrine-icon.png");
        public static Image SOLVED_SHRINE_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/solved-shrine-icon.png");
        public static Image MAEJA_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/maeja-icon.png");
        public static Image HAWK_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/hawk-icon.png"); 
        public static Image QUEST_ICON = Image.FromFile("data/resource packs/"+RESOURCE_PACK+"/quest-icon.png");

        // Images
        public static double SCREEN_HEIGHT = Screen.PrimaryScreen.WorkingArea.Height; // Screen dimensions
        public static double SCREEN_WIDTH = Screen.PrimaryScreen.WorkingArea.Width;
        public static int MAP_HEIGHT = (int)Math.Floor(SCREEN_HEIGHT/3.5); // Get perfect icon dimensions
        public static int MAP_WIDTH = (int)Math.Floor(SCREEN_WIDTH/3.5);
        public static int ICON_HEIGHT = 10; // default values to stop crashing
        public static int ICON_WIDTH = 10;

        // Fighting
        public const double SIMULTANEOUS_STRIKE_DAMAGE = 0.8;
        public const int NUM_TIMES_TO_SHATTER = 1;
        public const double SHATTER_DAMAGE = 0.3;

        // Character infos (and Creature infos)
        public struct AuroraInfo
        {
            public const double REGENERATE_MULTIPLIER = 0.1; // health = max health * regenerate multiplier
            public const int HEALTH = 100;
            public const int ACCURACY = 70;
            public const int POWER = 10;
            public const int TIME = 4;
        }
        public struct WolvesInfo
        {
            public const double REWARD_MULTIPLIER = 0.5; // max health*reward multiplier = reward
            public const double REGENERATE_MULTIPLIER = 0.05;
            public const int MIN_HEALTH = 50;
            public const int MAX_HEALTH = 75;
            public const int MIN_ACCURACY = 80;
            public const int MAX_ACCURACY = 100;
            public const int MIN_POWER = 30;
            public const int MAX_POWER = 50;
            public const int MIN_TIME = 2;
            public const int MAX_TIME = 4;

        }
        public struct TrollKingInfo
        {
            public const double REGENERATE_MULTIPLIER = 0.2;
            public const int PROXIMITY = 3; // The maximum distance to Aurora when he will move away from her
            public const int MIN_HEALTH = 100;
            public const int MAX_HEALTH = 150;
            public const int MIN_ACCURACY = 70;
            public const int MAX_ACCURACY = 100;
            public const int MIN_POWER = 40;
            public const int MAX_POWER = 70;
            public const int MIN_TIME = 4;
            public const int MAX_TIME = 7;

        }
        public struct SmallCreatureInfo
        {
            // The minimum and the range (how much more) is stated for each variable for
            // when it is used to determine a random value for it.
            // The range of the random number is used because it makes more sense when generating
            // the random number.

            public const double PROBABILITY = 0.1;
            public const int MIN_HEALTH = 15;
            public const int MAX_HEALTH = 25;
            public const int MIN_ACCURACY = 40;
            public const int MAX_ACCURACY = 100;
            public const int MIN_POWER = 5;
            public const int MAX_POWER = 15;
            public const int MIN_TIME = 3;
            public const int MAX_TIME = 5;
            public const double REWARD_MUTLIPLIER = 0.6;
        }
        public struct LargeCreatureInfo
        {
            // The minimum and the range (how much more) is stated for each variable for
            // when it is used to determine a random value for it.
            // The range of the random number is used because it makes more sense when generating
            // the random number.

            public const double PROBABILITY = 0.05;
            public const int MIN_HEALTH = 25;
            public const int MAX_HEALTH = 75;
            public const int MIN_ACCURACY = 60;
            public const int MAX_ACCURACY = 100;
            public const int MIN_POWER = 15;
            public const int MAX_POWER = 25;
            public const int MIN_TIME = 3;
            public const int MAX_TIME = 5;
            public const double REWARD_MUTLIPLIER = 0.9;
        }
        public struct WorldInfo
        {
            public const int NUM_SHRINES = 4;
            public const int MIN_NUM_VILLAGES = 4;
            public const int MAX_NUM_VILLAGES = 6;

        }

        // Text box color constants
        public static Color SUCCESS_FORE = Color.LightGreen;
        public static Color SUCCESS_BACK = Color.DarkGreen;
        public static Color WARNING_FORE = Color.Red;
        public static Color WARNING_BACK = Color.DarkRed;
        public static Color GOLD_FORE = Color.LightGoldenrodYellow;
        public static Color GOLD_BACK = Color.DarkGoldenrod;
        public static Color NORMAL_FORE = Color.Black;
        public static Color NORMAL_BACK = Color.White;

        // Weapon creation from gold
        public struct MaceInfo
        {
            public const double ACCURACY_MULTIPLIER = 1.1;
            public const double POWER_MULTIPLIER = 0.8;
            public const double TIME_QUOTIENT = 250.0; // This is inversly multiplied by the gold
        }
        public struct SwordInfo
        {
            public const double ACCURACY_MULTIPLIER = 1.2;
            public const double POWER_MULTIPLIER = 0.6;
            public const double TIME_QUOTIENT = 200.0; // This is inversly multiplied by the gold
        }
        public struct DaggerInfo
        {
            public const double ACCURACY_MULTIPLIER = 1.3;
            public const double POWER_MULTIPLIER = 0.4;
            public const double TIME_QUOTIENT = 150.0; // This is inversly multiplied by the gold
        }

        // Miscellaneous Constants
        public struct QuestInfo
        {
            public const int MIN_REWARD = 20;
            public const int MAX_REWARD = 30;
        }


    }
}

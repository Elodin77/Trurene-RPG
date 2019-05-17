/*
Trurene - The RPG is a console based game created for a school project.
Copyright (C) 2019  Djimon Jayasundera

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <https://www.gnu.org/licenses/>.

My personal email is <djimondjayasundera@icloud.com>, my school email is <1015097@student.ccgs.wa.edu.au>.
More information about the project (including downloads, the license, and manuals) can be found at <https://github.com/Elodin77/Trurene-RPG/>.

This file contains all of the functions important for the actual game algorithms. This doesn't 
include anything directly related to the Windows Processing Form User Interface.
 */


using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions; 
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Key = System.Windows.Input.Key;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Linq;
using System.Windows.Forms;
using static Trurene_RPG.Constants;
using System.Media;
using System.Windows.Documents;
using Brushes = System.Windows.Media.Brushes;

namespace Trurene_RPG
{
    class Program
    {

        // MAIN //
        [STAThread] // Marks function as a Standard Thread Apartment
        private static void Main(string[] args)
        {
            /* This is the main function. It does not actually run the game or coordinate
             * the other functions related to the game. But it has the code for the very start
             * of the game and the very end, but not the actual game itself. Because of the
             * nature of the User Interface. The UI actually coordinates and runs the game.
             */
            Thread.Sleep(500);
            // Create default values
            GenerateDefaultValues();
            // Show title 
            string text = "|Trurene - The RPG|";
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2)); // this is used to centre the text
            Console.WriteLine(new String('-', text.Length));
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(new String('-', text.Length));
            Console.WriteLine("\n");
            text = "A game by Djimon Jayasundera";
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
            text = "Computer Science ATAR 2019";
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
            Thread.Sleep(2000);
            Console.WriteLine("\n\n\nPRESS ANY KEY TO CONTINUE");
            Thread.Sleep(500);
            FlushKeyboard();
            Console.ReadKey(true);
            Console.Clear();

            // Show the lore
            string lore = File.ReadAllText("data/lore.txt", Encoding.UTF8); // Read in the lore
            Console.WriteLine("Press the <RIGHT ARROW> to speed up the lore");

            PrettyPrint(lore, 30); // Print out the lore
            Console.WriteLine("\n\n\nPRESS ANY KEY TO CONTINUE");
            Thread.Sleep(500);
            FlushKeyboard();
            Console.ReadKey(true);
            Console.Clear();
            string entry = "";
            bool failed = false;
            do
            {
                if (failed)
                {
                    Console.WriteLine("That is an invalid input! Make sure to input in CAPITALS.");
                }
                Console.Write("Do you want to create a new game or continue a current game ('NEW'/'CONTINUE'): ");
                entry = Console.ReadLine();
                failed = true;
            } while (entry != "NEW" && entry != "CONTINUE");
            if (entry == "NEW")
            {
                int enteredSize = 1;
                bool completed = false;
                do
                {
                    try
                    {
                        Console.Write("World size (number between 7 and 12): ");
                        enteredSize = Convert.ToInt32(Console.ReadLine());
                        completed = true;
                    }
                    catch
                    {
                        Console.WriteLine("That is an invalid input!");
                    }
                } while (!completed || enteredSize < 7 || enteredSize > 12);
                GenerateRandomWorld(enteredSize);
            }
            else
            {
                // detect save files
                string[] files = Directory.GetFiles(@"saves/", "*", System.IO.SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = Regex.Replace(files[i], @"saves/", ""); // just get the name of the save file and not the whole path
                }
                Console.WriteLine("Your Saved Games");
                foreach (string str in files)
                {
                    Console.WriteLine("\t- " + str);
                }

                while (true)
                {
                    Thread.Sleep(500); // give the user time
                    Console.Write("Load filename: ");
                    try
                    {
                        Load(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Either that file doesn't exist or the format is wrong!!!");
                    }
                }
            }

            Console.WriteLine("\n\n\nPRESS ANY KEY TO CONTINUE");
            Thread.Sleep(500);
            FlushKeyboard();
            Console.ReadKey(true);
            Console.Clear();

            // detect all folders in resource packs directory
            string[] folders = Directory.GetDirectories(@"data/resource packs", "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < folders.Length; i++)
            {
                folders[i] = Regex.Replace(folders[i],@"data/resource packs\\", ""); // just get the name of the resource pack
            }

            Console.WriteLine("The Developer's Resource Packs were the UNREFINED icons created and used by the developer in the development of the project.");
            Console.WriteLine("The External Resource Packs were not created by the developer and are far more REFINED versions of the original icons.");
            Console.WriteLine();
            // show the developer's resource packs
            Console.WriteLine("Developer's Resource Packs: ");
            foreach (string str in DEVELOPER_RESOURCE_PACKS)
            {
                Console.WriteLine("\t- " + str);
            }
            Console.WriteLine();
            // show externally made (developer did not make these) resource packs... (i.e. anything not recognised that was detected in the resource packs folder)
            Console.WriteLine("External Resource Packs (mods)");
            foreach (string str in folders)
            {
                if (!DEVELOPER_RESOURCE_PACKS.Contains(str))
                {
                    Console.WriteLine("\t- " + str);
                }
            }
            Console.WriteLine();
            // load the resource pack which the user wants to use
            while (true)
            {
                Thread.Sleep(500); // give the user time
                try
                {
                    Console.Write("Resource Pack: ");
                    RESOURCE_PACK = Console.ReadLine();
                    UpdateMapIcons(); // change the map icons to the new resource pack
                    break;
                }
                catch
                {
                    Console.WriteLine("Either that doesn't exist or the format is wrong!!!");
                }
            }



            Console.WriteLine("\n\n\nPRESS ANY KEY TO CONTINUE");
            Thread.Sleep(500);
            FlushKeyboard();
            Console.ReadKey(true);
            Console.Clear();

            UnitTesting.RunUnitTesting(); // Run the testing

            Console.WriteLine("Please turn on sound for the best experience.");
            Thread.Sleep(2000);

            // start sound
            backgroundMusic = new SoundPlayer(@"data/audio/background1.wav");
            backgroundMusic.PlayLooping();

            // Start the game
            NormalUI.StartupUri = new Uri("NormalUI.xaml", System.UriKind.Relative); // Make the app run code on startup
            NormalUI.Run(); // Run the app
            // Finish the game

            int score = CalculateScore();

            Console.WriteLine("\n\n\nPRESS ANY KEY TO CONTINUE");
            Thread.Sleep(500);
            FlushKeyboard();
            Console.ReadKey(true);
            Console.Clear();

            text = "|Trurene - The RPG|";
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2)); // this is used to centre the text
            Console.WriteLine(new String('-', text.Length));
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(new String('-', text.Length));
            Console.WriteLine("\n");
            text = "A game by Djimon Jayasundera";
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
            text = "Computer Science ATAR 2019";
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
            Console.WriteLine();
            text = "THANK YOU SO MUCH FOR PLAYING";
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
            Thread.Sleep(2000);
            Console.ReadKey();

            Console.WriteLine("\n\n\nPRESS ANY KEY TO CLOSE THE GAME");
            Thread.Sleep(1000);
            FlushKeyboard();
            Console.ReadKey();
        }


        // MOVEMENT //
        public static void MoveAurora()
        {
            /* This function does everything necessary to collect the information and move Aurora
             * to a valid position.
             */


            Position moveTo = world.aurora.pos;
            if (moveAction == "North")
            {
                moveTo = world.aurora.pos;
                moveTo.row -= 1;
            }
            else if (moveAction == "South")
            {
                moveTo = world.aurora.pos;
                moveTo.row += 1;
            }
            else if (moveAction == "East")
            {
                moveTo = world.aurora.pos;
                moveTo.col += 1;
            }
            else if (moveAction == "West")
            {
                moveTo = world.aurora.pos;
                moveTo.col -= 1;
            }
            if (CalcValidMoves(world.aurora.pos).Contains(moveTo))
            {
                world.aurora.pos = moveTo;
            }
            else
            {
                AddNotification("That is an invalid move!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Red });
                CustomMessageBox.ShowText("That is an invalid move!", "MOVEMENT ERROR", WARNING_BACK, WARNING_FORE);
            }

        }
        public static List<Position> CalcValidMoves(Position pos)
        {
            /* This function generates a list of valid moves which any entity at a given position can move to.
             * It is essentially the squares above, below and to the left and right.
             */
            List<Position> validMoves = new List<Position>();
            if (pos.row < world.rows-1)
            {
                Position validPos = new Position();
                validPos.row = pos.row + 1;
                validPos.col = pos.col;
                validMoves.Add(validPos);
            }
            if (pos.row > 0)
            {
                Position validPos = new Position();
                validPos.row = pos.row - 1;
                validPos.col = pos.col;
                validMoves.Add(validPos);
            }
            if (pos.col < world.cols - 1)
            {
                Position validPos = new Position();
                validPos.row = pos.row;
                validPos.col = pos.col + 1;
                validMoves.Add(validPos);
            }
            if (pos.col > 0)
            {
                Position validPos = new Position();
                validPos.row = pos.row;
                validPos.col = pos.col - 1;
                validMoves.Add(validPos);
            }

            return validMoves;
        }
        public static void UpdateWorld()
        {
            /* This function updates the world without moving Aurora. This includes destroying any villages and moving
             * each of the NPCs which can move.
             */
            // Destroy Villages
            if (DistanceBetween(world.trollKing.pos, world.targetVillagePosition) == 0)
            {
                world.destroyedVillages.Add(world.trollKing.pos);
                // [todo] bug here
                world.targetVillagePosition = world.villagePositions.Except(world.destroyedVillages).ToList()[random.Next(world.villagePositions.Length - world.destroyedVillages.Count() - 1)];
            }
            // Move Hawk if he is on a destroyed village
            if (world.destroyedVillages.Contains(world.hawkPosition))
            {
                world.hawkPosition = world.villagePositions.Except(world.destroyedVillages).ToList()[random.Next(world.villagePositions.Length - world.destroyedVillages.Count())];
            }
            // Move NPCs
            if (world.turnNum % 2 == 0)
            {
                MoveTrollKing();
            }
            if (world.turnNum % 3 == 0)
            {
                MoveWolves();
            }
        }
        public static void MoveTrollKing()
        {
            /* This function moves the Troll King one step closer to the target village.
             * However, if Aurora is with his detection proximity, he will instead move away from her.
             */
            List<Position> possibleMoves;
            if (DistanceBetween(world.trollKing.pos, world.aurora.pos) <= TrollKingInfo.PROXIMITY)
            {
                possibleMoves = CalcValidMoves(world.trollKing.pos).Except(MoveCloserTo(world.trollKing.pos, world.aurora.pos)).ToList();

            }
            else
            {
                possibleMoves = MoveCloserTo(world.trollKing.pos, world.targetVillagePosition);

            }
            if (possibleMoves.Count() == 0) // sometimes the Troll King can be pinned in a corner, where he has nowhere to go
            {
                possibleMoves = MoveCloserTo(world.trollKing.pos, world.targetVillagePosition);
            }
            world.trollKing.pos = possibleMoves[random.Next(possibleMoves.Count)];
        }
        public static void MoveWolves()
        {
            /* This moves the wolves one step closer to Aurora's position.
             */
            List<Position> possibleMoves = MoveCloserTo(world.wolves.pos, world.aurora.pos);
            try
            {
                world.wolves.pos = possibleMoves[random.Next(possibleMoves.Count)];
            }
            catch
            {
                // Do nothing because Aurora must have retreated as they are currently on the same square.
            }
        }
        public static List<Position> MoveCloserTo(Position current, Position target)
        {
            /* This function generates a list of possible moves to move the current position
             * closer to the target position.
             */
            List<Position> possibleFinalPos = new List<Position>();
            Position finalPos = new Position();
            if (current.row < target.row)
            {
                finalPos.row = current.row + 1;
                finalPos.col = current.col;
                possibleFinalPos.Add(finalPos);
            }
            else if (current.row > target.row)
            {
                finalPos.row = current.row - 1;
                finalPos.col = current.col;
                possibleFinalPos.Add(finalPos);
            }
            if (current.col < target.col)
            {
                finalPos.row = current.row;
                finalPos.col = current.col + 1;
                possibleFinalPos.Add(finalPos);
            }
            else if (current.col > target.col)
            {
                finalPos.row = current.row;
                finalPos.col = current.col - 1;
                possibleFinalPos.Add(finalPos);
            }
            return possibleFinalPos;
        }

        // USER INTERACTION //
        public static void NextTurn()
        {
            /* This is the function which is run for each turn. This function is run repeatedly to start each
             * turn in the game. The user runs this function with a button, but anti-cheat methods are implemented
             * to stop users exploiting the fact that they control when the turn "rolls over".
             * 
             * The functions which it does include:
             * updating the world.
             * showing any important messages to the user.
             * moving Aurora (the player).
             * doing any interactions important for fighting or villages.
             * stopping the game if it is over.
             * 
             */
            hawkOmniscience = false;
            UpdateBackgroundMusic();
            bool trollKingAlive = world.trollKing.health > 0;
            bool auroraAlive = world.aurora.health > 0;
            if (!(auroraAlive && trollKingAlive && world.villagePositions.Length > 0))
            {
                gameOver = true;
                NormalUI.Shutdown();
            }
            else
            {
                if (fighting)
                {
                    fighting = DoFightTurn();
                }
                else
                {
                    world.turnNum += 1;
                    UpdateWorld();
                    DoMessages();
                    MoveAurora();
                    CalcTurnEvents();

                }
            }

        }
        public static void CalcTurnEvents()
        {
            /* This function is one of the most complicated functions.
             * It determines all of the interactions and calls them between the player
             * and NPCs including the Troll King, the wolves, villages, shrines, and random fights.
             * 
             */

            fighting = false;
            fightingTrollKing = false;
            fightingWolves = false;
            enemyPreparedness = 0;
            auroraPreparedness = 0;
            bool somethingHappened = false; // Makes sure that only the certain amount of events happen
            // Check if Aurora fights the Troll King
            if (DistanceBetween(world.trollKing.pos, world.aurora.pos) == 0)
            {
                Creature trollKing = CharacterToCreature(world.trollKing);
                enemy = trollKing;
                fighting = true;
                AddNotification("You have encountered the TROLL KING!!!\n", new DependencyProperty[] { TextElement.ForegroundProperty, TextElement.FontWeightProperty }, new object[] { Brushes.Red, FontWeights.Bold });
                fightingTrollKing = true;
                somethingHappened = true;
            }
            // Otherwise, check if Aurora enters a village
            // This way she cannot enter a village which has just been destroyed the same turn.
            else if (world.villagePositions.Contains(world.aurora.pos) && !world.destroyedVillages.Contains(world.aurora.pos)) // villages are safe from wolves
            {
                DoVillage();
                somethingHappened = true;
            }
            else if (world.destroyedVillages.Contains(world.aurora.pos))
            {
                AddNotification("You see a destroyed village. There is nobody left.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Red });
            }
            // Check if Aurora is on Maeja's location
            if (DistanceBetween(world.aurora.pos, world.maejaPosition) == 0) // maeja's house is safe from wolves
            {
                DoMaeja();
                somethingHappened = true;
            }
            // Check if Aurora fights the wolves
            if (DistanceBetween(world.aurora.pos, world.wolves.pos) == 0)
            {
                Creature wolves = CharacterToCreature(world.wolves);
                enemy = wolves;
                fighting = true;
                AddNotification("You have encountered the WOLVES!!!\n", new DependencyProperty[] { TextElement.ForegroundProperty, TextElement.FontWeightProperty }, new object[] { Brushes.Red, FontWeights.Bold });
                
                fightingWolves = true;
                somethingHappened = true;
            }
            // Check if Aurora enters a shrine
            foreach (Shrine shrine in world.shrines)
            {
                if (DistanceBetween(world.aurora.pos, shrine.pos) == 0)
                {
                    DoShrine();
                    somethingHappened = true;
                    break;
                }
            }

            // Check if Aurora fights a small creature
            int fightCreature = 0; // 0 = none, 1 = small, 2 = large
            
            if (!somethingHappened && !fighting) {
                if (ProbabilityRandom(SmallCreatureInfo.PROBABILITY))
                {
                    fightCreature = 1;
                }
            }
            // Check if Aurora fights a large creature
            if (!somethingHappened && fightCreature == 0 && !fighting)
            {
                if (ProbabilityRandom(LargeCreatureInfo.PROBABILITY))
                {
                    fightCreature = 2;
                }
            }
            // NOTE: The probability is cascading, i.e. if small creature doesn't succeed then the large creature is tested.
            // Fight Aurora against the determined the creature
            if (fightCreature == 1)
            {
                // Edit the enemy variable since it is global and the WPF Application can access it.
                enemy.maxHealth = random.Next(SmallCreatureInfo.MIN_HEALTH,SmallCreatureInfo.MAX_HEALTH);
                enemy.health = enemy.maxHealth;
                enemy.attack = new int[3];
                enemy.attack[0] = random.Next(SmallCreatureInfo.MIN_ACCURACY,SmallCreatureInfo.MAX_ACCURACY);
                enemy.attack[1] = random.Next(SmallCreatureInfo.MIN_POWER, SmallCreatureInfo.MAX_POWER);
                enemy.attack[2] = random.Next(SmallCreatureInfo.MIN_TIME, SmallCreatureInfo.MAX_TIME);
                enemy.reward = (int) Math.Floor(SmallCreatureInfo.REWARD_MUTLIPLIER * enemy.maxHealth);
                // Fight the small creature
                fighting = true;
                AddNotification("You have encountered a SMALL CREATURE!!!\n", new DependencyProperty[] { TextElement.ForegroundProperty, TextElement.FontWeightProperty }, new object[] { Brushes.Red, FontWeights.Bold });
                
            }
            if (fightCreature == 2)
            {
                // Edit the values for enemy (so that the WPF Application can access them... since enemy is global)
                enemy.maxHealth = random.Next(LargeCreatureInfo.MIN_HEALTH, LargeCreatureInfo.MAX_HEALTH);
                enemy.health = enemy.maxHealth;
                enemy.attack = new int[3];
                enemy.attack[0] = random.Next(LargeCreatureInfo.MIN_ACCURACY, LargeCreatureInfo.MAX_ACCURACY);
                enemy.attack[1] = random.Next(LargeCreatureInfo.MIN_POWER, LargeCreatureInfo.MAX_POWER);
                enemy.attack[2] = random.Next(LargeCreatureInfo.MIN_TIME, LargeCreatureInfo.MAX_TIME);
                enemy.reward = (int)Math.Floor(LargeCreatureInfo.REWARD_MUTLIPLIER * enemy.maxHealth);
                // Fight the large creature
                fighting = true;
                AddNotification("You have encountered a LARGE CREATURE!!!\n", new DependencyProperty[] { TextElement.ForegroundProperty, TextElement.FontWeightProperty}, new object[] { Brushes.Red, FontWeights.Bold });
                

            }
            // Regenerate Aurora's health
            if (world.aurora.health > 0 && world.aurora.health < world.aurora.maxHealth && !fighting)
            {
                world.aurora.health += (int)Math.Floor(world.aurora.maxHealth * AuroraInfo.REGENERATE_MULTIPLIER);
                if (world.aurora.health > world.aurora.maxHealth)
                {
                    world.aurora.health = world.aurora.maxHealth;
                }
            }
            // Regenerate the Troll King's health
            if (world.trollKing.health > 0 && world.trollKing.health < world.trollKing.maxHealth && !fighting)
            {
                world.trollKing.health += (int)Math.Floor(world.trollKing.maxHealth * TrollKingInfo.REGENERATE_MULTIPLIER);
                if (world.trollKing.health > world.trollKing.maxHealth)
                {
                    world.trollKing.health = world.trollKing.maxHealth;
                }
            }
            // Regenerate the wolves' health
            if (world.wolves.health > 0 && !fighting)
            {
                world.wolves.health += (int)Math.Floor(world.wolves.maxHealth * WolvesInfo.REGENERATE_MULTIPLIER);
                if (world.wolves.health > world.wolves.maxHealth)
                {
                    world.wolves.health = world.wolves.maxHealth;
                }
            }
            else if (world.wolves.health <= 0) 
            {
                // Respawn the wolves because they must have been killed
                world.wolves.health = world.wolves.maxHealth;
                world.wolves.pos.row = random.Next(world.rows - 1);
                world.wolves.pos.col = random.Next(world.cols - 1);
                world.gold += (int)Math.Floor(WolvesInfo.REWARD_MULTIPLIER * world.wolves.attack[1]);
            }
            // Check if Aurora is on a quest location
            if (DistanceBetween(world.aurora.pos, world.questPosition) == 0 && world.questPosition.row != -1 && world.questPosition.col != -1)
            {
                world.questPosition.row = -1;
                world.questPosition.col = -1;
                world.gold += random.Next(QuestInfo.MIN_REWARD, QuestInfo.MAX_REWARD);
                PlaySound("coin.wav", 1.0);
                AddNotification("The friend walked up to you, you did a few jobs for them and they gave you a small pouch of gold. Wow.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Gold });


            }
            UpdateBackgroundMusic();

        }
        public static bool DoFightTurn()
        {
            /* This function runs the interaction for the fighting. 
             * The enemy is a global variable so that the WPF application can access it.
             */
            world.numFightTurns += 1; // High score is bad
            bool enemyStrike = false;
            bool auroraStrike = false;
            bool retreated = false;
            int simultaneousStrikes = 0;
            if (world.spells[2] == 2 && !goblinArtefactUsed) // If Goblin Spell learnt
            {
                goblinArtefactUsed = true;
                enemy.attack[0] -= 20;
                AddNotification("The enemy loses 20% accuracy through the " + artefacts[GOBLIN_INDEX]+"\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Green });
            }

            auroraStrike = false;
            enemyStrike = false;

            if (fightAction != "Retreat" && auroraPreparedness < world.aurora.attack[2])
            {
                fightAction = "Prepare"; // Automatically set it to prepare
            }
            if (enemyPreparedness < enemy.attack[2])
            {
                enemyPreparedness += 1;
            }
            else
            {
                enemyPreparedness -= enemy.attack[2];
                double prob = enemy.attack[0] / 100.0;
                if (ProbabilityRandom(prob))
                {
                    enemyStrike = true;
                }

            }
            // Do the entered action
            if (fightAction == "Strike")
            {
                auroraPreparedness -= world.aurora.attack[2];
                double prob = world.aurora.attack[0] / 100.0;
                if (ProbabilityRandom(prob))
                {
                    auroraStrike = true;
                }
                else
                {
                    PlaySound("miss.wav", 0.5);
                    AddNotification("You miss!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Red });
                    
                }

            }
            if (fightAction == "Prepare")
            {
                auroraPreparedness += 1;
            }
            if (fightAction == "Retreat")
            {
                double prob = world.aurora.attack[0] / 100.0;
                if (ProbabilityRandom(prob))
                {
                    AddNotification("You successfully retreat!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Green });
                    
                    retreated = true;
                    fighting = false;
                }
                else
                {
                    AddNotification("Your retreat fails!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Red });
                    
                }
            }
            // Check if there was a simultaneous strike
            if (auroraStrike && enemyStrike && simultaneousStrikes < NUM_TIMES_TO_SHATTER && !retreated)
            {
                weaponDamaged = true;
                PlaySound("parry.wav", 1.0);
                CustomMessageBox.ShowText("Nobody took damage!","FIGHTING",WARNING_BACK,WARNING_FORE);
                if (world.aurora.attack[1] > enemy.attack[1] && simultaneousStrikes != NUM_TIMES_TO_SHATTER)
                {
                    CustomMessageBox.ShowText("You strike simultaneously and DAMAGE the enemy's weapon!", "FIGHTING", SUCCESS_BACK, SUCCESS_FORE);
                    AddNotification("You strike simultaneously and DAMAGE the enemy's weapon!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Green });
                    enemy.attack[1] = (int)Math.Floor(enemy.attack[1] * SIMULTANEOUS_STRIKE_DAMAGE);

                }
                else if (world.aurora.attack[1] > enemy.attack[1] && simultaneousStrikes == NUM_TIMES_TO_SHATTER)
                {
                    CustomMessageBox.ShowText("You strike simultaneously and SHATTER the enemy's weapon!", "FIGHTING", SUCCESS_BACK, SUCCESS_FORE);
                    AddNotification("You strike simultaneously and SHATTER the enemy's weapon!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.DarkGreen });
                    enemy.attack[1] = (int)Math.Floor(enemy.attack[1] * SHATTER_DAMAGE);

                }
                else if (world.aurora.attack[1] < enemy.attack[1] && simultaneousStrikes != NUM_TIMES_TO_SHATTER)
                {
                    CustomMessageBox.ShowText("You strike simultaneously and DAMAGE your weapon!", "FIGHTING", WARNING_BACK, WARNING_FORE);
                    AddNotification("You strike simultaneously and DAMAGE your weapon!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Red });
                    world.aurora.attack[1] = (int)Math.Floor(world.aurora.attack[1] * SIMULTANEOUS_STRIKE_DAMAGE);

                }
                else if (world.aurora.attack[1] < enemy.attack[1] && simultaneousStrikes == NUM_TIMES_TO_SHATTER)
                {
                    CustomMessageBox.ShowText("You strike simultaneously and SHATTER your weapon!", "FIGHTING", WARNING_BACK, WARNING_FORE);
                    AddNotification("You strike simultaneously and SHATTER your weapon!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.DarkRed });
                    world.aurora.attack[1] = (int)Math.Floor(world.aurora.attack[1] * SHATTER_DAMAGE);
                }
                else
                {
                    CustomMessageBox.ShowText("You strike simultaneously and nobody takes damage", "FIGHTING", WARNING_BACK, WARNING_FORE);
                    AddNotification("You strike simultaneously and nobody takes damage!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Purple});
                }

                simultaneousStrikes += 1;
            }
            else if (auroraStrike && !retreated)
            {
                enemy.health -= world.aurora.attack[1];
                PlaySound("damage.wav", 1.0);
                AddNotification("You hit the enemy!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.DarkGreen });
                
                if (world.spells[0] == 2)
                {
                    int heal = (int)Math.Floor(world.aurora.attack[1] * VAMPIRE_HEAL);
                    PlaySound("heal.wav", 1.0);
                    AddNotification("You heal " + Convert.ToString(heal) + "HP through the " + artefacts[VAMPIRE_INDEX]+"\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Green });
                    world.aurora.health += heal;
                    if (world.aurora.health > world.aurora.maxHealth)
                    {
                        world.aurora.health = world.aurora.maxHealth;
                    }
                }
            }
            if (enemyStrike && !retreated && !auroraStrike)
            {
                world.aurora.health -= enemy.attack[1];
                PlaySound("damage.wav", 1.0);
                AddNotification("The enemy hit you!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.DarkRed });
            }
            else if (!retreated && !enemyStrike && enemyPreparedness==0)
            {
                PlaySound("miss.wav", 0.5);
                AddNotification("The enemy misses!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Green });
            }
            
            if (enemy.health <= 0)
            {
                world.gold += enemy.reward;
                PlaySound("coin.wav", 1.0);
                AddNotification("You found " + Convert.ToString(enemy.reward) + "G on the creature!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Gold });
                fighting = false;
                goblinArtefactUsed = false;
            }
            return fighting;
        }
        public static void DoMaeja()
        {
            /* This function does all of the events relating the user to talking with Maeja.
             * If the user it is currently learning any spells it teaches them to Aurora.
             */
            bool learnt = false;
            for (int i=0;i<4;i++)
            {
                if (world.spells[i] == 1)
                {
                    world.spells[i] = 2;
                    PlaySound("upgrade.wav",1.0);
                    CustomMessageBox.ShowText("Maeja teaches you the secrets of the " + artefacts[i], "MAEJA", SUCCESS_BACK, SUCCESS_FORE);
                    learnt = true;
                    if (i == 1) // If it is the Troll spell
                    {
                        world.aurora.maxHealth += 20;
                        world.aurora.health += 20;
                        CustomMessageBox.ShowText("The magic of the Troll Artefact gives you a bonus 20HP.", "MAEJA", SUCCESS_BACK, SUCCESS_FORE);
                    }
                }
            }
            if (!learnt)
            {
                CustomMessageBox.ShowText("There is nothing for me to teach you!", "MAEJA", WARNING_BACK, WARNING_FORE);
            }

        }
        public static void DoShrine()
        {
            /* This functions does all of the dialogye and generates the spell that the user, Aurora learns.
             * It also does the puzzle for the user to do when doing the shrine.
             */ 
            Shrine shrine = new Shrine();
            bool learning = false;
            int index;
            foreach (var shrine1 in world.shrines)
            {
                if (DistanceBetween(shrine1.pos, world.aurora.pos) == 0)
                {
                    shrine = shrine1;
                }
            }
            foreach (var spell in world.spells)
            {
                if (spell == 1)
                {
                    learning = true;
                }
            }
            if (shrine.solved == 0 && !learning)
            {
                AddNotification("You enter a shrine!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Black });
                int num1 = random.Next(100);
                int num2 = random.Next(100);
                CustomMessageBox.ShowText("Press OK when you are ready. You must enter within 5 seconds to open the secret door!", "SHRINE", NORMAL_BACK, NORMAL_FORE);
                DateTime time1 = DateTime.Now;
                string answer = CustomMessageBox.ShowTextEntry("What is " + Convert.ToString(num1) + " + " + Convert.ToString(num2),"SHRINE");
                DateTime time2 = DateTime.Now;
                double diffInSeconds = (time1 - time2).TotalSeconds;
                if (diffInSeconds<=5 && Convert.ToInt32(answer) == num1+num2) 
                {
                    AddNotification("The door closes behind you and a secret doorway opens. An artefact is glowing in the darkness!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Green });
                    if (world.spells[0] == 2 && world.spells[1] == 2 && world.spells[2] == 2) // Check if it is time to learn Omniscience
                    {
                        index = 3;
                    }
                    else
                    {
                        do
                        {
                            index = random.Next(3);
                        } while (world.spells[index] == 2);
                    }
                    AddNotification("You pick it up and see that it is the " + artefacts[index] + "\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Gold });
                    world.spells[index] = 1;
                    // Find the shrine and set it to solved.
                    for (int i = 0;i<4;i++)
                    {
                        if (DistanceBetween(world.shrines[i].pos, shrine.pos) == 0)
                        {
                            world.shrines[i].solved = 1;
                            break;
                        }
                    }


                }
                else
                {
                    AddNotification("The question vanishes and... nothing happens\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Red });
                }
            }
            else if (learning)
            {
                AddNotification("You see a shrine, but decide to wait until you show Maeja the one you already have.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Red });

            }
            else
            {
                AddNotification("You see a shrine, but realise that it has no artefact in it. Because you already took it.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Red });
            }
        }
        public static void DoVillage() 
        {
            /* This function chooses two people for the user to speak to. It runs the interactions for each one if
             * the user decides to talk to them. It also talks the user with Hawk if he is there.
             */
            // Check if Hawk is at village
            if (DistanceBetween(world.hawkPosition, world.aurora.pos) == 0)
            {
                string message = "HAWK: Hey Aurora! I drew where I think that the Troll King is on your map! ";
                message += "I also draw where the wolves are, I heard that they are hunting you. You should be careful.\n";
                AddNotification(message, new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Green });
                hawkOmniscience = true;
            }
            // Generate two random people from Quester, Hunter and Merchant.
            List<string> NPCs = new List<string>();
            NPCs.Add("Quester");
            NPCs.Add("Merchant");
            NPCs.Add("Hunter");
            var chosen = NPCs.OrderBy(x => random.Next()).Take(2);
            foreach (string person in chosen)
            {
                bool talk = CustomMessageBox.ShowConfirmation("You see a " + person + ". Do you want to talk to them?", "VILLAGE");
                if (talk)
                {
                    if (person == "Quester")
                    {
                        DoQuester();
                    }
                    else if (person == "Merchant")
                    {
                        DoMerchant();
                    }
                    else
                    {
                        DoHunter();
                        AddNotification("You walk back to the village and tell the Hunter how you killed the beast.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Black });
                    }
                }
                AddNotification("You walk away from the " + person + " and keep looking around.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Black });
               
            }
            AddNotification("You don't see anybody else.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Black });

        }
        public static void DoHunter()
        {
            /* This function runs the interaction which Aurora has with a Hunter, including fighting Aurora
             * against a large creature.
             */
            bool entry = CustomMessageBox.ShowConfirmation("Are you sure you want me to find you some nice loot ('YES/'NO')", "HUNTER");
            if (entry)
            {
                CustomMessageBox.ShowText("I hope you find some good loot... and survive.", "HUNTER", NORMAL_BACK, NORMAL_FORE);
                // Create the large creature
                enemy.maxHealth = random.Next(LargeCreatureInfo.MIN_HEALTH, LargeCreatureInfo.MAX_HEALTH);
                enemy.health = enemy.maxHealth;
                enemy.attack = new int[3];
                enemy.attack[0] = random.Next(LargeCreatureInfo.MIN_ACCURACY, LargeCreatureInfo.MAX_ACCURACY);
                enemy.attack[1] = random.Next(LargeCreatureInfo.MIN_POWER, LargeCreatureInfo.MAX_POWER);
                enemy.attack[2] = random.Next(LargeCreatureInfo.MIN_TIME, LargeCreatureInfo.MAX_TIME);
                enemy.reward = (int)Math.Floor(LargeCreatureInfo.REWARD_MUTLIPLIER * enemy.maxHealth);
                // Fight the large creature
                fighting = true;
                AddNotification("You have encountered a LARGE CREATURE!!!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Red });
                
            }
            else
            {
                CustomMessageBox.ShowText("I wouldn't go out there either.", "HUNTER", NORMAL_BACK, NORMAL_FORE);
            }
        }
        public static void DoMerchant() 
        {
            /* This function runs the interaction which Aurora has with any Merchants.
             * This NPC is actually the most complicated who is able to have an attitude towards
             * Aurora depending on her behaviour. This results in different weapon values.
             */
            string weapon = CustomMessageBox.ShowTextEntry("What weapon do you want ('MACE'/'SWORD'/'DAGGER')", "MERCHANT");
            string message = "";
            while (weapon != "MACE" && weapon != "SWORD" && weapon != "DAGGER")
            {
                CustomMessageBox.ShowText("Sorry, but I don't sell that.", "MERCHANT", WARNING_BACK, WARNING_FORE);
                weapon = CustomMessageBox.ShowTextEntry("What weapon do you want ('MACE'/'SWORD'/'DAGGER')", "MERCHANT");
            }
            CustomMessageBox.ShowText("Yes, I can get you one of those.", "MERCHANT", SUCCESS_BACK, SUCCESS_FORE);
            int gold = 0;
            do
            {
                bool doBreak = false;
                string strGold = CustomMessageBox.ShowTextEntry("But first show me your gold (enter a number)", "MERCHANT");
                try
                {
                    gold = Convert.ToInt32(strGold);
                    doBreak = true;
                }
                catch
                {
                    CustomMessageBox.ShowText("Sorry, but you can't see the weapon until you tell me how much gold you'll spend.", "MERCHANT", WARNING_BACK, WARNING_FORE);
                    doBreak = false;
                }
                if (doBreak)
                {
                    break;
                }
            } while (true);
            int[] stats = new int[3];
            int bonus = 0;
            if (gold > world.gold)
            {
                CustomMessageBox.ShowText("Hey! You trying to cheat me? I see " + Convert.ToString(world.gold) + " gold, the rest isn't gold! I hate liars.", "MERCHANT", WARNING_BACK, WARNING_FORE);
                gold = world.gold;
                bonus = -10;
                stats = GenerateWeapon(gold + bonus, weapon);
            }
            else if (gold < world.gold)
            {
                CustomMessageBox.ShowText("I'm sure you got more gold than that. Think you're better than me? I don't like you.", "MERCHANT", WARNING_BACK, WARNING_FORE);
                stats = GenerateWeapon(gold + bonus, weapon);
            }
            else
            {
                CustomMessageBox.ShowText("I love honest people. I'll go get a nice weapon for you.", "MERCHANT", SUCCESS_BACK, SUCCESS_FORE);
                bonus = 10;
                stats = GenerateWeapon(gold + bonus, weapon);
            }
            CustomMessageBox.ShowText("I found a nice weapon for you.", "MERCHANT", NORMAL_BACK, NORMAL_FORE);
            message = "I think it could hit " + Convert.ToString(stats[0]);
            message += "% of shots, with " + Convert.ToString(stats[1]);
            message += " strength. Although it would take you " + Convert.ToString(stats[2]);
            message += " seconds to prepare each shot.";
            CustomMessageBox.ShowText(message, "MERCHANT", NORMAL_BACK, NORMAL_FORE);
            do
            {
                try
                {
                    bool buy = CustomMessageBox.ShowConfirmation("Would you like to buy this weapon ('YES'/'NO')", "MERCHANT");
                    if (buy)
                    {
                        if (bonus == 10)
                        {
                            CustomMessageBox.ShowText("Here is your weapon. One of my best.", "MERCHANT", NORMAL_BACK, NORMAL_FORE);
                            world.aurora.attack = stats;
                            world.gold -= (gold);
                        }
                        else
                        {
                            CustomMessageBox.ShowText("Here it is. A better weapon than you deserve.", "MERCHANT", NORMAL_BACK, NORMAL_FORE);
                            world.aurora.attack = stats;
                            world.gold -= (gold);
                        }
                    }
                    else
                    {
                        if (bonus == 10)
                        {
                            CustomMessageBox.ShowText("A shame, you seemed like a nice fella.", "MERCHANT", NORMAL_BACK, NORMAL_FORE);
                        }
                        else
                        {
                            CustomMessageBox.ShowText("Well go on and get out! I have more respectful customers to attend to.", "MERCHANT", NORMAL_BACK, NORMAL_FORE);
                        }
                    }
                    break;
                }
                catch
                {
                    CustomMessageBox.ShowText("Invalid entry!", "CONFIRMATION ERROR", WARNING_BACK, WARNING_FORE);
                }
                
            } while (true);
        }
        public static void DoQuester()
        {
            /* This function runs the interaction that Aurora has with a Quester. This includes choosing a location and
             * changing the value of the world variable to include this location.
             */
            if (world.questPosition.row != -1 && world.questPosition.col != -1)
            {
                CustomMessageBox.ShowText("Hey, wait a moment. You're that guy! Heard about you, you haven't even done your last job, I don't trust you to do my job.", "QUESTER", WARNING_BACK, WARNING_FORE);

            }
            else
            {
                bool entry = CustomMessageBox.ShowConfirmation("Could you go to a place and do something for me?", "QUESTER");
                if (entry)
                {
                    CustomMessageBox.ShowText("I have added the location to your map. I marked it with a gold coin", "QUESTER", SUCCESS_BACK, SUCCESS_FORE);
                    world.questPosition.col = random.Next(world.cols - 1);
                    world.questPosition.row = random.Next(world.rows - 1);
                }
                else
                {
                    CustomMessageBox.ShowText("Well that is a shame. I guess I will look for someone else.", "QUESTER", NORMAL_BACK, NORMAL_FORE);
                }
            }
        }
        public static void DoMessages() 
        {
            /* This function runs the messages which the user will receive at the start of each turn.
             * This includes the location of the Troll King relative to Aurora and any proximity alerts to the wolves.
             */
            if (Math.Abs(world.aurora.pos.row - world.trollKing.pos.row) > Math.Abs(world.aurora.pos.col - world.trollKing.pos.col)) {
                if (world.aurora.pos.row > world.trollKing.pos.row)
                {
                    AddNotification("The Troll King is to the NORTH.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Blue });
                    
                }
                else
                {
                    AddNotification("The Troll King is to the SOUTH.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Blue });
                    
                }
            }
            else
            {
                if (world.aurora.pos.col > world.trollKing.pos.col)
                {
                    AddNotification("The Troll King is to the WEST.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Blue });
                    
                }
                else
                {
                    AddNotification("The Troll King is to the EAST.\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.Blue });
                    
                }
            }
            if (DistanceBetween(world.aurora.pos,world.wolves.pos)<=2)
            {
                CustomMessageBox.ShowText("You hear the howl of wolves.", "MESSAGE", WARNING_BACK, WARNING_FORE);
            }

        }

        // GAME WORLD //
        public static void GenerateRandomWorld(int size)
        {
            /* This function creates a random world.
             */

            // Create general world variables
            world.rows = size;
            world.cols = world.rows; // make it a square
            world.numVillages = random.Next(WorldInfo.MIN_NUM_VILLAGES, WorldInfo.MAX_NUM_VILLAGES);
            world.numShrines = WorldInfo.NUM_SHRINES;

            // Create village positions
            Position tempPosition = new Position();
            world.villagePositions = new Position[world.numVillages];
            for (int i = 0; i < world.numVillages; i++)
            {
                do
                {
                    tempPosition.row = random.Next(world.rows - 1);
                    tempPosition.col = random.Next(world.cols - 1);
                } while (world.villagePositions.Contains(tempPosition));
                world.villagePositions[i] = tempPosition;
            }
            
            // Create shrine positions
            world.shrines = new Shrine[world.numShrines];
            Position[] shrinePositions = new Position[world.numShrines];
            for (int i = 0; i < world.numShrines; i++)
            {
                do
                {
                    tempPosition.row = random.Next(world.rows - 1);
                    tempPosition.col = random.Next(world.cols - 1);

                } while (world.villagePositions.Contains(tempPosition) || shrinePositions.Contains(tempPosition));
                shrinePositions[i] = tempPosition;
                world.shrines[i] = new Shrine();
                world.shrines[i].pos = tempPosition;
                world.shrines[i].solved = 0;
            }

            // Create wolves
            world.wolves.maxHealth = random.Next(WolvesInfo.MIN_HEALTH, WolvesInfo.MAX_HEALTH);
            world.wolves.health = world.wolves.maxHealth;
            world.wolves.attack = new int[3];
            world.wolves.attack[0] = random.Next(WolvesInfo.MIN_ACCURACY, WolvesInfo.MAX_ACCURACY);
            world.wolves.attack[1] = random.Next(WolvesInfo.MIN_POWER, WolvesInfo.MAX_POWER);
            world.wolves.attack[2] = random.Next(WolvesInfo.MIN_TIME, WolvesInfo.MAX_TIME);
            do
            {
                tempPosition.row = random.Next(world.rows - 1);
                tempPosition.col = random.Next(world.cols - 1);

            } while (world.villagePositions.Contains(tempPosition) || shrinePositions.Contains(tempPosition));
            world.wolves.pos = tempPosition;

            // Create Troll King
            world.trollKing.maxHealth = random.Next(TrollKingInfo.MIN_HEALTH, TrollKingInfo.MAX_HEALTH);
            world.trollKing.health = world.trollKing.maxHealth;
            world.trollKing.attack = new int[3];
            world.trollKing.attack[0] = random.Next(TrollKingInfo.MIN_ACCURACY, TrollKingInfo.MAX_ACCURACY);
            world.trollKing.attack[1] = random.Next(TrollKingInfo.MIN_POWER, TrollKingInfo.MAX_POWER);
            world.trollKing.attack[2] = random.Next(TrollKingInfo.MIN_TIME, TrollKingInfo.MAX_TIME);
            do
            {
                tempPosition.row = random.Next(world.rows - 1);
                tempPosition.col = random.Next(world.cols - 1);

            } while (world.villagePositions.Contains(tempPosition) || shrinePositions.Contains(tempPosition) || DistanceBetween(world.wolves.pos, tempPosition) == 0);
            world.trollKing.pos = tempPosition;

            // Create Aurora
            world.aurora.attack[0] = AuroraInfo.ACCURACY;
            world.aurora.attack[1] = AuroraInfo.POWER;
            world.aurora.attack[2] = AuroraInfo.TIME;
            world.aurora.maxHealth = AuroraInfo.HEALTH;
            world.aurora.health = world.aurora.maxHealth;

            do
            {
                tempPosition.row = random.Next(world.rows - 1);
                tempPosition.col = random.Next(world.cols - 1);

            } while (world.villagePositions.Contains(tempPosition) || shrinePositions.Contains(tempPosition) || DistanceBetween(world.wolves.pos, tempPosition) == 0 || DistanceBetween(world.trollKing.pos, tempPosition) == 0);
            world.aurora.pos = tempPosition;

            // Create Maeja
            do
            {
                tempPosition.row = random.Next(world.rows - 1);
                tempPosition.col = random.Next(world.cols - 1);

            } while (world.villagePositions.Contains(tempPosition) || shrinePositions.Contains(tempPosition) || DistanceBetween(world.wolves.pos, tempPosition) == 0 || DistanceBetween(world.trollKing.pos, tempPosition) == 0 || DistanceBetween(world.aurora.pos,tempPosition) == 0);
            world.maejaPosition = tempPosition;
            // Create Hawk
            world.hawkPosition = world.villagePositions[random.Next(world.numVillages)];
            // Create destroyed villages
            world.destroyedVillages = new List<Position>();
            // Set Misc. values
            world.spells = new int[] { 0, 0, 0, 0 };
            world.turnNum = 0;
            world.questPosition = new Position();
            world.questPosition.row = -1;
            world.questPosition.col = -1;
            // Generate the first target for the Troll King
            world.targetVillagePosition = world.villagePositions[random.Next(world.villagePositions.Length - 1)];
            ICON_WIDTH = (int)Math.Floor(MAP_WIDTH / Convert.ToDouble(world.cols));
            ICON_HEIGHT = (int)Math.Floor(MAP_HEIGHT / Convert.ToDouble(world.rows));
            ICON_WIDTH = Math.Min(ICON_HEIGHT, ICON_WIDTH);
            ICON_HEIGHT = Math.Min(ICON_HEIGHT, ICON_WIDTH);
            MAP_HEIGHT = ICON_HEIGHT * world.rows;
            MAP_WIDTH = ICON_WIDTH * world.cols;


        }
        public static void UpdateMapIcons()
        {
            EMPTY_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/empty-icon.png");
            AURORA_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/aurora-icon.png");
            TROLL_KING_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/troll-king-icon.png");
            WOLVES_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/wolves-icon.png");
            INTACT_VILLAGE_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/intact-village-icon.png");
            DESTROYED_VILLAGE_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/destroyed-village-icon.png");
            UNSOLVED_SHRINE_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/unsolved-shrine-icon.png");
            SOLVED_SHRINE_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/solved-shrine-icon.png");
            MAEJA_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/maeja-icon.png");
            HAWK_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/hawk-icon.png");
            QUEST_ICON = Image.FromFile("data/resource packs/" + RESOURCE_PACK + "/quest-icon.png");
        }
        public static ImageSource CalculateMap()
        {
            /* This function is responsible for creating the map. The map is actually one large
                * picture, but this function gets each individual smaller image (icon) and essentially
                * "squashes" the images together to make the larger map. This function is used every
                * single turn to update what the map looks like.
                */

            List<List<Image>> map = new List<List<Image>>(world.rows);

            // Create the empty world
            for (int r = 0; r < world.rows; r++)
            {
                map.Add(new List<Image>(world.cols));  // Add images to list
                for (int c = 0; c < world.cols; c++)
                {
                    map[r].Add(EMPTY_ICON); // Add images to list
                }

            }

            try
            {
                // Add icons to the map
                foreach (Position pos in world.villagePositions) // Add the intact villages
                {
                    map[pos.row][pos.col] = INTACT_VILLAGE_ICON;
                }
                foreach (Position pos in world.destroyedVillages) // Add the destroyed villages 
                {
                    if (pos.row != -1 && pos.col != -1) // Makes everything way easier by making a -1 -1 default first destroyed village (if nothing else is destroyed).
                    {
                        map[pos.row][pos.col] = DESTROYED_VILLAGE_ICON;
                    }
                }
                foreach (Shrine shrine in world.shrines)
                {
                    if (shrine.solved == 0)
                    {
                        map[shrine.pos.row][shrine.pos.col] = UNSOLVED_SHRINE_ICON;
                    }
                    else
                    {
                        map[shrine.pos.row][shrine.pos.col] = SOLVED_SHRINE_ICON;
                    }
                }
                map[world.aurora.pos.row][world.aurora.pos.col] = AURORA_ICON; // Add Aurora's icon
                map[world.maejaPosition.row][world.maejaPosition.col] = MAEJA_ICON; // Add Maeja's icon
                map[world.hawkPosition.row][world.hawkPosition.col] = HAWK_ICON; // Add Hawk's icon


                if (world.spells[OMNISCIENCE_INDEX] == SPELL_LEARNT || hawkOmniscience) // Add Wolves and Troll King icons if Omniscience has been learnt or Hawk told her where they are
                {
                    map[world.wolves.pos.row][world.wolves.pos.col] = WOLVES_ICON;
                    map[world.trollKing.pos.row][world.trollKing.pos.col] = TROLL_KING_ICON;


                }
                if (world.questPosition.row != -1 && world.questPosition.col != -1) // If there is a quest add its icon
                {
                    map[world.questPosition.row][world.questPosition.col] = QUEST_ICON;
                }
            }
            catch { }



            // your code
            ImageSource source;
            using (Bitmap bitmapMap = new Bitmap(world.rows * ICON_WIDTH, world.cols * ICON_HEIGHT)) // this is to prevent a memory leak
            {
                Graphics g = Graphics.FromImage(bitmapMap);
                // Add images to the bitmap
                for (int r = 0; r < world.rows; r++)
                {
                    for (int c = 0; c < world.cols; c++)
                    {
                        g.DrawImage(map[r][c], (c) * ICON_WIDTH, (r) * ICON_HEIGHT, ICON_WIDTH, ICON_HEIGHT);
                    }
                }
                IntPtr hBitmap = bitmapMap.GetHbitmap();

                try
                {
                    source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(hBitmap);
                }
            }
            // Return the new ImageSource.
            return source;

        }

        // MISC IMPORTANT //
        public static Creature CharacterToCreature(Character character)
        {
            /* This function converts variables of type Creature to Character.
             * Note that there is data loss during the conversion (i.e. the position).
             * This will be used only for the troll king and the wolves.
             */
            Creature creature = new Creature();
            creature.maxHealth = character.maxHealth;
            creature.health = character.health;
            creature.attack = character.attack;
            // The wolves' reward multiplier is used because only the wolves can give rewards (since the troll king cannot)
            // and the small and large creatures won't use this function.
            creature.reward = (int)Math.Floor(creature.attack[1] * WolvesInfo.REWARD_MULTIPLIER);
            return creature;
        }
        public static Character CreatureToCharacter(Creature updated, Character old)
        {
            /* This function converts a creature into a character. Data loss is repaired using past
             * values from the original character. This is only used for the Troll King and the wolves.
             */
            old.health = updated.health;
            // The reason why the attack is not updated, is because the wolves and troll king cannot have
            // their weapons permanently broken.
            return old;
        }
        public static int DistanceBetween(Position pos1, Position pos2)
        {
            /* This checks if two positions are equal. It returns true if they are and
             * false if they are not equal.
             */
            int distance = 0;
            distance += Math.Abs(pos1.row - pos2.row);
            distance += Math.Abs(pos1.col - pos2.col);
            return distance;
        }
        public static bool ProbabilityRandom(double probability)
        {
            /* This function returns true or false depending on the probability.
             * It returns true with the given probability and false otherwise.
             */
            if (probability >= random.NextDouble())
            {
                return true;
            }
            return false;
        }
        public static int[] GenerateWeapon(int gold, string type)
        {

            int[] stats = new int[3];
            if (gold <= 0)
            {
                stats[0] = 0;
                stats[1] = 0;
                stats[2] = 0;
                return stats;
            }
            switch (type) {
                case "MACE":
                    stats[0] = (int)Math.Floor(gold * MaceInfo.ACCURACY_MULTIPLIER);
                    if (stats[0] > 100)
                    {
                        stats[0] = 100;
                    }
                    stats[1] = (int)Math.Floor(gold * MaceInfo.POWER_MULTIPLIER);
                    stats[2] = (int)Math.Floor(MaceInfo.TIME_QUOTIENT / Convert.ToDouble(gold));
                    break;
                case "SWORD":
                    stats[0] = (int)Math.Floor(gold * SwordInfo.ACCURACY_MULTIPLIER);
                    if (stats[0] > 100)
                    {
                        stats[0] = 100;
                    }
                    stats[1] = (int)Math.Floor(gold * SwordInfo.POWER_MULTIPLIER);
                    stats[2] = (int)Math.Floor(SwordInfo.TIME_QUOTIENT / Convert.ToDouble(gold));

                    break;
                case "DAGGER":
                    stats[0] = (int)Math.Floor(gold * DaggerInfo.ACCURACY_MULTIPLIER);
                    if (stats[0] > 100)
                    {
                        stats[0] = 100;
                    }
                    stats[1] = (int)Math.Floor(gold * DaggerInfo.POWER_MULTIPLIER);
                    stats[2] = (int)Math.Floor(DaggerInfo.TIME_QUOTIENT / Convert.ToDouble(gold));

                    break;
                
            }
            // Do some balancing of the stats (so the weapon isn't ridiculously overpowered or underpowered)
            if (stats[2] == 0) // Cause you're not allowed to have this (too OP)
            {
                stats[2] = 1;
            }
            if (stats[0] < 30)
            {
                stats[0] = 30;
            }
            if (stats[1] < 5)
            {
                stats[1] = 5;
            }
            if (stats[2] > 6)
            {
                stats[2] = 6;
            }
            return stats;
        }
        public static void Save(string filename)
        {
            /* This function writes all of the important values to a save file so that the user 
             * can load it later to play the game. It is given a value for a filename.
             */



            // Create a string array 
            string[] lines = new string[23];
            lines[0] = Convert.ToString(world.rows) + " " + Convert.ToString(world.cols) + " " + Convert.ToString(world.turnNum) + " " + Convert.ToString(world.numVillages) + " " + Convert.ToString(world.numShrines);
            lines[1] = Convert.ToString(world.hawkPosition.row) + " " + Convert.ToString(world.hawkPosition.col);
            lines[2] = Convert.ToString(world.maejaPosition.row) + " " + Convert.ToString(world.maejaPosition.col);
            lines[3] = Convert.ToString(world.questPosition.row) + " " + Convert.ToString(world.questPosition.col);
            lines[4] = Convert.ToString(world.aurora.pos.row) + " " + Convert.ToString(world.aurora.pos.col);
            lines[5] = Convert.ToString(world.aurora.health);
            lines[6] = Convert.ToString(world.aurora.maxHealth);
            lines[7] = Convert.ToString(world.aurora.attack[0]) + " " + Convert.ToString(world.aurora.attack[1]) + " " + Convert.ToString(world.aurora.attack[2]);
            lines[8] = Convert.ToString(world.spells[0]) + " " + Convert.ToString(world.spells[1]) + " " + Convert.ToString(world.spells[2]) + " " + Convert.ToString(world.spells[3]);
            lines[9] = Convert.ToString(world.gold);
            lines[10] = Convert.ToString(world.trollKing.pos.row) + " " + Convert.ToString(world.trollKing.pos.col);
            lines[11] = Convert.ToString(world.trollKing.health);
            lines[12] = Convert.ToString(world.trollKing.maxHealth);
            lines[13] = Convert.ToString(world.trollKing.attack[0]) + " " + Convert.ToString(world.trollKing.attack[1]) + " " + Convert.ToString(world.trollKing.attack[2]);
            lines[14] = Convert.ToString(world.wolves.pos.row) + " " + Convert.ToString(world.wolves.pos.col);
            lines[15] = Convert.ToString(world.wolves.health);
            lines[16] = Convert.ToString(world.wolves.maxHealth);
            lines[17] = Convert.ToString(world.wolves.attack[0]) + " " + Convert.ToString(world.wolves.attack[1]) + " " + Convert.ToString(world.wolves.attack[2]);
            for (int i = 0; i < world.villagePositions.Count() - 1; i++)
            {
                lines[18] += Convert.ToString(world.villagePositions[i].row) + " " + Convert.ToString(world.villagePositions[i].col) + ",";
            }
            lines[18] += Convert.ToString(world.villagePositions[world.villagePositions.Count() - 1].row) + " " + Convert.ToString(world.villagePositions[world.villagePositions.Count() - 1].col);
            try
            {
                for (int i = 0; i < world.destroyedVillages.Count() - 1; i++)
                {
                    lines[19] += Convert.ToString(world.destroyedVillages[i].row) + " " + Convert.ToString(world.destroyedVillages[i].col) + ",";
                }
                lines[19] += Convert.ToString(world.destroyedVillages[world.destroyedVillages.Count() - 1].row) + " " + Convert.ToString(world.destroyedVillages[world.destroyedVillages.Count() - 1].col);
            }
            catch { lines[19] = ""; }
                for (int i = 0; i < world.shrines.Count() - 1; i++)
            {
                lines[20] += Convert.ToString(world.shrines[i].pos.row) + " " + Convert.ToString(world.shrines[i].pos.col) + ",";
            }
            lines[20] += Convert.ToString(world.shrines[world.shrines.Count() - 1].pos.row) + " " + Convert.ToString(world.shrines[world.shrines.Count() - 1].pos.col);
            for (int i = 0; i < world.shrines.Count() - 1; i++)
            {
                lines[21] += Convert.ToString(world.shrines[i].solved);
                lines[21] += " ";
            }
            lines[21] += Convert.ToString(world.shrines[world.shrines.Count() - 1].solved);
            lines[22] = Convert.ToString(world.numFightTurns);

            // Delete any existing files
            if (File.Exists("saves/" + filename))
            {
                File.Delete("saves/" + filename);
            }
            // Write to the file
            using (StreamWriter sw = new StreamWriter("saves/" + filename, true))
            {
                foreach (var str in lines)
                {
                    sw.WriteLine(str);
                }
            }


        }
        public static void Load(string filename)
        {
            /* This function is very important. It is used by the user at the start of the program to start the game.
             * It opens a load file and assigns the values saved in the file to the world gamestate.
             */

            // Read in the game state from the file
            string gameState = File.ReadAllText("saves/" + filename, Encoding.UTF8); // Read in all contents from file
            // Since in windows a newline is represented by \r\n, Regex is used to replace the \r characters (because Regex is cool).
            // Actually, it is because '.Split' only works with chars, so it cannot split at an occurence of "/r/n"... since that is a string...

            gameState = Regex.Replace(gameState, @"\r", "");
            world.rows = Convert.ToInt32(gameState.Split('\n')[0].Split(' ')[0]); // Find row value
            world.cols = Convert.ToInt32(gameState.Split('\n')[0].Split(' ')[1]); // Find column value
            world.turnNum = Convert.ToInt32(gameState.Split('\n')[0].Split(' ')[2]); // Find turn number
            world.numVillages = Convert.ToInt32(gameState.Split('\n')[0].Split(' ')[3]);
            world.numShrines = Convert.ToInt32(gameState.Split('\n')[0].Split(' ')[2]);

            world.hawkPosition.row = Convert.ToInt32(gameState.Split('\n')[1].Split(' ')[0]);
            world.hawkPosition.col = Convert.ToInt32(gameState.Split('\n')[1].Split(' ')[1]);

            world.maejaPosition.row = Convert.ToInt32(gameState.Split('\n')[2].Split(' ')[0]);
            world.maejaPosition.col = Convert.ToInt32(gameState.Split('\n')[2].Split(' ')[1]);

            world.questPosition.row = Convert.ToInt32(gameState.Split('\n')[3].Split(' ')[0]);
            world.questPosition.col = Convert.ToInt32(gameState.Split('\n')[3].Split(' ')[1]);


            world.aurora.pos.row = Convert.ToInt32(gameState.Split('\n')[4].Split(' ')[0]); // Find aurora's row position
            world.aurora.pos.col = Convert.ToInt32(gameState.Split('\n')[4].Split(' ')[1]); // Find aurora's col position
            world.aurora.health = Convert.ToInt32(gameState.Split('\n')[5]); // Find aurora's health
            world.aurora.maxHealth = Convert.ToInt32(gameState.Split('\n')[6]); // Find aurora's maximum health
            world.aurora.attack = Array.ConvertAll(gameState.Split('\n')[7].Split(' '), s => int.Parse(s)); // Find aurora's attack stats
            world.spells = Array.ConvertAll(gameState.Split('\n')[8].Split(' '), s => int.Parse(s)); // Find aurora's collected spells
            world.gold = Convert.ToInt32(gameState.Split('\n')[9]); // Find aurora's gold
            world.trollKing.pos.row = Convert.ToInt32(gameState.Split('\n')[10].Split(' ')[0]);
            world.trollKing.pos.col = Convert.ToInt32(gameState.Split('\n')[10].Split(' ')[1]);
            world.trollKing.health = Convert.ToInt32(gameState.Split('\n')[11]);
            world.trollKing.maxHealth = Convert.ToInt32(gameState.Split('\n')[12]);
            world.trollKing.attack = Array.ConvertAll(gameState.Split('\n')[13].Split(' '), s => int.Parse(s));
            world.wolves.pos.row = Convert.ToInt32(gameState.Split('\n')[14].Split(' ')[0]);
            world.wolves.pos.col = Convert.ToInt32(gameState.Split('\n')[14].Split(' ')[1]);
            world.wolves.health = Convert.ToInt32(gameState.Split('\n')[15]);
            world.wolves.maxHealth = Convert.ToInt32(gameState.Split('\n')[16]);
            world.wolves.attack = Array.ConvertAll(gameState.Split('\n')[17].Split(' '), s => int.Parse(s));
            // Get village positions
            string[] tempArray = Array.ConvertAll(gameState.Split('\n')[18].Split(','), s => s);
            world.villagePositions = new Position[tempArray.Length]; // Initialise world.villagePositions
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] != "") // Doesn't actually need this line (but just a precaution since another block needed it).
                {
                    world.villagePositions[i].row = Convert.ToInt32(tempArray[i].Split(' ')[0]);
                    world.villagePositions[i].col = Convert.ToInt32(tempArray[i].Split(' ')[1]);
                }
            }
            // Get destroyed village positions
            world.destroyedVillages = new List<Position>(); // Initialise world.destroyedVillages
                tempArray = Array.ConvertAll(gameState.Split('\n')[19].Split(','), s => s);


                Position tempPos = new Position();
            for (int i = 0; i < tempArray.Length; i++)
            {
                // This block of code was the one which needed the line below (mentioned near here),
                // since sometimes the input will be empty when no villages have been destroyed.
                if (tempArray[i] != "") // Splitting empty string results in "" (so it must be skipped).

                {
                    tempPos.row = Convert.ToInt32(tempArray[i].Split(' ')[0]);
                    tempPos.col = Convert.ToInt32(tempArray[i].Split(' ')[1]);
                    world.destroyedVillages.Add(tempPos);
                }
            }
            // Generate the first target for the Troll King
            world.targetVillagePosition = world.villagePositions[random.Next(world.villagePositions.Length-1)];

            // Get shrine positions
            tempArray = Array.ConvertAll(gameState.Split('\n')[20].Split(','), s => s);
            world.shrines = new Shrine[tempArray.Length]; // Initialise world.shrines
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] != "") // Same idea... doesn't actually need it, but just a precaution.
                {
                    world.shrines[i].pos.row = Convert.ToInt32(tempArray[i].Split(' ')[0]);
                    world.shrines[i].pos.col = Convert.ToInt32(tempArray[i].Split(' ')[1]);
                }
            }
            // Get whether the shrines are solved
            tempArray = Array.ConvertAll(gameState.Split('\n')[21].Split(' '), s => s);
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (tempArray[i] != "")
                {
                    world.shrines[i].solved = Convert.ToInt32(tempArray[i]);
                }
            }
            world.numFightTurns = Convert.ToInt32(gameState.Split('\n')[22]);



            ICON_WIDTH = (int)Math.Floor(MAP_WIDTH / Convert.ToDouble(world.cols));
            ICON_HEIGHT = (int)Math.Floor(MAP_HEIGHT / Convert.ToDouble(world.rows));
            ICON_WIDTH = Math.Min(ICON_HEIGHT, ICON_WIDTH);
            ICON_HEIGHT = Math.Min(ICON_HEIGHT, ICON_WIDTH);
            MAP_HEIGHT = ICON_HEIGHT * world.rows;
            MAP_WIDTH = ICON_WIDTH * world.cols;
        }
        public static void GenerateDefaultValues()
        {
            /* This function generates default values to allow the update timer to work with variables not actually created yet.
             */
            world.rows = 1;
            world.cols = 1;
            world.aurora.attack = new int[3];
            world.aurora.attack[0] = 0;
            world.aurora.attack[1] = 0;
            world.aurora.attack[2] = 0;
            world.aurora.health = 1;
            world.aurora.maxHealth = 1;
            world.trollKing.maxHealth = 1;
            world.trollKing.health = 1;
            world.wolves.maxHealth = 1;
            world.wolves.health = 1;
            world.turnNum = 0;
            world.numFightTurns = 0;


        }
        public static int CalculateScore()
        {
            /* This function calculates the score based on a number of variables.
             * It also writes this breakdown to the console.
             */
            Console.Clear();
            Console.WriteLine("SCORE BREAKDOWN:");
            int score = 0;
            int addition;
            addition = world.numFightTurns * 1;
            Console.WriteLine("Time Spent Fighting: " + Convert.ToString(addition));
            score += addition;
            addition = world.turnNum * 10;
            Console.WriteLine("Total Number of Turns: " + Convert.ToString(addition));
            score += addition;
            addition = (world.aurora.maxHealth - world.aurora.health) * 5;
            Console.WriteLine("Aurora's Damage Taken: " + Convert.ToString(addition));
            score += addition;
            addition = world.aurora.attack[0] * (1);
            Console.WriteLine("Aurora's Accuracy: " + Convert.ToString(addition));
            score += addition;
            addition = world.aurora.attack[1] * (1);
            Console.WriteLine("Aurora's Power: " + Convert.ToString(addition));
            score += addition;
            addition = world.trollKing.maxHealth * (-1);
            Console.WriteLine("Troll King's Max Health: " + Convert.ToString(addition));
            score += addition;
            addition = world.trollKing.attack[0] * (-1);
            Console.WriteLine("Troll King's Accuracy: " + Convert.ToString(addition));
            score += addition;
            addition = world.trollKing.attack[1] * (-1);
            Console.WriteLine("Troll King's Power: " + Convert.ToString(addition));
            score += addition;
            addition = world.gold * (-1);
            Console.WriteLine("Final amount of gold: " + Convert.ToString(addition));
            score += addition;
            
            Console.WriteLine("\nFinal score: " + Convert.ToString(score));
            Console.WriteLine("\nP.S. The lower the score the better you did.");

            return score;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject); // add this function to dispose variables to prevent memory leaks



        // FANCY FUNCTIONS //
        public static void PrettyPrint(string text, int delay)
        {

            /* This function prints text in a cool way.
             * It simply prints each character with a slight delay.
             * This creates a cool effect.
             */
            foreach (char c in text)
            {
                Console.Write(c);

                if (Keyboard.IsKeyDown(StringToKey("Right"))) // If RIGHT ARROW is being pressed
                {
                    Thread.Sleep(delay / 10); // Short delay
                }
                else if (c == '\n')
                {
                    Thread.Sleep(1000); // Dramatic pause
                }
                else
                {
                    Thread.Sleep(delay); // Long delay
                }


            }
            Console.WriteLine();
        }
        public static void AddNotification(string text, DependencyProperty[] properties, object[] values)
        {
            /* This function "zips" properties with values (properties[i] goes with values[i]).
             * It also adds the FormattedText to the notifications queue.
             */
            FormattedText formattedText = new FormattedText();
            formattedText.text = text;
            formattedText.textProperties = new List<TextProperty>();
            TextProperty textProperty = new TextProperty();
            for (int i = 0; i < properties.Length; i++)
            {
                textProperty = new TextProperty();
                textProperty.property = properties[i];
                textProperty.value = values[i];
                formattedText.textProperties.Add(textProperty);
            }
            notificationsQueue.Add(formattedText); // Add the formattedText to the notifications queue
        }
        public static void FlushKeyboard() 
        {
            /* This function removes the input buffer which is important in some cases.
             */
            while (Console.KeyAvailable == true)
            {
                Console.ReadKey(true);
            }
        }
        public static void UpdateBackgroundMusic()
        {
            /* This function changes the background music track in the case of a different situation.
             */
            // change the sound tracks if necessary
            if (backgroundMusicTrack == "normal" && fighting)
            {
                backgroundMusic.Stop();
                backgroundMusic = new SoundPlayer(@"data/audio/background2.wav");
                backgroundMusic.PlayLooping();
                backgroundMusicTrack = "fighting";
            }
            if (backgroundMusicTrack == "fighting" && !fighting)
            {
                backgroundMusic.Stop();
                backgroundMusic = new SoundPlayer(@"data/audio/background1.wav");
                backgroundMusic.PlayLooping();
                backgroundMusicTrack = "normal";
            }
        }
        public static Key StringToKey(string keyString)
        {
            /* This function converts string to key objects (i.e. keys on a keyboard)
             */
            KeyConverter k = new KeyConverter();
            Key keyObject = (Key)k.ConvertFromString(keyString);
            return keyObject;
        }
        public static void PlaySound(string filename, double volume)
        {
            MediaPlayer myPlayer = new MediaPlayer();
            myPlayer.Open(new System.Uri("data/audio/"+filename, UriKind.Relative));
            myPlayer.Volume = volume;
            myPlayer.Play();
        }
        public static BitmapImage LoadImageAsBitmap(string filepath)
        {
            /* This function loads images from their filepath as bitmaps
             */
            // Create source
            BitmapImage bitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(filepath,UriKind.Relative); 
            bitmapImage.EndInit();
            return bitmapImage;
        }

        // GLOBAL VARIABLES //
        // Overall vars
        
        public static World world;
        public static Random random = new Random(); // Create a random object which is used for all random stuff
        public static System.Windows.Application NormalUI = new System.Windows.Application(); // Create a new application (WPF)
        public static SoundPlayer backgroundMusic = new SoundPlayer(@"data/audio/background1.wav");
        public static string backgroundMusicTrack = "normal";
        public static bool firstUIUpdate = false;

        // Game vars
        public static bool gameOver = false; // This is checked each turn to make sure that the game is not over
        public static string[] artefacts = { "Vampire Artefact", "Troll Artefact", "Goblin Artefact", "Omniscience Artefact" };
        public static string moveAction = "[MOVE ACTION]"; // Changes based on what button the user presses
        public static string fightAction = "[FIGHT ACTION]"; // Also changes based on what button the user presses
        public static int auroraPreparedness = 0;
        public static int enemyPreparedness = 0;
        public static Creature enemy = new Creature();
        public static int tick = 0;
        public static bool fighting = false;
        public static bool fightingTrollKing = false;
        public static bool fightingWolves = false;
        public static bool goblinArtefactUsed = false;
        public static bool weaponDamaged = false;
        public static bool hawkOmniscience = false;
        public static List<FormattedText> notificationsQueue = new List<FormattedText>(); // This is the text which still needs to be added to the notifications

        
        // DATA STRUCTURES //
        public struct World
        {
            // General world values
            public int rows;
            public int cols;
            public int turnNum;
            public int numVillages;
            public int numShrines;
            public int numFightTurns;
            // Locations
            public Position questPosition;
            public Position maejaPosition;
            public Position hawkPosition;
            public Position[] villagePositions;
            public List<Position> destroyedVillages;
            public Shrine[] shrines;
            public Position targetVillagePosition;
            // Characters
            public Character aurora;
            public Character trollKing;
            public Character wolves;
            // Aurora-specific values
            public int gold;
            public int[] spells;
        }
        public struct Character
        {
            public Position pos;
            public int health;
            public int maxHealth;
            public int[] attack;
        }
        public struct Creature
        {
            public int health;
            public int maxHealth;
            public int[] attack;
            public int reward;
        }
        public struct Shrine
        {
            public Position pos;
            public int solved;
        }
        public struct Position
        {
            public int row;
            public int col;
        }
        public struct FormattedText
        {
            public string text;
            public List<TextProperty> textProperties;
        }
        public struct TextProperty
        {
            public DependencyProperty property;
            public object value;
        }

    }
    // OTHER CLASSES SEPARATE FROM "Program".
    public static class CustomMessageBox 
    {
        /* A class for custom message boxes... cause they're better than the normies.
         * It has definitions for different types of message boxes including:
         * message boxes with a text entry.
         * message boxes with confirmations.
         * message boxes themes.
         * 
         */

        public static bool ShowConfirmation(string text,string caption)
        {
            /* This function creates a message box with a YES and NO button and returns which one the user clicked.
             */
            Console.WriteLine(text);
            Form messageBox = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximumSize = new System.Drawing.Size(900, 900),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            // Create the components
            Label textLabel = new Label() { Font = new Font("Calibri", 12), Left = 0, Top = 100, Text = text, MaximumSize = new System.Drawing.Size(900, 900), AutoSize = true };
            Button yes = new Button() { Font = new Font("Calibri", 12), Text = "YES", Left = 0, MaximumSize = new System.Drawing.Size(900, 900), Top = 0, DialogResult = DialogResult.Yes, AutoSize = true };
            Button no = new Button() { Font = new Font("Calibri", 12), Text = "NO", Left = 150, MaximumSize = new System.Drawing.Size(900, 900), Top = 0, DialogResult = DialogResult.No, AutoSize = true };
            yes.Click += (sender, e) => { messageBox.Close(); };
            no.Click += (sender, e) => { messageBox.Close(); };
            // Add the components to the form
            messageBox.Controls.Add(yes);
            messageBox.Controls.Add(no);
            messageBox.Controls.Add(textLabel);
            messageBox.AcceptButton = yes;

            return messageBox.ShowDialog() == DialogResult.Yes;
        }
        public static string ShowTextEntry(string text, string caption)
        {
            /* This function creates a message box which takes in a text input and returns that.
             */
            Console.WriteLine(text);
            Form messageBox = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximumSize = new System.Drawing.Size(900, 900),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
        };
            // Create the components
            Label textLabel = new Label() {Font = new Font("Calibri",12), Left = 0, Top = 100, Text = text, MaximumSize = new System.Drawing.Size(900, 900), AutoSize = true};
            TextBox textBox = new TextBox() { Font = new Font("Calibri", 12), Left = 0, Top = 50, Width = 400 };
            Button confirmation = new Button() { Font = new Font("Calibri", 12), Text = "OK", Left = 0, MaximumSize = new System.Drawing.Size(900, 900), Top = 0, DialogResult = DialogResult.OK, AutoSize=true};
            confirmation.Click += (sender, e) => { messageBox.Close(); };
            // Add the components to the form
            messageBox.Controls.Add(textBox);
            messageBox.Controls.Add(confirmation);
            messageBox.Controls.Add(textLabel);
            messageBox.AcceptButton = confirmation;

            return messageBox.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
        public static void ShowText (string text, string caption, System.Drawing.Color backColor, System.Drawing.Color foreColor)
        {
            /* This function creates a message box with just text.
             */
            Console.WriteLine(text);
            Form messageBox = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            // Create the components
            Label textLabel = new Label() { Font = new Font("Calibri", 12), Left = 0, Top = 50, Text = text, MaximumSize = new System.Drawing.Size(900, 900), AutoSize = true};
            Button confirmation = new Button() { Font = new Font("Calibri", 12), Text = "OK", Left = 0, AutoSize=true, MaximumSize = new System.Drawing.Size(900, 900), Top = 0, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { messageBox.Close(); };
            // Add the components to the form
            messageBox.Controls.Add(confirmation);
            messageBox.Controls.Add(textLabel);
            messageBox.BackColor = backColor;
            messageBox.ForeColor = foreColor;
            messageBox.ShowDialog(); // Show the message box
        }
    }
}



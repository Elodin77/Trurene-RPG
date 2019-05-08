/* This file actually has the potential to house the entire game in it, as the game
 * basically works by the user clicking the buttons. However, to make it more logical
 * and easy to understand, this file only contains functions important for the UI.
 * It will often have very short functions which simply call a function from the other file
 * which contains all of the functions related to the game. This is simply because those functions
 * to processes which are unrelated to the User Interface of the game.
 * It contains the interaction logic for NormalUI.xaml.
 * 
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static Trurene_RPG.Constants;
using Brushes = System.Windows.Media.Brushes;
using System.Threading;
using System.Timers;
using System.Windows.Threading;

namespace Trurene_RPG
{
    /// <summary>
    /// Interaction logic for NormalUI.xaml
    /// </summary>
    public partial class NormalUI : Window
    {
        // GLOBAL VARIABLES
        public static System.Timers.Timer UpdateEverythingTimer;
        public static System.Timers.Timer SaveTimer;
        public static System.Timers.Timer PrepareTimer;
        public static System.Timers.Timer UpdateNotificationsTimer;
        // FUNCTIONS
        public NormalUI()
        {
            // Start the timer to continually update the UI
            UpdateEverythingTimer = new System.Timers.Timer();
            UpdateEverythingTimer.Elapsed += new ElapsedEventHandler(UpdateEverything);
            UpdateEverythingTimer.Interval = 500; // this is a very costly function to run, so it has a reasonable time interval
            UpdateEverythingTimer.Enabled = true;
            // Start the timer to autosave
            SaveTimer = new System.Timers.Timer();
            SaveTimer.Elapsed += new ElapsedEventHandler(AutoSave);
            SaveTimer.Interval = 60000; // every minute 
            SaveTimer.Enabled = true;
            // Start the timer to autoprepare
            PrepareTimer = new System.Timers.Timer();
            PrepareTimer.Elapsed += new ElapsedEventHandler(AutoPrepare);
            PrepareTimer.Interval = 1000; 
            PrepareTimer.Enabled = true;
            // Start the timer to update notifications
            UpdateNotificationsTimer = new System.Timers.Timer();
            UpdateNotificationsTimer.Elapsed += new ElapsedEventHandler(UpdateNotifications);
            UpdateNotificationsTimer.Interval = 500; 
            UpdateNotificationsTimer.Enabled = true;
            // Start the UI
            InitializeComponent();


        }
        // Miscellaneous buttons
        public void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            
            try
            {
                Program.Load(CustomMessageBox.ShowTextEntry("Filename", "LOAD"));
            }
            catch
            {
                CustomMessageBox.ShowText("Filename or file format is invalid!", "LOAD ERROR", WARNING_BACK, WARNING_FORE);
                // Will happen if file format is wrong or file doesn't exist with that filename.
            }
        }
        public void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Program.Save(CustomMessageBox.ShowTextEntry("SAVE", "Filename"));
                Program.AddNotification("The game was successfully saved!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.PaleGreen });
            }
            catch
            {
                Program.AddNotification("The save filename is invalid!\n", new DependencyProperty[] { TextElement.ForegroundProperty }, new object[] { Brushes.PaleVioletRed });
                
            }
        }

        // Movement buttons
        public void WKeyPress(object sender, RoutedEventArgs e)
        {
            if (!Program.fighting)
            {
                Program.moveAction = "North";
                Program.NextTurn();
            }
            else
            {
                Program.fightAction = "Wait"; // this basically lets the enemy have a free turn
                Program.NextTurn();
            }
        }
        public void SKeyPress(object sender, RoutedEventArgs e)
        {
            if (!Program.fighting)
            {
                Program.moveAction = "South";
                Program.NextTurn();
            }
            else
            {
                Program.fightAction = "Strike";
                Program.NextTurn();
            }
        }
        public void AKeyPress(object sender, RoutedEventArgs e)
        {
            if (!Program.fighting)
            {
                Program.moveAction = "West";
                Program.NextTurn();
            }
        }
        public void DKeyPress(object sender, RoutedEventArgs e)
        {
            if (!Program.fighting)
            {
                Program.moveAction = "East";
                Program.NextTurn();
            }
            else
            {
                Program.fightAction = "Strike";
                Program.NextTurn();
            }
        }
        public void RKeyPress(object sender, RoutedEventArgs e)
        {
            if (Program.fighting)
            {
                Program.fightAction = "Strike";
                Program.NextTurn();
            }
        }




        public void TutorialButtonClick(object sender, RoutedEventArgs e)
        {
            /* This function goes through each piece of text on the UI, highlights it, and tells the user what it is.
             * It is like a super quick start guide for users who just want to learn what each button is.
             */

            SolidColorBrush color = Brushes.Yellow; // The color which the area is highlighted with

            // How you win
            CustomMessageBox.ShowText("The way that you win is by killing the TROLL KING before he destroys every VILLAGE without dying.", "TUTORIAL", NORMAL_BACK, NORMAL_FORE);
            CustomMessageBox.ShowText("You can get stronger by spending GOLD on weapons from MERCHANTS in VILLAGES.", "TUTORIAL", NORMAL_BACK, NORMAL_FORE);
            CustomMessageBox.ShowText("You can also get significantly stronger by going to the SHRINES, acquiring the ARTEFACTS, and bringing them to MAEJA to learn how to use them.", "TUTORIAL", NORMAL_BACK, NORMAL_FORE);


            // Map
            MapTextBlock.Background = color;
            CustomMessageBox.ShowText("The VILLAGE marked with METAL (grey) weapons is the location of HAWK.", "TUTORIAL", NORMAL_BACK, NORMAL_FORE);
            CustomMessageBox.ShowText("And the VILLAGE marked with MAGIC (blue) weapons is the location of MAEJA.", "TUTORIAL", NORMAL_BACK, NORMAL_FORE);
            Thread.Sleep(1000);
            MapTextBlock.Background = Brushes.Transparent;

            // Aurora

            AuroraPowerTextBlock.Background = color;
            CustomMessageBox.ShowText("The POWER is how much damage you do each hit.", "TUTORIAL", NORMAL_BACK, NORMAL_FORE);
            Thread.Sleep(1000);
            AuroraPowerTextBlock.Background = Brushes.Transparent;

            AuroraPreparednessTextBlock.Background = color;
            AuroraTimeTextBlock.Background = color;
            CustomMessageBox.ShowText("You can only strike when you are prepared enough.", "TUTORIAL", NORMAL_BACK, NORMAL_FORE);
            Thread.Sleep(1000);
            AuroraPreparednessTextBlock.Background = Brushes.Transparent;
            AuroraTimeTextBlock.Background = Brushes.Transparent;

            // End
            CustomMessageBox.ShowText("Congratulations, this is the end of the tutorial. Good luck on saving Trurene...", "TUTORIAL", GOLD_BACK, GOLD_FORE);


        }

        // Functions on timers (repeated after certain interval)
        public void UpdateEverything(object source, ElapsedEventArgs e)
        {
            /* This function updates every element of the UI based on the values in the Program class
             */

            try // Statistics and map (won't work at the start before they have been declared)
            {
                Dispatcher.Invoke(() =>
                {
                    // Update the map (height and width in the case of a load)
                    Map.Height = MAP_HEIGHT;
                    Map.Width = MAP_WIDTH;
                    Map.Source = Program.CalculateMap();

                    // Update the map (height and width in the case of a load)
                    Map.Height = MAP_HEIGHT;
                    Map.Width = MAP_WIDTH;
                    Map.Source = Program.CalculateMap();

                    // Update miscellanous variables
                    GoldTextBlock.Text = "Gold: " + Convert.ToString(Program.world.gold);
                    TurnTextBlock.Text = "Turn: " + Convert.ToString(Program.world.turnNum);
                    TickTextBlock.Text = "Tick: " + Convert.ToString(Program.tick);

                    // Update Aurora related variables
                    AuroraAccuracyTextBlock.Text = "Accuracy:\t" + Convert.ToString(Program.world.aurora.attack[0]);
                    AuroraHealthTextBlock.Text = "Health:\t" + Convert.ToString(Program.world.aurora.health);
                    AuroraMaxHealthTextBlock.Text = "Max Health:\t" + Convert.ToString(Program.world.aurora.maxHealth);
                    AuroraPowerTextBlock.Text = "Power:\t" + Convert.ToString(Program.world.aurora.attack[1]);
                    AuroraPreparednessTextBlock.Text = "Preparedness:\t" + Convert.ToString(Program.auroraPreparedness);
                    AuroraTimeTextBlock.Text = "Time:\t" + Convert.ToString(Program.world.aurora.attack[2]);

                    // Update the enemy's variables
                    if (Program.fighting)
                    {
                        EnemyAccuracyTextBlock.Text = "Accuracy:\t" + Convert.ToString(Program.enemy.attack[0]);
                        EnemyHealthTextBlock.Text = "Health:\t" + Convert.ToString(Program.enemy.health);
                        EnemyMaxHealthTextBlock.Text = "Max Health:\t" + Convert.ToString(Program.enemy.maxHealth);
                        EnemyPowerTextBlock.Text = "Power:\t" + Convert.ToString(Program.enemy.attack[1]);
                        EnemyPreparednessTextBlock.Text = "Preparedness:\t" + Convert.ToString(Program.enemyPreparedness);
                        EnemyTimeTextBlock.Text = "Time:\t" + Convert.ToString(Program.enemy.attack[2]);

                        EnemyAccuracyTextBlock.Background = Brushes.OrangeRed;
                        EnemyHealthTextBlock.Background = Brushes.OrangeRed;
                        EnemyMaxHealthTextBlock.Background = Brushes.OrangeRed;
                        EnemyPowerTextBlock.Background = Brushes.OrangeRed;
                        EnemyPreparednessTextBlock.Background = Brushes.OrangeRed;
                        EnemyTimeTextBlock.Background = Brushes.OrangeRed;
                        EnemyTextBlock.Background = Brushes.OrangeRed;

                        AuroraAccuracyTextBlock.Background = Brushes.LightGreen;
                        AuroraHealthTextBlock.Background = Brushes.LightGreen;
                        AuroraMaxHealthTextBlock.Background = Brushes.LightGreen;
                        AuroraPowerTextBlock.Background = Brushes.LightGreen;
                        AuroraPreparednessTextBlock.Background = Brushes.LightGreen;
                        AuroraTimeTextBlock.Background = Brushes.LightGreen;
                        AuroraTextBlock.Background = Brushes.LightGreen;

                        WaitButton.Background = Brushes.LightGoldenrodYellow;
                        RetreatButton.Background = Brushes.LightGoldenrodYellow;
                        NorthButton.Background = Brushes.Transparent;
                        SouthButton.Background = Brushes.Transparent;
                        EastButton.Background = Brushes.Transparent;
                        WestButton.Background = Brushes.Transparent;

                        if (Program.auroraPreparedness < Program.world.aurora.attack[2])
                        {
                            StrikeButton.Content = "PREPARE";
                            StrikeButton.Background = Brushes.OrangeRed;
                        }
                        else
                        {
                            StrikeButton.Content = "STRIKE";
                            StrikeButton.Background = Brushes.LightGreen;
                        }
                    }
                    else
                    {
                        EnemyAccuracyTextBlock.Text = "[Accuracy]";
                        EnemyHealthTextBlock.Text = "[Health]";
                        EnemyMaxHealthTextBlock.Text = "[Max Health]";
                        EnemyPowerTextBlock.Text = "[Power]";
                        EnemyPreparednessTextBlock.Text = "[Preparedness]";
                        EnemyTimeTextBlock.Text = "[Time]";

                        EnemyAccuracyTextBlock.Background = Brushes.Transparent;
                        EnemyHealthTextBlock.Background = Brushes.Transparent;
                        EnemyMaxHealthTextBlock.Background = Brushes.Transparent;
                        EnemyPowerTextBlock.Background = Brushes.Transparent;
                        EnemyPreparednessTextBlock.Background = Brushes.Transparent;
                        EnemyTimeTextBlock.Background = Brushes.Transparent;
                        EnemyTextBlock.Background = Brushes.Transparent;

                        AuroraAccuracyTextBlock.Background = Brushes.Transparent;
                        AuroraHealthTextBlock.Background = Brushes.Transparent;
                        AuroraMaxHealthTextBlock.Background = Brushes.Transparent;
                        AuroraPowerTextBlock.Background = Brushes.Transparent;
                        AuroraPreparednessTextBlock.Background = Brushes.Transparent;
                        AuroraTimeTextBlock.Background = Brushes.Transparent;
                        AuroraTextBlock.Background = Brushes.Transparent;

                        WaitButton.Background = Brushes.Transparent;
                        StrikeButton.Background = Brushes.Transparent;
                        RetreatButton.Background = Brushes.Transparent;
                        NorthButton.Background = Brushes.LightGoldenrodYellow;
                        SouthButton.Background = Brushes.LightGoldenrodYellow;
                        EastButton.Background = Brushes.LightGoldenrodYellow;
                        WestButton.Background = Brushes.LightGoldenrodYellow;
                    }


                });


            }
            catch
            {
                // couldn't update UI
            }


        }
        public void AutoSave(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (Convert.ToBoolean(checkAutoSave.IsChecked))
                {
                    Program.Save("autosave.txt");
                }
            });
        }

        public void AutoPrepare(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (Program.fighting && Convert.ToBoolean(checkAutoPrepare.IsChecked))
                {
                    if (Program.fightAction != "Retreat" && Program.auroraPreparedness < Program.world.aurora.attack[2] && Program.enemyPreparedness < Program.enemy.attack[2])
                    {
                        Program.fightAction = "Prepare"; // Automatically set it to prepare
                        Program.DoFightTurn();
                    }
                }
            });
        }
        public void KeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            /* This function handles what to do when certain keys are pressed
             */
            if (e.Key == Key.S)
            {
                SKeyPress(null,null);
            }
            if (e.Key == Key.W)
            {
                WKeyPress(null, null);
            }
            if (e.Key == Key.A)
            {
                AKeyPress(null, null);
            }
            if (e.Key == Key.D)
            {
                DKeyPress(null, null);
            }
            if (e.Key == Key.R)
            {
                RKeyPress(null, null);
            }
        }
        public void UpdateNotifications(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (Program.FormattedText formattedText in Program.notificationsQueue)
                {
                    // Create the run of text
                    TextRange textRange = new TextRange(notificationsRTF.Document.ContentEnd, notificationsRTF.Document.ContentEnd)
                    {
                        Text = formattedText.text
                    };
                    foreach (var textProperty in formattedText.textProperties)
                    {
                        textRange.ApplyPropertyValue(textProperty.property, textProperty.value);
                    }

                }
                Program.notificationsQueue = new List<Program.FormattedText>(); // Empty the queue
                notificationsRTF.ScrollToEnd(); // scroll to the bottom
            });
        }
        public void OnShutdown(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /* This function stops all of the threads, timers... to prevent any exceptions occuring by the tasks being cancelled.
             */
            PrepareTimer.Stop();
            SaveTimer.Stop();
            UpdateEverythingTimer.Stop();
            UpdateNotificationsTimer.Stop();
        }




    }

}

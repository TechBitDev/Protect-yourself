﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.script.simulated.protect.boost;

namespace WpfApp2
{
    public partial class GameReal : Window
    {

        //listy
        List<string> listaRzeczy = new List<string>();

        // Timers
        private Timer timerVirus;
        private Timer timerTrojan;
        private Timer timerRansomware;
        private Timer timerErrors;
        private Timer timerUpdate;
        private Timer czas;

        // Obiekt Random
        private Random rnd = new Random();

        // Liczby całkowite
        private int prog = 10;
        public int iloscVirus { get; set; } = 0;
        public int iloscTrojan { get; set; } = 0;
        public int iloscRansomware { get; set; } = 0;
        public int iloscErrors { get; set; } = 0;

        private int ileRazyUpdate = 0;
        public int gold { get; set; } = 0;
        public int timeVirus, timeTrojan, timeRansomware, timeErrors;
        private int errors;
        int iloscSekund;
        MainWindow mainWindow = new MainWindow();

        // Konstruktor
        public GameReal()
        {
            Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
            InitializeComponent();
            timeVirus = Protected.slowDownVirusTime();
            timerVirus = new Timer(rnd.Next(1,3) * 1000);
            timerVirus.Elapsed += TimerVirus_Elapsed;
            timerVirus.AutoReset = true;
            timerVirus.Start();

            czas = new Timer(1000);
            czas.AutoReset = true;
            czas.Elapsed += new ElapsedEventHandler(aktualizujCzas);
            czas.Start();

            timeTrojan = Protected.slowDownTrojanTime();
            timerTrojan = new Timer(timeTrojan * 1000);
            timerTrojan.Elapsed += TimerTrojan_Elapsed;
            timerTrojan.AutoReset = true;
            timerTrojan.Start();

            timeRansomware = Protected.slowDownRansomwareTime();
            timerRansomware = new Timer(timeRansomware * 1000);
            timerRansomware.Elapsed += TimerRansomware_Elapsed;
            timerRansomware.AutoReset = true;
            timerRansomware.Start();

            timeErrors = Protected.slowDownErrorsTime();
            timerErrors = new Timer(timeErrors * 1000);
            timerErrors.Elapsed += TimerErrors_Elapsed;
            timerErrors.AutoReset = true;
            timerErrors.Start();

            timerUpdate = new Timer(Protected.WaitForSecond());
            timerUpdate.Elapsed += TimerUpdate_Elapsed;
            timerUpdate.Start();

            virus.Content = iloscVirus;
            trojan.Content = iloscTrojan;
            ransomware.Content = iloscRansomware;
            error.Content = iloscErrors;
        }

        private void aktualizujCzas(object sender, ElapsedEventArgs e)
        {
            File.CreateText(@"C:\protyou\czas\czas.txt");
            File.WriteAllText(@"C:\protyou\czas\czas.txt", iloscSekund.ToString());
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timerErrors.Stop();
            timerUpdate.Stop();
            timerTrojan.Stop();
            timerVirus.Stop();
            timerRansomware.Stop();
            mainWindow.Show();
        }

        private void TimerVirus_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                DodajVirusa();
            });
        }

        private void TimerTrojan_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                DodajTrojana();
            });
        }

        private void TimerRansomware_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                DodajRansomware();
            });
        }

        private void TimerErrors_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                DodajBledy();
            });
        }

        private void TimerUpdate_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Aktualizuj();
            });
        }

        private void Aktualizuj()
        {
            errors = iloscVirus + iloscTrojan + iloscRansomware + iloscErrors;

            if (prog < errors)
            {
                updates.IsEnabled = true; // Ustaw przycisk na aktywny
            }
            else
            {
                updates.IsEnabled = false; // Ustaw przycisk na nieaktywny
            }
            if (iloscVirus == 10)
            {
                  MessageBox.Show("Zainfekowano komputer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                iloscVirus++;
            }
            
        }
        private void DodajVirusa()
        {
            iloscVirus++;
            virus.Content = iloscVirus;
        }

        private void DodajTrojana()
        {
            iloscTrojan++;
            trojan.Content = iloscTrojan;
        }

        private void shop(ListBoxItem itemList)
        {
            string selectedItem = itemList.Content.ToString();
            string[] item = selectedItem.Split(' ');
            Title = item[0];
            int itemPrice;
            if (item.Length >= 2 && int.TryParse(item[1], out itemPrice))
            {
                if (gold < Convert.ToInt32(item[1]))
                {
                    switch (item[0])
                    {
                        case "Slow_Down_Wirus":
                            timeVirus = Protected.slowDownVirusTime();
                            timerVirus.Interval = timeVirus*1000;
                            gold -= Convert.ToInt32(item[1]);
                            ileZlota.Content = gold;
                            break;
                        case "Slow_Down_Trojan":
                            timeTrojan = Protected.slowDownTrojanTime();
                            timerTrojan.Interval = timeTrojan * 1000;
                            gold -= Convert.ToInt32(item[1]);
                            ileZlota.Content = gold;
                            break;
                        case "Slow_Down_Ransomware":
                            timeRansomware = Protected.slowDownRansomwareTime();
                            timerRansomware.Interval = timeRansomware * 1000;
                            gold -= Convert.ToInt32(item[1]);
                            ileZlota.Content = gold;
                            break;
                        case "Slow_Down_Errors":
                            timeErrors = Protected.slowDownErrorsTime();
                            timerErrors.Interval = timeErrors * 1000;
                            gold -= Convert.ToInt32(item[1]);
                            ileZlota.Content = gold;
                            break;
                        case "FireWall":
                            if (Protected.FireWall())
                            {
                                iloscVirus -= (int)(iloscVirus * 0.8);
                                gold -= Convert.ToInt32(item[1]);
                                ileZlota.Content = gold;
                            }
                            break;
                        case "Speed_Updates":
                            Protected.boostUpdateTime();
                            gold -= Convert.ToInt32(item[1]);
                            ileZlota.Content = gold;
                            break;
                    }
                    
                }
            }
            }

        private void DodajRansomware()
        {
            iloscRansomware++;
            ransomware.Content = iloscRansomware;
        }


        private void DodajBledy()
        {
            iloscErrors++;
            error.Content = iloscErrors;
        }

        private void updates_Click(object sender, RoutedEventArgs e)
        {
            ileRazyUpdate++;
            prog = rnd.Next(5, 15);
            gold += rnd.Next(3, 5);
            ileZlota.Content = gold;
            iloscErrors -= (int)(iloscErrors * 0.8);
            iloscRansomware -= (int)(iloscRansomware * 0.8);
            iloscTrojan -= (int)(iloscTrojan * 0.8);
            iloscVirus -= (int)(iloscVirus * 0.8);

            error.Content = iloscErrors;
            ransomware.Content = iloscRansomware;
            trojan.Content = iloscTrojan;
            virus.Content = iloscVirus;

            if (iloscVirus < 0) iloscVirus = 0;
            if (iloscTrojan < 0) iloscTrojan = 0;
            if (iloscRansomware < 0) iloscRansomware = 0;
            if (iloscErrors < 0) iloscErrors = 0;
        }
        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lista.SelectedItem is ListBoxItem selectedItem)
            {
                shop(selectedItem);
                lista.SelectedItem = null; // Opcjonalnie, aby odznaczyć element po zakupie
            }
        }
    }
}
 

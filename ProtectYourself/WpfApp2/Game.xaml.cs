using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace WpfApp2
{
    /// <summary>
    /// Logika interakcji dla klasy Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        MainWindow mainWindow = new MainWindow();
        public Game()
        {
            InitializeComponent();
            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.playGame.IsEnabled = true;
            mainWindow.Show();
        }
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Sprawdź czy wprowadzony znak jest cyfrą
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Jeśli nie jest cyfrą, zatrzymaj jego przetwarzanie
            }
        }
        private void szyfuj(object sender, RoutedEventArgs e)
        {
            int klucz = Convert.ToInt32(key.Text);
            string msg= hasloDoHashu.Text;
            string encMsg = encrypt(msg, klucz);
            if (encMsg.Length > 10)
                output.FontSize = 20;
            else if (encMsg.Length > 20)
                output.FontSize = 15;
            else if (encMsg.Length > 30)
                output.FontSize = 10;
            output.Content = encMsg;
        }
        string encrypt(string msg, long key)
        {
            string msgEnc = "";
            foreach (var item in msg)
            {
                msgEnc += Convert.ToChar((int)item + key);
            }
            return decToHex(msgEnc);
        }
        string decToHex(string msg)
        {
            string ready = "";

            foreach (var item in msg)
            {
                // Convert each character to its ASCII value and then to a hexadecimal string
                int asciiValue = (int)item;
                string hex = asciiValue.ToString("X2"); // "X2" ensures two-digit hexadecimal representation
                ready += hex;
            }

            // Reverse the ready string
            char[] arr = ready.ToCharArray();
            Array.Reverse(arr);
            string rReady = new string(arr);

            return rReady;
        } 
        }

    }
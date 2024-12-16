using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace WpfApp2.script
{
    internal class Updates
    {
        public Updates(string serverUrl,string localPath)
        {
            checkUpdates(serverUrl, localPath);
        }
        private void checkUpdates(string serverUrl,string localUrl) {
            // Tworzenie obiektu do pobierania danych z serwera
            WebClient client = new WebClient();

            try
            {
                // Pobranie numeru wersji z serwera
                string serverVersion = client.DownloadString(serverUrl + "version.txt");

                // Pobranie numeru wersji lokalnej
                string localVersion = File.ReadAllText("version.txt");

                // Porównanie numerów wersji
                if (serverVersion != localVersion)
                {
                    Console.WriteLine("Nowa wersja dostępna. Rozpoczynanie aktualizacji...");

                    // Pobranie aktualizacji
                    client.DownloadFile(serverUrl + "update.zip", localUrl + "update.zip");

                    // Rozpakowanie aktualizacji
                    ZipFile.ExtractToDirectory(localUrl + "update.zip", localUrl);

                    // Przeniesienie zawartości aktualizacji do miejsca, gdzie znajduje się plik wykonywalny
                    string[] files = Directory.GetFiles(localUrl);
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        string destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                        File.Copy(file, destination, true);
                    }

                    Console.WriteLine("Aktualizacja zakończona pomyślnie.");
                }
                else
                {
                    Console.WriteLine("Twoja aplikacja jest aktualna.");
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("Błąd pobierania aktualizacji: " + ex.Message);
            }
        }
    }
}

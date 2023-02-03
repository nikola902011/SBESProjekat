using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class MenuHandler
    {
        public void InitializeMenu()
        {
            Console.WriteLine("Izaberite opciju (unesite broj od 1 do 7)");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("1. Izlistaj folder");
            Console.WriteLine("2. Procitaj sadrzaj fajla");
            Console.WriteLine("3. Kreiraj folder");
            Console.WriteLine("4. Kreiraj fajl");
            Console.WriteLine("5. Promeni naziv fajla ili foldera");
            Console.WriteLine("6. Premesti fajl ili folder");
            Console.WriteLine("7. Obrisi fajl ili folder");
            Console.WriteLine("0. Napusti program");
            Console.WriteLine("----------------------------------------------");
        }

        public bool ReceiveInput(ClientProxy proxy)
        {
           
            int option = -1;
            
            int.TryParse(Console.ReadLine(), out option);

            if (option == -1 || option > 7)
            {
                Console.WriteLine("Nepravilan unos, pokusajte ponovo.");
            }
            switch (option)
            {
                case 0:
                    
                    break;
                case 1:
                    Console.WriteLine("Unesite naziv foldera");
                    string folderContents = proxy.ShowFolder(Console.ReadLine());
                    Console.Write($"Sadrzaj foldera: {Environment.NewLine} {folderContents} {Environment.NewLine}");
                    break;
                case 2:
                    Console.WriteLine("Unesite naziv fajla");
                    string fileContents = proxy.ReadFile(Console.ReadLine());
                    Console.Write($"Sadrzaj fajla: {Environment.NewLine} {fileContents} {Environment.NewLine}");
                    break;
                case 3:

                    break;
                case 4:
                    
                    break;
                case 5:
                    
                    break;
                case 6:
                   
                    break;
                case 7:
                   
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}

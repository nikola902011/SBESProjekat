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
            bool isFile = false;
            int option = -1;
            bool res = false;
            
            int.TryParse(Console.ReadLine(), out option);

            if (option == -1 || option > 7)
            {
                Console.WriteLine("Nepravilan unos, pokusajte ponovo.");
            }
            switch (option)
            {
                case 0:
                    Console.WriteLine("Da li ste sigurno da zelite da napustite program? (DA/NE)");
                    string answer = Console.ReadLine();
                    if (string.Equals(answer, "DA"))
                    {
                        return false;

                    }
                    if (string.Equals(answer, "NE"))
                    {
                        InitializeMenu();
                    }
                    Console.WriteLine("Pogresan unos, probajte ponovo.");
                    InitializeMenu();
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
                    Console.WriteLine("Unesite naziv foldera koji zelite da kreirate");
                    res = proxy.CreateFolder(Console.ReadLine());
                    if (res) Console.WriteLine($" {Environment.NewLine} Uspesno kreiran folder");
                    break;
                case 4:
                    Console.WriteLine("Unesite naziv fajla koji zelite da kreirate");
                    string filename = Console.ReadLine();
                    Console.WriteLine("Unesite sadrzaj fajla");
                    string content = Console.ReadLine();
                    res = proxy.CreateFile(filename, content);
                    if (res) Console.WriteLine($" {Environment.NewLine} Fajl uspesno kreiran");
                    break;
                case 5:
                    try
                    {
                        isFile = IsFileOrFolder();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                    Console.WriteLine("Unesite naziv fajla ili foldera koji zelite da promenite");
                    string oldName = Console.ReadLine();
                    Console.WriteLine("Unesite novi naziv");
                    string newName = Console.ReadLine();
                    res = proxy.RenameFileOrFolder(oldName, newName, isFile);
                    if (res) Console.WriteLine($" {Environment.NewLine} Uspesno promenjeno ime fajla/foldera");
                    break;
                case 6:
                    try
                    {
                        isFile = IsFileOrFolder();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                    Console.WriteLine("Unesite naziv fajla/foldera koji zelite da premestite");
                    filename = Console.ReadLine();
                    Console.WriteLine("Unesite naziv foldera u koji zelite da premestite odabrani fajl");
                    string folder = Console.ReadLine();
                    res = proxy.MoveFile(filename, folder, isFile);
                    if (res) Console.WriteLine($" {Environment.NewLine} Fajl uspesno premesten");
                    break;
                case 7:
                    try
                    {
                        isFile = IsFileOrFolder();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                    Console.WriteLine("Unesite naziv fajla ili foldera koji zelite da obrisete");
                    Console.WriteLine("UPOZORENJE: Ukoliko brisete folder, obrisace se i svi fajlovi u njemu!");
                    string name = Console.ReadLine();
                    if (name.Contains("."))
                    {
                        isFile = true;
                    }
                    res = proxy.DeleteFile(name, isFile);
                    if (res) Console.WriteLine($" {Environment.NewLine} Fajl/folder uspesno obrisan");
                    break;
                default:
                    break;
            }
            return true;
        }
        public bool IsFileOrFolder()
        {
            Console.WriteLine("Ukoliko zelite da izvrsite operaciju nad fajlom, upisite slovo F");
            Console.WriteLine("Ukoliko zelite da izvrsite operaciju nad folderom, upisite slovo D");
            string fileOrFolder = Console.ReadLine();
            if (!fileOrFolder.Equals("F") && !fileOrFolder.Equals("D"))
            {
                throw new Exception("Pogresan unos, pokusajte ponovo");
            }
            if (fileOrFolder.Equals("F"))
            {
                return true;
            }
            return false;
        }
    }
}

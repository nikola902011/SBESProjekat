using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class FileSystemManagerService : IFileSystemManager
    {
        private string currentPath = Path.Combine(Directory.GetCurrentDirectory(), "root");
        public bool CreateFile(string filename, string fileContent)
        {
            CustomPrincipal principal = new CustomPrincipal(ServiceSecurityContext.Current.PrimaryIdentity);

            if (principal.IsInRole("Editor"))
            {
                if (string.IsNullOrEmpty(filename))
                {
                    try
                    {
                        Audit.CreateFailed(filename, "No file name provided.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                    throw new FaultException("Niste uneli naziv fajla!");
                }

                string pathString = Path.Combine(currentPath, filename);
                if (File.Exists(pathString))
                {
                    try
                    {
                        Audit.CreateFailed(filename, "File already exists.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    throw new FaultException("Vec postoji fajl sa unetim nazivom!");
                }
                using (StreamWriter sw = File.CreateText(pathString))
                {
                    sw.WriteLine(EncryptionManager.DecryptMessage(fileContent));

                }
                try
                {
                    Audit.CreatedSuccessfully(filename, "File successfully created");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return true;
            }
            else
            {
                throw new FaultException("Nemate pristup ovoj opciji");

            }
        }

        public bool CreateFolder(string folderName)
        {
            CustomPrincipal principal = new CustomPrincipal(ServiceSecurityContext.Current.PrimaryIdentity);

            if (principal.IsInRole("Editor"))
            {
                if (string.IsNullOrEmpty(folderName))
                {
                    try
                    {
                        Audit.CreateFailed(folderName, "No folder name provided");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    throw new FaultException("Niste uneli naziv foldera!");


                }

                string pathString = Path.Combine(currentPath, folderName);
                if (Directory.Exists(pathString))
                {
                    try
                    {
                        Audit.CreateFailed(folderName, "Folder already exists");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    throw new FaultException("Folder sa unetim nazivom vec postoji!");
                }
                Directory.CreateDirectory(pathString);

                try
                {
                    Audit.CreatedSuccessfully(folderName, "Folder successfully created");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return true;
            }
            else
            {
                throw new FaultException("Nemate pristup ovoj opciji");

            }
        }

        public bool DeleteFile(string filename, bool isFile)
        {
            CustomPrincipal principal = new CustomPrincipal(ServiceSecurityContext.Current.PrimaryIdentity);


            if (principal.IsInRole("Editor"))
            {
                string pathString = Path.Combine(currentPath, filename);
                if (isFile)
                {
                    if (!File.Exists(pathString))
                    {
                        throw new FaultException("Ne postoji fajl sa unetim nazivom!");

                    }
                    try
                    {
                        File.Delete(pathString);
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    if (!Directory.Exists(pathString))
                    {
                        throw new FaultException("Ne postoji folder sa unetim nazivom!");

                    }
                    try
                    {
                        Directory.Delete(pathString, true);
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                return true;
            }
            else
            {
                throw new FaultException("Nemate pristup ovoj opciji");

            }
        }

        public bool MoveFile(string filename, string pathToFolder, bool isFile)
        {
            CustomPrincipal principal = new CustomPrincipal(ServiceSecurityContext.Current.PrimaryIdentity);

            if (principal.IsInRole("Editor"))
            {
                if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(pathToFolder))
                {
                    throw new FaultException("Niste uneli naziv fajla!");
                }
                string pathString = Path.Combine(currentPath, filename);
                string path = Path.Combine(currentPath, pathToFolder);

                string newPath = Path.Combine(path, filename);

                if (isFile)
                {
                    if (!File.Exists(pathString))
                    {
                        throw new FaultException("Ne postoji fajl sa unetim nazivom!");


                    }
                    if (File.Exists(Path.Combine(pathToFolder, filename)))
                    {
                        throw new FaultException("Fajl sa istim nazivom vec postoji u folderu!");
                    }

                    File.Move(pathString, newPath);
                }
                else
                {
                    if (!Directory.Exists(pathString))
                    {
                        throw new FaultException("Ne postoji folder sa unetim nazivom!");
                    }
                    if (Directory.Exists(Path.Combine(pathToFolder, filename)))
                    {
                        throw new FaultException("Folder sa istim nazivom vec postoji u folderu!");
                    }
                    Directory.Move(pathString, newPath);

                }
                return true;
            }
            else
            {
                throw new FaultException("Nemate pristup ovoj opciji");
            }
        }

        public string ReadFile(string filename)
        {
            CustomPrincipal principal = new CustomPrincipal(ServiceSecurityContext.Current.PrimaryIdentity);


            if (principal.IsInRole("Viewer") || principal.IsInRole("Editor"))
            {
                if (string.IsNullOrEmpty(filename))
                {
                    throw new FaultException("Niste uneli naziv fajla!");
                }
                string pathString = Path.Combine(currentPath, filename);
                if (!File.Exists(pathString))
                {
                    throw new FaultException("Ne postoji fajl sa unetim nazivom!");
                }

                string[] lines = File.ReadAllLines(pathString);

                StringBuilder builder = new StringBuilder();
                foreach (string value in lines)
                {
                    builder.Append(value);
                    builder.Append(Environment.NewLine);
                }

                return EncryptionManager.EncryptMessage(builder.ToString());
            }
            else
            {
                throw new FaultException("Nemate pristup ovoj opciji");
            }
        }

        public bool RenameFileOrFolder(string oldName, string newName, bool isFile)
        {
            CustomPrincipal principal = new CustomPrincipal(ServiceSecurityContext.Current.PrimaryIdentity);


            if (principal.IsInRole("Editor"))
            {
                if (String.IsNullOrEmpty(oldName) || String.IsNullOrEmpty(newName))
                {
                    throw new FaultException("Niste uneli naziv fajla!");
                }

                string pathString = Path.Combine(currentPath, oldName);
                if (isFile)
                {
                    if (!File.Exists(pathString))
                    {
                        throw new FaultException("Ne postoji fajl sa unetim nazivom!");

                    }

                    string newPath = Path.Combine(currentPath, newName);
                    if (File.Exists(newPath))
                    {
                        throw new FaultException("Vec postoji fajl sa unetim nazivom!");
                    }
                    File.Move(pathString, newPath);

                }
                else
                {
                    if (!Directory.Exists(pathString))
                    {
                        throw new FaultException("Ne postoji folder sa unetim nazivom!");

                    }

                    string newPath = Path.Combine(currentPath, newName);
                    if (Directory.Exists(newPath))
                    {
                        throw new FaultException("Vec postoji folder sa unetim nazivom!");
                    }
                    Directory.Move(pathString, newPath);
                }
                return true;
            }
            else
            {
                throw new FaultException("Nemate pristup ovoj opciji");
            }
        }

        public string ShowFolder(string filename)
        {
            CustomPrincipal principal = new CustomPrincipal(ServiceSecurityContext.Current.PrimaryIdentity);


            if (principal.IsInRole("Viewer") || principal.IsInRole("Editor"))
            {
                string pathString = Path.Combine(currentPath, filename);

                DirectoryInfo di = new DirectoryInfo(pathString);
                if (!di.Exists)
                {
                    throw new FaultException("Folder sa unetim nazivom ne postoji!");
                }
                string[] files = Directory.GetFileSystemEntries(pathString, "*", SearchOption.AllDirectories);
                StringBuilder builder = new StringBuilder();
                foreach (string value in files)
                {
                    builder.Append(value);
                    builder.Append(Environment.NewLine);
                }
                return builder.ToString();
            }
            else
            {
                throw new FaultException("Nemate pristup ovoj opciji");
            }
        }
    }
}

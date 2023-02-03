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
            throw new NotImplementedException();
        }

        public bool CreateFolder(string folderName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(string filename, bool isFile)
        {
            throw new NotImplementedException();
        }

        public bool MoveFile(string filename, string pathToFolder, bool isFile)
        {
            throw new NotImplementedException();
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

                return builder.ToString();
            }
            else
            {
                throw new FaultException("Nemate pristup ovoj opciji");
            }
        }

        public bool RenameFileOrFolder(string oldName, string newName, bool isFile)
        {
            throw new NotImplementedException();
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

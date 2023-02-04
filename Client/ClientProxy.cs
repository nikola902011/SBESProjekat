using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class ClientProxy : ChannelFactory<IFileSystemManager>, IFileSystemManager, IDisposable
    {
        public IFileSystemManager factory;

        public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
        }

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {

        }

        public bool CreateFile(string filename, string fileContent)
        {
            bool result = false;
            try
            {
                result = factory.CreateFile(filename, fileContent);
            }
            catch (FaultException e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public bool CreateFolder(string folderName)
        {
            bool result = false;
            try
            {
                result = factory.CreateFolder(folderName);
            }
            catch (FaultException e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        public bool DeleteFile(string filename, bool isFile)
        {
            bool result = false;
            try
            {
                result = factory.DeleteFile(filename, isFile);
            }
            catch (FaultException e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public bool MoveFile(string filename, string pathToFolder, bool isFile)
        {
            bool result = false;
            try
            {
                result = factory.MoveFile(filename, pathToFolder, isFile);
            }
            catch (FaultException e)
            {
                Console.WriteLine(e.Message);
            }


            return result;
        }

        public string ReadFile(string filename)
        {
            string contents = " ";
            try
            {
                contents = factory.ReadFile(filename);
                return contents;
            }
            catch (FaultException e)
            {
                Console.WriteLine(e.Message);
            }

            return contents;
        }

        public bool RenameFileOrFolder(string oldName, string newName, bool isFile)
        {
            bool result = false;
            try
            {
                result = factory.RenameFileOrFolder(oldName, newName, isFile);
            }
            catch (FaultException e)
            {
                Console.WriteLine(e.Message);
            }


            return result;
        }

        public string ShowFolder(string filename)
        {
            string result = "";
            try
            {
                result = factory.ShowFolder(filename);

            }
            catch (FaultException e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
    }
}

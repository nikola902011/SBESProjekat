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
            throw new NotImplementedException();
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

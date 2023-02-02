using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IFileSystemManager
    {
        [OperationContract]
        string ShowFolder(string filename);
        [OperationContract]
        string ReadFile(string filename);
        [OperationContract]
        bool CreateFolder(string folderName);
        [OperationContract]
        bool CreateFile(string filename, string fileContent);
        [OperationContract]
        bool RenameFileOrFolder(string oldName, string newName, bool isFile);
        [OperationContract]
        bool MoveFile(string filename, string pathToFolder, bool isFile);
        [OperationContract]
        bool DeleteFile(string filename, bool isFile);
    }
}

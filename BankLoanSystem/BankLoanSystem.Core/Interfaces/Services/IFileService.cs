using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace BankLoanSystem.Core.Interfaces.Services
{
    public interface IFileService
    {
        string? UploadFile(string FolderName, IFormFile? newFile);
        void DeleteFile(string filePath);
        string? UpdateFile(string FolderName, IFormFile? newFile, string? oldFilePath);
    }
}

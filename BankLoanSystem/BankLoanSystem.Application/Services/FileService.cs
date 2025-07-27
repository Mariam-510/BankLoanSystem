using BankLoanSystem.Core.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLoanSystem.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public string? UploadFile(string FolderName, IFormFile? newFile)
        {
            if (newFile != null && newFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newFile.FileName);

                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, FolderName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    newFile.CopyTo(stream);
                }
                return Path.Combine(FolderName, fileName).Replace("\\", "/");
            }
            return null;

        }

        public void DeleteFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string fullPath = Path.Combine(webHostEnvironment.WebRootPath, filePath.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }


        public string? UpdateFile(string FolderName, IFormFile? newFile, string? oldFilePath)
        {
            DeleteFile(oldFilePath);

            if (newFile != null)
            {
                return UploadFile(FolderName, newFile);
            }

            return null;

        }

    }
}

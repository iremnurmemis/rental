
using Microsoft.AspNetCore.Http;

namespace Core
{
    public class FileHelperManager : IFileHelper
    {
        


        public string Add(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var imagePath = FilePath.Full(uniqueFileName);
            using FileStream fileStream = new(imagePath, FileMode.Create);
            file.CopyTo(fileStream);
            fileStream.Flush();
            return uniqueFileName;
        }

        public void Delete(string path)
        {
            if (Path.Exists(FilePath.Full(path))) //varsa true
            {
                File.Delete(FilePath.Full(path));
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }

        public void Update(IFormFile file, string imagePath)
        {
            var fullpath = FilePath.Full(imagePath);
            if (Path.Exists(fullpath))
            {
                using FileStream fileStream = new(fullpath, FileMode.Create);
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            else
            {
                throw new DirectoryNotFoundException();
            }

        }

    }
}

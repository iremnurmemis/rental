
using Microsoft.AspNetCore.Http;

namespace Core
{
    public class FileHelperManager : IFileHelper
    {
        //public void Delete(string filePath)//Burada ki string filePath, 'CarImageManager'dan gelen dosyamın kaydedildiği adres ve adı 
        //{
        //    if (File.Exists(filePath))//if kontrolü ile parametrede gelen adreste öyle bir dosya var mı diye kontrol ediliyor.
        //    {
        //        File.Delete(filePath);//Eğer dosya var ise dosya bulunduğu yerden siliniyor.
        //    }
        //}

        //public string Update(IFormFile file, string filePath, string root)//Dosya güncellemek için ise gelen parametreye baktığımızda Güncellenecek yeni dosya, Eski dosyamızın kayıt dizini, ve yeni bir kayıt dizini
        //{
        //    if (File.Exists(filePath))// Tekrar if kontrolü ile parametrede gelen adreste öyle bir dosya var mı diye kontrol ediliyor.
        //    {
        //        File.Delete(filePath);//Eğer dosya var ise dosya bulunduğu yerden siliniyor.
        //    }
        //    return Upload(file, root);// Eski dosya silindikten sonra yerine geçecek yeni dosyaiçin alttaki *Upload* metoduna yeni dosya ve kayıt edileceği adres parametre olarak döndürülüyor.
        //}

        //public string Upload(IFormFile file, string root)
        //{
        //    if (file.Length > 0)
        //    {
        //        if (!Directory.Exists(root))
        //            Directory.CreateDirectory(root);

        //        string extension = Path.GetExtension(file.FileName);
        //        string guid = Guid.NewGuid().ToString();
        //        string filePath = Path.Combine(root, guid + extension);

        //        using (FileStream fileStream = File.Create(filePath))
        //        {
        //            file.CopyTo(fileStream);
        //            fileStream.Flush();
        //            return filePath;
        //        }
        //    }
        //    return null;
        //}


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

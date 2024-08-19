
using Microsoft.AspNetCore.Http;

namespace Core
{
    public interface IFileHelper
    {
        string Add(IFormFile file);
        void Delete(string path);
        void Update(IFormFile file, string imagePath);
    }
}

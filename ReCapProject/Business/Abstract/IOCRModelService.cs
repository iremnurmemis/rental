

namespace Business
{
    public interface IOCRModelService
    {
        string RunPythonScript(byte[] imageBytes);   //ehliyet belgesi  yükleme aşamasında yüklenen belgenin kontrölünü yapan ve belgeden veri döndüren python modeli
       
    }
    
}
    


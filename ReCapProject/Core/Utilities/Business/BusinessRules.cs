
using Core.Interceptors.Utilities.Results;

namespace Core
{
    public class BusinessRules
    {
        //params verince run içerisine istediğimiz kadar IResult parametresini verebiliyoruz.logics iş kuralları 
        public static IResult Run(params IResult[] logics) 
        {
            foreach (var logic in logics) 
            {
                if(!logic.Success)
                    return logic;  // başarısız olanı businessa haberdar ediyoruz.direkt kuralın kendisini döndürüyoruz
            }

            return null;
        }
    }
}

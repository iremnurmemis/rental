using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface IColorService
    {
        IDataResult<List<Color>> GetAll();
        IResult Add(Color color);
        IResult Delete(Color color);
        IResult Update(Color color);
        
      
    }
}

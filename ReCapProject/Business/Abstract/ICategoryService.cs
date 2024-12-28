
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface ICategoryService
    {
        IResult Add(Category category);
        IDataResult<List<Category>> GetAll();
        IDataResult<Category> GetByCategoryId(int Id);
        IResult Update(Category category);
        IResult Delete(Category category);
    }
}

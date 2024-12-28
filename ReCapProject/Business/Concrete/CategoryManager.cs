
using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;
using Npgsql.Internal;

namespace Business
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        public IResult Add(Category category)
        {
            _categoryDal.Add(category);
            return new SuccessDataResult<Category>("category eklendi");
        }

        public IResult Delete(Category category)
        {
           _categoryDal.Delete(category);
            return new SuccessDataResult<Category>("category silindi");
        }

        public IDataResult<List<Category>> GetAll()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetAll(),"categoryler listelendi");
        }

        public IDataResult<Category> GetByCategoryId(int Id)
        {
            return new SuccessDataResult<Category>(_categoryDal.Get(c => c.Id == Id), "ID bilgisine göre category");
        }

        public IResult Update(Category category)
        {
            _categoryDal.Update(category);
            return new SuccessDataResult<Category>("category bilgisi güncellendi");
        }
    }
}

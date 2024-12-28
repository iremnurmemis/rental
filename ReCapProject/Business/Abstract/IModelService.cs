
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface IModelService
    {
        IResult Add(Model model);
        IDataResult<List<Model>> GetAll();
        IDataResult<Model> GetByModelId(int Id);
        IResult Update(Model model);
        IResult Delete(Model model);
    }
}

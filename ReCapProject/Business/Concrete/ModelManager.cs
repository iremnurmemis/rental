
using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;
using Npgsql.Internal;

namespace Business
{
    public class ModelManager : IModelService
    {
        IModelDal _modelDal;
        public ModelManager(IModelDal modelDal)
        {
            _modelDal = modelDal;
        }
        public IResult Add(Model model)
        {
            _modelDal.Add(model);
            return new SuccessDataResult<Model>("model eklendi");
        }

        public IResult Delete(Model model)
        {
           _modelDal.Delete(model);
            return new SuccessDataResult<Model>("model silindi");
        }

        public IDataResult<List<Model>> GetAll()
        {
            return new SuccessDataResult<List<Model>>(_modelDal.GetAll(),"modeller listelendi");
        }

        public IDataResult<Model> GetByModelId(int Id)
        {
            return new SuccessDataResult<Model>(_modelDal.Get(m => m.Id == Id), "model Id bilgisine göre getirildi");
        }

        public IResult Update(Model model)
        {
            _modelDal.Update(model);
            return new SuccessDataResult<Model>("model bilgisi güncellendi");
        }
    }
}

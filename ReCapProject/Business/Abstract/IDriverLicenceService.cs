
using Core.Interceptors.Utilities.Results;
using Entities;
using Microsoft.AspNetCore.Http;

namespace Business
{
    public interface IDriverLicenceService
    {
        IDataResult<DriverLicence> Add(DriverLicence driverLicence);
        IDataResult<DriverLicence> GetUserDriverLicenceInfo(int userId);
        IDataResult<List<DriverLicence>> GetAllDriverLicences();
        IResult UploadDriverLicence(IFormFile file,int userId);


    }
}

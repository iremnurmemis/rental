
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface IContactFormService
    {
        IResult Add(ContactForm contactForm);
        IDataResult<List<ContactForm>> GetAll();
    }
}

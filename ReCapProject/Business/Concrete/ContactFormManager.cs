
using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;
using System.Drawing;

namespace Business
{
    public class ContactFormManager : IContactFormService
    {
        private readonly IContactFormDal _contactFormDal;
        public ContactFormManager(IContactFormDal contactFormDal)
        {
            _contactFormDal = contactFormDal;
        }

        public IResult Add(ContactForm contactForm)
        {
            contactForm.CreateTime = DateTime.UtcNow;
           _contactFormDal.Add(contactForm);
            return new SuccessResult("iletişim formu başarıyla eklendi");
        }

        public IDataResult<List<ContactForm>> GetAll()
        {
            return new SuccessDataResult<List<ContactForm>>(_contactFormDal.GetAll(),"formlar Listelendi");
        }
    }
}

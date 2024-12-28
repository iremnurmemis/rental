
namespace Business
{
    public interface IEmailHelper
    {
        void SendEmail(string ToEmail, string subject, string body);
    }
}

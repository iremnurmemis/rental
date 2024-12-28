
using Core.Interceptors.Utilities.Results;
using Entities;

namespace Business
{
    public interface INotificationService
    {

        IDataResult<Notification> SendNotification(string ToEmail, string subject, string body);
        IDataResult<List<Notification>> GetUnreadNotifications(string email); //bir kullanıcının okunmayan maillerini listelemek için
        void MarkAsRead(int notificationId);
    }
}

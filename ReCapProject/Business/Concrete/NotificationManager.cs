

using Azure.Messaging;
using Core.Interceptors.Utilities.Results;
using DataAccess;
using Entities;

namespace Business.Concrete
{
    public class NotificationManager : INotificationService
    {
        private readonly INotificationDal _notificationDal;
        private readonly IEmailHelper _emailHelper;
        public NotificationManager(INotificationDal notificationDal,IEmailHelper emailHelper)
        { 
            _notificationDal = notificationDal;
            _emailHelper = emailHelper; 
        }


        public IDataResult<Notification> SendNotification(string ToEmail, string subject, string body)
        {
            var notification = new Notification
            {
              Email = ToEmail,
              Message=body,
              CreatedAt=DateTime.UtcNow,
              IsRead=false
            };

            _notificationDal.Add(notification);

            _emailHelper.SendEmail(ToEmail, subject,body);
            return new DataResult<Notification>(notification, true, "Bildirim başarıyla gönderildi");
        }

        public IDataResult<List<Notification>> GetUnreadNotifications(string email)
        {
           return  new SuccessDataResult<List<Notification>>(_notificationDal.GetAll(n=>n.Email == email && n.IsRead==false), "Okunmamış bildirimler başarıyla alındı");
        }

        public void MarkAsRead(int notificationId)
        {
            var notification = _notificationDal.Get(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _notificationDal.Update(notification);
            }
        }

       
    }
}

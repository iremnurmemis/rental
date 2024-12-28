using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Bildirim gönderme
        [HttpPost("send")]
        public IActionResult SendNotification(string toEmail, string subject, string body)
        {
            var result = _notificationService.SendNotification(toEmail, subject, body);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Okunmamış bildirimleri getirme
        [HttpGet("unread/{email}")]
        public IActionResult GetUnreadNotifications(string email)  //email adresi
        {
            var result = _notificationService.GetUnreadNotifications(email);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Bildirimi okundu olarak işaretleme
        [HttpPut("markAsRead/{id}")]
        public IActionResult MarkAsRead(int id)
        {
            try
            {
                _notificationService.MarkAsRead(id);
                return Ok(new { message = "Bildirim başarıyla okundu olarak işaretlendi." });
            }
            catch
            {
                return BadRequest(new { message = "Bildirim işaretlenemedi." });
            }
        }
    }
}

using Business;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("GetAllPayments")]
        public IActionResult GetAllPayments()
        {
            var result = _paymentService.GetAll();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("GetPaymentByUserId")]
        public IActionResult GetPaymentsByUserId(int userId)
        {
            var result = _paymentService.GetAllPaymentByUserId(userId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("GetUserPayments")]
        public IActionResult GetPaymentsByUserIdFrontend(int userId)
        {
            var result = _paymentService.GetAllPaymentByUserIdFrontend(userId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("GetPaymentsFront")]
        public IActionResult GetPaymentsFront()
        {
            var result = _paymentService.GetAllPaymentsFront();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }


        [HttpPost("AddPayment")]
        public IActionResult AddPayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Geçersiz veri.");
            }

            var result = _paymentService.Add(payment);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }


        [HttpPost("Delete")]
        public IActionResult DeletePayment(int id)
        {
            var payment = _paymentService.GetAll().Data.FirstOrDefault(p => p.Id == id);
            if (payment == null)
            {
                return NotFound("Belirtilen ID'ye sahip ödeme bulunamadı.");
            }

            var result = _paymentService.Delete(payment);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }



}

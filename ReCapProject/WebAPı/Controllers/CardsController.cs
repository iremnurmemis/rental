using Business;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace WebAPı
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private ICardService _cardService;
        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost("addCard")]
        public IActionResult AddCard(CreatCardTokenDto card)
        {
            var result = _cardService.AddCard(card).Result;
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result); 
        }


        [HttpPost("deleteCard")]
        public IActionResult DeleteCard(Card card)
        {
            var result = _cardService.DeleteCard(card);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getUserCards")]
        public IActionResult GetUserCards(int userId)
        {
            var result = _cardService.GetUserCards(userId); 
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

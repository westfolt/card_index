using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TextCardDto>>> Get()
        {
            IEnumerable<TextCardDto> cards = null;

            try
            {
                cards = await _cardService.GetAllAsync();
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (cards == null || !cards.Any())
                return NotFound();

            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TextCardDto>> GetById(int id)
        {
            TextCardDto card = null;

            try
            {
                card = await _cardService.GetByIdAsync(id);
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (card == null)
                return NotFound();
            return Ok(card);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> Add([FromBody] TextCardDto model)
        {
            int insertId;
            if (model == null)
                return BadRequest(new Response(false, "No model passed"));
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                insertId = await _cardService.AddAsync(model);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully added card with id: {insertId}"));
        }

        [HttpPut("id")]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] TextCardDto model)
        {
            model.Id = id;
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                await _cardService.UpdateAsync(model);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully updated card with id: {id}"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> Delete(int id)
        {
            try
            {
                await _cardService.DeleteAsync(id);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully deleted card with id: {id}"));
        }
    }
}

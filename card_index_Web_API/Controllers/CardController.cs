using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_Web_API.Controllers
{
    /// <summary>
    /// Processes requests to text cards
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Moderator,Registered")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        /// <summary>
        /// Constructor, inject text card service here
        /// </summary>
        /// <param name="cardService">Text card service object</param>
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        /// <summary>
        /// Returns all text cards from DB
        /// </summary>
        /// <returns>All text cards collection</returns>
        [HttpGet]
        [AllowAnonymous]
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

        /// <summary>
        /// Get text card by specified id
        /// </summary>
        /// <param name="id">Text card id to search</param>
        /// <returns>Text card item</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
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

        /// <summary>
        /// Adds new text card to DB
        /// </summary>
        /// <param name="model">New text card object</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPost]
        [Authorize(Roles="Admin,Moderator")]
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

        /// <summary>
        /// Changes existing text card
        /// </summary>
        /// <param name="id">Text card id to change</param>
        /// <param name="model">New text card object, must have the same id</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
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

        /// <summary>
        /// Deletes existing text card
        /// </summary>
        /// <param name="id">Id of text card to delete</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
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

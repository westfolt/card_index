using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor, inject text card service here
        /// </summary>
        /// <param name="cardService">Text card service object</param>
        /// <param name="userService">User service object</param>
        public CardController(ICardService cardService, IUserService userService)
        {
            _cardService = cardService;
            _userService = userService;
        }

        /// <summary>
        /// Returns all text cards from DB preselected by filter and with pagination
        /// </summary>
        /// <returns>Response object, contains selected cards and total number in db
        /// that matches given filter</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<DataShapingResponse<TextCardDto>>> Get([FromQuery] CardFilterParametersModel cardFilterParameters)
        {
            DataShapingResponse<TextCardDto> response = new DataShapingResponse<TextCardDto>();

            response.TotalNumber = await _cardService.GetTotalNumberByFilterAsync(cardFilterParameters);
            response.Data = await _cardService.GetAllAsync(cardFilterParameters);

            if (response.Data == null || !response.Data.Any())
            {
                response.Data = new List<TextCardDto>();
                return Ok(response);
            }

            return Ok(response);
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

            card = await _cardService.GetByIdAsync(id);

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
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<Response>> Add([FromBody] TextCardDto model)
        {
            int insertId;
            if (model == null)
                return BadRequest(new Response(false, "No model passed"));
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            insertId = await _cardService.AddAsync(model);

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

            await _cardService.UpdateAsync(model);

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
            await _cardService.DeleteAsync(id);
            return Ok(new Response(true, $"Successfully deleted card with id: {id}"));
        }
        /// <summary>
        /// Gets rating details, connecting given user and card,
        /// takes data for currently logged in user
        /// </summary>
        /// <param name="cardId">Id of card</param>
        /// <returns>Object containing rate details, null obj if nothing found</returns>
        [HttpGet("rate")]
        public async Task<ActionResult<RateDetailDto>> GetRatingForCardUser([FromQuery] int cardId)
        {
            RateDetailDto rate = null;

            var loggedInId = (await _userService.GetByEmailAsync(User.FindFirstValue(ClaimTypes.Name))).Id;
            rate = await _cardService.GetRateDetailByUserIdCardId(loggedInId, cardId);

            return Ok(rate);
        }
        /// <summary>
        /// Gives rating to text card from currently logged in user
        /// </summary>
        /// <param name="newDetails"></param>
        /// <returns></returns>
        [HttpPost("rate")]
        public async Task<ActionResult<Response>> GiveRatingToCard([FromBody] RateDetailDto newDetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            //user can modify only marks, given by himself
            var loggedInId = (await _userService.GetByEmailAsync(User.FindFirstValue(ClaimTypes.Name))).Id;
            newDetails.UserId = loggedInId;
            await _cardService.AddRatingToCard(newDetails);

            return Ok(new Response(true,
                $"Successfully added rating: {newDetails.RateValue} to card id: {newDetails.TextCardId}"));
        }
    }
}

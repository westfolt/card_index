using System.Collections.Generic;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        /// Returns all text cards from DB preselected by pagination filter
        /// </summary>
        /// <returns>Response object, contains selected cards and total number in db</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<DataShapingResponse<TextCardDto>>> Get([FromQuery] PagingParametersModel pagingParametersModel)
        {
            DataShapingResponse<TextCardDto> response = new DataShapingResponse<TextCardDto>();

            try
            {
                response.TotalNumber = await _cardService.GetTotalNumber();
                response.Data = await _cardService.GetAllAsync(pagingParametersModel);
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (response.Data == null || !response.Data.Any())
                return NotFound();

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
        [Authorize(Roles = "Admin,Moderator")]
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
        /// <summary>
        /// Gets rating details, connecting given user and card
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="cardId">Id of card</param>
        /// <returns>Object containing rate details</returns>
        [HttpGet("rate")]
        public async Task<ActionResult<RateDetailDto>> GetRatingForCardUser([FromQuery]int userId, int cardId)
        {
            RateDetailDto rate = null;
            
            try
            {
                rate = await _cardService.GetRateDetailByUserIdCardId(userId, cardId);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            if (rate == null)
                return NotFound();

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
            try
            {
                //user can modify only marks, given by himself
                var loggedInId = (await _userService.GetByEmailAsync(User.FindFirstValue(ClaimTypes.Name))).Id;
                if (loggedInId != newDetails.UserId)
                    return BadRequest(new Response()
                        { Errors = new List<string> { "Trying to modify other user's mark!" } });

                await _cardService.AddRatingToCard(newDetails);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true,
                $"Successfully added rating: {newDetails.RateValue} to card id: {newDetails.TextCardId}"));
        }
    }
}

using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using card_index_Web_API.Filters;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace card_index_Web_API.Controllers
{
    /// <summary>
    /// Processes requests to genres
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Moderator,Registered")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly ILogger<GenreController> _logger;

        /// <summary>
        /// Constructor, inject genre service here
        /// </summary>
        /// <param name="genreService">Genre service object</param>
        public GenreController(IGenreService genreService, ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        /// <summary>
        /// Returns all genres from DB preselected by pagination filter
        /// </summary>
        /// <returns>All genres collection, contains selected genres and total number in db</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<DataShapingResponse<GenreDto>>> Get([FromQuery] PagingParametersModel pagingParametersModel)
        {
            DataShapingResponse<GenreDto> response = new DataShapingResponse<GenreDto>();

            try
            {
                response.TotalNumber = await _genreService.GetTotalNumberAsync();
                response.Data = await _genreService.GetAllAsync(pagingParametersModel);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(ex.Message);
            }

            if (response.Data == null || !response.Data.Any())
                return NotFound();

            return Ok(response);
        }
        /// <summary>
        /// Returns all genres from DB without filters
        /// </summary>
        /// <returns>All genres collection</returns>
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GenreDto>>> Get()
        {
            IEnumerable<GenreDto> genres = null;
            genres = await _genreService.GetAllAsync();

            if (genres == null || !genres.Any())
                return NotFound();

            return Ok(genres);
        }

        /// <summary>
        /// Get genre by specified name
        /// </summary>
        /// <param name="name">Genre name to search</param>
        /// <returns>Genre item</returns>
        [HttpGet("{name}")]
        [AllowAnonymous]
        public async Task<ActionResult<GenreDto>> GetByName(string name)
        {
            GenreDto genre = null;

            genre = await _genreService.GetByNameAsync(name);

            if (genre == null)
                return NotFound();

            return Ok(genre);
        }

        /// <summary>
        /// Adds new genre to DB
        /// </summary>
        /// <param name="model">New genre object</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> Add([FromBody] GenreDto model)
        {
            int insertId;
            if (model == null)
                return BadRequest(new Response(false, "No model passed"));
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
            
            insertId = await _genreService.AddAsync(model);
            return Ok(new Response(true, $"Successfully added genre with id: {insertId}"));
        }

        /// <summary>
        /// Changes existing genre
        /// </summary>
        /// <param name="id">Genre id to change</param>
        /// <param name="model">New genre object, must have the same id</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] GenreDto model)
        {
            model.Id = id;
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
            
            await _genreService.UpdateAsync(model);
            return Ok(new Response(true, $"Successfully updated genre with id: {id}"));
        }

        /// <summary>
        /// Deletes existing genre
        /// </summary>
        /// <param name="id">Id of genre to delete</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> Delete(int id)
        {
            await _genreService.DeleteAsync(id);
            return Ok(new Response(true, $"Successfully deleted genre with id: {id}"));
        }
    }
}

using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using card_index_Web_API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_Web_API.Controllers
{
    /// <summary>
    /// Processes requests to authors
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Moderator,Registered")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        /// <summary>
        /// Constructor, inject author service here
        /// </summary>
        /// <param name="authorService">Genre service object</param>
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Returns all authors from DB preselected by pagination filter
        /// </summary>
        /// <returns>All authors collection, contains selected genres and total number in db</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<DataShapingResponse<AuthorDto>>> Get([FromQuery] PagingParametersModel pagingParametersModel)
        {
            DataShapingResponse<AuthorDto> response = new DataShapingResponse<AuthorDto>
            {
                TotalNumber = await _authorService.GetTotalNumberAsync(),
                Data = await _authorService.GetAllAsync(pagingParametersModel)
            };


            if (response.Data == null || !response.Data.Any())
            {
                return NotFound();
            }

            return Ok(response);
        }
        /// <summary>
        /// Returns all authors from DB without filters
        /// </summary>
        /// <returns>All authors collection</returns>
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> Get()
        {
            var authors = await _authorService.GetAllAsync();
            if (authors == null || !authors.Any())
            {
                return NotFound();
            }

            return Ok(authors);
        }
        /// <summary>
        /// Get author by specified id
        /// </summary>
        /// <param name="id">Author id to search</param>
        /// <returns>Author item</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthorDto>> GetById(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        /// <summary>
        /// Adds new author to DB
        /// </summary>
        /// <param name="model">New author object</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<Response>> Add([FromBody] AuthorDto model)
        {
            var insertId = await _authorService.AddAsync(model);
            return Ok(new Response(true, $"Successfully added author with id: {insertId}"));
        }

        /// <summary>
        /// Changes existing author
        /// </summary>
        /// <param name="id">Author id to change</param>
        /// <param name="model">New author object, must have the same id</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] AuthorDto model)
        {
            model.Id = id;
            await _authorService.UpdateAsync(model);
            return Ok(new Response(true, $"Successfully updated author with id: {id}"));
        }

        /// <summary>
        /// Deletes existing author
        /// </summary>
        /// <param name="id">Id of author to delete</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> Delete(int id)
        {
            await _authorService.DeleteAsync(id);
            return Ok(new Response(true, $"Successfully deleted author with id: {id}"));
        }
    }
}

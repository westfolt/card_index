using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace card_index_Web_API.Controllers
{
    /// <summary>
    /// Processes requests to authors
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
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
        /// Returns all authors from DB
        /// </summary>
        /// <returns>All authors collection</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> Get()
        {
            IEnumerable<AuthorDto> authors = null;

            try
            {
                authors = await _authorService.GetAllAsync();
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (authors == null || !authors.Any())
                return NotFound();

            return Ok(authors);
        }

        /// <summary>
        /// Get author by specified id
        /// </summary>
        /// <param name="id">Author id to search</param>
        /// <returns>Author item</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetById(int id)
        {
            AuthorDto author = null;

            try
            {
                author = await _authorService.GetByIdAsync(id);
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (author == null)
                return NotFound();
            return Ok(author);
        }

        /// <summary>
        /// Adds new author to DB
        /// </summary>
        /// <param name="model">New author object</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPost]
        public async Task<ActionResult<Response>> Add([FromBody] AuthorDto model)
        {
            int insertId;
            if (model == null)
                return BadRequest(new Response(false, "No model passed"));
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                insertId = await _authorService.AddAsync(model);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully added author with id: {insertId}"));
        }

        /// <summary>
        /// Changes existing author
        /// </summary>
        /// <param name="id">Author id to change</param>
        /// <param name="model">New author object, must have the same id</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpPut("id")]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] AuthorDto model)
        {
            model.Id = id;
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                await _authorService.UpdateAsync(model);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully updated author with id: {id}"));
        }

        /// <summary>
        /// Deletes existing author
        /// </summary>
        /// <param name="id">Id of author to delete</param>
        /// <returns>Http status code of operation with response object</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> Delete(int id)
        {
            try
            {
                await _authorService.DeleteAsync(id);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully deleted author with id: {id}"));
        }
    }
}

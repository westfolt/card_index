using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Dto;
using card_index_BLL.Models.Identity.Infrastructure;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace card_index_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDto>>> Get()
        {
            IEnumerable<GenreDto> genres = null;

            try
            {
                genres = await _genreService.GetAllAsync();
            }
            catch (CardIndexException ex)
            {
                BadRequest(ex.Message);
            }

            if (genres == null || !genres.Any())
                return NotFound();

            return Ok(genres);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<GenreDto>> GetByName(string name)
        {
            GenreDto card = null;

            try
            {
                card = await _genreService.GetByNameAsync(name);
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
        public async Task<ActionResult<Response>> Add([FromBody] GenreDto model)
        {
            int insertId;
            if (model == null)
                return BadRequest(new Response(false, "No model passed"));
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                insertId = await _genreService.AddAsync(model);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully added genre with id: {insertId}"));
        }

        [HttpPut("id")]
        public async Task<ActionResult<Response>> Update(int id, [FromBody] GenreDto model)
        {
            model.Id = id;
            if (!ModelState.IsValid)
                return BadRequest(new Response()
                { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });

            try
            {
                await _genreService.UpdateAsync(model);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully updated genre with id: {id}"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> Delete(int id)
        {
            try
            {
                await _genreService.DeleteAsync(id);
            }
            catch (CardIndexException ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

            return Ok(new Response(true, $"Successfully deleted genre with id: {id}"));
        }
    }
}

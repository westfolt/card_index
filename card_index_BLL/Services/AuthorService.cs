using AutoMapper;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using card_index_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_BLL.Services
{
    /// <summary>
    /// Implements interface between webapi and repository
    /// </summary>
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="mapper">Object for entities mapping</param>
        /// <param name="unitOfWork">Object to work with DAL</param>
        public AuthorService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// Gets all authors from DB and maps to Dto
        /// </summary>
        /// <returns>Author Dto list</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            try
            {
                var takenFromDb = await _unitOfWork.AuthorRepository.GetAllWithDetailsAsync();
                var mapped = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get authors", ex);
            }
        }
        /// <summary>
        /// Overloaded version, takes parameters model with filtering data
        /// </summary>
        /// <param name="parameters">Parameters model, filters result</param>
        /// <returns>Filtered authors</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<IEnumerable<AuthorDto>> GetAllAsync(PagingParametersModel parameters)
        {
            var filter = new PagingParameters { PageNumber = parameters.PageNumber, PageSize = parameters.PageSize };
            try
            {
                var takenFromDb = await _unitOfWork.AuthorRepository.GetAllWithDetailsAsync(filter);
                var mapped = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get text cards", ex);
            }
        }
        /// <summary>
        /// Gets Author with given id if exists
        /// </summary>
        /// <param name="id">Id of user to search</param>
        /// <returns>Author mapped to Dto</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<AuthorDto> GetByIdAsync(int id)
        {
            try
            {
                var takenFromDb = await _unitOfWork.AuthorRepository.GetByIdWithDetailsAsync(id);
                var mapped = _mapper.Map<Author, AuthorDto>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get author with id: {id}", ex);
            }
        }

        /// <summary>
        /// Adds new author to db
        /// </summary>
        /// <param name="model">Author model to add</param>
        /// <returns>id of added author</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<int> AddAsync(AuthorDto model)
        {
            try
            {
                var mapped = _mapper.Map<AuthorDto, Author>(model);
                mapped.Id = 0;
                await _unitOfWork.AuthorRepository.AddAsync(mapped);
                await _unitOfWork.SaveChangesAsync();
                return mapped.Id;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot add author to db", ex);
            }
        }

        /// <summary>
        /// Updates existing author
        /// </summary>
        /// <param name="model">Author Dto with new info</param>
        /// <returns>Async operation</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task UpdateAsync(AuthorDto model)
        {
            try
            {
                var mapped = _mapper.Map<AuthorDto, Author>(model);
                _unitOfWork.AuthorRepository.Update(mapped);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot update author with id: {model.Id}", ex);
            }
        }

        /// <summary>
        /// Deletes author with given id from db
        /// </summary>
        /// <param name="modelId">Id of author to delete</param>
        /// <returns>Async operation</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task DeleteAsync(int modelId)
        {
            try
            {
                await _unitOfWork.AuthorRepository.DeleteByIdAsync(modelId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot delete author with id: {modelId}", ex);
            }
        }

        /// <summary>
        /// Gets authors for given time period
        /// </summary>
        /// <param name="startYear">Start year to search</param>
        /// <param name="endYear">End year to search</param>
        /// <returns>Authors list matching given years range</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<IEnumerable<AuthorDto>> GetAuthorsForPeriodAsync(int startYear, int endYear)
        {
            try
            {
                var takenFromDb = (await _unitOfWork.AuthorRepository.GetAllWithDetailsAsync())
                    .Where(a => a.YearOfBirth >= startYear && a.YearOfBirth <= endYear);
                return _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(takenFromDb);
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get authors for desired date range", ex);
            }
        }
        /// <summary>
        /// Gets total number of authors stored
        /// </summary>
        /// <returns>Author number</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<int> GetTotalNumberAsync()
        {
            try
            {
                return await _unitOfWork.AuthorRepository.GetTotalNumberAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get number of authors", ex);
            }
        }
    }
}
